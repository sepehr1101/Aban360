using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Mappings
{
    public class HandoverMapper : Profile
    {
        public HandoverMapper()
        {
            CreateMap<HandoverCreateDto, Handover>();
            CreateMap<HandoverDeleteDto, Handover>();
            CreateMap<HandoverUpdateDto, Handover>();
            CreateMap<Handover, HandoverGetDto>();
        }
    }
}