using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Social_Media.Helpers;
using SocialMedia.Core.Entities.DTO.Role;
using SocialMedia.Core.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace Social_Media.Controllers
{
    [Route("api/role")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;

        public RoleController(IRoleService roleService, ILogger<RoleController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieve all roles.
        /// </summary>
        /// <returns>A list of roles.</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Retrieve all roles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var data = await _roleService.GetAllRoleAsync();
                if (data is null)
                    return ApiResponseHelper.NotFound("No roles found.");

                return ApiResponseHelper.Success(data, "Retrieve roles successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all roles");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieve a role by ID.
        /// </summary>
        /// <param name="id">The ID of the role.</param>
        /// <returns>The matching role.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get role by ID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRoleById(string id)
        {
            try
            {
                var data = await _roleService.GetRoleByIdAsync(id);
                if (data == null)
                    return ApiResponseHelper.NotFound("Role not found.");

                return ApiResponseHelper.Success(data, "Retrieve role successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting role by ID");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Create a new role.
        /// </summary>
        /// <param name="dto">The role information.</param>
        /// <returns>The created role.</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new role")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateRole(RoleDTO dto)
        {
            try
            {
                var result = await _roleService.AddRoleAsync(dto);
                return ApiResponseHelper.Success(result, "Role created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating role");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Update an existing role.
        /// </summary>
        /// <param name="id">The role ID to update.</param>
        /// <param name="dto">The updated role data.</param>
        /// <returns>The updated role.</returns>
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update role by ID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateRole(string id, RoleDTO dto)
        {
            try
            {
                var result = await _roleService.UpdateRoleAsync(id, dto);
                if (result == null)
                    return ApiResponseHelper.NotFound("Role not found.");

                return ApiResponseHelper.Success(result, "Role updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating role");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Delete a role by ID.
        /// </summary>
        /// <param name="id">The ID of the role.</param>
        /// <returns>True if deleted.</returns>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete role by ID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRole(string id)
        {
            try
            {
                var success = await _roleService.DeleteRoleAsync(id);
                if (!success)
                    return ApiResponseHelper.NotFound("Role not found.");

                return ApiResponseHelper.Success(success, "Role deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting role");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get the role ID of the currently authenticated user.
        /// </summary>
        /// <returns>The logged-in user's role ID.</returns>
        [HttpGet("user-role-id")]
        [SwaggerOperation(Summary = "Get role ID of the current user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserRoleId()
        {
            try
            {
                var id = await _roleService.GetRoleIdUserAsync();
                return ApiResponseHelper.Success(id, "Retrieve role ID successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting user role ID");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}