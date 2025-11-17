using Social_Media.Helpers;
using SocialMedia.Core.DTO.Report;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Entities.DTO;

namespace SocialMedia.Core.Services
{
    public interface IReportService
    {
        Task<RetriveReportDTO?> CreateReportAsync(ReportDTO dto);
        Task<RetriveReportDTO?> GetReportAsync(int Id);
        Task<IEnumerable<RetriveReportDTO?>?> GetAllReportsAsync();
        Task<IEnumerable<RetriveReportDTO?>?> GetReportsByUserIdAsync(string userId);
        Task<RetriveReportDTO?> UpdateStatusAsync(int Id, ReportStatus status);
        Task<RetriveReportDTO?> UpdateActionAsync(int Id, string action);
        Task DeleteAsync(int Id);
    }
}
