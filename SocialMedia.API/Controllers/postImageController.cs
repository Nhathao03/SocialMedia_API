using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.Services;
using SocialMedia.Core.Entities.DTO;
using SocialMedia.Core.Entities;
using SocialMedia.Core.DTO.Post;
using Swashbuckle.AspNetCore.Annotations;
using Social_Media.Helpers;

namespace Social_Media.Controllers
{
    [Route("api/postImage")]
    [ApiController]
    public class postImageController : ControllerBase
    {
        private readonly IPostImageService _postImageService;
        private readonly ILogger<postImageController> _logger;

        public postImageController(IPostImageService postImageService, ILogger<postImageController> logger)
        {
            _postImageService = postImageService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a specific post image by its unique Id.
        /// </summary>
        /// <param name="Id">The Id of the post image to retrieve.</param>
        /// <returns>
        /// Returns 200 OK with the post image data if found, 
        /// 404 Not Found if the post image does not exist, 
        /// or 500 Internal Server Error if an exception occurs.
        /// </returns>
        [HttpGet("{Id:int}")]
        [SwaggerOperation(Summary = "Retrieve a post image by Id", Description = "Retrieves a specific post image using its Id.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPostImageById(int Id)
        {
            try
            {
                if (Id <= 0)
                {
                    _logger.LogWarning("InvalId post image Id: {Id}", Id);
                    return ApiResponseHelper.BadRequest("InvalId post image Id.");
                }

                var postImage = await _postImageService.GetPostImageByIdAsync(Id);
                if (postImage == null)
                {
                    _logger.LogInformation("Post image with Id {Id} not found.", Id);
                    return ApiResponseHelper.NotFound("Post image not found.");
                }

                _logger.LogInformation("Post image with Id {Id} retrieved successfully.", Id);
                return ApiResponseHelper.Success(postImage, "Post image retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving post image with Id {Id}", Id);
                return StatusCode(500, $"Error: {ex.InnerException?.Message}");
            }
        }

        /// <summary>
        /// Updates an existing post image.
        /// </summary>
        /// <param name="dto">The data transfer object containing the updated post image information.</param>
        /// <returns>
        /// Returns 200 OK if the image is successfully updated, 
        /// 400 Bad Request for invalId data, 
        /// or 500 Internal Server Error if an exception occurs.
        /// </returns>
        [HttpPut]
        [SwaggerOperation(Summary = "Update an existing post image", Description = "Updates the details of an existing post image.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePostImage([FromBody] PostImageDTO dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("PostImageDTO is null.");
                return ApiResponseHelper.BadRequest("Post image data is required.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("InvalId PostImageDTO model state.");
                return BadRequest(ModelState);
            }

            try
            {
                await _postImageService.UpdatePostImageAsync(dto);
                return ApiResponseHelper.Success("Post image updated successfully.");
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Error: {ex.InnerException?.Message}");
            }
        }

        /// <summary>
        /// Deletes a post image by its unique Id.
        /// </summary>
        /// <param name="Id">The Id of the post image to delete.</param>
        /// <returns>
        /// Returns 200 OK if deletion is successful, 
        /// 404 Not Found if the image does not exist, 
        /// or 500 Internal Server Error if an exception occurs.
        /// </returns>
        [HttpDelete("{Id:int}")]
        [SwaggerOperation(Summary = "Delete a post image", Description = "Deletes a specific post image by its Id.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePostImage(int Id)
        {
            if (Id <= 0)
            {
                _logger.LogWarning("InvalId post image Id: {Id}", Id);
                return ApiResponseHelper.BadRequest("InvalId post image Id.");
            }

            try
            {
                var deleted = await _postImageService.DeletePostImageAsync(Id);
                if (!deleted)
                {
                    _logger.LogInformation("Post image with Id {Id} not found for deletion.", Id);
                    return ApiResponseHelper.NotFound("Post image not found.");
                }

                return ApiResponseHelper.Success(deleted, "Post image deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting post image with Id {Id}", Id);
                return StatusCode(500, $"Error: {ex.InnerException?.Message}");
            }
        }

        /// <summary>
        /// Retrieves all post images associated with a specific post Id.
        /// </summary>
        /// <param name="postId">The Id of the post to retrieve images for.</param>
        /// <returns>
        /// Returns 200 OK with a list of images, 
        /// 404 Not Found if no images exist, 
        /// or 500 Internal Server Error if an exception occurs.
        /// </returns>
        [HttpGet("byPost/{postId:int}")]
        [SwaggerOperation(Summary = "Retrieve post images by Post Id", Description = "Retrieves all post images associated with a specific post Id.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPostImagesByPostId(int postId)
        {
            try
            {
                var postImages = await _postImageService.GetPostImagesByPostIdAsync(postId);
                if (postImages == null || !postImages.Any())
                {
                    _logger.LogInformation("No post images found for Post Id {PostId}", postId);
                    return ApiResponseHelper.NotFound("No post images found for the specified post Id.");
                }

                return ApiResponseHelper.Success(postImages, "Post images retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving post images for Post Id {PostId}", postId);
                return StatusCode(500, $"Error: {ex.InnerException?.Message}");
            }
        }

        /// <summary>
        /// Retrieves all post images uploaded by a specific user.
        /// </summary>
        /// <param name="userId">The Id of the user whose post images are being retrieved.</param>
        /// <returns>
        /// Returns 200 OK with a list of user’s post images, 
        /// 404 Not Found if none are found, 
        /// or 500 Internal Server Error if an exception occurs.
        /// </returns>
        //[HttpGet("byUser/{userId}")]
        //[SwaggerOperation(Summary = "Retrieve post images by User Id", Description = "Retrieves all post images associated with a specific user Id.")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> GetPostImagesByUserId(string userId)
        //{
        //    try
        //    {
        //        var postImages = await _postImageService.GetPostImagesByUserIdAsync(userId);
        //        if (postImages == null || !postImages.Any())
        //        {
        //            _logger.LogInformation("No post images found for User Id {UserId}", userId);
        //            return ApiResponseHelper.NotFound("No post images found for the specified user Id.");
        //        }

        //        return ApiResponseHelper.Success(postImages, "Post images retrieved successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error retrieving post images for User Id {UserId}", userId);
        //        return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving post images.");
        //    }
        //}
    }
}
