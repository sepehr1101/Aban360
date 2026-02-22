using Aban360.Common.BaseEntities;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Queries.Implementations
{
    internal sealed class MeterChangeGetHandler : IMeterChangeGetHandler
    {
        private readonly ITavizQueryService _tavizQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        public MeterChangeGetHandler(
            ITavizQueryService tavizQueryService,
            ICommonMemberQueryService commonMemberQueryService)
        {
            _tavizQueryService = tavizQueryService;
            _tavizQueryService.NotNull(nameof(tavizQueryService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));
        }

        public async Task<IEnumerable<MeterChangeInfoOutputDto>> Handle(string billId, CancellationToken cancellationToken)
        {
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber=await _commonMemberQueryService.Get(billId);
            IEnumerable<MeterChangeInfoOutputDto> resutl = await _tavizQueryService.Get(zoneIdAndCustomerNumber);
            return resutl;
        }
    }
}
