using Aban360.SystemPool.Domain.Features.Logging.Dto.Input;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Output;

namespace Aban360.SystemPool.Application.Features.Logging.Handlers.Queries.Conracts
{
    public interface ILoggingGetyDateTimeHandler
    {
        Task<IEnumerable<LoggingOutputDto>> Handle(LoggingInputByStringDto inputDto, CancellationToken cancellationToken);
    }
}
