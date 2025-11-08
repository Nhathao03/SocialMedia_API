using Social_Media.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.DTO.Report
{
    public class RetriveReportDTO
    {
        public int Id { get; set; }
        public int ReportedId { get; set; }
        public string ReporterId { get; set; }
        public string Description { get; set; }
        public ReportType ReportType { get; set; }
        public ReportStatus ReportStatus { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } 
    }
}
