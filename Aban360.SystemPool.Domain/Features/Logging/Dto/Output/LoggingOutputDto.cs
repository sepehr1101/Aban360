namespace Aban360.SystemPool.Domain.Features.Logging.Dto.Output
{
    public record LoggingOutputDto
    {
        public DateTime DateTimeGrogorian { get; set; }
        public string DateJalali { get; set; } = default!;
        public string Time { get; set; }=default!;
        public string LogLevel { get; set; } = default!;
        public string? Message { get; set; }
        public string? Exception { get; set; }
        public string? Properties { get; set; }
    }
}
