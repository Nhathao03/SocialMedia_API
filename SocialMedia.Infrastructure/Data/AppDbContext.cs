using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Entities.CommentEntity;
using SocialMedia.Core.Entities.Entity;
using SocialMedia.Core.Entities.FriendEntity;
using SocialMedia.Core.Entities.PostEntity;
using SocialMedia.Core.Entities.RoleEntity;
using SocialMedia.Core.Entities.UserEntity;

namespace SocialMedia.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Post> posts { get; set; }
        public DbSet<Comment> comments { get; set; }
        public DbSet<Like> likes { get; set; }
        public DbSet<Message> messages { get; set; }
        public DbSet<PostCategory> post_category { get; set; }
        public DbSet<PostImage> post_image { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<UserLogins> usersLogins { get; set; }
        public DbSet<Address> addresses { get; set; }
        public DbSet<Friends> friends { get; set; }
        public DbSet<Type_Friends> type_friends { get; set; }
        public DbSet<FriendRequest> friendRequests { get; set; }
        public DbSet<Report> reports { get; set; }
        public DbSet<Role> role { get; set; }
        public DbSet<RoleCheck> roleChecks { get; set; }
        public DbSet<Notification> notifications { get; set; }
        public DbSet<CommentReplies> commentReplies { get; set; }
    }
}
