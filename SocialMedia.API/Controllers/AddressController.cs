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
                return ApiResponseHelper.InternalServerError("An unexpected error occurred while retrieving addresses.");
            }
        }


        /// <summary>
        /// Retrives a address by its unique identifier.
        /// </summary>
        /// <param name="id">The unique Id of the address.</param>
        /// <returns>Return the address object if found.</returns>
        /// <response code="200">Address retrieved successfully.</response>
        /// <response code="404">Address not found</response>
        /// <response code="400">Invalid ID provided</response>
        [HttpGet("{id:int}")]
        [SwaggerOperation(Summary = "Retrieve address by ID", Description = "Retrieves a specific address using its ID.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAddressByIdAsync(int id)
        {
            try
            {
                if(id <= 0)
                {
                    _logger.LogWarning("Invalid address Id: {Id}", id);
                    return ApiResponseHelper.BadRequest("Invalid address Id. Id must be greater than zero");
                }

                var address = await _addressService.GetAddressByIdAsync(id);
                if (address is null)
                {
                    _logger.LogInformation("Address not found. ID: {Id}", id);
                    return ApiResponseHelper.NotFound("Address not found");
                }

                _logger.LogInformation("Address retrived successfully. ID: {Id}", id);
                return ApiResponseHelper.Success(address, "Address retrieved successfully");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving address with ID: {Id}", id);
                return ApiResponseHelper.InternalServerError("An unexpected error occurred while retrieving the address.");
            }
        }

        /// <summary>
        /// Add a new address.
        /// </summary>
        /// <param name="dto">The address data transfer object containing content and metadata.</param>
        /// <returns>Returns the created address object or validation errors</returns>
        /// <response code="201">Address added successfully</response>
        /// <response code="400">Invalid input data.</response>
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
                _logger.LogWarning("Invalid model state while create address: {@ModelState}", ModelState);
                return ApiResponseHelper.BadRequest("Invalid address data.");
            }

            try
            {
                var createdAddress = await _addressService.AddAddressAsync(dto);

                if (createdAddress is null)
                {
                    _logger.LogWarning("Failed to create address for DTO: {@Dto}", dto);
                    return ApiResponseHelper.BadRequest("Failed to create address.");
                }

                _logger.LogInformation("Address created successfully with ID: {Id}", createdAddress.Id);
                return ApiResponseHelper.Created(createdAddress, "Address created successfully.");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating address.");
                return ApiResponseHelper.InternalServerError("An unexpected error occurred while creating the address.");
            }
        }

        /// <summary>
        /// Updates an existing address by its ID.
        /// </summary>
        /// <param name="id">The ID of the address to update.</param>
        /// <param name="modelDto">The updated address data.</param>
        /// <returns>Updated address information.</returns>
        /// <response code="200">address updated successfully.</response>
        /// <response code="400">Invalid input data.</response>
        /// <response code="404">address not found.</response>
        [HttpPut("{id:int}")]
        [SwaggerOperation(Summary = "Update a address by ID", Description = "Updates the content of an existing address.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateaddressAsync(int id, [FromBody] AddressDTO modelDto)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid address ID for update: {Id}", id);
                return ApiResponseHelper.BadRequest("Invalid address ID. ID must be greater than zero.");
            }

            if (modelDto is null)
            {
                _logger.LogWarning("Null address DTO provided for update. ID: {Id}", id);
                return ApiResponseHelper.BadRequest("Request body cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for address update. ID: {Id}", id);
                return ApiResponseHelper.BadRequest("Invalid address data.");
            }

            try
            {
                var updatedaddress = await _addressService.UpdateAddressAsync(id, modelDto);
                if (updatedaddress is null)
                {
                    _logger.LogInformation("address not found for update. ID: {Id}", id);
                    return ApiResponseHelper.NotFound("address not found.");
                }

                _logger.LogInformation("address updated successfully. ID: {Id}", id);
                return ApiResponseHelper.Success(updatedaddress, "address updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating address. ID: {Id}", id);
                return ApiResponseHelper.InternalServerError("An unexpected error occurred while updating the address.");
            }
        }

        /// <summary>
        /// Deletes a address by its ID.
        /// </summary>
        /// <param name="id">The ID of the address to delete.</param>
        /// <returns>No content if deletion succeeds.</returns>
        /// <response code="204">address deleted successfully.</response>
        /// <response code="400">Invalid ID provided.</response>
        /// <response code="404">address not found.</response>
        [HttpDelete("{id:int}")]
        [SwaggerOperation(Summary = "Delete a address by ID", Description = "Deletes a specific address using its ID.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteaddressByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid ID provided for deletion: {Id}", id);
                return ApiResponseHelper.BadRequest("Invalid address ID. ID must be greater than zero.");
            }

            try
            {
                var deleted = await _addressService.DeleteAddressAsync(id);
                _logger.LogInformation("address deleted successfully. ID: {Id}", id);
                return ApiResponseHelper.Success("Address delete successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting address. ID: {Id}", id);
                return ApiResponseHelper.InternalServerError("An unexpected error occurred while deleting the address.");
            }
        }
    }
} 
