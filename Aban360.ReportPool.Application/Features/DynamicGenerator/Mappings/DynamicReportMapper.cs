using Aban360.ReportPool.Domain.Features.DynamicGenerator.Dto.Commands;
using Aban360.ReportPool.Domain.Features.DynamicGenerator.Dto.Queries;
using Aban360.ReportPool.Domain.Features.DynamicGenerator.Entities;
using AutoMapper;

namespace Aban360.ReportPool.Application.Features.DynamicGenerator.Mappings
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
