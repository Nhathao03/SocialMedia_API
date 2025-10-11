using SocialMedia.Core.DTO.Account;
using SocialMedia.Core.Entities.DTO.Account;
using SocialMedia.Core.Entities.Entity;

namespace SocialMedia.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUser();
        Task<User?> GetUserById(string id);
        Task<User?> UpdateUser(User user);
        Task<bool> DeleteUser(string id);
        Task<User?> RegisterAccount(User user);
        Task<bool> IsEmailExistsAsync(string email);
        Task<bool> IsPhoneExistsAsync(string phoneNumber);
        Task<User> GetUserByEmail(string email);
        Task<bool> CheckPasswordSignInAsync(LoginDTO model, string password);
        User? GetUserByRefreshToken(string refreshToken);
    }
}
