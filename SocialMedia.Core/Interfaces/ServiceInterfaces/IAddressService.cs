using SocialMedia.Core.DTO.Account;
using SocialMedia.Core.Entities.UserEntity;

namespace SocialMedia.Core.Services
{
    public interface IAddressService
    {
        Task<List<RetriveAddressDTO>?> GetAllAddressAsync();
        Task<RetriveAddressDTO?> GetAddressByIdAsync(int Id);
        Task<RetriveAddressDTO?> AddAddressAsync(AddressDTO address);
        Task<RetriveAddressDTO?> UpdateAddressAsync(int addressId, AddressDTO addressDto);
        Task<bool> DeleteAddressAsync(int Id);
    }
}
