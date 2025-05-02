using Aban360.BlobPool.Domain.Features.DMS.Dto.Commands;
using Aban360.BlobPool.Domain.Features.DMS.Dto.Queries;
using Aban360.BlobPool.Domain.Features.DMS.Entities;
using AutoMapper;

namespace Aban360.BlobPool.Application.Features.DMS.Mappings
{
    public class DocumentEntityMapper : Profile
    {
        public DocumentEntityMapper()
        {
            CreateMap<DocumentEntityCreateDto, DocumentEntity>();
            CreateMap<DocumentEntityDeleteDto, DocumentEntity>();
            CreateMap<DocumentEntityUpdateDto, DocumentEntity>();
            CreateMap<DocumentEntity, DocumentEntityGetDto>();
        }
    }
}
