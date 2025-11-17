using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities.Entity;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Repositories
{
    public class UserLoginRepository : IUserLoginRepository
    {
        private readonly AppDbContext _context;

        public UserLoginRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserLogins>> GetAllUserLogin()
        {
            return await _context.UserLogins.ToListAsync();
        }

        public async Task<UserLogins> GetUserLoginById(int Id)
        {
            return await _context.UserLogins.FirstOrDefaultAsync(p => p.Id == Id);
        }

        public async Task UpdateUserLogin(UserLogins userLogins)
        {
            _context.UserLogins.Update(userLogins);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserLogin(int Id)
        {
            var UserLogin = await _context.UserLogins.FindAsync(Id);
            if (UserLogin != null)
            {
                _context.UserLogins.Remove(UserLogin);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddUserLogin(UserLogins userLogins)
        {
            _context.AddAsync(userLogins);
            await _context.SaveChangesAsync();
        }

    }
}
