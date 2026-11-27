using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;
using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using AutoMapper;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Mappings
{
    public class ExecutableMimetypeMapper:Profile
    {
        public ExecutableMimetypeMapper()
        {
            CreateMap<ExecutableMimetypeCreateDto, ExecutableMimetype>();
            CreateMap<ExecutableMimetypeDeleteDto, ExecutableMimetype>();
            CreateMap<ExecutableMimetypeUpdateDto, ExecutableMimetype>();
            CreateMap<ExecutableMimetype, ExecutableMimetypeGetDto>();
        }
    }
}
