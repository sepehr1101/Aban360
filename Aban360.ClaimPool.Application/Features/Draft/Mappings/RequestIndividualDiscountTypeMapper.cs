using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Mappings
{
    public class RequestIndividualDiscountTypeMapper : Profile
    {
        public RequestIndividualDiscountTypeMapper()
        {
            CreateMap<RequestIndividualDiscountTypeCreateDto, RequestIndividualDiscountType>();
            CreateMap<RequestIndividualDiscountTypeDeleteDto, RequestIndividualDiscountType>();
            CreateMap<RequestIndividualDiscountTypeUpdateDto, RequestIndividualDiscountType>();
            CreateMap<RequestIndividualDiscountType, RequestIndividualDiscountTypeGetDto>();
        }
    }
}
