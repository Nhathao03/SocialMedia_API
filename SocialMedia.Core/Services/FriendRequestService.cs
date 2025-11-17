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

        public async Task<FriendRequest?> GetFriendRequestByIdAsync(int Id)
        {
            return await _unitOfWork.FriendRequestRepository.GetFriendRequestByIdAsync(Id);
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
                switch (existingRequest.Status)
                {
                    case (int)Constants.FriendRequestStatus.Pending:
                        if (existingRequest.SenderId == dto.ReceiverId)
                        {
                            existingRequest.Status = (int)Constants.FriendRequestStatus.Accepted;
                            existingRequest.CreatedAt = DateTime.UtcNow;
                            await _unitOfWork.FriendRequestRepository.UpdateFriendRequestAsync(existingRequest);

                            _logger.LogInformation(
                                "Mutual request detected. Friend request between {SenderId} and {ReceiverId} auto-accepted.",
                                dto.SenderId, dto.ReceiverId);

                            var addFriend = new FriendDTO
                            {
                                UserId = existingRequest.SenderId,
                                FriendId = existingRequest.ReceiverId,
                                Type_FriendsId = (int)Constants.FriendsStatus.Normal
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
            friendRequest.Status = (int)Constants.FriendRequestStatus.Pending;

            var result = await _unitOfWork.FriendRequestRepository.AddFriendRequestAsync(friendRequest);
            return _mapper.Map<RetriveFriendRequestDTO>(result);
        }

        public async Task<RetriveFriendRequestDTO?> UpdateFriendRequestAsync(int Id, FriendRequestDTO dto)
        {
            _logger.LogInformation("Updating friend request with Id {CommentId}", Id);
            var existingFriendrequest = _unitOfWork.FriendRequestRepository.GetFriendRequestByIdAsync(Id);
            if (existingFriendrequest is null)
            {
                throw new KeyNotFoundException($"Friend request with Id {Id} not exits");
            }

            var friendrequest = _mapper.Map<FriendRequest>(dto);
            var result = await _unitOfWork.FriendRequestRepository.UpdateFriendRequestAsync(friendrequest);
            _logger.LogInformation("Friend request updated with Id {friendrequestId}", result?.Id);
            return _mapper.Map<RetriveFriendRequestDTO?>(result);
        }

        public async Task<bool> DeleteFriendRequestAsync(int Id)
        {
            _logger.LogInformation("Deleting friend request with Id {friendreequestId}", Id);
            var exitstingfriendrequest = await _unitOfWork.FriendRequestRepository.GetFriendRequestByIdAsync(Id);
            if (exitstingfriendrequest is null)
                throw new KeyNotFoundException($"Friend request with Id {Id} does not exits.");
            var result = await _unitOfWork.FriendRequestRepository.DeleteFriendRequestAsync(Id);
            _logger.LogInformation("Friend request with Id {friendrequestId} deleted successfully", Id);
            return result;
        }

        public async Task<List<FriendRequest>?> GetFriendRequestByUserIdAsync(string Id)
        {
            var friendrequest = await _unitOfWork.FriendRequestRepository.GetFriendRequestByUserIdAsync(Id);
            if( friendrequest is null)
            {
                throw new KeyNotFoundException($"Friend request with Id {Id} not exits");
            }
            return friendrequest;
        }

        public async Task<RetriveFriendDTO?> AcceptFriendRequestAsync(int Id)
        {
            _logger.LogInformation("Adding a new friend with Friend Request Id {FriendRequestId}", Id);
            if (Id <= 0)
            {
                throw new ArgumentException("InvalId Friend Request Id.");
            }
            var existingFriendRequest = await _unitOfWork.FriendRequestRepository.GetFriendRequestByIdAsync(Id);
            if (existingFriendRequest is null)
            {
                throw new KeyNotFoundException($"Friend Request with Id {Id} not exists.");
            }

            var friendData = new FriendDTO
            {
                UserId = existingFriendRequest.SenderId,
                FriendId = existingFriendRequest.ReceiverId,
                Type_FriendsId = (int)Constants.FriendsStatus.Normal,
            };

            var friend = _mapper.Map<Friends>(friendData);
            var result = await _unitOfWork.FriendRepository.AddFriendAsync(friend);
            existingFriendRequest.Status = (int)Constants.FriendRequestStatus.Accepted;
            await _unitOfWork.FriendRequestRepository.UpdateFriendRequestAsync(existingFriendRequest);
            _logger.LogInformation("Friend added with Id {FriendId}", result?.Id);
            return _mapper.Map<RetriveFriendDTO>(result);
        }

        public async Task RejectFriendRequestAsync(int Id)
        {
            _logger.LogInformation("Reject friend request Id {FriendRequstId}", Id);
            if (Id <= 0)
            {
                throw new ArgumentException("InvalId friend request Id");
            }
            var existingFriendRequest = await _unitOfWork.FriendRequestRepository.GetFriendRequestByIdAsync(Id);
            if (existingFriendRequest is null)
            {
                throw new KeyNotFoundException($"Friend Request with Id {Id} not exists.");
            }

            existingFriendRequest.Status = (int)Constants.FriendRequestStatus.Rejected;
            var result = await _unitOfWork.FriendRequestRepository.UpdateFriendRequestAsync(existingFriendRequest);
            _logger.LogInformation("Friend rejected with Id {FriendId}", result?.Id);
        }

        public async Task CancelFriendRequestAsync(int Id)
        {
            _logger.LogInformation("Cancel friend request Id {FriendRequstId}", Id);
            if (Id <= 0)
            {
                throw new ArgumentException("InvalId friend request Id");
            }
            var existingFriendRequest = await _unitOfWork.FriendRequestRepository.GetFriendRequestByIdAsync(Id);
            if (existingFriendRequest is null)
            {
                throw new KeyNotFoundException($"Friend Request with Id {Id} not exists.");
            }

            existingFriendRequest.Status = (int)Constants.FriendRequestStatus.Canceled;
            var result = await _unitOfWork.FriendRequestRepository.UpdateFriendRequestAsync(existingFriendRequest);
            _logger.LogInformation("Friend canceled with Id {FriendId}", result?.Id);
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
