using SocialMedia.Core.Entities;
using SocialMedia.Core.Entities.DTO;

namespace SocialMedia.Core.Services
{
    public interface IReportService
    {
        Task CreateReport(ReportDTO model);
        Task<IEnumerable<Report>> GetAllReports();
        Task DeleteReport(int reportId);
        Task<Report> GetReportById(int reportId);
        Task MarkReportAsReviewed(int reportId);
    }
}
