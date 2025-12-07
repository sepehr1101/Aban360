using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Queries.Implementations
{
    internal sealed class BillToReturnListGetHandler : IBillToReturnListGetHandler
    {
        private readonly ICustomerInfoService _customerInfoService;
        private readonly IBillQueryService _billQueryService;
        public BillToReturnListGetHandler(
            ICustomerInfoService customerInfoService,
            IBillQueryService billQueryService)
        {
            _customerInfoService = customerInfoService;
            _customerInfoService.NotNull(nameof(customerInfoService));

            _billQueryService = billQueryService;
            _billQueryService.NotNull(nameof(billQueryService));
        }

        public async Task<IEnumerable<BillsCanRemovedOutputDto>> Handle(SearchInput input, CancellationToken cancellationToken)
        {
            ZoneIdAndCustomerNumberGetDto zoneIdAndCustomerNumber = await _customerInfoService.GetZoneIdAndCustomerNumber(input.Input);
            ReturnedBillSearchDto returnedBillSearchDto = new(zoneIdAndCustomerNumber.ZoneId, zoneIdAndCustomerNumber.CustomerNumber);

            IEnumerable<BillsCanRemovedOutputDto> billsCanReturned = await _billQueryService.GetToReturned(returnedBillSearchDto);
            return billsCanReturned;
        }
    }
}
