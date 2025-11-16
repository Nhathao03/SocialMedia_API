using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities.UserEntity;
using SocialMedia.Core.Interfaces.RepositoriesInterfaces;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Repositories
{
    public class AddressRepository :IAddressRepository
    {
        private readonly AppDbContext _context;

        public AddressRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Address>?> GetAllAddressAsync()
        {
            return await _context.Addresses.ToListAsync();
        }

        public async Task<Address?> GetAddressByIdAsync(int Id)
        {
            return await _context.Addresses.FindAsync(Id);
        }

        public async Task<Address?> UpdateAddressAsync(Address address)
        {
            _context.Addresses.Update(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<bool> DeleteAddressAsync(int Id)
        {
            var Address = await _context.Addresses.FindAsync(Id);
            if (Address is null)
                return false;
            _context.Addresses.Remove(Address);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Address?> AddAddressAsync(Address address)
        {
            _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();
            return address;
        }
    }
}
