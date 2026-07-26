using Microsoft.AspNetCore.Http;

namespace Aban360.MeterPool.Domain.Features.Apk.Commands
{
    public record ApkInfoInsertInputDto
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public IFormFile FileContent { get; set; }
        public string? Description { get; set; }
    }
}
