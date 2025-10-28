using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.Services;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Entities.DTO;
using Swashbuckle.AspNetCore.Annotations;
using Social_Media.Helpers;
using SocialMedia.Core.DTO.Friend;

namespace Social_Media.Controllers
{
    [Route("api/friend")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        private readonly IFriendsService _friendsService;
        private readonly ILogger<FriendController> _logger;
        public FriendController(IFriendsService friendsService, ILogger<FriendController> logger)
        {
            _friendsService = friendsService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves list friend recantly added for a user.
        /// </summary>
        /// <param name="id">The unique ID of the user.</param>
        /// <returns>Returns list object if found.</returns>
        /// <response code="200">List friends retrieved successfully.</response>
        /// <response code="400">Invalid ID provided.</response>
        /// <response code="404">Friend not found.</response>
        [HttpGet("recently-added/{id}")]
        [SwaggerOperation(Summary = "Get list friend recently added for a user", Description = "Retrieves list friend recently added for a user by their unique ID.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFriendRecentlyAddedAsync([FromRoute] string id)
        {
            _logger.LogInformation("Getting list friend recently added for user with ID {UserId}", id);
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("Invalid user ID provided.");
                return ApiResponseHelper.BadRequest("Invalid user ID.");
            }
            var friends = await _friendsService.GetFriendRecentlyAddedAsync(id);
            if (friends is null || !friends.Any())
            {
                _logger.LogInformation("No friends found for user with ID {UserId}", id);
                return ApiResponseHelper.NotFound("No friends found.");
            }
            _logger.LogInformation("List friends retrieved successfully for user with ID {UserId}", id);
            return ApiResponseHelper.Success(friends, "Get friends recently add successfully.");
        }

        /// <summary>
        /// Retrieves list friend base on hometown
        /// </summary>
        /// <param name="id">The unique ID of the user.</param>
        /// <returns>Returns list object if found.</returns>
        /// <response code="200">List friends retrieved successfully.</response>
        /// <response code="400">Invalid ID provided.</response>
        /// <response code="404">Friend not found.</response>
        [HttpGet("hometown/{id}")]
        [SwaggerOperation(Summary = "Get list friend base on hometown for a user", Description = "Retrieves list friend base on hometown for a user by their unique ID.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFriendBaseOnHomeTown([FromRoute] string id)
        {
            _logger.LogInformation("Getting list friend base on hometown with ID {UserId}", id);
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("Invalid user ID provided.");
                return ApiResponseHelper.BadRequest("Invalid user ID.");
            }
            var friends = await _friendsService.GetFriendBaseOnHomeTownAsync(id);
            if (friends is null || !friends.Any())
            {
                _logger.LogInformation("No friends found for user with ID {UserId}", id);
                return ApiResponseHelper.NotFound("No friends found.");
            }
            _logger.LogInformation("List friends retrieved successfully for user with ID {UserId}", id);
            return ApiResponseHelper.Success(friends, "Get friends base on hometown successfully.");
        }

        /// <summary>
        /// Update friend
        /// </summary>  
        /// <param name="id">The unique ID of the friend to update.</param>
        /// <param name="dto">The friend data transfer object containing updated content and metadata.</param>
        /// <returns >Returns the updated friend object or validation errors.</returns>
        /// <response code="200">Friend updated successfully.</response>
        /// <response code="400">Invalid ID provided.</response>
        /// <response code="404">Friend not found.</response>
        [HttpPut("{id:int}")]
        [SwaggerOperation(Summary = "Update an existing friend", Description = "Updates an existing friend based on the provided ID and data.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateFriendsAsync([FromRoute] int id, [FromBody] FriendDTO dto)
        {
            _logger.LogInformation("Updating friend with ID {FriendId}", id);
            if (id <= 0)
            {
                _logger.LogWarning("Invalid Friend ID provided.");
                return ApiResponseHelper.BadRequest("Invalid Friend ID.");
            }
            try
            {
                var result = await _friendsService.UpdateFriendsAsync(id, dto);
                _logger.LogInformation("Friend updated successfully with ID {FriendId}", result?.Id);
                return ApiResponseHelper.Success(result, "Friend updated successfully.");
            }
            catch (KeyNotFoundException knfEx)
            {
                _logger.LogWarning(knfEx, "Friend with ID {FriendId} not found.", id);
                return ApiResponseHelper.NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating friend with ID {FriendId}", id);
                return ApiResponseHelper.InternalServerError("An unexpected error occurred while updating the friend.");
            }
        }

        /// <summary>
        /// Delete friend
        /// </summary>  
        /// <param name="id">The unique ID of the friend to delete.</param>
        /// <return >Returns a success message or validation errors.</returns>
        /// <response code="200">Friend deleted successfully.</response>
        /// <response code="400">Invalid ID provided.</response>
        [HttpDelete("{id:int}")]
        [SwaggerOperation(Summary = "Delete an existing friend", Description = "Deletes an existing friend based on the provided ID.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteFriendByIdAsync([FromRoute] int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid ID provided for deletion: {Id}", id);
                return ApiResponseHelper.BadRequest("Invalid friend ID. ID must be greater than zero.");
            }

            try
            {
                await _friendsService.DeleteFriendsAsync(id);
                _logger.LogInformation("Friend deleted successfully. ID: {Id}", id);
                return ApiResponseHelper.Success("Friend deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting friend. ID: {Id}", id);
                return ApiResponseHelper.InternalServerError("An unexpected error occurred while deleting the friend.");
            }
        }

        /// <summary>
        /// Get all friends of a user
        /// </summary>
        /// <param name="userId">The unique Id of the user</param>
        /// <returns>List friends of a user</returns>
        /// <response code="200">List friends retrieved successfully.</response>
        /// <response code="400">Invalid ID provided.</response>
        /// <response code="404">Friend not found.</response>
        [HttpGet("{userId}")]
        [SwaggerOperation(Summary = "Get all friends of a user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFriends(string userId)
        {
            _logger.LogInformation("Getting list friend of each user with ID {userId}", userId);
            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogWarning("Invalid user ID provided.");
                return ApiResponseHelper.BadRequest("Invalid user ID.");
            }
            var friends = await _friendsService.GetFriendOfEachUserAsync(userId);
            if (friends is null || !friends.Any())
            {
                _logger.LogInformation("No friends found for user with ID {userId}", userId);
                return ApiResponseHelper.NotFound("No friends found.");
            }
            _logger.LogInformation("List friends retrieved successfully for user with ID {UserId}", userId);
            return ApiResponseHelper.Success(friends, "Get friends of each user successfully.");
        }

    }
}
