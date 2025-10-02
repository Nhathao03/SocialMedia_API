using SocialMedia.Infrastructure.Repositories;
using SocialMedia.Core.Entities.UserEntity;
using SocialMedia.Core.Entities.Entity;

namespace SocialMedia.Core.Services
{
    public class UserLoginService : IUserLoginService
    {
        private readonly IUserLoginRepository _userLoginRepository;

        public UserLoginService(IUserLoginRepository repository)
        {
            _userLoginRepository = repository;
        }

        public async Task<IEnumerable<UserLogins>> GetAllUserLoginsAsync()
        {
            return await _userLoginRepository.GetAllUserLogin();
        }

        public async Task<UserLogins> GetUserLoginByIdAsync(int id)
        {
            return await _userLoginRepository.GetUserLoginById(id);
        }

        public async Task AddUserLoginsAsync(UserLogins userLogins)
        {
            await _userLoginRepository.AddUserLogin(userLogins);
        }
        public async Task UpdateUserLoginAsync(UserLogins userLogins)
        {
            await _userLoginRepository.UpdateUserLogin(userLogins);
        }

        public async Task DeleteUserLoginAsync(int id)
        {
            await _userLoginRepository.DeleteUserLogin(id);
        }
    }
}
