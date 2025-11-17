using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Core.Services;
using SocialMedia.Core.Entities.DTO;
using SocialMedia.Core.Entities.DTO.Account;
using SocialMedia.Core.Entities.Email;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ResetPasswordDTO = SocialMedia.Core.Entities.Email.ResetPasswordDTO;
using SocialMedia;
using Swashbuckle.AspNetCore.Annotations;
using Social_Media.Helpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;
namespace Social_Media.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRoleCheckService _roleCheckService;
        private readonly IConfiguration _config;
        private static Dictionary<string, (string Otp, DateTime Expiry)> _otpStore = new();
        private readonly EmailService _emailService;

        public ProfileController(IUserService userService,
            IRoleCheckService roleCheckService,
            IConfiguration configuration,
            EmailService emailService)
        {
            _userService = userService;
            _roleCheckService = roleCheckService;
            _config = configuration;
            _emailService = emailService;
        }

        /// <summary>
        /// Get the logged-in user's profile.
        /// </summary>
        /// <remarks>
        /// Returns user details including profile info and addresses.
        /// </remarks>
        /// <response code="200">Profile retrieved successfully</response>
        /// <response code="401">If user is not logged in</response>
        /// <response code="404">If user not found</response>
        [HttpGet("get-profile")]
        [SwaggerOperation(Summary = "Get user profile", Description = "Retrieves the profile of the currently logged-in user.")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return ApiResponseHelper.Unauthorized("User is not authenticated.");

            var user = await _userService.GetProfileAsync(userId);
            if (user is null)
                return ApiResponseHelper.NotFound("User not found.");

            return ApiResponseHelper.Success(user, "Profile retrieved successfully.");
        }

        /// <summary>
        /// Update the logged-in user's profile.
        /// </summary>
        /// <param name="personalInformationDto">Profile update details (Display Name, Phone Number, etc.)</param>
        /// <response code="200">Profile updated successfully</response>
        /// <response code="401">If user is not logged in</response>
        /// <response code="404">If user not found</response>
        [HttpPut("update-profile")]
        [SwaggerOperation(Summary = "Update personal information", Description = "Updates the personal information of the currently logged-in user.")]
        public async Task<IActionResult> UpdatePersonalInformation([FromBody] UpdateProfileDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.UpdateProfileAsync(userId, dto);
            return ApiResponseHelper.Success(result, "Update personal information success");
        }

        /// <summary>
        /// Change the logged-in user's password.
        /// </summary>
        /// <param name="changePasswordDto">Old password and new password</param>
        /// <response code="200">Password changed successfully</response>
        /// <response code="400">If old password is incorrect or update failed</response>
        /// <response code="401">If user is not logged in</response>
        [HttpPut("change-password")]
        [SwaggerOperation(Summary = "Change password", Description = "Changes the password of the currently logged-in user.")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (changePasswordDto is null)
            {
                return ApiResponseHelper.BadRequest("ChangePasswordDTO cannot be null.");
            }
            if (changePasswordDto.newPass != changePasswordDto.verifyPass)
            {
                return ApiResponseHelper.BadRequest("New password and verify password do not match.");
            }
            else
            {
                await _userService.ChangePasswordAsync(userId, changePasswordDto);
                return ApiResponseHelper.Success("Change password success");
            }
        }

        /// <summary>
        /// Change the logged-in user's contact.
        /// </summary>
        /// <param name="manageContactDto">Contact detail</param>
        /// <response code="200">Update contact successfully</response>
        /// <response code="400">If manageContactDto null</response>
        /// <response code="401">If user is not logged in</response>
        [HttpPut("update-contact")]
        [SwaggerOperation(Summary = "Manage contact", Description = "Manage the contact information of the currently logged-in user.")]
        public async Task<IActionResult> ManageContact([FromBody] UpdateContactDTO manageContactDto)
         {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (manageContactDto is null)
            {
                return ApiResponseHelper.BadRequest("ManageContactDTO cannot be null.");
            }
            await _userService.ManageContact(userId, manageContactDto);
            return ApiResponseHelper.Success("Update contact success");
        }

        /// <summary>
        /// Change the logged-in user's background.
        /// </summary>
        /// <param name="backgroundDTO"></param>
        /// <response code="200">Update background successfully</response>
        /// <response code="400">If backgroundDto null</response>
        /// <response code="401">If user is not logged in</response>
        [HttpPut("update-background")]
        [SwaggerOperation(Summary = "Update background", Description = "Update the background image of the currently logged-in user.")]
        public async Task<IActionResult> UpdateBackgroundUser([FromBody] UpdateBackgroundDTO backgroundDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(backgroundDto is null)
            {
                return ApiResponseHelper.BadRequest("BackgroundDTO cannot be null.");
            }
            await _userService.UploadBackgroundUser(userId, backgroundDto);
            return ApiResponseHelper.Success("Update background user success");
        }

        /// <summary>
        /// Forgot password - send OTP to email
        /// </summary>
        /// <param name="request"></param>
        /// <response code="200">Send OTP successfully</response>
        /// <response code="400">If request null</response>
        /// <response code="401">If user is not logged in</response>
        [HttpPost("forgot-password")]
        [SwaggerOperation(Summary = "Forgot password", Description = "Sends an OTP to the user's email for password reset.")]
        public async Task<IActionResult> ForgotPasswordAsync(string email)
        {
            // Check if email is not exist in database
            if (!await _userService.IsEmailExistsAsync(email))
            {
                return ApiResponseHelper.BadRequest("Email does not exist");
            }

            // Create OTP 6 numbers
            var otp = new Random().Next(100000, 999999).ToString();

            // Save OTP 5 minutes
            _otpStore[email] = (otp, DateTime.UtcNow.AddMinutes(5));

            // Send OTP to Email
            await _emailService.SendOtpAsync(email, otp);

            return ApiResponseHelper.Success("OTP sent to email successfully");
        }

        /// <summary>
        /// Reset password using OTP
        /// </summary>
        /// <param name="request"></param>
        /// <response code="200">Reset password successfully</response>
        /// <response code="400">If request null</response>
        /// <response code="401">If user is not logged in</response>
        [HttpPost("reset-password")]
        [SwaggerOperation(Summary = "Reset password", Description = "Resets the user's password using the provIded OTP.")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordDTO dto)
        {

            var (otp, expiry) = _otpStore[dto.Email];

            // Check if OTP is valId and not expired
            if (otp != dto.Otp || DateTime.UtcNow > expiry)
                return ApiResponseHelper.BadRequest("InvalId or expired OTP");

            if (dto.NewPassword != dto.ConfirmPassword)
                return ApiResponseHelper.BadRequest("New password and confirm password do not match");

            var user = await _userService.GetUserByEmailAsync(dto.Email);
            if (user == null) return BadRequest("User not exist");

            // Hash new password by BCrypt
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await _userService.UpdateUserAsync(user);

            // Delete OTP after successful reset
            _otpStore.Remove(dto.Email);

            return ApiResponseHelper.Success("Reset password successfully");
        }

        /// <summary>
        /// Delete the logged-in user's account.
        /// </summary>
        /// <remarks>
        /// Permanently deletes the user's account and related data.
        /// </remarks>
        /// <response code="200">Account deleted successfully</response>
        /// <response code="400">If deletion failed</response>
        /// <response code="401">If user is not logged in</response>
        [HttpDelete("delete-account")]
        [SwaggerOperation(Summary = "Delete account", Description = "Deletes the current logged-in user's account permanently.")]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null) ApiResponseHelper.Unauthorized("User is not authenticated.");
            
            var result = await _userService.DeleteUserAsync(userId);
            if(!result) return ApiResponseHelper.BadRequest("Delete user failed");
            return ApiResponseHelper.Success("", "Account deleted successfully.");
        }

    }
}
