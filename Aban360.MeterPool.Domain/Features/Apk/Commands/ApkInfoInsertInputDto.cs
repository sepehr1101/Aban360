using Microsoft.AspNetCore.Http;

namespace Aban360.MeterPool.Domain.Features.Apk.Commands
{
    public record ApkInfoInsertInputDto
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public IFormFile File { get; set; }
        public string? Description { get; set; }
    }
}
