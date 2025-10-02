using SocialMedia.Core.Entities.UserEntity;

namespace SocialMedia.Core.Interfaces.RepositoriesInterfaces
{
    public interface IAddressRepository
    {
        Task<IEnumerable<Address>> GetAllAddress();
        Task<Address> GetAddressById(int id);
        Task UpdateAddress(Address Address);
        Task DeleteAddress(int id);
        Task<int> AddNewAddress (Address address);
    }
}
