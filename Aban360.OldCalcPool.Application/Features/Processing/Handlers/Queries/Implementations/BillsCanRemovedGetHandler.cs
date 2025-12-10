using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Queries.Implementations
{
    internal sealed class BillsCanRemovedGetHandler : IBillsCanRemovedGetHandler
    {
        private readonly ICustomerInfoService _customerInfoService;
        private readonly IBillQueryService _billQueryService;
        public BillsCanRemovedGetHandler(
            ICustomerInfoService customerInfoService,
            IBillQueryService billQueryService)
        {
            _customerInfoService = customerInfoService;
            _customerInfoService.NotNull(nameof(customerInfoService));

            _billQueryService = billQueryService;
            _billQueryService.NotNull(nameof(billQueryService));
        }

        public async Task<IEnumerable<BillsCanRemovedOutputDto>> Handle(SearchInput inputDto, CancellationToken cancellationToken)
        {
            //validation
            ZoneIdAndCustomerNumberGetDto zoneIdAndCustomerNumber = await _customerInfoService.GetZoneIdAndCustomerNumber(inputDto.Input);
            RemovedBillSearchDto removedBillSearchDto = new(zoneIdAndCustomerNumber.ZoneId, zoneIdAndCustomerNumber.CustomerNumber);

            IEnumerable<BillsCanRemovedOutputDto> billsCanRemoved = await _billQueryService.GetToRemove(removedBillSearchDto);
            billsCanRemoved.Select(b => b.ZoneTitle = zoneIdAndCustomerNumber.ZoneTitle);

            return billsCanRemoved;

        }
    }

}
