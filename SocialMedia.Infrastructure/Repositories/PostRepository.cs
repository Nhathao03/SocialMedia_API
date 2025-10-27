using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.DTO.Post;
using SocialMedia.Core.Entities.DTO;
using SocialMedia.Core.Entities.DTO.Comment;
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

        public async Task<IEnumerable<Post>> GetAllPost()
        {
            return await _context.posts.Include(p => p.Comments).Include(p => p.Likes).Include(p => p.PostCategory).Include(p => p.PostImages).ToListAsync();
        }

        public async Task<IEnumerable<PostDTO>> GetRecentPostsAsync(int page , int pageSize)
        {
            var query = _context.posts
                .Include(p => p.Comments).ThenInclude(c => c.user)
                .Include(p => p.Likes).ThenInclude(l => l.user)
                .Include(p => p.PostCategory)
                .Include(p => p.user)
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PostDTO
                {
                    postId = p.ID,
                    UserId = p.UserID.ToString(),
                    Content = p.Content,
                    ViewsCount = p.Views,
                    SharesCount = p.Share,
                    Username = p.user != null ? p.user.Fullname : null,
                    UserAvatar = p.user != null ? p.user.Avatar : null,
                    Comments = p.Comments.Select(c => new CommentDTO
                    {
                        UserId = c.UserId,
                        PostId = c.PostId,
                        Content = c.Content,
                        CreatedAt = c.CreatedAt,
                        Image = c.Image,
                    }).ToList(),
                    Likes = p.Likes.Select(l => new LikeDTO
                    {
                        UserID = l.UserId,
                        postID = l.PostId,
                        CreatedAt = l.CreatedAt,
                        UserName_like = l.user != null ? l.user.Fullname : null,
                    }).ToList(),
                    postCategory = p.PostCategory != null ? new PostCategoryDTO
                    {
                        Name = p.PostCategory.Name
                    } : null,
                    PostImages = p.PostImages.Select(i => new PostImageDTO
                    {
                        ID = i.ID,
                        Url = i.Url,
                        PostId = i.PostId
                    }).ToList(),
                });

            return await query.ToListAsync();
        }

        public async Task<Post> GetPostById(int id)
        {
            return await _context.posts.Include(p => p.Comments).Include(p => p.Likes).Include(p => p.PostCategory).Include(p => p.PostImages).FirstOrDefaultAsync(p => p.ID == id);
        }

        public async Task<Post> AddPost(Post post)
        {
            _context.posts.AddAsync(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task UpdatePost(Post post)
        {
            _context.posts.Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePost(int id)
        {
            var post = await _context.posts.FindAsync(id);
            if (post != null)
            {
                _context.posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }

        //Get posts by userID
        public async Task<IEnumerable<PostDTO>> GetPostsByUserID(string userID)
        {
           return await _context.posts
                .Include(p => p.Comments).ThenInclude(c => c.user)
                .Include(p => p.Likes).ThenInclude(l => l.user)
                .Include(p => p.PostCategory)
                .Include(p => p.user)
                .OrderByDescending(p => p.CreatedAt)
                .Where(p => p.UserID == userID)
                .Select(p => new PostDTO
                {
                    postId = p.ID,
                    UserId = p.UserID.ToString(),
                    Content = p.Content,
                    ViewsCount = p.Views,
                    SharesCount = p.Share,
                    Username = p.user != null ? p.user.Fullname : null,
                    UserAvatar = p.user != null ? p.user.Avatar : null,
                    Comments = p.Comments.Select(c => new CommentDTO
                    {
                        UserId = c.UserId,
                        PostId = c.PostId,
                        Content = c.Content,
                        CreatedAt = c.CreatedAt,
                        Image = c.Image,
                    }).ToList(),
                    Likes = p.Likes.Select(l => new LikeDTO
                    {
                        UserID = l.UserId,
                        postID = l.PostId,
                        CreatedAt = l.CreatedAt,
                        UserName_like = l.user != null ? l.user.Fullname : null,
                    }).ToList(),
                    postCategory = p.PostCategory != null ? new PostCategoryDTO
                    {
                        Name = p.PostCategory.Name
                    } : null,
                    PostImages = p.PostImages.Select(i => new PostImageDTO
                    {
                        ID = i.ID,
                        Url = i.Url,
                        PostId = i.PostId
                    }).ToList(),
                }).ToListAsync();
        }

    }
}
