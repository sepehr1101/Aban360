using Aban360.SystemPool.Domain.Contants;

namespace Aban360.SystemPool.Domain.Features.Loging.Dto.Input
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
