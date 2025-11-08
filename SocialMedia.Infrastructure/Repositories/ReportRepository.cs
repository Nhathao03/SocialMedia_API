using Microsoft.EntityFrameworkCore;
using Social_Media.Helpers;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Entities.DTO;
using SocialMedia.Infrastructure.Data;
using System.Reflection.Metadata;

namespace SocialMedia.Infrastructure.Repositories
{
    public class ReportRepository :IReportRepository
    {
        private readonly AppDbContext _context;
        public ReportRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Report?> AddAsync(Report model)
        {
            _context.reports.AddAsync(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<IEnumerable<Report?>?> GetAllAsync()
        {
            return await _context.reports.ToListAsync();
        }

        public async Task<Report?> GetByIdAsync(int id)
        {
            return await _context.reports.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Report?>?> GetByUserIdAsync(string userId)
        {
            return await _context.reports.Where(r => r.ReporterId == userId).ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var report = await _context.reports.FindAsync(id);
            if (report != null)
            {
                _context.reports.Remove(report);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Report?> UpdateAsync(Report model)
        {
            _context.reports.Update(model);
            await _context.SaveChangesAsync();
            return model;
        }
    }
}
