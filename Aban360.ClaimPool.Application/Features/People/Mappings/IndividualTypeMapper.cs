using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Mappings
{
    public class IndividualTypeTypeMapper : Profile
    {
        public IndividualTypeTypeMapper()
        {
            CreateMap<IndividualTypeCreateDto, IndividualType>().ReverseMap();
            CreateMap<IndividualTypeDeleteDto, IndividualType>().ReverseMap();
            CreateMap<IndividualTypeUpdateDto, IndividualType>().ReverseMap();
            CreateMap<IndividualTypeGetDto, IndividualType>().ReverseMap();
        }
    }
}
