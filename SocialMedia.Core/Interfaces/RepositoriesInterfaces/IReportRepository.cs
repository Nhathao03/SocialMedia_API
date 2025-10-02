using SocialMedia.Core.Entities;
using SocialMedia.Core.Entities.DTO;

namespace SocialMedia.Infrastructure.Repositories
{
    public interface IReportRepository
    {
        Task CreateReport(Report model);
        Task<IEnumerable<Report>> GetAllReports();
        Task DeleteReport(int reportId);
        Task<Report> GetReportById(int reportId);
        Task MarkReportAsReviewed(int reportId);
        Task<IEnumerable<Report>> GetReportsByUserId(string userId);
    }
}
