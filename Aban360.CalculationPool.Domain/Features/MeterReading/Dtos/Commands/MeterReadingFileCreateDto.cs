using Microsoft.AspNetCore.Http;

namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record MeterReadingFileCreateDto
    {
        public string? Description { get; set; }
        public IFormFile ReadingFile { get; set; } = default!;
    }
}
