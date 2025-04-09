using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;
using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using AutoMapper;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Mappings
{
    public class DocumentMapper : Profile
    {
        public DocumentMapper()
        {
            CreateMap<DocumentDeleteDto, Document>();
            CreateMap<Document, DocumentGetDto>();
        }
    }
}
