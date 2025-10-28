using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SocialMedia.Core.Services;
using SocialMedia.Core.Entities;
using Social_Media.Hubs;
using Social_Media.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using SocialMedia.Core.DTO.Message;

namespace Social_Media.Controllers
{
    [Route("api/message")]
    [ApiController]
    public class ChatMessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ILogger<ChatMessageController> _logger;

        public ChatMessageController(IMessageService messageService, IHubContext<ChatHub> hubContext, ILogger<ChatMessageController> logger)
        {
            _messageService = messageService;
            _hubContext = hubContext;
            _logger = logger;
        }

        /// <summary>
        /// Retrieve a message by its unique identifier.
        /// </summary>
        /// <param name="id">The unique ID of the message </param>
        /// <returns>Returns the message object if found.</returns>
        /// <response code="200">Message retrieved successfully.</response>
        /// <response code="400">Invalid input data</response>
        /// <response code="404">Message not found</response>
        [HttpGet("{id:int}")]
        [SwaggerOperation(Summary = "Retrieve a message by ID", Description = "Retrieves a specific message using its ID.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMessageByIdAsync(int id)
        {
            try
            {
                if(id <= 0)
                {
                    _logger.LogWarning("Invalid message ID: {MessageId}", id);
                    return ApiResponseHelper.BadRequest("Invalid input data. ID must be greater than zero.");
                }

                var message = await _messageService.GetMessageByIdAsync(id);
                if(message == null)
                {
                    _logger.LogInformation("Message with ID {MessageId} not found", id);
                    return ApiResponseHelper.NotFound("Message not found.");
                }
                _logger.LogInformation("Message with ID {MessageId} retrieved successfully", id);
                return ApiResponseHelper.Success(message, "Message retrieved successfully.");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error retrieving message with ID {MessageId}", id);
                return ApiResponseHelper.InternalServerError("An error occurred while retrieving the message.");
            }
        }

        /// <summary>
        /// Retrives the latest message between two users.
        /// </summary>
        /// <param name="userId1">The unique Id of the user1</param>
        /// <param name="userId2">The unique Id of the user2</param>
        /// <returns>Returns the latest message object between two users.</returns>
        /// <response code="200">Message retrieved successfully.</response>
        /// <response code="400">Invalid input data</response>
        /// <response code="404">Message not found</response>
        [HttpGet("lastest-message/{userId1}/{userId2}")]
        public async Task<IActionResult> GetLatestMessageAsync(string userId1, string userId2)
        {
            if (string.IsNullOrWhiteSpace(userId1) || string.IsNullOrWhiteSpace(userId2))
            {
                _logger.LogWarning("Invalid user IDs: {UserId1}, {UserId2}", userId1, userId2);
                return ApiResponseHelper.BadRequest("User IDs cannot be null or empty.");
            }
            try
            {
                var latestMessage = await _messageService.GetMessageLastestAsync(userId1, userId2);
                if (latestMessage == null || !latestMessage.Any())
                {
                    _logger.LogInformation("No messages found between users {UserId1} and {UserId2}", userId1, userId2);
                    return ApiResponseHelper.NotFound("Message not found.");
                }
                _logger.LogInformation("Latest message between users {UserId1} and {UserId2} retrieved successfully", userId1, userId2);
                return ApiResponseHelper.Success(latestMessage, "Message retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving latest message between users {UserId1} and {UserId2}", userId1, userId2);
                return ApiResponseHelper.InternalServerError("An error occurred while retrieving the latest message.");
            }
        }
        /// <summary>
        /// Send a new message
        /// </summary>
        /// <param name="model">The model data transfer object containing content and metadata.</param>
        /// <returns>Returns the created message object or validation errors</returns>
        /// <response code="201">Message sent successfully.</response>
        /// <response code="400">Invalid input data</response>
        /// <response code="404">Message not found</response>
        [HttpPost]
        [SwaggerOperation(Summary = "Send a new message", Description = "Sends a new message from one user to another.")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SendMessageAsync([FromBody] MessageDTO model)
        {
            if (model is null)
            {
                _logger.LogWarning("Message model is null");
                return ApiResponseHelper.BadRequest("Model data cannot be null");
            }

            if(!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid message model state");
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _messageService.AddMessageAsync(model);
                if(result == null)
                {
                    _logger.LogWarning("Failed to add message to the database");
                    return ApiResponseHelper.NotFound("Message could not be created.");
                }

                try
                {
                    await _hubContext.Clients.User(model.ReceiverId).SendAsync("ReceiveMessage", model.SenderId, model.Content);
                    await _hubContext.Clients.User(model.SenderId).SendAsync("ReceiveMessage", model.SenderId, model.Content);
                }
                catch (Exception hubEx)
                {
                    _logger.LogWarning(hubEx, "Failed to send SignalR message");
                    return ApiResponseHelper.Success("Message sent successfully, but failed to notify via SignalR.");
                }
                _logger.LogInformation("Message from {SenderId} to {ReceiverId} sent successfully", model.SenderId, model.ReceiverId);
                return ApiResponseHelper.Success(result, "Message sent successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message from {SenderId} to {ReceiverId}", model.SenderId, model.ReceiverId);
                return ApiResponseHelper.InternalServerError("An unexpected error occurred while sending the message.");
            }

        }

        /// <summary>
        /// Retrieve messages between receiver and sender
        /// </summary>
        /// <param name="receiverId">The unique Id of the receiverId</param>
        /// <param name="senderId">The unique Id of the senderId</param>
        /// <response code="201">Message retrieved successfully.</response>
        /// <response code="400">Invalid input data</response>
        /// <response code="404">Message not found</response>
        [HttpGet("message-receiver-sender/{receiverId}/{senderId}")]
        [SwaggerOperation(Summary = "Retrieve messages between receiver and sender", Description = "Retrieves messages exchanged between a specific receiver and sender.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMessagesAsync(string receiverId, string senderId)
        {
            if (receiverId == null || senderId == null)
            {
                _logger.LogWarning("receiverId or senderId is null");
                return ApiResponseHelper.BadRequest("receiverId and senderId can not be null");
            }
            try
            {
                var messages = await _messageService.GetMessageByReceiverIdAndSenderIdAsync(receiverId, senderId);
                if (messages == null || !messages.Any())
                {
                    _logger.LogInformation("No messages found between receiver {ReceiverId} and sender {SenderId}", receiverId, senderId);
                    return ApiResponseHelper.NotFound("No message found");
                }
                _logger.LogInformation("Messages between receiver {ReceiverId} and sender {SenderId} retrieved successfully", receiverId, senderId);
                return ApiResponseHelper.Success(messages, "Message retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving messages between receiver {ReceiverId} and sender {SenderId}", receiverId, senderId);
                return ApiResponseHelper.InternalServerError("An unexpected error occurred while retrieving messages.");

            }
        }

        /// <summary>
        /// Update message by Id
        /// </summary>
        /// <param name="id">The unique Id of the message to update</param>
        /// <param name="dto">The updated message data</param>
        /// <response code="200">Update successfully.</response>
        /// <response code="400">Invalid input data.</response>
        /// <response code="404">Message not found.</response>
        [HttpPut("{id:int}")]
        [SwaggerOperation(Summary = "Update a message by ID", Description = "Updates the details of a specific message using its ID.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateMessageAsync(int id, [FromBody] MessageDTO dto)
        {
            if (dto is null)
            {
                _logger.LogWarning("Message DTO is null for update");
                return ApiResponseHelper.BadRequest("Message data cannot be null");
            }

            if(!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for message update");
                return BadRequest(ModelState);
            }

            try
            {
                var message = await _messageService.UpdateMessageAsync(id, dto);
                if(message == null)
                {
                    _logger.LogInformation("Message with ID {Id} not found for update", id);
                    return ApiResponseHelper.NotFound("Message not found");
                }
                _logger.LogInformation("Message with ID {Id} updated successfully", id);
                return ApiResponseHelper.Success(message, "Message updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating message {Id}", id);
                return StatusCode(500, new { message = "An unexpected error occurred" });
            }
        }

        /// <summary>
        /// Delete message by Id
        /// </summary>
        /// <param name="id">The unique Id of the message</param>
        /// <response code="200">Delete successfully.</response>
        /// <response code="400">Invalid input data</response>
        [HttpDelete("{id:int}")]
        [SwaggerOperation(Summary = "Delete a message by ID", Description = "Deletes a specific message using its ID.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteMessageAsync  (int id)
        {
            if(id <= 0)
            {
                _logger.LogWarning("Invalid message ID for deletion: {Id}", id);
                return ApiResponseHelper.BadRequest("Invalid input data. ID must be greater than zero.");
            }

            try
            {  
                var result = await _messageService.DeleteMessageAsync(id);
                _logger.LogInformation("Message with ID {Id} deleted successfully", id);
                return ApiResponseHelper.Success("Message deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting message {Id}", id);
                return StatusCode(500, new { message = "Failed to delete message" });
            }
        }
    }
}
