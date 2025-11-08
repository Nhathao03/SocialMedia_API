using Microsoft.AspNetCore.Mvc;
using Social_Media.Helpers;
using SocialMedia.Core.DTO.Post;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Entities.Entity;
using SocialMedia.Core.Entities.PostEntity;
using SocialMedia.Core.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Http.Headers;

namespace Social_Media.Controllers
{
    [Route("api/like")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly ILikeService _likeService;
        private readonly ILogger<LikeController> _logger;   

        public LikeController(ILikeService likeService,
            ILogger<LikeController> logger)
        {
            _likeService = likeService;
            _logger = logger;
        }

        /// <summary>
        /// Add reaction like to entity
        /// </summary>
        /// <param name="dto">The like data to add</param>
        /// <response code="201">Reacted successfully</response>
        /// <response code="400">Invalid input data.</response>
        /// <response code="404">Already reacted.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddReact(LikeDTO dto)
        {
            _logger.LogInformation("Add reaction to entity");
            if (string.IsNullOrWhiteSpace(dto.UserId) || dto.EntityId <= 0)
            {
                _logger.LogWarning("Invalid input data");
                return ApiResponseHelper.BadRequest("Invalid input data");
            }
            try
            {
                await _likeService.AddReactionAsync(dto);
                return ApiResponseHelper.Created("Entity reacted");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Already reacted");
                return ApiResponseHelper.BadRequest("Already reacted");
            }
        }

        /// <summary>
        /// Unlike entity
        /// </summary>
        /// <param name="dto">The like data to remove</param>
        /// <response code="200">Unliked entity successfully</response>
        /// <response code="400">Invalid input data.</response>
        /// <response code="404">Not react yet.</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UnlikePost(LikeDTO dto)
        {
            _logger.LogInformation("Unlike post");
            if (string.IsNullOrWhiteSpace(dto.UserId) || dto.EntityId <= 0)
            {
                _logger.LogWarning("Invalid input data");
                return ApiResponseHelper.BadRequest("Invalid input data");
            }
            try
            {
                await _likeService.RemoveReactionAsync(dto);
                return ApiResponseHelper.Success("Post unliked");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Not react yet");
                return ApiResponseHelper.BadRequest("Not react yet");
            }
        }

        /// <summary>
        /// Toggle entity
        /// </summary>
        /// <param name="dto">The like data to toggle</param>
        /// <response code="200">Toggle entity successfully</response>
        /// <response code="400">Invalid input data.</response>
        [HttpPost("toggle")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ToggleReactEntity(LikeDTO dto)
        {
            _logger.LogInformation("Toggle post");
            if (string.IsNullOrWhiteSpace(dto.UserId) || dto.EntityId <= 0)
            {
                _logger.LogWarning("Invalid input data");
                return ApiResponseHelper.BadRequest("Invalid input data");
            }
            try
            {
                var result = await _likeService.ToggleReactionAsync(dto);
                return ApiResponseHelper.Success(new { Status = result ? "Reacted" : "Unliked" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling reaction");
                return ApiResponseHelper.InternalServerError("Error toggling reaction");
            }
        }

        /// <summary>
        /// React count post
        /// </summary>
        /// <param name="entityId">The unique id of the entity</param>
        /// <param name="entityTypeEnum">Type of entity</param>
        /// <response code="200">Retrives like count entity successfully</response>
        /// <response code="400">Invalid input data.</response>
        [HttpGet("count/{entityId:int}")]
        [SwaggerOperation(Summary = "Get like count entity based on LikeDTO")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEntityCount(int entityId, EntityTypeEnum entityTypeEnum)
        {
            _logger.LogInformation("Get post like count");
            if (entityId <= 0)
            {
                _logger.LogWarning("Invalid input data");
                return ApiResponseHelper.BadRequest("Invalid input data");
            }
            try
            {
                var count = await _likeService.GetReactionCountAsync(entityId, entityTypeEnum);
                return ApiResponseHelper.Success(new { Count = count });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving like count");
                return ApiResponseHelper.InternalServerError("Error retrieving like count");
            }
        }

        /// <summary>
        /// Get entity likers
        /// </summary>
        /// <param name="dto">The like data to get users</param>
        /// <response code="200">Retrives entity likers successfully</response>
        /// <response code="400">Invalid input data.</response>
        [HttpGet("users/{entityId:int}")]
        [SwaggerOperation(Summary = "Get entity liker based on entityId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEntityLikers(int entityId, EntityTypeEnum entity)
        {
            _logger.LogInformation("Get post likers");
            if (entityId <= 0)
            {
                _logger.LogWarning("Invalid input data");
                return ApiResponseHelper.BadRequest("Invalid input data");
            }
            try
            {
                var users = await _likeService.GetUsersReactionAsync(entityId, entity);
                return ApiResponseHelper.Success(users, "Retrives entity likers");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving likers");
                return ApiResponseHelper.InternalServerError("Error retrieving likers");
            }
        }
    }
}
