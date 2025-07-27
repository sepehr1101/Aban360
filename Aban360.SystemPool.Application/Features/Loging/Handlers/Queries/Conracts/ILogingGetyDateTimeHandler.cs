using Aban360.SystemPool.Domain.Features.Loging.Dto.Input;
using Aban360.SystemPool.Domain.Features.Loging.Dto.Output;

namespace Aban360.SystemPool.Application.Features.Loging.Handlers.Queries.Conracts
{
    public interface ILogingGetyDateTimeHandler
    {
        Task<IEnumerable<LogingOutputDto>> Handle(LogingInputByStringDto inputDto, CancellationToken cancellationToken);
    }
}
