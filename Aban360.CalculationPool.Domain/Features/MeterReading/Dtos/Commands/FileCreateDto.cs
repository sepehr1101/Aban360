namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record FileCreateDto
    {
        public string FileName { get; set; }
        public string? FilePath { get; set; }
        public string? Description { get; set; }
        public FileCreateDto(string fileName, string filePath, string? description)
        {
            FileName = fileName;
            FilePath = filePath;
            Description = description;
        }
        public FileCreateDto()
        {
        }
    }
}
