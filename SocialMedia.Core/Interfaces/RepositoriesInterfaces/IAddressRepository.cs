using SocialMedia.Core.Entities.UserEntity;

namespace SocialMedia.Core.Interfaces.RepositoriesInterfaces
{
    public interface IAddressRepository
    {
        Task<List<Address>?> GetAllAddressAsync();
        Task<Address?> GetAddressByIdAsync(int id);
        Task<Address?> UpdateAddressAsync(Address Address);
        Task<Address?> AddAddressAsync (Address address);
        Task<bool> DeleteAddressAsync(int id);
    }
}
