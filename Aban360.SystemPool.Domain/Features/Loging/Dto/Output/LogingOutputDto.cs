namespace Aban360.SystemPool.Domain.Features.Loging.Dto.Output
{
    public record LogingOutputDto
    {
        public DateTime DateTimeGrogorian { get; set; }
        public string DateJalali { get; set; }
        public string Time { get; set; }
        public string LogLevel { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
    }
}
