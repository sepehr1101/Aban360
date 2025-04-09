using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;
using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using AutoMapper;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Mappings
{
    public class DocumentTypeMapper:Profile
    {
        public DocumentTypeMapper()
        {
            CreateMap<DocumentTypeCreateDto, DocumentType>();
            CreateMap<DocumentTypeDeleteDto, DocumentType>();
            CreateMap<DocumentTypeUpdateDto, DocumentType>();
            CreateMap<DocumentType, DocumentTypeGetDto>();
        }
    }
}
