using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record MeterReadingFileCreateDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public IFormFile ReadingFile { get; set; }

        [JsonIgnore]
        public string FilePath { get; set; }

    }
}
