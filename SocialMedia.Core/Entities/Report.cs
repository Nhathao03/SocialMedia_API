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
        public int ReportedEntityId { get; set; }
        public ReportEntityType EntityType { get; set; }
        public string ReporterId { get; set; }
        public User Reporter { get; set; }
        public string Description { get; set; }
        public string Reason { get; set; }
        public ReportStatus ReportStatus { get; set; } = ReportStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
