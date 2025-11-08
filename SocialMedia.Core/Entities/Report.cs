using Social_Media.Helpers;
using SocialMedia.Core.Entities.Entity;
using SocialMedia.Core.Entities.PostEntity;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace SocialMedia.Core.Entities
{
    public class Report
    {
        [Key]
        public int Id { get; set; }
        public int ReportedId { get; set; }
        public string ReporterId { get; set; }
        public string Description { get; set; }
        public ReportType ReportType { get; set; }
        public ReportStatus ReportStatus { get; set; } = ReportStatus.Pending;
        public string Reason { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relationship
        public User? User { get; set; }
    }

}
