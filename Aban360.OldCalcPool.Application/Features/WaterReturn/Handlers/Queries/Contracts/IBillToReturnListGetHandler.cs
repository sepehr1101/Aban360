using Aban360.Common.BaseEntities;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Queries.Contracts
{
    public interface IBillToReturnListGetHandler
    {
        Task<IEnumerable<BillsCanRemovedOutputDto>> Handle(SearchInput input, CancellationToken cancellationToken);
    }
}
