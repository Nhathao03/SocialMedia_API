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
        /// Retrieves a comment by its unique identifier.
        /// </summary>
        /// <param name="id">The unique ID of the comment.</param>
        /// <returns>Returns the comment object if found.</returns>
        /// <response code="200">Comment retrieved successfully.</response>
        /// <response code="400">Invalid ID provided.</response>
        /// <response code="404">Comment not found.</response>
        [HttpGet("{id:int}")]
        [SwaggerOperation(
            Summary = "Retrieve a comment by ID",
            Description = "Retrieves a specific comment using its ID."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCommentByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid comment ID: {Id}", id);
                    return ApiResponseHelper.BadRequest("Invalid comment ID. ID must be greater than zero.");
                }

                var comment = await _commentService.GetCommentByIdAsync(id);
                if (comment is null)
                {
                    _logger.LogInformation("Comment not found. ID: {Id}", id);
                    return ApiResponseHelper.NotFound("Comment not found.");
                }

                _logger.LogInformation("Comment retrieved successfully. ID: {Id}", id);
                return ApiResponseHelper.Success(comment, "Comment retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving comment with ID: {Id}", id);
                return ApiResponseHelper.InternalServerError("An unexpected error occurred while retrieving the comment.");
            }
        }

        /// <summary>
        /// Retrives comments by Post Id.
        /// </summary>
        /// <param name="id">The unique Id of the post</param>
        /// <returns>Returns the list commets if found</returns>
        /// <response code="200">Comment retrieved successfully.</response>
        /// <response code="400">Comment not found.</response>
        /// <response code="404">Invalid ID provided.</response>
        [HttpGet("comment-by-postId/{id:int}")]
        [SwaggerOperation(
            Summary = "Retrieve comments by Post ID",
            Description = "Retrieves comments associated with a specific Post ID."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCommentByPostId(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid comment ID: {Id}", id);
                    return ApiResponseHelper.BadRequest("Invalid comment ID. ID must be greater than zero.");
                }

                var comment = await _commentService.GetCommentByPostIdAsync(id);
                if (comment is null)
                {
                    _logger.LogInformation("Comment not found. ID: {Id}", id);
                    return ApiResponseHelper.NotFound("Comment not found."); 
                }

                _logger.LogInformation("Comment retrieved successfully. ID: {Id}", id);
                return ApiResponseHelper.Success(comment, "Comment retrieved successfully.");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving comment with ID: {Id}", id);
                return ApiResponseHelper.InternalServerError("An unexpected error occurred while retrieving the comment.");
            }
        }

        /// <summary>
        /// Creates a new comment.
        /// </summary>
        /// <param name="dto">The comment data transfer object containing content and metadata.</param>
        /// <returns>Returns the created comment object or validation errors.</returns>
        /// <response code="201">Comment created successfully.</response>
        /// <response code="400">Invalid input data.</response>
        /// <response code="404">Comment not found.</response>
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new comment", Description = "Creates a new comment based on the provided data.")]
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
                _logger.LogWarning("Invalid model state while creating comment: {@ModelState}", ModelState);
                return ApiResponseHelper.BadRequest("Invalid comment data.");
            }

            try
            {
                var createdComment = await _commentService.AddCommentAsync(dto);

                if (createdComment is null)
                {
                    _logger.LogWarning("Failed to create comment for DTO: {@Dto}", dto);
                    return ApiResponseHelper.NotFound("Comment not found");
                }

                _logger.LogInformation("Comment created successfully with ID: {Id}", createdComment.ID);
                return ApiResponseHelper.Created(createdComment, "Comment created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating comment.");
                return ApiResponseHelper.InternalServerError("An unexpected error occurred while creating the comment.");
            }
        }

        /// <summary>
        /// Updates an existing comment by its ID.
        /// </summary>
        /// <param name="id">The ID of the comment to update.</param>
        /// <param name="modelDto">The updated comment data.</param>
        /// <returns>Updated comment information.</returns>
        /// <response code="200">Comment updated successfully.</response>
        /// <response code="400">Invalid input data.</response>
        /// <response code="404">Comment not found.</response>
        [HttpPut("{id:int}")]
        [SwaggerOperation(Summary = "Update a comment by ID", Description = "Updates the content of an existing comment.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCommentAsync(int id, [FromBody] CommentDTO modelDto)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid comment ID for update: {Id}", id);
                return ApiResponseHelper.BadRequest("Invalid comment ID. ID must be greater than zero.");
            }

            if (modelDto is null)
            {
                _logger.LogWarning("Null comment DTO provided for update. ID: {Id}", id);
                return ApiResponseHelper.BadRequest("Request body cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state while updating comment ID {Id}: {@ModelState}", id, ModelState);
                return ApiResponseHelper.BadRequest("Invalid comment data.");
            }
            try
            {
                var updatedComment = await _commentService.UpdateCommentAsync(id, modelDto);
                if (updatedComment is null)
                {
                    _logger.LogInformation("Comment not found for update. ID: {Id}", id);
                    return ApiResponseHelper.NotFound("Comment not found.");
                }

                _logger.LogInformation("Comment updated successfully. ID: {Id}", id);
                return ApiResponseHelper.Success(updatedComment, "Comment updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating comment. ID: {Id}", id);
                return ApiResponseHelper.InternalServerError("An unexpected error occurred while updating the comment.");
            }
        }

        /// <summary>
        /// Deletes a comment by its ID.
        /// </summary>
        /// <param name="id">The ID of the comment to delete.</param>
        /// <returns>No content if deletion succeeds.</returns>
        /// <response code="204">Comment deleted successfully.</response>
        /// <response code="400">Invalid ID provided.</response>
        [HttpDelete("{id:int}")]
        [SwaggerOperation(Summary = "Delete a comment by ID", Description = "Deletes a specific comment using its ID.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCommentByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid ID provided for deletion: {Id}", id);
                return ApiResponseHelper.BadRequest("Invalid comment ID. ID must be greater than zero.");
            }

            try
            {
                await _commentService.DeleteCommentAsync(id);
                _logger.LogInformation("Comment deleted successfully. ID: {Id}", id);
                return ApiResponseHelper.Success("Comment deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting comment. ID: {Id}", id);
                return ApiResponseHelper.InternalServerError("An unexpected error occurred while deleting the comment.");
            }
        }
    }
}
