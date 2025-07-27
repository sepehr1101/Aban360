using Aban360.SystemPool.Domain.Features.Logging.Dto.Input;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Output;

namespace Aban360.SystemPool.Persistence.Features.Logging.Queries.Contracts
{
    public interface ILoggingGetByDateTimeQueryService
    {
        Task<IEnumerable<LoggingOutputDto>> Get(LoggingInputByDateTimeDto input);
    }
}
