using SocialMedia.Core.Entities.UserEntity;

namespace SocialMedia.Core.Interfaces.RepositoriesInterfaces
{
    public interface IAddressRepository
    {
        Task<List<Address>?> GetAllAddressAsync();
        Task<Address?> GetAddressByIdAsync(int Id);
        Task<Address?> UpdateAddressAsync(Address Address);
        Task<Address?> AddAddressAsync (Address address);
        Task<bool> DeleteAddressAsync(int Id);
    }
}
