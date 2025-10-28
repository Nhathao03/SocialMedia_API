using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.Services;
using Social_Media.Helpers;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Entities.DTO.Account;
using SocialMedia.Core.Entities.DTO.RoleCheck;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
namespace Social_Media.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRoleCheckService _roleCheckService;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;
        private static Dictionary<string, (string Otp, DateTime Expiry)> _otpStore = new();

        public AuthController(IUserService userService,
            IRoleCheckService roleCheckService,
            IConfiguration configuration,
            ILogger<AuthController> logger)
        {
            _userService = userService;
            _roleCheckService = roleCheckService;
            _config = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Register a new user account.
        /// </summary>
        /// <param name="model">The resgister data transfer object containing content and metadata.</param>
        /// <returns>Returns the created user object or validation errors</returns>
        /// <response code="201">Register successfully</response>
        /// <response code="400">Invalid input data</response>
        [HttpPost("register")]
        [SwaggerOperation(Summary = "Register a new user", Description = "Creates a new user account using email, password.")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterAccountAsync([FromBody] RegisterDTO? model)
        {
            if (model is null)
            {
                _logger.LogWarning("Received null RegisterDTO in RegisterAccountAsync.");
                return ApiResponseHelper.BadRequest("Model cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state in RegisterAccountAsync: {ModelState}", ModelState);
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _userService.RegisterAccountAsync(model);
                if (!result.Success)
                {
                    _logger.LogWarning("Registration failed in RegisterAccountAsync: {Errors}", string.Join(", ", result.Errors));
                    return ApiResponseHelper.BadRequest(string.Join(", ", result.Errors));
                }
                _logger.LogInformation("User registered successfully with email: {Email}", model.Email);
                return ApiResponseHelper.Created(result, "Register successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred in RegisterAccountAsync.");
                return ApiResponseHelper.InternalServerError("An error occurred during registration.");
            }
        }

        /// <summary>
        /// Login user account.
        /// </summary>
        /// <param name="model">The login data transfer object containing content and metadata.</param>
        /// <returns>Returns the refreshToken and accessToken or validation errors</returns>
        /// <response code="201">Login successfully</response>
        /// <response code="400">Invalid input data</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("login")]
        [SwaggerOperation(Summary = "User login", Description = "Logs in user with email and password, returns JWT tokens and refresh token.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LoginAccountAsync([FromBody] LoginDTO? model)
        {
            if (model is null)
            {
                _logger.LogWarning("Received null LoginDTO in LoginAccountAsync.");
                return ApiResponseHelper.BadRequest("Model cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state in LoginAccountAsync: {ModelState}", ModelState);
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _userService.LoginAsync(model);

                if (!result.Success)
                {
                    _logger.LogWarning("Login failed in LoginAccountAsync: {Errors}", string.Join(", ", result.Errors));
                    return ApiResponseHelper.Unauthorized(string.Join("", "", result.Errors));
                }

                var jwtSettings = _config.GetSection("Jwt");
                Response.Cookies.Append("token", result.AccessToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["Expires"]))
                }); ;

                _logger.LogInformation("User logged in successfully with email: {Email}", model.Email);
                return ApiResponseHelper.Success(new { result.RefreshToken, result.AccessToken }, "Login successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred in LoginAccountAsync.");
                return ApiResponseHelper.InternalServerError("An error occurred during login.");

            }
        }

        /// <summary>
        /// Decode JWT token.
        /// </summary>
        /// <param name="jwtToken">The unique token of the user</param>
        /// <returns>Information of token after decode</returns>
        /// <response code="200">Decode token successfully</response>
        /// <response code="400">Invalid input data</response>
        [HttpPost("Decode")]
        [SwaggerOperation(Summary = "Decode JWT token", Description = "Decodes a JWT token and returns its header and payload.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Decode([FromBody] string jwtToken)
        {

            if (string.IsNullOrWhiteSpace(jwtToken))
            {
                _logger.LogWarning("JWT token is null or empty in Decode.");
                return ApiResponseHelper.BadRequest("JWT token is required.");
            }
                
            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(jwtToken))
            {
                _logger.LogWarning("Invalid JWT token format in Decode.");
                return ApiResponseHelper.BadRequest("Invalid JWT token format.");
            }
               
            try
            {
                var token = handler.ReadJwtToken(jwtToken);

                // Map claim key to easy readable keys
                var claimKeyMap = new Dictionary<string, string>
                {
                    {"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "DisplayName" },
                    {"http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "role" },
                };
                // Transform claims dictionary
                var payload = token.Payload
                    .ToDictionary(
                        kvp => claimKeyMap.ContainsKey(kvp.Key) ? claimKeyMap[kvp.Key] : kvp.Key,
                        kvp => kvp.Value
                    );

                var result = new
                {
                    Header = token.Header,
                    Payload = payload
                };

                _logger.LogInformation("JWT token decoded successfully in Decode.");
                return ApiResponseHelper.Success(result, "Decode token successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while decoding JWT token in Decode.");
                return ApiResponseHelper.InternalServerError("An error occurred while decoding the JWT token.");
            }
        }


        /// <summary>
        /// Lougout user account.
        /// </summary>
        [HttpGet("logout")]
        [Authorize]
        [SwaggerOperation(Summary = "Logout", Description = "Removes JWT token from cookies and logs out the user.")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("token");
            _logger.LogInformation("User logged out successfully in Logout.");
            return ApiResponseHelper.Success("", "Logged out");
        }

        /// <summary>
        /// Refresh JWT tokens.
        /// </summary>
        /// <param name="refreshToken">The unique refreshToken of the user</param>
        /// <returns>Returns the accessToken and refreshToken</returns>
        /// <response code="201">Access token added to cookies successfuly</response>
        /// <response code="400">Invalid input data</response>
        [HttpPost("refresh-token")]
        [SwaggerOperation(Summary = "Refresh JWT tokens", Description = "Refreshes JWT access and refresh tokens using a valid refresh token.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            var user = _userService.GetUserByRefreshToken(refreshToken);
            if (user == null)
            {
                _logger.LogWarning("Invalid refresh token provided in RefreshToken.");
                return ApiResponseHelper.Unauthorized("Invalid refresh token.");
            }
            
            var newAccessToken = await _userService.GenerateAccessTokenAsync(user.Id);
            var newRefreshToken = await _userService.GenerateRefreshTokenAsync(user);

            Response.Cookies.Append("token", newAccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:Expires"]))
            });

            _logger.LogInformation("JWT tokens refreshed successfully in RefreshToken.");
            return ApiResponseHelper.Success(new
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            }, "Access token added to cookies successfuly");
        }
    }
}
