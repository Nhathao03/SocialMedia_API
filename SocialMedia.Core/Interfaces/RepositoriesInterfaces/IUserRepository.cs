using SocialMedia.Core.Entities.DTO.AccountUser;
using SocialMedia.Core.Entities.Entity;

namespace SocialMedia.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUser();
        Task<User> GetUserById(string id);
        Task UpdateUser(User user);
        Task DeleteUser(string id);
        Task<User> RegisterAccount(User user);
        Task<bool> IsEmailExistsAsync(string email);
        Task<bool> IsPhoneExistsAsync(string phoneNumber);
        Task<User> GetUserByEmail(string email);
        Task<bool> CheckPasswordSignInAsync(LoginDTO model, string password);
    }
}
