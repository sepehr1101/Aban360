using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts;

namespace Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Implementations
{
    internal sealed class BillReturnCauseGetAllHandler : IBillReturnCauseGetAllHandler
    {
        private readonly IBillReturnCauseQueryService _billReturnCauseQueryService;

        public BillReturnCauseGetAllHandler(IBillReturnCauseQueryService billReturnCauseQueryService)
        {
            _billReturnCauseQueryService = billReturnCauseQueryService;
            _billReturnCauseQueryService.NotNull(nameof(billReturnCauseQueryService));
        }
        public async Task<IEnumerable<BillReturnCauseGetDto>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<BillReturnCauseGetDto> result = await _billReturnCauseQueryService.Get();
            return result;
        }
        public async Task<IEnumerable<NumericDictionary>> HandleByDictionary(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> result = await _billReturnCauseQueryService.GetByDictionary();
            return result;
        }
    }
}
