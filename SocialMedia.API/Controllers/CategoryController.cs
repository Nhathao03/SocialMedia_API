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
        /// Get category by Id.
        /// </summary>
        /// <param name="Id">The unique Id of the category</param>
        /// <response code="200">Categories retrieved successfully</response>
        /// <response code="404">Category not found</response>
        /// <response code="400">InvalId input data</response>
        [HttpGet("{Id:int}")]
        [SwaggerOperation(Summary = "Retrieve a post category by Id", Description = "Retrieves a specific post category using its Id.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType (StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoryByIdAsync(int Id)
        {
            try
            {
                if (Id <= 0)
                {
                    _logger.LogWarning("InvalId category Id: {CategoryId}", Id);
                    return ApiResponseHelper.BadRequest("InvalId category Id.");
                }
                var postcategory = await _categoryService.GetPostCategoryByIdAsync(Id);
                if (postcategory == null)
                {
                    _logger.LogInformation("Category with Id {CategoryId} not found.", Id);
                    return ApiResponseHelper.NotFound("Category not found.");
                }
                _logger.LogInformation("Category with Id {CategoryId} retrieved successfully.", Id);
                return ApiResponseHelper.Success(postcategory, "Category retrieved successfully.");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error retrieving category with Id {CategoryId}", Id);
                return StatusCode(500, $"Error: {ex.InnerException?.Message}");
			}
        }

        /// <summary>
        /// Create new catergory
        /// </summary>
        /// <param name="model">The model data transfer object containing content and metadata</param>
        /// <response code="201">Add new category successfully</response>
        /// <response code="400">InvalId input data</response>
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
                _logger.LogWarning("InvalId model state in CreateCategoryAsync: {ModelState}", ModelState);
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
                _logger.LogInformation("Category created successfully with Id {CategoryId}.", createdCategory.Id);
                return ApiResponseHelper.Created(createdCategory, "Category created successfully.");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Exception occurred in CreateCategoryAsync.");
                return StatusCode(500, $"Error: {ex.InnerException?.Message}");
			}

        }

        /// <summary>
        /// Update category 
        /// </summary>
        /// <param name="Id">The Id of the category by its Id</param> 
        /// <param name="model">The updated category data</param> 
        /// <returns>The updated category details</returns>
        /// <response code="200">Category updated successfully</response>
        /// <response code="400">InlavId input data</response>
        /// <response code="404">Category not found</response>
        //Update category by Id (Admin only)
        //[Authorize(Roles = "Admin")]
        [HttpPut("{Id:int}")]
        [SwaggerOperation(Summary = "Update an existing post category", Description = "Updates the details of an existing post category.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategoryAsync(int Id, [FromBody] PostCategoryDTO model)
        {
            if (Id <= 0)
            {
                _logger.LogWarning("InvalId category Id for update: {Id}", Id);
                return ApiResponseHelper.BadRequest("InvalId category Id. Id must be greater than zero.");
            }

            if (model is null)
            {
                _logger.LogWarning("Null category DTO provIded for update. Id: {Id}", Id);
                return ApiResponseHelper.BadRequest("Request body cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("InvalId model state while updating category Id {Id}: {@ModelState}", Id, ModelState);
                return ApiResponseHelper.BadRequest("InvalId category data.");
            }

            try
            {
                var updatedcategory = await _categoryService.UpdatePostCategoryAsync(Id, model);
                if (updatedcategory is null)
                {
                    _logger.LogInformation("category not found for update. Id: {Id}", Id);
                    return ApiResponseHelper.NotFound("category not found.");
                }

                _logger.LogInformation("category updated successfully. Id: {Id}", Id);
                return ApiResponseHelper.Success(updatedcategory, "category updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating category. Id: {Id}", Id);
                return StatusCode(500, $"Error: {ex.InnerException?.Message}");
			}
        }

        /// <summary>
        /// Deletes a category by its Id.
        /// </summary>
        /// <param name="Id">The Id of the category to delete.</param>
        /// <returns>No content if deletion succeeds.</returns>
        /// <response code="204">category deleted successfully.</response>
        /// <response code="400">InvalId Id provIded.</response>
        /// <response code="404">category not found.</response>
        [HttpDelete("{Id:int}")]
        [SwaggerOperation(Summary = "Delete a category by Id", Description = "Deletes a specific category using its Id.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletecategoryByIdAsync(int Id)
        {
            if (Id <= 0)
            {
                _logger.LogWarning("InvalId Id provIded for deletion: {Id}", Id);
                return ApiResponseHelper.BadRequest("InvalId category Id. Id must be greater than zero.");
            }

            try
            {
                var deleted = await _categoryService.DeletePostCategoryAsync(Id);
                _logger.LogInformation("category deleted successfully. Id: {Id}", Id);
                return ApiResponseHelper.Success("Category delete successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting category. Id: {Id}", Id);
                return StatusCode(500, $"Error: {ex.InnerException?.Message}");
			}
        }
    }
}
