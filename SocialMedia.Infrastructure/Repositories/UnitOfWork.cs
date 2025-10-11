using SocialMedia.Core.Interfaces.RepositoriesInterfaces;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Core.Interfaces.ServiceInterfaces;

namespace SocialMedia.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IAddressRepository AddressRepository { get; }
        public ICommentRepository CommentRepository { get; }
        public ICommentReactionsRepository CommentReactionsRepository { get; }
        public ICommentRepliesRepository CommentRepliesRepository { get; }
        public IFriendRepository FriendRepository { get; }
        public IFriendRequestRepository FriendRequestRepository { get; }
        public ILikeRepository LikeRepository { get; }
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

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            AddressRepository = new AddressRepository(_context);
            CommentRepository = new CommentRepository(_context);
            CommentReactionsRepository = new CommentReactionsRepository(_context);
            CommentRepliesRepository = new CommentRepliesRepository(_context);
            FriendRepository = new FriendRepository(_context);
            FriendRequestRepository = new FriendRequestRepository(_context);
            LikeRepository = new LikeRepository(_context);
            MessageRepository = new MessageRepository(_context);
            NotificationRepository = new NotificationRepository(_context);
            PostImageRepository = new PostImageRepository(_context);
            PostCategoryRepository = new PostCategoryRepository(_context);
            PostRepository = new PostRepository(_context);
            ReportRepository = new ReportRepository(_context);
            RoleCheckRepository = new RoleCheckRepository(_context, RoleRepository);
            RoleRepository = new RoleRepository(_context);
            SearchRepository = new SearchRepository(_context);
            TypeFriendsRepository = new TypeFriendsRepository(_context);
            UserLoginRepository = new UserLoginRepository(_context);
            UserRepository = new UserRepository(_context);
        }
    }
}
