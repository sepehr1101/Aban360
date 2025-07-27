using Aban360.SystemPool.Domain.Features.Loging.Dto.Input;
using Aban360.SystemPool.Domain.Features.Loging.Dto.Output;

namespace Aban360.SystemPool.Persistence.Features.Loging.Queries.Contracts
{
    public interface ILoggingGetByDateTimeQueryService
    {
        Task<IEnumerable<LoggingOutputDto>> Get(LoggingInputByDateTimeDto input);
    }
}
