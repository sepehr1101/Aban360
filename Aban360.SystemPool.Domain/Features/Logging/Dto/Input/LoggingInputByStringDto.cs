using Aban360.SystemPool.Domain.Contants;

namespace Aban360.SystemPool.Domain.Features.Logging.Dto.Input
{
    public record LoggingInputByStringDto
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }

        public string FromTime { get; set; }
        public string ToTime { get; set; }

        public LogLevelEnum LogLevel { get; set; }
    }
}
