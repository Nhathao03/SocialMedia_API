using SocialMedia.Core.Entities.DTO.AccountUser;
using SocialMedia.Core.Entities.Entity;

namespace SocialMedia.Core.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(string id);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(string id);
        Task<AuthResultDTO> RegisterAccountAsync (RegisterDTO registerDTO);
        Task UpdatePersonalInformation (PersonalInformationDTO personalInformationDTO);
        Task ChangePassword(ChangePasswordDTO changePasswordDTO);
        Task ManageContact(ManageContactDTO manageContactDTO);
        Task UploadBackgroundUser(BackgroundDTO backgroundDTO);
        Task<bool> IsEmailExistsAsync(string email);
        Task<bool> IsPhoneExistsAsync(string phoneNumber);
        Task<User> GetUserByEmailAsync(string email);
        Task<AuthResultDTO> LoginAsync(LoginDTO model);
    }
}
