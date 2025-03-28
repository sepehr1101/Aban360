using Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Queries.Contracts
{
    public interface IUsageLevel2GetSingleHandler
    {
        Task<UsageLevel2GetDto> Handle(short id, CancellationToken cancellationToken);
    }
}
