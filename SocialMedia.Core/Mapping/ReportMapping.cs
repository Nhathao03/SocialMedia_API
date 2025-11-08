using AutoMapper;
using SocialMedia.Core.DTO.Report;
using SocialMedia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Mapping
{
    public class ReportMapping : Profile
    {
        public ReportMapping() 
        {
            CreateMap<Report, ReportDTO>().ReverseMap();
            CreateMap<Report, RetriveReportDTO>().ReverseMap();
        }
    }
}
