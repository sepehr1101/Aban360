using Aban360.SystemPool.Domain.Contants;

namespace Aban360.SystemPool.Domain.Features.Logging.Dto.Input
{
    public record LoggingInputByStringDto
    {
        public string FromDate { get; set; } = default!;
        public string ToDate { get; set; }=default!;

        public string FromTime { get; set; } = default!;
        public string ToTime { get; set; } = default!;

        public LogLevelEnum LogLevel { get; set; }
    }
}
