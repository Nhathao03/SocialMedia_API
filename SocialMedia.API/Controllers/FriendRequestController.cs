using Microsoft.AspNetCore.Mvc;
using Social_Media.Helpers;
using SocialMedia.Core.DTO.Friend;
using SocialMedia.Core.Entities.FriendEntity;
using SocialMedia.Core.Services;
using Swashbuckle.AspNetCore.Annotations;
using static Social_Media.Helpers.Constants;
namespace Social_Media.Controllers
{
    [Route("api/friendrequest")]
    public class FriendRequestController : ControllerBase
    {
        private readonly IFriendRequestService _friendRequestService;
        private readonly ILogger _logger;

        public FriendRequestController(IFriendRequestService friendRequestService,
            ILogger<FriendController> logger)
        {
            _friendRequestService = friendRequestService;
            _logger = logger;
        }

        /// <summary>
        /// Add friend request
        /// </summary>
        /// <param name="dto">The friend request data transfer object containing content and metadata. </param>
        /// <returns>Returns the created friend request object or validation errors</returns>
        /// <response code="201">Friend request created successfully.</response>
        /// <response code="400">Invalid input data.</response>
        /// <response code="404">Friend request not found.</response>
        [HttpPost]
        public async Task<IActionResult> SendFriendRequestAsync([FromBody] FriendRequestDTO dto)
        {
            if (dto is null)
            {
                _logger.LogWarning("Received null friend request DTO in AddFriendRequestAsync.");
                return ApiResponseHelper.BadRequest("Request body cannot be null.");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state while creating friend request: {@ModelState}", ModelState);
                return ApiResponseHelper.BadRequest("Invalid frined request data.");
            }

            try
            {
                var friendrequest = await _friendRequestService.SendFriendRequestAsync(dto);
                if (friendrequest is null)
                {
                    _logger.LogWarning("Failed to create friend request for DTO: {@Dto}", dto);
                    return ApiResponseHelper.NotFound("Friend request not found");
                }

                _logger.LogInformation("Friend request created successfully with ID: {Id}", friendrequest.Id);
                switch (friendrequest.status)
                {
                    case (int)Constants.FriendRequestStatus.Pending:
                        return ApiResponseHelper.Created(friendrequest, "Friend request created successfully.");

                    case (int)Constants.FriendRequestStatus.Accepted:
                        return ApiResponseHelper.Created(friendrequest, "Both users are now friends.");

                    case (int)Constants.FriendRequestStatus.Blocked:
                        return ApiResponseHelper.BadRequest("Cannot send friend request because one user is blocked.");

                    case (int)Constants.FriendRequestStatus.Canceled:
                        return ApiResponseHelper.Created(friendrequest, "Friend request was cancelled.");

                    default:
                        _logger.LogWarning("Unknown friend request status: {Status}", friendrequest.status);
                        return ApiResponseHelper.Success(friendrequest, "Friend request processed with unknown status.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating comment.");
                return ApiResponseHelper.InternalServerError("An unexpected error occurred while creating the comment.");
            }
        }

        /// <summary>
        /// Add friend 
        /// </summary>
        /// <param name="id">The friend data transfer object containing content and metadata.</param>
        /// <returns>Returns the created friend object or validation errors.</returns>
        /// <response code="200">Add a new friend successfully.</response>
        /// <response code="400">Invalid ID provided.</response>
        /// <response code="404">Friend not found.</response>
        [HttpPost("{id:int}")]
        [SwaggerOperation(Summary = "Add a new friend", Description = "Adds a new friend based on the provided friend request ID.")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AcceptFriendRequestAsync([FromRoute] int id)
        {
            _logger.LogInformation("Adding a new friend with Friend Request ID {FriendRequestId}", id);
            if (id <= 0)
            {
                _logger.LogWarning("Invalid Friend Request ID provided.");
                return ApiResponseHelper.BadRequest("Invalid Friend Request ID.");
            }
            try
            {
                var result = await _friendRequestService.AcceptFriendRequestAsync(id);
                _logger.LogInformation("Friend added successfully with ID {FriendId}", result?.Id);
                return ApiResponseHelper.Success(result, "Friend added successfully.");
            }
            catch (KeyNotFoundException knfEx)
            {
                _logger.LogWarning(knfEx, "Friend Request with ID {FriendRequestId} not found.", id);
                return ApiResponseHelper.NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding friend with Friend Request ID {FriendRequestId}", id);
                return ApiResponseHelper.InternalServerError("An unexpected error occurred while adding the friend.");
            }
        }

        /// <summary>
        /// Reject friend request
        /// </summary>
        /// <param name="id">The unique ID of the friend to reject</param>
        /// <response code="200">Reject friend request successfully.</response>
        /// <response code="400">Invalid ID provided.</response>
        [HttpPut("reject-friend-request/{id:int}")]
        [SwaggerOperation(Summary = "Reject friend request existing", Description = "Reject an existing friend request based on the provided Id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RejectFriendRequestAsync(int id)
        {
            _logger.LogInformation("Reject friend request with ID {FriendRequestId}", id);
            if (id <= 0)
            {
                _logger.LogWarning("Invalid Friend Request ID provided.");
                return ApiResponseHelper.BadRequest("Invalid Friend Request ID.");
            }
            try
            {
                await _friendRequestService.RejectFriendRequestAsync(id);
                _logger.LogInformation("Reject friend request successfully with ID {FriendId}", id);
                return ApiResponseHelper.Success(id, "Friend request reject successfully.");
            }
            catch (KeyNotFoundException knfEx)
            {
                _logger.LogWarning(knfEx, "Friend Request with ID {FriendRequestId} not found.", id);
                return ApiResponseHelper.NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reject friend request with Friend Request ID {FriendRequestId}", id);
                return ApiResponseHelper.InternalServerError("An unexpected error occurred while reject the friend request.");
            }
        }

        /// <summary>
        /// Cancel friend request
        /// </summary>
        /// <param name="id">The unique ID of the friend to reject</param>
        /// <response code="200">Reject friend request successfully.</response>
        /// <response code="400">Invalid ID provided.</response>
        [HttpPut("cancel-friend-request/{id:int}")]
        [SwaggerOperation(Summary = "Cancel friend request existing", Description = "Cancel an existing friend request based on the provided Id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelFriendRequestAsync(int id)
        {
            _logger.LogInformation("Cancel friend request with ID {FriendRequestId}", id);
            if (id <= 0)
            {
                _logger.LogWarning("Invalid Friend Request ID provided.");
                return ApiResponseHelper.BadRequest("Invalid Friend Request ID.");
            }
            try
            {
                await _friendRequestService.CancelFriendRequestAsync(id);
                _logger.LogInformation("Friend canceled successfully with ID {FriendId}", id);
                return ApiResponseHelper.Success(id, "Friend canceled successfully.");
            }
            catch (KeyNotFoundException knfEx)
            {
                _logger.LogWarning(knfEx, "Friend Request with ID {FriendRequestId} not found.", id);
                return ApiResponseHelper.NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancel friend request with Friend Request ID {FriendRequestId}", id);
                return ApiResponseHelper.InternalServerError("An unexpected error occurred while cancel the friend request.");
            }
        }

        /// <summary>
        /// Get sent request
        /// </summary>
        /// <param name="userId">The unique ID of the friend request</param>
        /// <returns>List of friend requests sent by current user</returns>
        /// <response code="200">Retrived request successfully.</response>
        /// <response code="404">Not request found.</response>
        /// <response code="400">Invalid input data.</response>
        [HttpGet("sent")]
        [SwaggerOperation(Summary = "List of friend requests sent by current user")]
        public async Task<IActionResult> GetSentRequests([FromQuery] string userId)
        {
            try
            {
                if (userId is null)
                {
                    _logger.LogWarning("Invalid userId: {userId}", userId);
                    return ApiResponseHelper.BadRequest("Invalid userId.");
                }
                var requests = await _friendRequestService.GetSentRequestAsync(userId);
                if (requests == null)
                {
                    _logger.LogInformation("Requests with ID {userId} not found.", userId);
                    return ApiResponseHelper.NotFound("Requests not found.");
                }
                _logger.LogInformation("Request with ID {userId} retrieved successfully.", userId);
                return ApiResponseHelper.Success(requests, "Request retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving requests with userId {userId}", userId);
                return ApiResponseHelper.InternalServerError("An error occurred while retrieving the category.");
            }
        }

        /// <summary>
        /// Get received requests
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List of friend requests received by current user</returns>
        /// <response code="200">Retrived request successfully.</response>
        /// <response code="404">Not request found.</response>
        /// <response code="400">Invalid input data.</response>
        [HttpGet("received")]
        [SwaggerOperation(Summary = "List of friend requests received by current user")]
        public async Task<IActionResult> GetReceivedRequests([FromQuery] string userId)
        {
            try
            {
                if (userId is null)
                {
                    _logger.LogWarning("Invalid userId: {userId}", userId);
                    return ApiResponseHelper.BadRequest("Invalid userId.");
                }
                var requests = await _friendRequestService.GetReceivedRequestAsync(userId);
                if (requests == null)
                {
                    _logger.LogInformation("Requests with ID {userId} not found.", userId);
                    return ApiResponseHelper.NotFound("Requests not found.");
                }
                _logger.LogInformation("Request with ID {userId} retrieved successfully.", userId);
                return ApiResponseHelper.Success(requests, "Request retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving requests with userId {userId}", userId);
                return ApiResponseHelper.InternalServerError("An error occurred while retrieving the category.");
            }
        }

    }
}
