using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.DTO.Post;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Entities.CommentEntity;
using SocialMedia.Core.Entities.DTO;
using SocialMedia.Core.Entities.DTO.Comment;
using SocialMedia.Core.Entities.Entity;
using SocialMedia.Core.Entities.PostEntity;
using SocialMedia.Infrastructure.Data;
using System.Drawing.Printing;

namespace SocialMedia.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;

        public PostRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Post?> GetPostByIdAsync(int id)
        {
            return await _context.posts
                .Include(p => p.PostCategory)
                .Include(p => p.PostImages)
                .FirstOrDefaultAsync(p => p.ID == id);
        }

        public async Task<Post?> AddPostAsync(Post post)
        {
            _context.posts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }
        public async Task<Post?> UpdatePostAsync(Post post)
        {
            _context.posts.Update(post);
            await _context.SaveChangesAsync();
            return post;
        }
        public async Task<bool> DeletePost(int id)
        {
            var post = await _context.posts.FindAsync(id);
            if (post is null)
                return false;
            _context.posts.Remove(post);
            await _context.SaveChangesAsync();
            return true;
        }

        //public async Task<IEnumerable<RetrivePostDTO>?> GetPostsByUserIdAsync(string userId)
        //{
        //    var posts = await _context.posts
        //        .Where(p => p.UserID == userId)
        //        .Include(p => p.PostCategory)
        //        .Include(p => p.user)
        //        .Include(p => p.PostImages)
        //        .Include(p => p.Comments)
        //        .Select(p => new RetrivePostDTO
        //        {
        //            Id = p.ID,
        //            UserID = p.UserID,
        //            Content = p.Content,
        //            Views = p.Views,
        //            Share = p.Share,
        //            CreatedAt = p.CreatedAt,
        //            UpdatedAt = p.UpdatedAt,

        //            // Post Category
        //            PostCategory = p.PostCategory == null ? null : new PostCategory
        //            {
        //                Name = p.PostCategory.Name
        //            },

        //            // User info
        //            user = p.user == null ? null : new User
        //            {
        //                Id = p.user.Id,
        //                Fullname = p.user.Fullname,
        //                Avatar = p.user.Avatar
        //            },

        //            // Post images
        //            PostImages = p.PostImages.Select(pi => new PostImage
        //            {
        //                ID = pi.ID,
        //                PostId = pi.PostId,
        //                Url = pi.Url
        //            }).ToList(),

        //            // Comments 
        //            Comments = p.Comments.Select(c => new Comment
        //            {
        //                ID = c.ID,
        //                UserId = c.UserId,
        //                PostId = c.PostId,
        //                Content = c.Content,
        //                CreatedAt = c.CreatedAt,
        //                UpdatedAt = c.UpdatedAt,

        //                // Comment Replies
        //                CommentReplies = _context.commentReplies
        //                    .Where(crp => crp.CommentId == c.ID)
        //                    .Select(crp => new CommentReplies
        //                    {
        //                        Id = crp.Id,
        //                        UserId = crp.UserId,
        //                        CommentId = crp.CommentId,
        //                        Content = crp.Content,
        //                        Image = crp.Image,
        //                        CreatedAt = crp.CreatedAt,
        //                        UpdatedAt = crp.UpdatedAt
        //                    }).ToList()
        //            }).ToList(),

        //            // Likes 
        //            Likes = _context.likes
        //                .Where(l => l.EntityId == p.ID)
        //                .Select(l => new Like
        //                {
        //                    Id = l.Id,
        //                    UserId = l.UserId,
        //                    EntityId = l.EntityId
        //                }).ToList()
        //        })
        //        .ToListAsync();

        //    return posts;
        //}
        //public async Task<IEnumerable<RetrivePostDTO>?> GetPostsByUserIdAsync(string userId)
        //{
        //    var posts = await _context.posts
        //        .Where(p => p.UserID == userId)
        //        .Include(p => p.PostCategory)
        //        .Include(p => p.user)
        //        .Include(p => p.PostImages)
        //        .Include(p => p.Comments)
        //        .Select(p => new RetrivePostDTO
        //        {
        //            Id = p.ID,
        //            UserID = p.UserID,
        //            Content = p.Content,
        //            Views = p.Views,
        //            Share = p.Share,
        //            CreatedAt = p.CreatedAt,
        //            UpdatedAt = p.UpdatedAt,

        //            // Post Category
        //            PostCategory = p.PostCategory == null ? null : new PostCategory
        //            {
        //                Name = p.PostCategory.Name
        //            },

        //            // User info
        //            user = p.user == null ? null : new User
        //            {
        //                Id = p.user.Id,
        //                Fullname = p.user.Fullname,
        //                Avatar = p.user.Avatar
        //            },

        //            // Post images
        //            PostImages = p.PostImages.Select(pi => new PostImage
        //            {
        //                ID = pi.ID,
        //                PostId = pi.PostId,
        //                Url = pi.Url
        //            }).ToList(),

        //            // Comments 
        //            Comments = p.Comments.Select(c => new Comment
        //            {
        //                ID = c.ID,
        //                UserId = c.UserId,
        //                PostId = c.PostId,
        //                Content = c.Content,
        //                CreatedAt = c.CreatedAt,
        //                UpdatedAt = c.UpdatedAt,

        //                // Comment Replies
        //                CommentReplies = _context.commentReplies
        //                    .Where(crp => crp.CommentId == c.ID)
        //                    .Select(crp => new CommentReplies
        //                    {
        //                        Id = crp.Id,
        //                        UserId = crp.UserId,
        //                        CommentId = crp.CommentId,
        //                        Content = crp.Content,
        //                        Image = crp.Image,
        //                        CreatedAt = crp.CreatedAt,
        //                        UpdatedAt = crp.UpdatedAt
        //                    }).ToList()
        //            }).ToList(),

        //            // Likes 
        //            Likes = _context.likes
        //                .Where(l => l.EntityId == p.ID)
        //                .Select(l => new Like
        //                {
        //                    Id = l.Id,
        //                    UserId = l.UserId,
        //                    EntityId = l.EntityId
        //                }).ToList()
        //        })
        //        .ToListAsync();

        //    return posts;
        //}

        public async Task<IEnumerable<Post>?> GetRecentPostsAsync(int page, int pageSize)
        {
            return await _context.posts
                .Include(p => p.PostCategory)
                .Include(p => p.PostImages)
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
