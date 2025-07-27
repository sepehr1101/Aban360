using Aban360.SystemPool.Domain.Features.Loging.Dto.Input;
using Aban360.SystemPool.Domain.Features.Loging.Dto.Output;

namespace Aban360.SystemPool.Persistence.Features.Loging.Queries.Contracts
{
    public interface ILogingGetByDateTimeQueryService
    {
        Task<IEnumerable<LogingOutputDto>> Get(LogingInputByDateTimeDto input);
    }
}
