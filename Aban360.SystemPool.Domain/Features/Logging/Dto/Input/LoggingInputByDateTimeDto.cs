using Aban360.SystemPool.Domain.Contants;

namespace Aban360.SystemPool.Domain.Features.Logging.Dto.Input
{
    public record LoggingInputByDateTimeDto
    {
        public DateTime FromDateTime { get; set; }
        public DateTime ToDateTime { get; set; }
        public LogLevelEnum LogLevel { get; set; }

        public LoggingInputByDateTimeDto(DateTime _from, DateTime _to, LogLevelEnum _loglevel)
        {
            FromDateTime = _from;
            ToDateTime = _to;
            LogLevel = _loglevel;
        }
    }
}
