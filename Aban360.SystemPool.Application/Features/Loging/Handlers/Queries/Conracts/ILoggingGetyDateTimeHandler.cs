using Aban360.SystemPool.Domain.Features.Loging.Dto.Input;
using Aban360.SystemPool.Domain.Features.Loging.Dto.Output;

namespace Aban360.SystemPool.Application.Features.Loging.Handlers.Queries.Conracts
{
    public interface ILoggingGetyDateTimeHandler
    {
        Task<IEnumerable<LoggingOutputDto>> Handle(LoggingInputByStringDto inputDto, CancellationToken cancellationToken);
    }
}
