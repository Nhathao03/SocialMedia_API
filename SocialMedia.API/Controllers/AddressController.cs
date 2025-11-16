using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Social_Media.Helpers;
using SocialMedia.Core.DTO.Account;
using SocialMedia.Core.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
namespace Social_Media.Controllers
{
    [Route("api/address")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly ILogger<AddressController> _logger;
        public AddressController(IAddressService addressService, ILogger<AddressController> logger)
        {
            _addressService = addressService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all addresses.
        /// </summary>
        /// <returns>A list of all addresses.</returns>
        /// <response code="200">Addresses retrieved successfully.</response>
        /// <response code="404">No addresses found.</response>
        /// <response code="500">An unexpected error occurred.</response>
        [HttpGet]
        [SwaggerOperation(Summary = "Retrieve all addresses", Description = "Retrieves a list of all stored user addresses.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAddressesAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all addresses...");

                var addresses = await _addressService.GetAllAddressAsync();

                if (addresses == null || !addresses.Any())
                {
                    _logger.LogInformation("No addresses found in the database.");
                    return ApiResponseHelper.NotFound("No addresses found.");
                }

                _logger.LogInformation("Retrieved {Count} addresses successfully.", addresses.Count());
                return ApiResponseHelper.Success(addresses, "Addresses retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all addresses.");
                return StatusCode(500, $"Error: {ex.InnerException?.Message}");
            }
        }


        /// <summary>
        /// Retrives a address by its unique Identifier.
        /// </summary>
        /// <param name="Id">The unique Id of the address.</param>
        /// <returns>Return the address object if found.</returns>
        /// <response code="200">Address retrieved successfully.</response>
        /// <response code="404">Address not found</response>
        /// <response code="400">InvalId Id provIded</response>
        [HttpGet("{Id:int}")]
        [SwaggerOperation(Summary = "Retrieve address by Id", Description = "Retrieves a specific address using its Id.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAddressByIdAsync(int Id)
        {
            try
            {
                if(Id <= 0)
                {
                    _logger.LogWarning("InvalId address Id: {Id}", Id);
                    return ApiResponseHelper.BadRequest("InvalId address Id. Id must be greater than zero");
                }

                var address = await _addressService.GetAddressByIdAsync(Id);
                if (address is null)
                {
                    _logger.LogInformation("Address not found. Id: {Id}", Id);
                    return ApiResponseHelper.NotFound("Address not found");
                }

                _logger.LogInformation("Address retrived successfully. Id: {Id}", Id);
                return ApiResponseHelper.Success(address, "Address retrieved successfully");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving address with Id: {Id}", Id);
                return StatusCode(500, $"Error: {ex.InnerException?.Message}");
            }
        }

        /// <summary>
        /// Add a new address.
        /// </summary>
        /// <param name="dto">The address data transfer object containing content and metadata.</param>
        /// <returns>Returns the created address object or valIdation errors</returns>
        /// <response code="201">Address added successfully</response>
        /// <response code="400">InvalId input data.</response>
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Add new address", Description = "Adds a new address for the authenticated user.")]
        public async Task<IActionResult> CreateNewAddressAsync([FromBody] AddressDTO dto)
        {
            if(dto is null)
            {
                _logger.LogWarning("Received null address DTO in CreateNewAddressAsync");
                return ApiResponseHelper.BadRequest("Request body cannot be null");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("InvalId model state while create address: {@ModelState}", ModelState);
                return ApiResponseHelper.BadRequest("InvalId address data.");
            }

            try
            {
                var createdAddress = await _addressService.AddAddressAsync(dto);

                if (createdAddress is null)
                {
                    _logger.LogWarning("Failed to create address for DTO: {@Dto}", dto);
                    return ApiResponseHelper.BadRequest("Failed to create address.");
                }

                _logger.LogInformation("Address created successfully with Id: {Id}", createdAddress.Id);
                return ApiResponseHelper.Created(createdAddress, "Address created successfully.");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating address.");
                return StatusCode(500, $"Error: {ex.InnerException?.Message}");
            }
        }

        /// <summary>
        /// Updates an existing address by its Id.
        /// </summary>
        /// <param name="Id">The Id of the address to update.</param>
        /// <param name="modelDto">The updated address data.</param>
        /// <returns>Updated address information.</returns>
        /// <response code="200">address updated successfully.</response>
        /// <response code="400">InvalId input data.</response>
        /// <response code="404">address not found.</response>
        [HttpPut("{Id:int}")]
        [SwaggerOperation(Summary = "Update a address by Id", Description = "Updates the content of an existing address.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateaddressAsync(int Id, [FromBody] AddressDTO modelDto)
        {
            if (Id <= 0)
            {
                _logger.LogWarning("InvalId address Id for update: {Id}", Id);
                return ApiResponseHelper.BadRequest("InvalId address Id. Id must be greater than zero.");
            }

            if (modelDto is null)
            {
                _logger.LogWarning("Null address DTO provIded for update. Id: {Id}", Id);
                return ApiResponseHelper.BadRequest("Request body cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("InvalId model state for address update. Id: {Id}", Id);
                return ApiResponseHelper.BadRequest("InvalId address data.");
            }

            try
            {
                var updatedaddress = await _addressService.UpdateAddressAsync(Id, modelDto);
                if (updatedaddress is null)
                {
                    _logger.LogInformation("address not found for update. Id: {Id}", Id);
                    return ApiResponseHelper.NotFound("address not found.");
                }

                _logger.LogInformation("address updated successfully. Id: {Id}", Id);
                return ApiResponseHelper.Success(updatedaddress, "address updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating address. Id: {Id}", Id);
                return StatusCode(500, $"Error: {ex.InnerException?.Message}");
            }
        }

        /// <summary>
        /// Deletes a address by its Id.
        /// </summary>
        /// <param name="Id">The Id of the address to delete.</param>
        /// <returns>No content if deletion succeeds.</returns>
        /// <response code="204">address deleted successfully.</response>
        /// <response code="400">InvalId Id provIded.</response>
        /// <response code="404">address not found.</response>
        [HttpDelete("{Id:int}")]
        [SwaggerOperation(Summary = "Delete a address by Id", Description = "Deletes a specific address using its Id.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteaddressByIdAsync(int Id)
        {
            if (Id <= 0)
            {
                _logger.LogWarning("InvalId Id provIded for deletion: {Id}", Id);
                return ApiResponseHelper.BadRequest("InvalId address Id. Id must be greater than zero.");
            }

            try
            {
                var deleted = await _addressService.DeleteAddressAsync(Id);
                _logger.LogInformation("address deleted successfully. Id: {Id}", Id);
                return ApiResponseHelper.Success("Address delete successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting address. Id: {Id}", Id);
                return StatusCode(500, $"Error: {ex.InnerException?.Message}");
            }
        }
    }
} 
