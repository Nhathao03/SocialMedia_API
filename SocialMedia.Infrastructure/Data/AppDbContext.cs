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

        // POSTS
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostCategory> PostCategories { get; set; }
        public DbSet<PostImage> PostImages { get; set; }
        public DbSet<Like> Likes { get; set; }

        // COMMENTS
        public DbSet<Comment> Comments { get; set; }
        // USERS
        public DbSet<User> Users { get; set; }
        public DbSet<UserLogins> UserLogins { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<UserFollowes> UserFollowes { get; set; }

        // FRIENDS
        public DbSet<Friends> Friends { get; set; }
        public DbSet<Type_Friends> TypeFriends { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }

        // MESSAGES
        public DbSet<Message> Messages { get; set; }

        // REPORT
        public DbSet<Report> Reports { get; set; }

        // ROLE
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleCheck> RoleChecks { get; set; }

        // NOTIFICATION
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Tên bảng chuẩn
            modelBuilder.Entity<Post>().ToTable("Posts");
            modelBuilder.Entity<PostCategory>().ToTable("PostCategories");
            modelBuilder.Entity<PostImage>().ToTable("PostImages");
            modelBuilder.Entity<Like>().ToTable("Likes");

            modelBuilder.Entity<Comment>().ToTable("Comments");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<UserLogins>().ToTable("UserLogins");
            modelBuilder.Entity<Address>().ToTable("Addresses");
            modelBuilder.Entity<UserFollowes>().ToTable("UserFollowes");
            modelBuilder.Entity<UserFollowes>()
                .HasOne(uf => uf.Follower)
                .WithMany(u => u.UserFollower)
                .HasForeignKey(uf => uf.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserFollowes>()
                .HasOne(uf => uf.Following)
                .WithMany(u => u.UserFollowing)
                .HasForeignKey(uf => uf.FollowingId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friends>().ToTable("Friends");
            modelBuilder.Entity<Friends>()
                .HasOne(f => f.User)
                .WithMany(u => u.UserFriendsA)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Friends>()
                .HasOne(f => f.Friend)
                .WithMany(u => u.UserFriendsB)
                .HasForeignKey(f => f.FriendId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Type_Friends>().ToTable("TypeFriends");
            modelBuilder.Entity<FriendRequest>().ToTable("FriendRequests");
            // FriendRequest Sender -> User
            modelBuilder.Entity<FriendRequest>()
                .HasOne(fr => fr.Sender)
                .WithMany(u => u.SentFriendRequests)
                .HasForeignKey(fr => fr.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            // FriendRequest Receiver -> User
            modelBuilder.Entity<FriendRequest>()
                .HasOne(fr => fr.Receiver)
                .WithMany(u => u.ReceivedFriendRequests)
                .HasForeignKey(fr => fr.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>().ToTable("Messages");
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Report>().ToTable("Reports");

            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<RoleCheck>().ToTable("RoleChecks");

            modelBuilder.Entity<Notification>().ToTable("Notifications");
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Sender)
                .WithMany(u => u.SentNotifications)
                .HasForeignKey(n => n.SenderId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Receiver)
                .WithMany(u => u.ReceivedNotifications)
                .HasForeignKey(n => n.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
