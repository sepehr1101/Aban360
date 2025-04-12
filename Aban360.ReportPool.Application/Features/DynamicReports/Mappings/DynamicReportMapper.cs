using Aban360.ReportPool.Domain.Features.DynamicReports.Dto.Commands;
using Aban360.ReportPool.Domain.Features.DynamicReports.Dto.Queries;
using Aban360.ReportPool.Domain.Features.DynamicReports.Entities;
using AutoMapper;

namespace Aban360.ReportPool.Application.Features.DynamicReports.Mappings
{
    public class DynamicReportMapper : Profile
    {
        public DynamicReportMapper()
        {
            CreateMap<DynamicReportCreateDto, DynamicReport>();
            CreateMap<DynamicReportDeleteDto, DynamicReport>();
            CreateMap<DynamicReportUpdateDto, DynamicReport>();
            CreateMap<DynamicReport, DynamicReportGetDto>();
        }
    }
}
