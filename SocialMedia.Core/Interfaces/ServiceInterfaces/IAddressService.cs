using SocialMedia.Core.DTO.Account;
using SocialMedia.Core.Entities.UserEntity;

namespace SocialMedia.Core.Services
{
    public interface IAddressService
    {
        Task<List<Address>?> GetAllAddressAsync();
        Task<Address?> GetAddressByIdAsync(int id);
        Task<RetriveAddressDTO?> AddAddressAsync(AddressDTO address);
        Task<RetriveAddressDTO?> UpdateAddressAsync(int addressId, AddressDTO addressDto);
        Task<bool> DeleteAddressAsync(int id);
    }
}
