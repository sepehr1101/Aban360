using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Mappings
{
    public class IndividualDiscountTypeMapper : Profile
    {
        public IndividualDiscountTypeMapper()
        {
            CreateMap<IndividualDiscountTypeCreateDto, IndividualDiscountType>();
            CreateMap<IndividualDiscountTypeDeleteDto, IndividualDiscountType>();
            CreateMap<IndividualDiscountTypeUpdateDto, IndividualDiscountType>();
            CreateMap<IndividualDiscountType,IndividualDiscountTypeGetDto>();
        }
    }
}
