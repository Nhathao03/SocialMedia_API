using SocialMedia.Infrastructure.Repositories;
using Social_Media.Helpers;
using SocialMedia.Core.Entities.FriendEntity;
using SocialMedia.Core.DTO.Friend;
using SocialMedia.Core.Interfaces.ServiceInterfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using static Social_Media.Helpers.Constants;

namespace SocialMedia.Core.Services
{
    public class FriendRequestService : IFriendRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<FriendRequestService> _logger;
        public FriendRequestService(IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<FriendRequestService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<FriendRequest?> GetFriendRequestByIdAsync(int id)
        {
            return await _unitOfWork.FriendRequestRepository.GetFriendRequestByIdAsync(id);
        }

        public async Task<RetriveFriendRequestDTO?> SendFriendRequestAsync(FriendRequestDTO dto)
        {
            _logger.LogInformation("Attempting to add a new friend request.");

            if (dto is null)
                throw new ArgumentNullException(nameof(dto), "FriendRequest data is required.");

            if (string.IsNullOrWhiteSpace(dto.SenderId))
                throw new ArgumentException("SenderId cannot be empty.", nameof(dto.SenderId));

            if (string.IsNullOrWhiteSpace(dto.ReceiverId))
                throw new ArgumentException("ReceiverId cannot be empty.", nameof(dto.ReceiverId));

            if (dto.SenderId == dto.ReceiverId)
                throw new ArgumentException("Sender and Receiver cannot be the same user.");

            var existingRequest = await _unitOfWork.FriendRequestRepository
                .GetFriendRequestBetweenUsersAsync(dto.SenderId, dto.ReceiverId);

            if (existingRequest != null)
            {
                switch (existingRequest.status)
                {
                    case (int)Constants.FriendRequestStatus.Pending:
                        if (existingRequest.SenderID == dto.ReceiverId)
                        {
                            existingRequest.status = (int)Constants.FriendRequestStatus.Accepted;
                            existingRequest.CreatedAt = DateTime.UtcNow;
                            await _unitOfWork.FriendRequestRepository.UpdateFriendRequestAsync(existingRequest);

                            _logger.LogInformation(
                                "Mutual request detected. Friend request between {SenderId} and {ReceiverId} auto-accepted.",
                                dto.SenderId, dto.ReceiverId);

                            var addFriend = new FriendDTO
                            {
                                UserID = existingRequest.SenderID,
                                FriendID = existingRequest.ReceiverID,
                                Type_FriendsID = (int)Constants.FriendsEnum.Normal
                            };
                            var mapperFriend = _mapper.Map<Friends>(addFriend);

                            await _unitOfWork.FriendRepository.AddFriendAsync(mapperFriend);
                            return _mapper.Map<RetriveFriendRequestDTO>(existingRequest);
                        }

                        _logger.LogInformation("Friend request already pending between {SenderId} and {ReceiverId}.",
                            dto.SenderId, dto.ReceiverId);
                        return _mapper.Map<RetriveFriendRequestDTO>(existingRequest);

                    case (int)Constants.FriendRequestStatus.Accepted:
                        _logger.LogInformation("Users {SenderId} and {ReceiverId} are already friends.",
                            dto.SenderId, dto.ReceiverId);
                        return _mapper.Map<RetriveFriendRequestDTO>(existingRequest);

                    case (int)Constants.FriendRequestStatus.Blocked:
                        _logger.LogWarning("Cannot send friend request. {ReceiverId} has blocked {SenderId}.",
                            dto.ReceiverId, dto.SenderId);
                        throw new InvalidOperationException("Cannot send request — user is blocked.");

                    case (int)Constants.FriendRequestStatus.Rejected:
                    case (int)Constants.FriendRequestStatus.Canceled:
                    case (int)Constants.FriendRequestStatus.Removed:
                        break;
                }
            }

            var friendRequest = _mapper.Map<FriendRequest>(dto);
            friendRequest.status = (int)Constants.FriendRequestStatus.Pending;

            var result = await _unitOfWork.FriendRequestRepository.AddFriendRequestAsync(friendRequest);
            return _mapper.Map<RetriveFriendRequestDTO>(result);
        }

        public async Task<RetriveFriendRequestDTO?> UpdateFriendRequestAsync(int id, FriendRequestDTO dto)
        {
            _logger.LogInformation("Updating friend request with ID {CommentId}", id);
            var existingFriendrequest = _unitOfWork.FriendRequestRepository.GetFriendRequestByIdAsync(id);
            if (existingFriendrequest is null)
            {
                throw new KeyNotFoundException($"Friend request with Id {id} not exits");
            }

            var friendrequest = _mapper.Map<FriendRequest>(dto);
            var result = await _unitOfWork.FriendRequestRepository.UpdateFriendRequestAsync(friendrequest);
            _logger.LogInformation("Friend request updated with Id {friendrequestId}", result?.ID);
            return _mapper.Map<RetriveFriendRequestDTO?>(result);
        }

        public async Task<bool> DeleteFriendRequestAsync(int id)
        {
            _logger.LogInformation("Deleting friend request with Id {friendreequestId}", id);
            var exitstingfriendrequest = await _unitOfWork.FriendRequestRepository.GetFriendRequestByIdAsync(id);
            if (exitstingfriendrequest is null)
                throw new KeyNotFoundException($"Friend request with Id {id} does not exits.");
            var result = await _unitOfWork.FriendRequestRepository.DeleteFriendRequestAsync(id);
            _logger.LogInformation("Friend request with ID {friendrequestId} deleted successfully", id);
            return result;
        }

        public async Task<List<FriendRequest>?> GetFriendRequestByUserIdAsync(string id)
        {
            var friendrequest = await _unitOfWork.FriendRequestRepository.GetFriendRequestByUserIdAsync(id);
            if( friendrequest is null)
            {
                throw new KeyNotFoundException($"Friend request with Id {id} not exits");
            }
            return friendrequest;
        }

        public async Task<RetriveFriendDTO?> AcceptFriendRequestAsync(int id)
        {
            _logger.LogInformation("Adding a new friend with Friend Request ID {FriendRequestId}", id);
            if (id <= 0)
            {
                throw new ArgumentException("Invalid Friend Request ID.");
            }
            var existingFriendRequest = await _unitOfWork.FriendRequestRepository.GetFriendRequestByIdAsync(id);
            if (existingFriendRequest is null)
            {
                throw new KeyNotFoundException($"Friend Request with Id {id} not exists.");
            }

            var friendData = new FriendDTO
            {
                UserID = existingFriendRequest.SenderID,
                FriendID = existingFriendRequest.ReceiverID,
                Type_FriendsID = (int)Constants.FriendsEnum.Normal,
            };

            var friend = _mapper.Map<Friends>(friendData);
            var result = await _unitOfWork.FriendRepository.AddFriendAsync(friend);
            existingFriendRequest.status = (int)Constants.FriendRequestStatus.Accepted;
            await _unitOfWork.FriendRequestRepository.UpdateFriendRequestAsync(existingFriendRequest);
            _logger.LogInformation("Friend added with ID {FriendId}", result?.ID);
            return _mapper.Map<RetriveFriendDTO>(result);
        }

        public async Task RejectFriendRequestAsync(int id)
        {
            _logger.LogInformation("Reject friend request ID {FriendRequstId}", id);
            if (id <= 0)
            {
                throw new ArgumentException("Invalid friend request ID");
            }
            var existingFriendRequest = await _unitOfWork.FriendRequestRepository.GetFriendRequestByIdAsync(id);
            if (existingFriendRequest is null)
            {
                throw new KeyNotFoundException($"Friend Request with Id {id} not exists.");
            }

            existingFriendRequest.status = (int)Constants.FriendRequestStatus.Rejected;
            var result = await _unitOfWork.FriendRequestRepository.UpdateFriendRequestAsync(existingFriendRequest);
            _logger.LogInformation("Friend rejected with ID {FriendId}", result?.ID);
        }

        public async Task CancelFriendRequestAsync(int id)
        {
            _logger.LogInformation("Cancel friend request ID {FriendRequstId}", id);
            if (id <= 0)
            {
                throw new ArgumentException("Invalid friend request ID");
            }
            var existingFriendRequest = await _unitOfWork.FriendRequestRepository.GetFriendRequestByIdAsync(id);
            if (existingFriendRequest is null)
            {
                throw new KeyNotFoundException($"Friend Request with Id {id} not exists.");
            }

            existingFriendRequest.status = (int)Constants.FriendRequestStatus.Canceled;
            var result = await _unitOfWork.FriendRequestRepository.UpdateFriendRequestAsync(existingFriendRequest);
            _logger.LogInformation("Friend canceled with ID {FriendId}", result?.ID);
        }

        public async Task<List<FriendRequest>?> GetSentRequestAsync(string userId)
        {
            return await _unitOfWork.FriendRequestRepository.GetSentRequestAsync(userId);
        }

        public async Task<List<FriendRequest>?> GetReceivedRequestAsync(string userId)
        {
            return await _unitOfWork.FriendRequestRepository.GetReceivedRequestAsync(userId);
        }

    }
}
