using SocialMedia.Core.Entities.DTO;
using SocialMedia.Core.Entities.Entity;

namespace SocialMedia.Infrastructure.Repositories
{
    public interface IUserLoginRepository
    {
        Task<IEnumerable<UserLogins>> GetAllUserLogin();
        Task<UserLogins> GetUserLoginById(int id);
        Task UpdateUserLogin(UserLogins userLogins);
        Task DeleteUserLogin(int id);
        Task AddUserLogin(UserLogins userLogins);
    }
}
