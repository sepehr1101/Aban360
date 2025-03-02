using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Mappings
{
    public class EstateBoundTypeBoundTypeMapper:Profile
    {
        public EstateBoundTypeBoundTypeMapper()
        {
            CreateMap<EstateBoundTypeCreateDto, EstateBoundType>().ReverseMap();
            CreateMap<EstateBoundTypeDeleteDto, EstateBoundType>().ReverseMap();
            CreateMap<EstateBoundTypeUpdateDto, EstateBoundType>().ReverseMap();
            CreateMap<EstateBoundTypeGetDto, EstateBoundType>().ReverseMap();
        }
    }
}
