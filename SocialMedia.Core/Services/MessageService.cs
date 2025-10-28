﻿using SocialMedia.Infrastructure.Repositories;
using SocialMedia.Core.Entities;
using Microsoft.Extensions.Logging;
using SocialMedia.Core.DTO.Message;
using AutoMapper;
using SocialMedia.Core.Interfaces.ServiceInterfaces;

namespace SocialMedia.Core.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MessageService> _logger;
        private readonly IMapper _mapper; 
        public MessageService(IUnitOfWork unitOfWork,
            ILogger<MessageService> logger,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<Message>?> GetAllMessageAsync()
        {
            return await _unitOfWork.MessageRepository.GetAllMessagesAsync();
        }

        public async Task<Message?> GetMessageByIdAsync(int id)
        {
            return await _unitOfWork.MessageRepository.GetMessageByIdAsync(id);
        }

        public async Task<RetriveMessageDTO?> AddMessageAsync(MessageDTO dto)
        {
           _logger.LogInformation("Adding new message from {SenderId} to {ReceiverId}", dto?.SenderId, dto?.ReceiverId);
              if (dto == null)
                throw new ArgumentNullException(nameof(MessageDTO), "Message data is required.");
              if(string.IsNullOrWhiteSpace(dto.Content))
                throw new ArgumentException("Message content cannot be empty.", nameof(dto.Content));
              var message = _mapper.Map<Message>(dto);
            var result = await _unitOfWork.MessageRepository.AddMessageAsync(message);
            _logger.LogInformation("Message added with ID {MessageId}", result?.ID);
            return _mapper.Map<RetriveMessageDTO>(result);
        }

        public async Task<RetriveMessageDTO?> UpdateMessageAsync(int id, MessageDTO dto)
        {
            _logger.LogInformation("Updating message with ID {MessageId}", id);
            var existingMessage = await _unitOfWork.MessageRepository.GetMessageByIdAsync(id);
            if (existingMessage is null)
            {
                throw new KeyNotFoundException($"Message with Id {id} not exits.");
            }
            var message = _mapper.Map(dto, existingMessage);
            var result =  await _unitOfWork.MessageRepository.UpdateMessageAsync(message);
            _logger.LogInformation("Message updated with ID {MessageId}", result?.ID);
            return _mapper.Map<RetriveMessageDTO>(result);
        }

        public async Task<bool> DeleteMessageAsync(int id)
        {
            _logger.LogInformation("Deleting message with ID {MessageId}", id);
            var exitsMessage = await _unitOfWork.MessageRepository.GetMessageByIdAsync(id);
            if (exitsMessage is null)
            {
                throw new KeyNotFoundException($"Message with Id {id} not exits.");
            }
            var result = await _unitOfWork.MessageRepository.DeleteMessageAsync(id);
            _logger.LogInformation("Message with ID {MessageId} deleted successfully", id);
            return true;
        } 

        public async Task<List<Message>> GetMessageByReceiverIdAndSenderIdAsync(string userId1, string userId2)
        {
            return await _unitOfWork.MessageRepository.GetMessageByReceiverIdAndSenderIdAsync(userId1, userId2);
        }

        public async Task<List<Message>> GetMessageLastestAsync(string userId1, string userId2)
        {
            return await _unitOfWork.MessageRepository.GetMessageLastestAsync(userId1, userId2);
        }
    }
}
