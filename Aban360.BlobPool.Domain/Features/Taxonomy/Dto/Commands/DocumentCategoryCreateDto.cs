
using Microsoft.AspNetCore.Http;

namespace Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands
{
    public record DocumentCategoryCreateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public IFormFile Icon { get; set; }
        public string? Css { get; set; }
    }
}
