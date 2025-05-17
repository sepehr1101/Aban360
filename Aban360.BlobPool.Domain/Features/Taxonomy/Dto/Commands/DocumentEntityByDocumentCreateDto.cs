
using Aban360.BlobPool.Domain.Features.DMS.Dto.Commands;

namespace Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands
{
    public record DocumentEntityByDocumentCreateDto
    {
        public DocumentEntityCreateDto documentEntityCreateDto { get; set; }
        public DocumentCreateDto documentCreateDto { get; set; }
    }
}
