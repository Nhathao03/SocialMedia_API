using SocialMedia.Core.DTO.Account;
using SocialMedia.Core.Entities.DTO.Account;
using SocialMedia.Core.Entities.Entity;

namespace SocialMedia.Core.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User?>?> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(string id);
        Task UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(string id);
        Task<AuthResultDTO?> RegisterAccountAsync (RegisterDTO registerDTO);

        //Profile
        Task<ProfileDTO?> GetProfileAsync(string userId);
        Task<ProfileDTO?> UpdateProfileAsync (string userId, UpdateProfileDTO profileDto);
        Task<bool> ChangePasswordAsync(string userId, ChangePasswordDTO changePasswordDTO);
        Task<ProfileDTO?> ManageContact(string userId, UpdateContactDTO manageContactDTO);
        Task<ProfileDTO?> UploadBackgroundUser(string userId, UpdateBackgroundDTO backgroundDTO);
        Task<bool> IsEmailExistsAsync(string email);
        Task<bool> IsPhoneExistsAsync(string phoneNumber);
        Task<string> GenerateAccessTokenAsync(string userId);
        Task<string> GenerateRefreshTokenAsync(User user);
        Task<User?> GetUserByEmailAsync(string email);
        Task<AuthResultDTO?> LoginAsync(LoginDTO model);
        User? GetUserByRefreshToken(string refreshToken);
    }
}
