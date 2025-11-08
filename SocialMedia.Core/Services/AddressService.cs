using AutoMapper;
using Microsoft.Extensions.Logging;
using SocialMedia.Core.DTO.Account;
using SocialMedia.Core.Entities.Entity;
using SocialMedia.Core.Entities.UserEntity;
using SocialMedia.Core.Interfaces.RepositoriesInterfaces;
using SocialMedia.Core.Interfaces.ServiceInterfaces;

namespace SocialMedia.Core.Services
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AddressService> _logger;

        public AddressService(IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<AddressService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<Address>?> GetAllAddressAsync()
        {
            return await _unitOfWork.AddressRepository.GetAllAddressAsync();
        }

        public async Task<Address?> GetAddressByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving address by ID {AddressId}", id);
            return await _unitOfWork.AddressRepository.GetAddressByIdAsync(id);
        }

        public async Task<RetriveAddressDTO?> AddAddressAsync(AddressDTO dto)
        {
            _logger.LogInformation("Adding new address with name {AddressName}", dto?.name);
            if (dto == null)
                throw new ArgumentNullException(nameof(AddressDTO), "Address data is required.");
            if (string.IsNullOrWhiteSpace(dto.name))
                throw new ArgumentException("Address name cannot be empty.", nameof(dto.name));

            var address = _mapper.Map<Address>(dto);
            var result = await _unitOfWork.AddressRepository.AddAddressAsync(address);
            _logger.LogInformation("Address added with ID {AddressId}", result?.ID);
            return _mapper.Map<RetriveAddressDTO>(result);
        }

        public async Task<RetriveAddressDTO?> UpdateAddressAsync(int addressId, AddressDTO addressDto)
        {
            _logger.LogInformation("Updating address with ID {AddressId}", addressId);
            var existingAddress = await _unitOfWork.AddressRepository.GetAddressByIdAsync(addressId);
            if (existingAddress is null)
            {
                throw new KeyNotFoundException($"Address with Id {addressId} not exits.");
            }

            var address = _mapper.Map(addressDto, existingAddress);
            var result = await _unitOfWork.AddressRepository.UpdateAddressAsync(address);
            _logger.LogInformation("Address with ID {AddressId} updated successfully", addressId);
            return _mapper.Map<RetriveAddressDTO>(result);
        }

        public async Task<bool> DeleteAddressAsync(int id)
        {
            _logger.LogInformation("Deleting address with ID {AddressId}", id);
            var exitsAddress = await _unitOfWork.AddressRepository.GetAddressByIdAsync(id);
            if(exitsAddress is null)
            {
                throw new KeyNotFoundException($"Address with Id {id} not exits.");
            }
            
            var result = await _unitOfWork.AddressRepository.DeleteAddressAsync(id);
            _logger.LogInformation("Address with ID {AddressId} deleted successfully", id);
            return result;
        }
        
    }
}
