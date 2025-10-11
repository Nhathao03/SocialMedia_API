using SocialMedia.Infrastructure.Repositories;
using SocialMedia.Core.Entities.DTO.Account;
using SocialMedia.Core.Entities.DTO.RoleCheck;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SocialMedia.Core.Entities.Entity;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using AutoMapper;
using SocialMedia.Core.Interfaces.ServiceInterfaces;
using SocialMedia.Core.Entities.RoleEntity;
using SocialMedia.Core.DTO.Account;

namespace SocialMedia.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _logger;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork,
            ILogger<UserService> logger,
            IConfiguration config,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _config = config;
            _mapper = mapper;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _unitOfWork.UserRepository.GetAllUser();
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _unitOfWork.UserRepository.GetUserById(id);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _unitOfWork.UserRepository.GetUserByEmail(email);
        }
        public async Task UpdateUserAsync(User user)
        {
            await _unitOfWork.UserRepository.UpdateUser(user);
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            _logger.LogInformation("Attempting to delete account for user {UserId}", userId);
            var user = await _unitOfWork.UserRepository.GetUserById(userId);
            if(user is null)
            {
                _logger.LogInformation("Delete failed: User with ID {UserId} not found.", userId);
                return false;
            }

            var result = await _unitOfWork.UserRepository.DeleteUser(userId);
            if (!result)
                _logger.LogInformation("Delete failed: Error deleting user with ID {UserId}.", userId);
            else
                _logger.LogInformation("User with ID {UserId} deleted successfully.", userId);

            await _unitOfWork.RoleCheckRepository.DeleteRoleCheckByUserId(userId);
            return true;
        }

        //Generate userID
        private string GenerateUserID()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 20)
              .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }

        //Register account
        public async Task<AuthResultDTO> RegisterAccountAsync(RegisterDTO model)
        {
            _logger.LogInformation("Registering new user with email: {Email}", model.Email);

            var emailExists = await _unitOfWork.UserRepository.IsEmailExistsAsync(model.Email);
            if(emailExists)
            {
                _logger.LogWarning("Registeration failed: Email {Email} already in use.", model.Email);
                return new AuthResultDTO
                {
                    Success = false,
                    Errors = new[] { "Email already in use." }
                };
            }

            if (!string.IsNullOrEmpty(model.PhoneNumber) && await _unitOfWork.UserRepository.IsPhoneExistsAsync(model.PhoneNumber))
            {
                _logger.LogInformation("Registeration failed: Phone number {PhoneNumber} already in use.", model.PhoneNumber);
                return new AuthResultDTO
                {
                    Success = false,
                    Errors = new[] { "Phone number already in use." }
                };
            }

            var user = new User
            {
                Id = GenerateUserID(),
                Email = model.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                PhoneNumber = model.PhoneNumber,
                Fullname = model.FullName,
                Birth = model.Birth,
                gender = null,
                Avatar = "user/avatar/avatar_user.png",
                BackgroundProfile = "user/background/1.jpg",
                NormalizeEmail = NormalizeString(model.Email),
                NormalizeUsername = NormalizeString(model.FullName),
            };
            var result =  await _unitOfWork.UserRepository.RegisterAccount(user);

            if(result == null)
            {
                _logger.LogError("Registeration failed for email: {Email}", model.Email);
                return new AuthResultDTO
                {
                    Success = false,
                    Errors = new[] { "Error while registering." }
                };
            }

            //Get id role user
            var roleId = await _unitOfWork.RoleRepository.GetRoleIdUser();
            var roleUser = new RoleCheck
            {
                UserID = user.Id,
                RoleID = roleId,
            };
            await _unitOfWork.RoleCheckRepository.AddRoleCheck(roleUser);

           return new AuthResultDTO
           {
               Success = true
           };
        }

        public async Task<AuthResultDTO> LoginAsync(LoginDTO model)
        {
            _logger.LogInformation("Attempting login for email: {Email}", model.Email);
            var user = await _unitOfWork.UserRepository.GetUserByEmail(model.Email);
            if (user == null)
            {
                return new AuthResultDTO
                {
                    Success = false,
                    Errors = new[] { "Invalid credentials." }
                };
            }

            var result = await _unitOfWork.UserRepository.CheckPasswordSignInAsync(model, model.Password);
            if (!result)
            {
                _logger.LogWarning("Login failed for email: {Email} due to invalid password.", model.Email);
                return new AuthResultDTO
                {
                    Success = false,
                    Errors = new[] { "Invalid credentials." }
                };
            }

            bool isAdmin = await _unitOfWork.RoleCheckRepository.IsAdminAsync(user.Id);
            string role = isAdmin ? "Admin" : "User";

            // Generate JWT token
            var accessToken = await GenerateAccessTokenAsync(user.Id);
            string refreshToken = await GenerateRefreshTokenAsync(user);

            _logger.LogInformation("Login successful for email: {Email}", model.Email);
            return new AuthResultDTO
            {
                Success = true,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        //Generate JWT Token
        public async Task<string> GenerateAccessTokenAsync(string userId)
        {
            _logger.LogInformation("Generating access token for user ID: {UserId}", userId);

            var jwtSettings = _config.GetSection("Jwt");
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

            DateTime expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["Expires"]));

            bool isAdmin = await _unitOfWork.RoleCheckRepository.IsAdminAsync(userId);
            string role = isAdmin ? "Admin" : "User";

            var user = await _unitOfWork.UserRepository.GetUserById(userId);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, userId!),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.Name, user.Fullname),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim("userId", userId)
            };

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken tokenGenerator = new JwtSecurityToken(
               issuer: jwtSettings["Issuer"],
               audience: jwtSettings["Audience"],
               claims: claims,
               expires: expiration,
               signingCredentials: signingCredentials
            );
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(tokenGenerator);
            _logger.LogInformation("Access token generated for user ID: {UserId}", userId);
            return token;
        }

        // Generate refresh token
        public async Task<string> GenerateRefreshTokenAsync(User user)
        {
            _logger.LogInformation("Generating refresh token for user ID: {UserId}", user.Id);
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            var token = Convert.ToBase64String(randomNumber);
            user.RefreshToken = token;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _unitOfWork.UserRepository.UpdateUser(user);
            _logger.LogInformation("Refresh token generated and saved for userId: {UserId}", user.Id);

            return token;
        }

        // Get user by refresh token
        public User? GetUserByRefreshToken(string refreshToken)
        {
            var user = _unitOfWork.UserRepository.GetUserByRefreshToken(refreshToken);
            if(user == null)
                _logger.LogWarning("No user found for the provided refresh token.");
            return user;
        }

        // Normalize string by removing diacritics and converting to lowercase
        private static string NormalizeString(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            input = input.ToLowerInvariant();
            var normalized = input.Normalize(System.Text.NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var c in normalized)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }
            return sb.ToString().Normalize(System.Text.NormalizationForm.FormC);
        }

        //Get profile user async
        public async Task<ProfileDTO?> GetProfileAsync(string userId)
        {
            _logger.LogInformation("Retrieving profile for user ID: {UserId}", userId);
            var user = await _unitOfWork.UserRepository.GetUserById(userId);
            if (user is null)
            {
                _logger.LogInformation("User with ID {UserId} not found.", userId);
                return null;
            }

            var profile = _mapper.Map<ProfileDTO>(user);
            _logger.LogInformation("Profile retrieved successfully for user ID: {UserId}", userId);
            return profile;
        }
        //Update information user async
        public async Task<ProfileDTO?> UpdateProfileAsync (string userId, UpdateProfileDTO profileDto)
        {
            _logger.LogInformation("Updating profile for user ID: {UserId}", userId);
            var user = await _unitOfWork.UserRepository.GetUserById(userId);
            if (user is null)
            {
                _logger.LogInformation("Update failed: User with ID {UserId} not found.", userId);
                return null;
            }

            user.Fullname = profileDto.fullname ?? user.Fullname;
            user.Birth = profileDto.Birth ?? user.Birth;
            user.addressID = profileDto.addressID ?? user.addressID;
            user.gender = profileDto.gender ?? user.gender;
            user.Avatar = profileDto.avatar ?? user.Avatar;

            var result =  await _unitOfWork.UserRepository.UpdateUser(user);
            if (result is null)
            {
                _logger.LogInformation("Update failed: Error updating user with ID {UserId}.", userId);
                return null;
            }

            var profile = _mapper.Map<ProfileDTO>(user);

            _logger.LogInformation("Profile updated successfully for user ID: {UserId}", userId);
            return profile;
        }

        //Change Password
        public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordDTO dto)
        {
            _logger.LogInformation("Changing password for user ID: {UserId}", userId);

            var user = await _unitOfWork.UserRepository.GetUserById(userId);
            if (user is null)
            {
                _logger.LogInformation("Change password failed: User with ID {UserId} not found.", userId);
                return false;
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.currentPass, user.Password);
            if (isPasswordValid)
            {
                string newPass = BCrypt.Net.BCrypt.HashPassword(dto.newPass);
                user.Password = newPass;
                var result = await _unitOfWork.UserRepository.UpdateUser(user);
                if (result is null)
                {
                    _logger.LogInformation("Change password failed: Error updating password for user ID {UserId}.", userId);
                    return false;
                }
            }
            _logger.LogInformation("Password changed successfully for user ID: {UserId}", userId);
            return true;
        }

        public async Task<ProfileDTO?> ManageContact (string userId, UpdateContactDTO manageContactDTO)
        {
            _logger.LogInformation("Managing contact for user ID: {UserId}", userId);
            var user = await _unitOfWork.UserRepository.GetUserById(userId);
            if(user is null)
            {
                _logger.LogInformation("Manage contact failed: User with ID {UserId} not found.", userId);
                return null;
            }

            user.PhoneNumber = manageContactDTO.phoneNumber;
            var result = await _unitOfWork.UserRepository.UpdateUser(user);
            _logger.LogInformation("Contact updated successfully for user ID: {UserId}", userId);
            return _mapper.Map<ProfileDTO?>(result);
        }
        
        public async Task<ProfileDTO?> UploadBackgroundUser(string userId, UpdateBackgroundDTO backgroundDTO)
        {
            _logger.LogInformation("Uploading background for user ID: {UserId}", userId);
            var user = await _unitOfWork.UserRepository.GetUserById(userId);
            if (user is null ) {
                _logger.LogInformation("Upload background failed: User with ID {UserId} not found.", userId);
                return null;
            }

            user.BackgroundProfile = backgroundDTO.backgroundImage;
            await _unitOfWork.UserRepository.UpdateUser(user);
            
            _logger.LogInformation("Background updated successfully for user ID: {UserId}", userId);
            return _mapper.Map<ProfileDTO>(user);
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _unitOfWork.UserRepository.IsEmailExistsAsync(email);
        }

        public async Task<bool> IsPhoneExistsAsync(string phoneNumber)
        {
            return await _unitOfWork.UserRepository.IsPhoneExistsAsync(phoneNumber);
        }
    }
}
