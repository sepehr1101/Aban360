using Microsoft.AspNetCore.Http;

namespace Aban360.SystemPool.Domain.Features.Logging.Dto.Input
{
    public record AssessmentLogInsertDto
    {
        public IFormFile File { get; set; }
    }
}
