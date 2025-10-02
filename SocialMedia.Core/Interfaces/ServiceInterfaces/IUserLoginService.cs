using SocialMedia.Core.Entities.DTO;
using SocialMedia.Core.Entities.Entity;
namespace SocialMedia.Core.Services
{
    public interface IUserLoginService
    {
        Task<IEnumerable<UserLogins>> GetAllUserLoginsAsync();
        Task<UserLogins> GetUserLoginByIdAsync(int id);
        Task UpdateUserLoginAsync(UserLogins userLogins);
        Task DeleteUserLoginAsync(int id);
        Task AddUserLoginsAsync (UserLogins userLogins);
    }
}
