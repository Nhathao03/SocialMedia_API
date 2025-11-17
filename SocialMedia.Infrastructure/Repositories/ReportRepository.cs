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
            _context.Reports.AddAsync(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<IEnumerable<Report?>?> GetAllAsync()
        {
            return await _context.Reports.ToListAsync();
        }

        public async Task<Report?> GetByIdAsync(int Id)
        {
            return await _context.Reports.FirstOrDefaultAsync(r => r.Id == Id);
        }

        public async Task<IEnumerable<Report?>?> GetByUserIdAsync(string userId)
        {
            return await _context.Reports.Where(r => r.ReporterId == userId).ToListAsync();
        }

        public async Task DeleteAsync(int Id)
        {
            var report = await _context.Reports.FindAsync(Id);
            if (report != null)
            {
                _context.Reports.Remove(report);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Report?> UpdateAsync(Report model)
        {
            _context.Reports.Update(model);
            await _context.SaveChangesAsync();
            return model;
        }
    }
}
