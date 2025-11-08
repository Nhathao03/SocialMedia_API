using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Social_Media.Helpers;
using SocialMedia.Core.DTO.Post;
using SocialMedia.Core.Entities.PostEntity;
using SocialMedia.Core.Services;
using Swashbuckle.AspNetCore.Annotations;


namespace Social_Media.Controllers
{
    [Route("api/post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IPostImageService _postImageService;

        public PostController(IPostService postService,
            IPostImageService postImageService)
        {
            _postService = postService;
            _postImageService = postImageService;
        }

        /// <summary>
        /// Retrieves a post by its ID.
        /// </summary>
        /// <param name="id">The unique ID of the post to retrieve.</param>
        /// <returns>Returns the post details if found; otherwise, 404 Not Found.</returns>
        [HttpGet("{id:int}")]
        [SwaggerOperation(Summary = "Retrieves a post by its ID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPostById(int id)
        {
            try
            {
                var post = await _postService.GetPostByIdAsync(id);
                if (post == null)
                    return ApiResponseHelper.NotFound($"Post with Id {id} not found.");
                return ApiResponseHelper.Success(post, "Post retrieved successfully.");
            }
            catch (Exception)
            {
                return ApiResponseHelper.InternalServerError("An unexpected error occurred while retrieving the post.");
            }
        }

        /// <summary>
        /// Creates a new post.
        /// </summary>
        /// <param name="dto">The post data to create.</param>
        /// <returns>Returns the created post object.</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new post")]
        [ProducesResponseType(typeof(RetrivePostDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddPost([FromBody] PostDTO dto)
        {
            if (dto == null)
                return ApiResponseHelper.BadRequest("PostDTO cannot be null.");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdPost = await _postService.AddPostAsync(dto);
                if (createdPost == null)
                    return ApiResponseHelper.BadRequest("Failed to create post.");
                return CreatedAtAction(nameof(GetPostById), new { id = createdPost.Id }, createdPost);
            }
            catch (Exception)
            {
                return ApiResponseHelper.InternalServerError("An unexpected error occurred while creating the post.");
            }
        }

        /// <summary>
        /// Updates an existing post by its ID.
        /// </summary>
        /// <param name="id">The ID of the post to update.</param>
        /// <param name="dto">The updated post data.</param>
        /// <returns>Returns the updated post object.</returns>
        [HttpPut("{id:int}")]
        [SwaggerOperation(Summary = "Updates an existing post by its ID")]
        [ProducesResponseType(typeof(RetrivePostDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] PostDTO dto)
        {
            if (dto == null)
                return ApiResponseHelper.BadRequest("PostDTO cannot be null.");
            if (id <= 0)
                return ApiResponseHelper.BadRequest("Invalid post ID.");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var updatedPost = await _postService.UpdatePostAsync(id, dto);
                if (updatedPost == null)
                    return ApiResponseHelper.NotFound($"Post with Id {id} not found.");
                return ApiResponseHelper.Success(updatedPost, "Post updated successfully.");
            }
            catch (Exception)
            {
                return ApiResponseHelper.InternalServerError("An unexpected error occurred while updating the post.");
            }
        }

        /// <summary>
        /// Deletes a post by its ID.
        /// </summary>
        /// <param name="id">The ID of the post to delete.</param>
        /// <returns>Returns true if the deletion was successful.</returns>
        [HttpDelete("{id:int}")]
        [SwaggerOperation(Summary = "Deletes a post by its ID")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePost(int id)
        {
            if (id <= 0)
                return ApiResponseHelper.BadRequest("Invalid post ID.");
            try
            {
                var deleted = await _postService.DeletePostAsync(id);
                if (!deleted)
                    return ApiResponseHelper.NotFound($"Post with Id {id} not found.");
                return ApiResponseHelper.Success(deleted, "Post deleted successfully.");
            }
            catch (Exception)
            {
                return ApiResponseHelper.InternalServerError("An unexpected error occurred while deleting the post.");
            }
        }

        /// <summary>
        /// Retrieves all posts created by a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose posts to retrieve.</param>
        /// <returns>Returns a list of posts by the specified user.</returns>
        //[HttpGet("user/{userId}")]
        //[SwaggerOperation(Summary = "Retrieves all posts created by a specific user")]
        //[ProducesResponseType(typeof(IEnumerable<RetrivePostDTO>), StatusCodes.Status200OK)]
        //public async Task<IActionResult> GetPostsByUserId(string userId)
        //{
        //    try
        //    {
        //        var posts = await _postService.GetPostsByUserIdAsync(userId);
        //        return ApiResponseHelper.Success(posts, "Posts retrieved successfully.");
        //    }
        //    catch (Exception)
        //    {
        //        return ApiResponseHelper.InternalServerError("An unexpected error occurred while retrieving posts by user ID.");
        //    }
        //}

        /// <summary>
        /// Retrieves the most recent posts with pagination support.
        /// </summary>
        /// <param name="page">The current page number (default = 1).</param>
        /// <param name="pageSize">The number of posts per page (default = 10).</param>
        /// <returns>Returns a paginated list of the most recent posts.</returns>
        [HttpGet("recent")]
        [SwaggerOperation(Summary = "Retrieves the most recent posts with pagination support")]
        [ProducesResponseType(typeof(IEnumerable<RetrivePostDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRecentPosts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {  
                var posts = await _postService.GetRecentPostsAsync(page, pageSize);
                return ApiResponseHelper.Success(posts, "Recent posts retrieved successfully.");
            }
            catch (Exception)
            {
                return ApiResponseHelper.InternalServerError("An unexpected error occurred while retrieving recent posts.");
            }
        }
    }
}
