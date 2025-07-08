using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Mappings
{
    public class CustomerInfoMapper : Profile
    {
        public CustomerInfoMapper()
        {
            CreateMap<CustomerInfoLevel1UpdateDto,CustomerInfoUpdateDto>();
            CreateMap<CustomerInfoLevel2UpdateDto,CustomerInfoUpdateDto>();
        }
    }
}
