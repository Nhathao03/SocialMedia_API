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
        /// Retrieves a specific post image by its unique ID.
        /// </summary>
        /// <param name="id">The ID of the post image to retrieve.</param>
        /// <returns>
        /// Returns 200 OK with the post image data if found, 
        /// 404 Not Found if the post image does not exist, 
        /// or 500 Internal Server Error if an exception occurs.
        /// </returns>
        [HttpGet("{id:int}")]
        [SwaggerOperation(Summary = "Retrieve a post image by ID", Description = "Retrieves a specific post image using its ID.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPostImageById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid post image ID: {Id}", id);
                    return ApiResponseHelper.BadRequest("Invalid post image ID.");
                }

                var postImage = await _postImageService.GetPostImageByIdAsync(id);
                if (postImage == null)
                {
                    _logger.LogInformation("Post image with ID {Id} not found.", id);
                    return ApiResponseHelper.NotFound("Post image not found.");
                }

                _logger.LogInformation("Post image with ID {Id} retrieved successfully.", id);
                return ApiResponseHelper.Success(postImage, "Post image retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving post image with ID {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the post image.");
            }
        }

        /// <summary>
        /// Updates an existing post image.
        /// </summary>
        /// <param name="dto">The data transfer object containing the updated post image information.</param>
        /// <returns>
        /// Returns 200 OK if the image is successfully updated, 
        /// 400 Bad Request for invalid data, 
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
                _logger.LogWarning("Invalid PostImageDTO model state.");
                return BadRequest(ModelState);
            }

            try
            {
                await _postImageService.UpdatePostImageAsync(dto);
                return ApiResponseHelper.Success("Post image updated successfully.");
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the post image.");
            }
        }

        /// <summary>
        /// Deletes a post image by its unique ID.
        /// </summary>
        /// <param name="id">The ID of the post image to delete.</param>
        /// <returns>
        /// Returns 200 OK if deletion is successful, 
        /// 404 Not Found if the image does not exist, 
        /// or 500 Internal Server Error if an exception occurs.
        /// </returns>
        [HttpDelete("{id:int}")]
        [SwaggerOperation(Summary = "Delete a post image", Description = "Deletes a specific post image by its Id.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePostImage(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid post image ID: {Id}", id);
                return ApiResponseHelper.BadRequest("Invalid post image ID.");
            }

            try
            {
                var deleted = await _postImageService.DeletePostImageAsync(id);
                if (!deleted)
                {
                    _logger.LogInformation("Post image with ID {Id} not found for deletion.", id);
                    return ApiResponseHelper.NotFound("Post image not found.");
                }

                return ApiResponseHelper.Success(deleted, "Post image deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting post image with ID {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the post image.");
            }
        }

        /// <summary>
        /// Retrieves all post images associated with a specific post ID.
        /// </summary>
        /// <param name="postId">The ID of the post to retrieve images for.</param>
        /// <returns>
        /// Returns 200 OK with a list of images, 
        /// 404 Not Found if no images exist, 
        /// or 500 Internal Server Error if an exception occurs.
        /// </returns>
        [HttpGet("byPost/{postId:int}")]
        [SwaggerOperation(Summary = "Retrieve post images by Post ID", Description = "Retrieves all post images associated with a specific post ID.")]
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
                    _logger.LogInformation("No post images found for Post ID {PostId}", postId);
                    return ApiResponseHelper.NotFound("No post images found for the specified post ID.");
                }

                return ApiResponseHelper.Success(postImages, "Post images retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving post images for Post ID {PostId}", postId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving post images.");
            }
        }

        /// <summary>
        /// Retrieves all post images uploaded by a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose post images are being retrieved.</param>
        /// <returns>
        /// Returns 200 OK with a list of user’s post images, 
        /// 404 Not Found if none are found, 
        /// or 500 Internal Server Error if an exception occurs.
        /// </returns>
        //[HttpGet("byUser/{userId}")]
        //[SwaggerOperation(Summary = "Retrieve post images by User ID", Description = "Retrieves all post images associated with a specific user ID.")]
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
        //            _logger.LogInformation("No post images found for User ID {UserId}", userId);
        //            return ApiResponseHelper.NotFound("No post images found for the specified user ID.");
        //        }

        //        return ApiResponseHelper.Success(postImages, "Post images retrieved successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error retrieving post images for User ID {UserId}", userId);
        //        return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving post images.");
        //    }
        //}
    }
}
