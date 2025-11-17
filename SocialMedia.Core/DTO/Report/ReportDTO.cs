using Social_Media.Helpers;
using SocialMedia.Core.Entities.Entity;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Core.DTO.Report
{
    public class ReportDTO
    {
        [Required]
        public int ReportedEntityId { get; set; }
        public ReportEntityType EntityType { get; set; }
        [Required]
        public string ReporterId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Reason { get; set; }
        public ReportStatus ReportStatus { get; set; } = ReportStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
