using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record MeterReadingFileByFormFileCreateDto
    {
        public string Title { get; set; }
        public IFormFile ReadingFile { get; set; }

        [JsonIgnore]
        public string FilePath { get; set; }

    }
}
