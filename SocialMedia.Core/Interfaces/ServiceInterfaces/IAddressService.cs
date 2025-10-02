using SocialMedia.Core.Entities.DTO;
using SocialMedia.Core.Entities.UserEntity;

namespace SocialMedia.Core.Services
{
    public interface IAddressService
    {
        Task<IEnumerable<Address>> GetAllAddressAsync();
        Task<Address> GetAddressByIdAsync(int id);
        Task UpdateAddressAsync(AddressDTO modelDTO);
        Task DeleteAddressAsync(int id);
        Task<int> AddAddressAsync(AddressDTO modelDTO);
    }
}
