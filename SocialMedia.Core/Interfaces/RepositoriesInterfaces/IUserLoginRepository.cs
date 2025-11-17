using SocialMedia.Core.Entities.DTO;
using SocialMedia.Core.Entities.Entity;

namespace SocialMedia.Infrastructure.Repositories
{
    public interface IUserLoginRepository
    {
        Task<IEnumerable<UserLogins>> GetAllUserLogin();
        Task<UserLogins> GetUserLoginById(int Id);
        Task UpdateUserLogin(UserLogins userLogins);
        Task DeleteUserLogin(int Id);
        Task AddUserLogin(UserLogins userLogins);
    }
}
