using SocialMedia.Core.Entities;
using SocialMedia.Core.Entities.DTO;

namespace SocialMedia.Infrastructure.Repositories
{
    public interface IReportRepository
    {
        Task<Report?> AddAsync(Report model);
        Task<IEnumerable<Report?>?> GetAllAsync();
        Task DeleteAsync(int Id);
        Task<Report?> UpdateAsync(Report report);
        Task<Report?> GetByIdAsync(int Id);
        Task<IEnumerable<Report?>?> GetByUserIdAsync(string userId);
    }
}
