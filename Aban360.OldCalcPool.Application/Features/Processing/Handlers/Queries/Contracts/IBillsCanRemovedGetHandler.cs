using Aban360.Common.BaseEntities;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Queries.Contracts
{
    public interface IBillsCanRemovedGetHandler
    {
        Task<IEnumerable<BillsCanRemoveOutputDto>> Handle(SearchInput inputDto, CancellationToken cancellationToken);
    }

}
