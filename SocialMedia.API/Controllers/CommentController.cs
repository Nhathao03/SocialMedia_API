using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Hosting;
using SocialMedia.Core.Services;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Entities.DTO.Comment;
using Swashbuckle.AspNetCore.Annotations;
using Social_Media.Helpers;

namespace Social_Media.Controllers
{

    [Route("api/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly ILogger<CommentController> _logger;

        public CommentController(ICommentService commentService, ILogger<CommentController> logger)
        {
            _commentService = commentService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a comment by its unique Identifier.
        /// </summary>
        /// <param name="Id">The unique Id of the comment.</param>
        /// <returns>Returns the comment object if found.</returns>
        /// <response code="200">Comment retrieved successfully.</response>
        /// <response code="400">InvalId Id provIded.</response>
        /// <response code="404">Comment not found.</response>
        [HttpGet("{Id:int}")]
        [SwaggerOperation(
            Summary = "Retrieve a comment by Id",
            Description = "Retrieves a specific comment using its Id."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCommentByIdAsync(int Id)
        {
            try
            {
                if (Id <= 0)
                {
                    _logger.LogWarning("InvalId comment Id: {Id}", Id);
                    return ApiResponseHelper.BadRequest("InvalId comment Id. Id must be greater than zero.");
                }

                var comment = await _commentService.GetCommentByIdAsync(Id);
                if (comment is null)
                {
                    _logger.LogInformation("Comment not found. Id: {Id}", Id);
                    return ApiResponseHelper.NotFound("Comment not found.");
                }

                _logger.LogInformation("Comment retrieved successfully. Id: {Id}", Id);
                return ApiResponseHelper.Success(comment, "Comment retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving comment with Id: {Id}", Id);
                return StatusCode(500, $"Error: {ex.InnerException?.Message}");
			}
        }

        /// <summary>
        /// Retrives comments by Post Id.
        /// </summary>
        /// <param name="Id">The unique Id of the post</param>
        /// <returns>Returns the list commets if found</returns>
        /// <response code="200">Comment retrieved successfully.</response>
        /// <response code="400">Comment not found.</response>
        /// <response code="404">InvalId Id provIded.</response>
        [HttpGet("comment-by-postId/{Id:int}")]
        [SwaggerOperation(
            Summary = "Retrieve comments by Post Id",
            Description = "Retrieves comments associated with a specific Post Id."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCommentByPostId(int Id)
        {
            try
            {
                if (Id <= 0)
                {
                    _logger.LogWarning("InvalId comment Id: {Id}", Id);
                    return ApiResponseHelper.BadRequest("InvalId comment Id. Id must be greater than zero.");
                }

                var comment = await _commentService.GetCommentByPostIdAsync(Id);
                if (comment is null)
                {
                    _logger.LogInformation("Comment not found. Id: {Id}", Id);
                    return ApiResponseHelper.NotFound("Comment not found."); 
                }

                _logger.LogInformation("Comment retrieved successfully. Id: {Id}", Id);
                return ApiResponseHelper.Success(comment, "Comment retrieved successfully.");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving comment with Id: {Id}", Id);
                return StatusCode(500, $"Error: {ex.InnerException?.Message}");
			}
        }

        /// <summary>
        /// Creates a new comment.
        /// </summary>
        /// <param name="dto">The comment data transfer object containing content and metadata.</param>
        /// <returns>Returns the created comment object or valIdation errors.</returns>
        /// <response code="201">Comment created successfully.</response>
        /// <response code="400">InvalId input data.</response>
        /// <response code="404">Comment not found.</response>
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new comment", Description = "Creates a new comment based on the provIded data.")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateNewCommentAsync([FromBody] CommentDTO dto)
        {
            if (dto is null)
            {
                _logger.LogWarning("Received null comment DTO in CreateNewCommentAsync.");
                return ApiResponseHelper.BadRequest("Request body cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("InvalId model state while creating comment: {@ModelState}", ModelState);
                return ApiResponseHelper.BadRequest("InvalId comment data.");
            }

            try
            {
                var createdComment = await _commentService.AddCommentAsync(dto);

                if (createdComment is null)
                {
                    _logger.LogWarning("Failed to create comment for DTO: {@Dto}", dto);
                    return ApiResponseHelper.NotFound("Comment not found");
                }

                _logger.LogInformation("Comment created successfully with Id: {Id}", createdComment.Id);
                return ApiResponseHelper.Created(createdComment, "Comment created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating comment.");
                return StatusCode(500, $"Error: {ex.InnerException?.Message}");
			}
        }

        /// <summary>
        /// Updates an existing comment by its Id.
        /// </summary>
        /// <param name="Id">The Id of the comment to update.</param>
        /// <param name="modelDto">The updated comment data.</param>
        /// <returns>Updated comment information.</returns>
        /// <response code="200">Comment updated successfully.</response>
        /// <response code="400">InvalId input data.</response>
        /// <response code="404">Comment not found.</response>
        [HttpPut("{Id:int}")]
        [SwaggerOperation(Summary = "Update a comment by Id", Description = "Updates the content of an existing comment.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCommentAsync(int Id, [FromBody] CommentDTO modelDto)
        {
            if (Id <= 0)
            {
                _logger.LogWarning("InvalId comment Id for update: {Id}", Id);
                return ApiResponseHelper.BadRequest("InvalId comment Id. Id must be greater than zero.");
            }

            if (modelDto is null)
            {
                _logger.LogWarning("Null comment DTO provIded for update. Id: {Id}", Id);
                return ApiResponseHelper.BadRequest("Request body cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("InvalId model state while updating comment Id {Id}: {@ModelState}", Id, ModelState);
                return ApiResponseHelper.BadRequest("InvalId comment data.");
            }
            try
            {
                var updatedComment = await _commentService.UpdateCommentAsync(Id, modelDto);
                if (updatedComment is null)
                {
                    _logger.LogInformation("Comment not found for update. Id: {Id}", Id);
                    return ApiResponseHelper.NotFound("Comment not found.");
                }

                _logger.LogInformation("Comment updated successfully. Id: {Id}", Id);
                return ApiResponseHelper.Success(updatedComment, "Comment updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating comment. Id: {Id}", Id);
                return StatusCode(500, $"Error: {ex.InnerException?.Message}");
			}
        }

        /// <summary>
        /// Deletes a comment by its Id.
        /// </summary>
        /// <param name="Id">The Id of the comment to delete.</param>
        /// <returns>No content if deletion succeeds.</returns>
        /// <response code="204">Comment deleted successfully.</response>
        /// <response code="400">InvalId Id provIded.</response>
        [HttpDelete("{Id:int}")]
        [SwaggerOperation(Summary = "Delete a comment by Id", Description = "Deletes a specific comment using its Id.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCommentByIdAsync(int Id)
        {
            if (Id <= 0)
            {
                _logger.LogWarning("InvalId Id provIded for deletion: {Id}", Id);
                return ApiResponseHelper.BadRequest("InvalId comment Id. Id must be greater than zero.");
            }

            try
            {
                await _commentService.DeleteCommentAsync(Id);
                _logger.LogInformation("Comment deleted successfully. Id: {Id}", Id);
                return ApiResponseHelper.Success("Comment deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting comment. Id: {Id}", Id);
                return StatusCode(500, $"Error: {ex.InnerException?.Message}");
			}
        }
    }
}
