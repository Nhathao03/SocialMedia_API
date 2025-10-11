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
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }
        /// <summary>
        /// Add a new address.
        /// </summary>
        /// <param name="addressDto">The address information to add.</param>
        /// <returns>Newly created address.</returns>
        //[Authorize(Roles = "Admin")]
        [HttpPost("AddAddress")]
        public async Task<IActionResult> AddAddress([FromBody] AddressDTO addressDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _addressService.AddAddressAsync(addressDto);
            return ApiResponseHelper.Success(result, "Address added successfully.");
        }

        /// <summary>
        /// Get all addresses
        /// </summary>
        /// <returns>List of addresses.</returns>
        [HttpGet("GetAllAddresses")]
        [SwaggerOperation(Summary = "Retrieve all user addresses", Description = "Retrieves a list of all addresses.")]
        public async Task<IActionResult> GetAllAddressed()
        {
            var address = await _addressService.GetAllAddressAsync();
            return ApiResponseHelper.Success(address, "Addresses retrieved successfully.");
        }

        /// <summary>
        /// Update an existing address.
        /// </summary>
        /// <param name="addressId">The ID of the address to update.</param>
        /// <param name="addressDto">Updated address information.</param>
        /// <returns>Updated address details.</returns>
        [HttpPut("UpdateAddress")]
        [SwaggerOperation(Summary = "Update address", Description = "Updates the details of an existing address belonging to the authenticated user.")]
        public async Task<IActionResult> UpdateAddress(int addressId, [FromBody] AddressDTO addressDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _addressService.UpdateAddressAsync(addressId, addressDto);
            return ApiResponseHelper.Success(result, "Address updated successfully.");
        }

        /// <summary>
        /// Delete an address.
        /// </summary>
        /// <param name="addressId">The ID of the address to delete.</param>
        /// <returns>Status of the deletion operation.</returns>
        [HttpDelete("{addressId}")]
        [SwaggerOperation(Summary = "Delete address", Description = "Deletes an existing address if it belongs to the authenticated user.")]
        public async Task<IActionResult> DeleteAddress(int addressId)
        {
            var address = await _addressService.GetAddressByIdAsync(addressId);
            if (address == null) return NotFound();
            await _addressService.DeleteAddressAsync(addressId);
            return ApiResponseHelper.Success("Address deleted successfully.");
        }
    }
}
