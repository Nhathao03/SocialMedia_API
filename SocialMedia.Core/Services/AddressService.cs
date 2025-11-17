using AutoMapper;
using Microsoft.Extensions.Logging;
using SocialMedia.Core.DTO.Account;
using SocialMedia.Core.Entities.UserEntity;
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

        public async Task<List<RetriveAddressDTO>?> GetAllAddressAsync()
        {
            var data = await _unitOfWork.AddressRepository.GetAllAddressAsync();
            return _mapper.Map<List<RetriveAddressDTO>>(data);
        }

        public async Task<RetriveAddressDTO?> GetAddressByIdAsync(int Id)
        {
            _logger.LogInformation("Retrieving address by Id {AddressId}", Id);
            var result = await _unitOfWork.AddressRepository.GetAddressByIdAsync(Id);
            return _mapper.Map<RetriveAddressDTO>(result);
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
            _logger.LogInformation("Address added with Id {AddressId}", result?.Id);
            return _mapper.Map<RetriveAddressDTO>(result);
        }

        public async Task<RetriveAddressDTO?> UpdateAddressAsync(int addressId, AddressDTO addressDto)
        {
            _logger.LogInformation("Updating address with Id {AddressId}", addressId);
            var existingAddress = await _unitOfWork.AddressRepository.GetAddressByIdAsync(addressId);
            if (existingAddress is null)
            {
                throw new KeyNotFoundException($"Address with Id {addressId} not exits.");
            }

            var address = _mapper.Map(addressDto, existingAddress);
            var result = await _unitOfWork.AddressRepository.UpdateAddressAsync(address);
            _logger.LogInformation("Address with Id {AddressId} updated successfully", addressId);
            return _mapper.Map<RetriveAddressDTO>(result);
        }

        public async Task<bool> DeleteAddressAsync(int Id)
        {
            _logger.LogInformation("Deleting address with Id {AddressId}", Id);
            var exitsAddress = await _unitOfWork.AddressRepository.GetAddressByIdAsync(Id);
            if(exitsAddress is null)
            {
                throw new KeyNotFoundException($"Address with Id {Id} not exits.");
            }
            
            var result = await _unitOfWork.AddressRepository.DeleteAddressAsync(Id);
            _logger.LogInformation("Address with Id {AddressId} deleted successfully", Id);
            return result;
        }
        
    }
}
