using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities.DTO.Account;
using SocialMedia.Core.Entities.Entity;
using SocialMedia.Infrastructure.Data;
using System.Text;

namespace SocialMedia.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUser()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUserById(string Id)
        {
            return await _context.Users.FirstOrDefaultAsync(p => p.Id == Id);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(p => p.Email == email);
        }
        public async Task<User?> UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUser(string Id)
        {
            var user = await _context.Users.FindAsync(Id);
            if (user is null)
                return false;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        //Register account
        public async Task<User?> RegisterAccount(User user)
        {
            _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }        

        // Check exist email
        public async Task<bool> IsEmailExistsAsync(string email)
        {
            var existEmail =  await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (existEmail != null)
            {
                return true; // Email already exists
            }
            return false; // Email does not exist
        }
        // Check exist phone number
        public async Task<bool> IsPhoneExistsAsync(string phoneNumber)
        {
            var existPhone = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
            if (existPhone != null)
            {
                return true; // Phone number already exists
            }
            return false; // Phone number does not exist
        }

        // Check password sign in
        public async Task<bool> CheckPasswordSignInAsync(LoginDTO model, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return true; // Password is correct
            }
            return false; // Password is incorrect
        }

        // Get user by refresh token
        public User? GetUserByRefreshToken(string refreshToken)
        {
            return _context.Users.FirstOrDefault(u => u.RefreshToken == refreshToken && u.RefreshTokenExpiryTime > DateTime.UtcNow);
        }
    }
}
