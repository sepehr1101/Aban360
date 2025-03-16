using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Mappings
{
    public class EstateBoundTypeBoundTypeMapper:Profile
    {
        public EstateBoundTypeBoundTypeMapper()
        {
            CreateMap<EstateBoundTypeCreateDto, EstateBoundType>();
            CreateMap<EstateBoundTypeDeleteDto, EstateBoundType>();
            CreateMap<EstateBoundTypeUpdateDto, EstateBoundType>();
            CreateMap<EstateBoundType,EstateBoundTypeGetDto>();   
        }
    }
}
