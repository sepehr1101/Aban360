using Microsoft.AspNetCore.Http;

namespace Aban360.SystemPool.Domain.Features.Logging.Dto.Input
{
    public record MeterApkLogInsertDto
    {
        public IFormFile File { get; set; }
    }
}
