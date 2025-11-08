using SocialMedia.Core.Entities;
using SocialMedia.Core.Entities.DTO;

namespace SocialMedia.Infrastructure.Repositories
{
    public interface IReportRepository
    {
        Task<Report?> AddAsync(Report model);
        Task<IEnumerable<Report?>?> GetAllAsync();
        Task DeleteAsync(int id);
        Task<Report?> UpdateAsync(Report report);
        Task<Report?> GetByIdAsync(int id);
        Task<IEnumerable<Report?>?> GetByUserIdAsync(string userId);
    }
}
