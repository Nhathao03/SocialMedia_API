using Microsoft.EntityFrameworkCore;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Core.Entities.Entity;

namespace SocialMedia.Infrastructure.Repositories
{
    public class SearchRepository : ISearchRepository
    {
        private readonly AppDbContext _context;
        public SearchRepository(AppDbContext context)
        {
            _context = context;
        }
    }
}
