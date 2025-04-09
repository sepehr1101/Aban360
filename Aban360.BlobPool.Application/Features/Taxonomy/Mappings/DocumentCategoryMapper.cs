using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;
using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using AutoMapper;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Mappings
{
    public class DocumentCategoryMapper:Profile
    {
        public DocumentCategoryMapper()
        {
            CreateMap<DocumentCategoryCreateDto, DocumentCategory>();
            CreateMap<DocumentCategoryDeleteDto, DocumentCategory>();
            CreateMap<DocumentCategoryUpdateDto, DocumentCategory>();
            CreateMap<DocumentCategory, DocumentCategoryGetDto>();
        }
    }
}
