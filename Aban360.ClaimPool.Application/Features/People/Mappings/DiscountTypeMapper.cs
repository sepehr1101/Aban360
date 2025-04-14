using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Mappings
{
    public class DiscountTypeMapper : Profile
    {
        public DiscountTypeMapper()
        {
            CreateMap<DiscountType,DiscountTypeGetDto>();
        }
    }
}
