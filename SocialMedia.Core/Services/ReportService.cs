using SocialMedia.Infrastructure.Repositories;
using Social_Media.Helpers;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Entities.DTO;
using SocialMedia.Core.DTO.Report;
using SocialMedia.Core.Interfaces.ServiceInterfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace SocialMedia.Core.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ReportService> _logger;

        public ReportService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ReportService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<RetriveReportDTO?> CreateReportAsync(ReportDTO dto)
        {
            _logger.LogInformation("Creating a new report");
            if (dto == null)
                throw new ArgumentNullException(nameof(ReportDTO), "Report data is required.");
            var report = _mapper.Map<Report>(dto);
            var result = await _unitOfWork.ReportRepository.AddAsync(report);
            _logger.LogInformation("Report created with Id {ReportId}", result?.Id);
            return _mapper.Map<RetriveReportDTO>(result);
        }

        public async Task<RetriveReportDTO?> GetReportAsync(int Id)
        {
            if (Id <= 0)
                throw new ArgumentException("InvalId report Id.", nameof(Id));
            var report = await _unitOfWork.ReportRepository.GetByIdAsync(Id);
            _logger.LogInformation("Retrieved report with Id {ReportId}", Id);
            return _mapper.Map<RetriveReportDTO>(report);
        }

        public async Task<IEnumerable<RetriveReportDTO?>?> GetAllReportsAsync()
        {
            _logger.LogInformation("Retrieving all reports");
            var reports = await _unitOfWork.ReportRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<RetriveReportDTO>>(reports);
        }

        public async Task<IEnumerable<RetriveReportDTO?>?> GetReportsByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User Id cannot be null or empty.", nameof(userId));
            _logger.LogInformation("Retrieving reports for user {UserId}", userId);
            var reports = await _unitOfWork.ReportRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<RetriveReportDTO>>(reports);
        }

        public async Task<RetriveReportDTO?> UpdateStatusAsync(int Id, ReportStatus status)
        {
            if (Id <= 0)
                throw new ArgumentException("InvalId report Id.", nameof(Id));
            _logger.LogInformation("Updating status of report Id {ReportId} to {Status}", Id, status);
            var report = await _unitOfWork.ReportRepository.GetByIdAsync(Id);
            if (report == null)
            {
                _logger.LogWarning("Report with Id {ReportId} not found", Id);
                return null;
            }
            report.ReportStatus = status;
            var updatedReport = await _unitOfWork.ReportRepository.UpdateAsync(report);
            _logger.LogInformation("Report Id {ReportId} status updated to {Status}", Id, status);
            return _mapper.Map<RetriveReportDTO>(updatedReport);
        }

        public async Task<RetriveReportDTO?> UpdateActionAsync(int Id, string actionTaken)
        {
            if (Id <= 0)
                throw new ArgumentException("InvalId report Id.", nameof(Id));
            if (string.IsNullOrEmpty(actionTaken))
                throw new ArgumentException("Action taken cannot be null or empty.", nameof(actionTaken));
            _logger.LogInformation("Updating action of report Id {ReportId} to {ActionTaken}", Id, actionTaken);
            var report = await _unitOfWork.ReportRepository.GetByIdAsync(Id);
            if (report == null)
            {
                _logger.LogWarning("Report with Id {ReportId} not found", Id);
                return null;
            }
            var notification = new Notification
            {
                SenderId = report.ReporterId,
                TargetType = TargetTypeEnum.Report,
                NotificationType = NotificationTypeEnum.ReportUpdated,
                TargetId = report.Id,
                Content = $"Action taken on your report (Id: {report.Id}): {actionTaken}",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };
            await _unitOfWork.NotificationRepository.AddNotificationAsync(notification);
            return _mapper.Map<RetriveReportDTO>(report);
        }

        public async Task DeleteAsync(int Id)
        {
            if (Id <= 0)
                throw new ArgumentException("InvalId report Id.", nameof(Id));
            _logger.LogInformation("Deleting report with Id {ReportId}", Id);
            await _unitOfWork.ReportRepository.DeleteAsync(Id);
            _logger.LogInformation("Report with Id {ReportId} deleted", Id);
        }
    }
}
