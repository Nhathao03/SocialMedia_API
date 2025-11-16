using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Social_Media.Helpers;
using Social_Media.Hubs;
using SocialMedia.Core.DTO.Notification;
using SocialMedia.Core.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Social_Media.Controllers
{
    [Route("api/notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IMapper _mapper;
        public NotificationController(INotificationService notificationService,
            IHubContext<NotificationHub> hubContext,
            IMapper mapper)
        {
            _notificationService = notificationService;
            _hubContext = hubContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all notifications of current user
        /// </summary>
        /// <returns>List notification of current user</returns>
        /// <response code="200">Retrives notification successfully.</response>
        /// <response code="404">Not notification found.</response>
        [HttpGet]
        [SwaggerOperation(Summary = "Get all notifications of current user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserNotifications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notifications = await _notificationService.GetNotificationsByUserIdAsync(userId);
            if (notifications == null)
                return ApiResponseHelper.NotFound();
            return ApiResponseHelper.Success(notifications,"Retrives all notifications od current user.");
        }

        /// <summary>
        /// Get unread notifications of current user
        /// </summary>
        /// <returns>List unread notification of current user</returns>
        /// <response code="200">Retrives notification successfully.</response>
        /// <response code="404">Not notification found.</response>
        [HttpGet("unread")]
        [SwaggerOperation(Summary = "Get unread notifications of current user")]
        public async Task<IActionResult> GetUnreadNotifications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notifications = await _notificationService.GetUnreadNotificationsAsync(userId);
            if (notifications == null)
                return ApiResponseHelper.NotFound();
            return ApiResponseHelper.Success(notifications, "Retrives list unread notification of current user");
        }

        /// <summary>
        /// Mark a notification as read
        /// </summary>
        /// <response code="200">Mark as read successfully.</response>
        /// <response code="400">InvalId input data</response>
        [HttpPut("read/{Id:int}")]
        [SwaggerOperation(Summary = "Mark a notification as read")]
        public async Task<IActionResult> MarkAsRead(int Id)
        {
            if (Id <= 0) return ApiResponseHelper.BadRequest("InvalId input data");
            try
            {
                await _notificationService.MarkAsReadAsync(Id);
                return ApiResponseHelper.Success("Notification marked as read.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.InnerException?.Message}");
            }
        }

        /// <summary>
        /// Mark all notifications as read
        /// </summary>
        /// <response code = "200" > Mark all read successfully.</response> 
        [HttpPut("read-all")]
        [SwaggerOperation(Summary = "Mark all notifications as read")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _notificationService.MarkAllAsReadAsync(userId);
                return ApiResponseHelper.Success("All notifications marked as read.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.InnerException?.Message}");
            }
        }

        /// <summary>
        /// Get unread notification count
        /// </summary>
        /// <returns>List of unread notification</returns>
        /// <response code = "200"> Retrives unread notification successfully.</response> 
        /// <response code = "404"> Not notification found.</response> 
        [HttpGet("unread-count")]
        [SwaggerOperation(Summary = "Get unread notification count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var count = await _notificationService.GetUnreadCountAsync(userId);
                if (count == 0) return ApiResponseHelper.NotFound();
                return ApiResponseHelper.Success(new { count }, "Count unread notification successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.InnerException?.Message}");
            }    
        }

        /// <summary>
        /// Creates a new notification for a specific user.
        /// </summary>
        /// <param name="dto">Notification creation data including receiver, message, and type.</param>
        /// <returns>Returns the created notification information.</returns>
        /// <response code="200">Notification created successfully.</response>
        /// <response code="400">InvalId input data.</response>
        /// <response code="401">Unauthorized — JWT token is missing or invalId.</response>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new notification manually",
            Description = "Allows a logged-in user (Identified by JWT) to create a new notification for a specific receiver. " +
                          "Normally used for testing or admin purposes.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateNotification([FromBody] NotificationCreateDTO dto)
        {
            if (dto == null)
            {
                return ApiResponseHelper.BadRequest("Notification data is required.");
            }

            try
            {
                var result = await _notificationService.CreateNotificationAsync(dto);
                try
                { 
                    await _hubContext.Clients.User(dto.SenderId).SendAsync("ReceiveNotification", result);
                }
                catch(Exception ex)
                {
                    return ApiResponseHelper.InternalServerError($"Notification created but failed to send real-time update: {ex.Message}");
                }
                return ApiResponseHelper.Success("Notification created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.InnerException?.Message}");
            }
        }

    }
}
