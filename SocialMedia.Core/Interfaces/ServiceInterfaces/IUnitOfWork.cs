using SocialMedia.Core.Interfaces.RepositoriesInterfaces;
using SocialMedia.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces.ServiceInterfaces
{
    public interface IUnitOfWork
    {
        public IAddressRepository AddressRepository { get; }
        public ICommentRepository CommentRepository { get; }
        public IFriendRepository FriendRepository { get; }
        public IFriendRequestRepository FriendRequestRepository { get; }
        public ILikeRepository LikePostRepository { get; }
        public IMessageRepository MessageRepository { get; }
        public INotificationRepository NotificationRepository { get; }
        public IPostImageRepository PostImageRepository { get; }
        public IPostCategoryRepository PostCategoryRepository { get; }
        public IPostRepository PostRepository { get; }
        public IReportRepository ReportRepository { get; }
        public IRoleCheckRepository RoleCheckRepository { get; }
        public IRoleRepository RoleRepository { get; }
        public ISearchRepository SearchRepository { get; }
        public ITypeFriendsRepository TypeFriendsRepository { get; }
        public IUserLoginRepository UserLoginRepository { get; }
        public IUserRepository UserRepository { get; }
    }
}
