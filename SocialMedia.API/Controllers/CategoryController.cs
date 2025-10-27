using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.Services;
using SocialMedia.Core.Entities.PostEntity;
using Social_Media.Helpers;
using System.Runtime.InteropServices;
using Swashbuckle.AspNetCore.Annotations;
using SocialMedia.Core.DTO.Post;

namespace Social_Media.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IPostCategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController( IPostCategoryService category, ILogger<CategoryController> logger)
        {
            _categoryService = category;
            _logger = logger;
        }

        /// <summary>
        /// Get all post categories.
        /// </summary>
        /// <returns>Returns the list categories</returns>
        /// <response code="200">Categories retrieved successfully</response>
        /// <response code="404">List categories not found.</response>
        [HttpGet]
        [SwaggerOperation(Summary = "Retrieve all post categories", Description = "Retrieves a list of all post categories.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllCategoryAsync()
        {
            var postcategory = await _categoryService.GetAllPostCategoryAsync();
            if (postcategory is null) 
            {
                _logger.LogInformation("No categories found.");
                return ApiResponseHelper.NotFound("No categories found.");
            }
            _logger.LogInformation("Categories retrieved successfully.");
            return ApiResponseHelper.Success(postcategory, "Categories retrieved successfully.");
        }


        /// <summary>
        /// Get category by ID.
        /// </summary>
        /// <param name="id">The unique Id of the category</param>
        /// <response code="200">Categories retrieved successfully</response>
        /// <response code="404">Category not found</response>
        /// <response code="400">Invalid input data</response>
        [HttpGet("{id:int}")]
        [SwaggerOperation(Summary = "Retrieve a post category by ID", Description = "Retrieves a specific post category using its ID.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType (StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoryByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid category ID: {CategoryId}", id);
                    return ApiResponseHelper.BadRequest("Invalid category ID.");
                }
                var postcategory = await _categoryService.GetPostCategoryByIdAsync(id);
                if (postcategory == null)
                {
                    _logger.LogInformation("Category with ID {CategoryId} not found.", id);
                    return ApiResponseHelper.NotFound("Category not found.");
                }
                _logger.LogInformation("Category with ID {CategoryId} retrieved successfully.", id);
                return ApiResponseHelper.Success(postcategory, "Category retrieved successfully.");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error retrieving category with ID {CategoryId}", id);
                return ApiResponseHelper.InternalServerError("An error occurred while retrieving the category.");
            }
        }

        /// <summary>
        /// Create new catergory
        /// </summary>
        /// <param name="model">The model data transfer object containing content and metadata</param>
        /// <response code="201">Add new category successfully</response>
        /// <response code="400">Invalid input data</response>
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        [SwaggerOperation(Summary = "Add a new post category", Description = "Creates a new post category.")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] PostCategoryDTO model)
        {
            if (model is null)
            {
                _logger.LogWarning("Received null category DTO in CreateCategoryAsync.");
                return ApiResponseHelper.BadRequest("Category data cannot be null.");
            }

            if(!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state in CreateCategoryAsync: {ModelState}", ModelState);
                return BadRequest(ModelState);
            }
            try
            {
                var createdCategory = await _categoryService.AddPostCategoryAsync(model);
                if (createdCategory == null)
                {
                    _logger.LogWarning("Failed to create category in CreateCategoryAsync.");
                    return ApiResponseHelper.NotFound("Category not found");
                }
                _logger.LogInformation("Category created successfully with ID {CategoryId}.", createdCategory.Id);
                return ApiResponseHelper.Created(createdCategory, "Category created successfully.");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Exception occurred in CreateCategoryAsync.");
                return ApiResponseHelper.InternalServerError("An error occurred while creating the category.");
            }

        }

        /// <summary>
        /// Update category 
        /// </summary>
        /// <param name="id">The ID of the category by its ID</param> 
        /// <param name="model">The updated category data</param> 
        /// <returns>The updated category details</returns>
        /// <response code="200">Category updated successfully</response>
        /// <response code="400">Inlavid input data</response>
        /// <response code="404">Category not found</response>
        //Update category by ID (Admin only)
        //[Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        [SwaggerOperation(Summary = "Update an existing post category", Description = "Updates the details of an existing post category.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategoryAsync(int id, [FromBody] PostCategoryDTO model)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid category ID for update: {Id}", id);
                return ApiResponseHelper.BadRequest("Invalid category ID. ID must be greater than zero.");
            }

            if (model is null)
            {
                _logger.LogWarning("Null category DTO provided for update. ID: {Id}", id);
                return ApiResponseHelper.BadRequest("Request body cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state while updating category ID {Id}: {@ModelState}", id, ModelState);
                return ApiResponseHelper.BadRequest("Invalid category data.");
            }

            try
            {
                var updatedcategory = await _categoryService.UpdatePostCategoryAsync(id, model);
                if (updatedcategory is null)
                {
                    _logger.LogInformation("category not found for update. ID: {Id}", id);
                    return ApiResponseHelper.NotFound("category not found.");
                }

                _logger.LogInformation("category updated successfully. ID: {Id}", id);
                return ApiResponseHelper.Success(updatedcategory, "category updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating category. ID: {Id}", id);
                return ApiResponseHelper.InternalServerError("An unexpected error occurred while updating the category.");
            }
        }

        /// <summary>
        /// Deletes a category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        /// <returns>No content if deletion succeeds.</returns>
        /// <response code="204">category deleted successfully.</response>
        /// <response code="400">Invalid ID provided.</response>
        /// <response code="404">category not found.</response>
        [HttpDelete("{id:int}")]
        [SwaggerOperation(Summary = "Delete a category by ID", Description = "Deletes a specific category using its ID.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletecategoryByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid ID provided for deletion: {Id}", id);
                return ApiResponseHelper.BadRequest("Invalid category ID. ID must be greater than zero.");
            }

            try
            {
                var deleted = await _categoryService.DeletePostCategoryAsync(id);
                _logger.LogInformation("category deleted successfully. ID: {Id}", id);
                return ApiResponseHelper.Success("Category delete successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting category. ID: {Id}", id);
                return ApiResponseHelper.InternalServerError("An unexpected error occurred while deleting the category.");
            }
        }
    }
}
