using Microsoft.AspNetCore.Http;

namespace Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands
{
    public record DocumentCreateDto
    {
        public IFormFile document { get; set; }
        public short DocumentTypeId { get; set; }
        public string? Description { get; set; }
    }
}