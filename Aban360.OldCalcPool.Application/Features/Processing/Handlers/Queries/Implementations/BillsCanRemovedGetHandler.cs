using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Queries.Implementations
{
    internal sealed class BillsCanRemovedGetHandler : IBillsCanRemovedGetHandler
    {
        private readonly ICommonMemberQueryService _customerInfoService;
        private readonly IBedBesQueryService _billQueryService;
        public BillsCanRemovedGetHandler(
            ICommonMemberQueryService customerInfoService,
            IBedBesQueryService billQueryService)
        {
            _customerInfoService = customerInfoService;
            _customerInfoService.NotNull(nameof(customerInfoService));

            _billQueryService = billQueryService;
            _billQueryService.NotNull(nameof(billQueryService));
        }

        public async Task<IEnumerable<BillsCanRemoveOutputDto>> Handle(SearchInput inputDto, CancellationToken cancellationToken)
        {
            //validation
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _customerInfoService.Get(inputDto.Input);
            RemovedBillSearchDto removedBillSearchDto = new(zoneIdAndCustomerNumber.ZoneId, zoneIdAndCustomerNumber.CustomerNumber);

            IEnumerable<BillsCanRemoveOutputDto> billsCanRemoved = await _billQueryService.GetToRemove(removedBillSearchDto);
            billsCanRemoved.Select(b => b.ZoneTitle = zoneIdAndCustomerNumber.ZoneTitle);

            return billsCanRemoved;

        }
    }

}
