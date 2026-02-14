using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Contracts;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.Transactions;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using DNTPersianUtils.Core;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Implementations
{
    internal sealed class RemoveBillHandler : IRemoveBillHandler
    {
        private readonly ICustomerInfoQueryService _customerInfoQueryService;
        private readonly IBedBesCommandService _bedBesCommandService;
        private readonly IHBedBesCommanddService _hbedBesCommanddService;
        private readonly IKasrHaService _kasrHaService;
        private readonly IBedBesQueryService _billQueryService;
        public RemoveBillHandler(
            ICustomerInfoQueryService customerInfoQueryService,
            IBedBesCommandService bedBesCommandService,
            IHBedBesCommanddService hbedBesCommanddService,
            IKasrHaService kasrHaService,
            IBedBesQueryService billQueryService)
        {
            _customerInfoQueryService= customerInfoQueryService;
            _customerInfoQueryService.NotNull(nameof(customerInfoQueryService));

            _bedBesCommandService = bedBesCommandService;
            _bedBesCommandService.NotNull(nameof(bedBesCommandService));

            _hbedBesCommanddService = hbedBesCommanddService;
            _hbedBesCommanddService.NotNull(nameof(hbedBesCommanddService));

            _kasrHaService = kasrHaService;
            _kasrHaService.NotNull(nameof(kasrHaService));

            _billQueryService = billQueryService;
            _billQueryService.NotNull(nameof(billQueryService));
        }

        public async Task Handle(RemoveBillInputDto input, CancellationToken cancellationToken)
        {
            RemoveBillDataInputDto removeBill = await GetRemoveBillInputDto(input);
            removeBill.ToDayDateJalali = DateTime.Now.ToShortPersianDateString();

            //await _bedBesCommandService.Delete(removeBill.Id);//todo: tobeSure: DeleteBedBes OR Del=true?
            //await _kasrHaService.Delete(removeBill);
            //await _hbedBesCommanddService.Insert(removeBill);
        }
        public async Task<RemoveBillDataInputDto> GetRemoveBillInputDto(RemoveBillInputDto input)
        {
            ZoneIdAndCustomerNumberOutputDto zoneIdAndCustomerNumber = await _customerInfoQueryService.GetZoneIdAndCustomerNumber(input.BillId);
            RemoveBillGetDto removebillGet = new(input.Id, zoneIdAndCustomerNumber.ZoneId, zoneIdAndCustomerNumber.CustomerNumber);
            RemoveBillDataInputDto result = await _billQueryService.GetToRemove(removebillGet);
            return result;
        }
    }
}