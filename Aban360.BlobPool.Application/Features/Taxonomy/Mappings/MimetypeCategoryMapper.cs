using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;
using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using AutoMapper;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Mappings
{
    public class MimetypeCategoryMapper:Profile
    {
        public MimetypeCategoryMapper()
        {
            CreateMap<MimetypeCategoryCreateDto, MimetypeCategory>();
            CreateMap<MimetypeCategoryDeleteDto, MimetypeCategory>();
            CreateMap<MimetypeCategoryUpdateDto, MimetypeCategory>();
            CreateMap<MimetypeCategory, MimetypeCategoryGetDto>();
        }
    }
}
