using Microsoft.AspNetCore.Http;

namespace Aban360.ReportPool.Domain.Features.DynamicGenerator.Dto.Commands
{
    public record DynamicReportCreateDto
    {
        public string Name { get; set; } = null!;

        public long Version { get; set; }

        public Guid UserId { get; set; }

        public IFormFile Document{ get; set; }

        public short DocumentTypeId { get; set; } 

        public string? Description { get; set; }
    }
}
