using Social_Media.Helpers;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Core.DTO.Report
{
    public class ReportDTO
    {
        [Required]
        public string ReporterId { get; set; }
        [Required]
        public int ReportedId { get; set; }
        [Required]
        public string Reason { get; set; }
        public string Description { get; set; }
        [Required]
        public ReportType ReportType { get; set; }

    }
}
