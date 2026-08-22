using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations;
using Aban360.ReportPool.Application.Features.WaterInvoice.Handler.Contracts;
using Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class MeterReadingDetailLatestInfoByReadingNumberGetHandler : IMeterReadingDetailLatestInfoByReadingNumberGetHandler
    {
        private readonly IBillTransactionDetailsGetHandler _billTransactionDetailsGetHandler;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly ICommonZoneService _commonZoneService;
        public MeterReadingDetailLatestInfoByReadingNumberGetHandler(
            IBillTransactionDetailsGetHandler billTransactionDetailsGetHandler,
            ICommonMemberQueryService commonMemberQueryService,
            ICommonZoneService commonZoneService)
        {
            _billTransactionDetailsGetHandler = billTransactionDetailsGetHandler;
            _billTransactionDetailsGetHandler.NotNull(nameof(billTransactionDetailsGetHandler));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));
        }

        public async Task<ReportOutput<BillTransactionDetailHeaderOutputDto, BillTransactionDetailDataOutputDto>> Handle(string readingNumber, IAppUser appUser, CancellationToken cancellationToken)
        {
            IEnumerable<int> myZoneIds = await _commonZoneService.GetMyZoneIds(appUser);
            ICollection<ZoneIdAndCustomerNumberAndBillId> customersInfo = new List<ZoneIdAndCustomerNumberAndBillId>();
            foreach (var zoneId in myZoneIds)
            {
                ZoneIdAndCustomerNumberAndBillId customerInfo = await _commonMemberQueryService.Get(new ZoneIdAndReadingNumber(zoneId, readingNumber), false);
                customersInfo.Add(customerInfo);
            }
            if (!customersInfo.Any())
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidReadingNumber);
            }
            if ((customersInfo?.Count() ?? 0) > 0)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidMoreThan1ReadingNumber);
            }
            ReportOutput<BillTransactionDetailHeaderOutputDto, BillTransactionDetailDataOutputDto> result = await _billTransactionDetailsGetHandler.Handle(customersInfo.First().BillId, appUser, cancellationToken);
            return result;
        }
    }
}
