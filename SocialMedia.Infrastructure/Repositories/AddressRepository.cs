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
            return await _context.addresses.ToListAsync();
        }

        public async Task<Address?> GetAddressByIdAsync(int id)
        {
            return await _context.addresses.FindAsync(id);
        }

        public async Task<Address?> UpdateAddressAsync(Address address)
        {
            _context.addresses.Update(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<bool> DeleteAddressAsync(int id)
        {
            var Address = await _context.addresses.FindAsync(id);
            if (Address is null)
                return false;
            _context.addresses.Remove(Address);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Address?> AddAddressAsync(Address address)
        {
            await _context.addresses.AddAsync(address);
            await _context.SaveChangesAsync();
            return address;
        }
    }
}
