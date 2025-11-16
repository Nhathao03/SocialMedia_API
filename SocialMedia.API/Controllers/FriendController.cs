using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.Services;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Entities.DTO;
using Swashbuckle.AspNetCore.Annotations;
using Social_Media.Helpers;
using SocialMedia.Core.DTO.Friend;

namespace Social_Media.Controllers
{
    [Route("api/friends")]
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
        /// Get all friends of a user
        /// </summary>
        /// <param name="userId">The unique Id of the user</param>
        /// <returns>List friends of a user</returns>
        /// <response code="200">List friends retrieved successfully.</response>
        /// <response code="400">InvalId Id provIded.</response>
        /// <response code="404">Friend not found.</response>
        [HttpGet("{userId}")]
        [SwaggerOperation(Summary = "Get all friends of a user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFriends(string userId)
        {
            _logger.LogInformation("Getting list friend of each user with Id {userId}", userId);
            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogWarning("InvalId user Id provIded.");
                return ApiResponseHelper.BadRequest("InvalId user Id.");
            }
            try
            {
                var friends = await _friendsService.GetFriendOfEachUserAsync(userId);
                if (friends is null || !friends.Any())
                {
                    _logger.LogInformation("No friends found for user with Id {userId}", userId);
                    return ApiResponseHelper.NotFound("No friends found.");
                }
                _logger.LogInformation("List friends retrieved successfully for user with Id {userId}", userId);
                return ApiResponseHelper.Success(friends, "Get friends successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving friends for user with Id {userId}", userId);
                return StatusCode(500, $"Error: {ex.InnerException?.Message}");
			}
        }

        /// <summary>
        /// Retrieves list friend recantly added for a user.
        /// </summary>
        /// <param name="Id">The unique Id of the user.</param>
        /// <returns>Returns list object if found.</returns>
        /// <response code="200">List friends retrieved successfully.</response>
        /// <response code="400">InvalId Id provIded.</response>
        /// <response code="404">Friend not found.</response>
        [HttpGet("recently-added/{Id}")]
        [SwaggerOperation(Summary = "Get list friend recently added for a user", Description = "Retrieves list friend recently added for a user by their unique Id.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFriendRecentlyAddedAsync([FromRoute] string Id)
        {
            _logger.LogInformation("Getting list friend recently added for user with Id {UserId}", Id);
            if (string.IsNullOrWhiteSpace(Id))
            {
                _logger.LogWarning("InvalId user Id provIded.");
                return ApiResponseHelper.BadRequest("InvalId user Id.");
            }
            try
            {
                var friends = await _friendsService.GetFriendRecentlyAddedAsync(Id);
                if (friends is null || !friends.Any())
                {
                    _logger.LogInformation("No friends found for user with Id {UserId}", Id);
                    return ApiResponseHelper.NotFound("No friends found.");
                }
                _logger.LogInformation("List friends retrieved successfully for user with Id {UserId}", Id);
                return ApiResponseHelper.Success(friends, "Get friends recently add successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving recently added friends for user with Id {UserId}", Id);
                return StatusCode(500, $"Error: {ex.InnerException?.Message}");
			}
        }

        /// <summary>
        /// Retrieves list friend base on hometown
        /// </summary>
        /// <param name="Id">The unique Id of the user.</param>
        /// <returns>Returns list object if found.</returns>
        /// <response code="200">List friends retrieved successfully.</response>
        /// <response code="400">InvalId Id provIded.</response>
        /// <response code="404">Friend not found.</response>
        [HttpGet("hometown/{Id}")]
        [SwaggerOperation(Summary = "Get list friend base on hometown for a user", Description = "Retrieves list friend base on hometown for a user by their unique Id.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFriendBaseOnHomeTown([FromRoute] string Id)
        {
            _logger.LogInformation("Getting list friend base on hometown with Id {UserId}", Id);
            if (string.IsNullOrWhiteSpace(Id))
            {
                _logger.LogWarning("InvalId user Id provIded.");
                return ApiResponseHelper.BadRequest("InvalId user Id.");
            }
            try
            {
                var friends = await _friendsService.GetFriendBaseOnHomeTownAsync(Id);
                if (friends is null || !friends.Any())
                {
                    _logger.LogInformation("No friends found for user with Id {UserId}", Id);
                    return ApiResponseHelper.NotFound("No friends found.");
                }
                _logger.LogInformation("List friends retrieved successfully for user with Id {UserId}", Id);
                return ApiResponseHelper.Success(friends, "Get friends base on hometown successfully.");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving friends based on hometown for user with Id {UserId}", Id);
                return StatusCode(500, $"Error: {ex.InnerException?.Message}");
			}
        }

        /// <summary>
        /// Check friend ship
        /// </summary>
        /// <param name="userId">The unique userId of the user</param>
        /// <param name="targetUserId">The unique userId of the user</param>
        /// <returns>Status friend ship of user</returns>
        /// <response code="200">Status retrieved successfully.</response>
        /// <response code="400">InvalId Id provIded.</response>
        [HttpGet("check-friendship/{userId}/{targetUserId}")]
        [SwaggerOperation(Summary = "Check friend ship between two user", Description = "Retrives the status base their unique of userId and targetUserId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CheckFriendshipAsync(string userId, string targetUserId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogWarning("InvalId input data");
                return ApiResponseHelper.BadRequest("InvalId input data.");
            }
            try
            {
                var status = await _friendsService.CheckFriendshipAsync(userId, targetUserId);
                _logger.LogInformation($"Status: {status}");
                switch (status)
                {
                    case FriendShipStatus.Friends:
                        return ApiResponseHelper.Success(status, "Friends");
                    case FriendShipStatus.Pending:
                        return ApiResponseHelper.Success(status, "Pending");
                    case FriendShipStatus.Rejected:
                        return ApiResponseHelper.Success(status, "Reject");
                    default:
                        return ApiResponseHelper.Success(status, "None");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking friendship between userId {UserId} and targetUserId {TargetUserId}", userId, targetUserId);
                return StatusCode(500, $"Error: {ex.InnerException?.Message}");
			}
        }

        /// <summary>
        /// Unfriend 
        /// </summary>
        /// <param name="userAId">The unique Id of the user</param>
        /// <param name="userBId">The unique Id of the user</param>
        /// <response code="200">Unfriend successfully.</response>
        /// <response code="400">InvalId Id provIded.</response>
        [HttpDelete("unfriend/{userAId}/{userBId}")]
        [SwaggerOperation(Summary = "Unfriend base on userAId and userBId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Unfriend(string userAId, string userBId)
        {
            _logger.LogInformation("Unfriend based on userAId {userAId} and userBId {userBId}", userAId, userBId);
            if (string.IsNullOrWhiteSpace(userAId) || string.IsNullOrWhiteSpace(userBId))
            {
                _logger.LogWarning("InvalId input data");
                return ApiResponseHelper.BadRequest("InvalId input data");
            }
            try
            {
                var result = await _friendsService.DeleteFriendsAsync(userAId, userBId);
                _logger.LogInformation("Unfriend successfully.");
                return ApiResponseHelper.Success("Unfriend successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while unfriending between userAId {UserAId} and userBId {UserBId}", userAId, userBId);
                return StatusCode(500, $"Error: {ex.InnerException?.Message}");
			}
        }
    }
}
