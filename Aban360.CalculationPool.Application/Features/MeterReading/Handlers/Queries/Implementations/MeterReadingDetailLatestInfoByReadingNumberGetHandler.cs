using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
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
        private readonly IMeterReadingDetailQueryService _meterReadingDetailQueryService;
        private readonly IMeterFlowQueryService _meterFlowQueryService;
        private readonly ICommonZoneService _commonZoneService;
        public MeterReadingDetailLatestInfoByReadingNumberGetHandler(
            IBillTransactionDetailsGetHandler billTransactionDetailsGetHandler,
            ICommonMemberQueryService commonMemberQueryService,
            IMeterFlowQueryService meterFlowQueryService,
            IMeterReadingDetailQueryService meterReadingDetailQueryService,
            ICommonZoneService commonZoneService)
        {
            _billTransactionDetailsGetHandler = billTransactionDetailsGetHandler;
            _billTransactionDetailsGetHandler.NotNull(nameof(billTransactionDetailsGetHandler));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));

            _meterFlowQueryService = meterFlowQueryService;
            _meterFlowQueryService.NotNull(nameof(meterFlowQueryService));

            _meterReadingDetailQueryService = meterReadingDetailQueryService;
            _meterReadingDetailQueryService.NotNull(nameof(meterReadingDetailQueryService));
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
            var (CurrentMeterDeteJalali, CurrentMeterNumber, CurrentCounterStateCode) = await GetMeterReadingData(result.ReportHeader.BillId);

            result.ReportHeader.CurrentMeterDeteJalali = CurrentMeterDeteJalali;
            result.ReportHeader.CurrentMeterNumber = CurrentMeterNumber;
            result.ReportHeader.CurrentCounterStateCode = CurrentCounterStateCode;

            return result;
        }
        private async Task<(string, int, short)> GetMeterReadingData(string billId)
        {
            MeterReadingDetailDataOutputDto? meterReadingDetail = await _meterReadingDetailQueryService.Get(billId);
            if (meterReadingDetail is not null)
            {
                MeterFlowGetDto meterFlowInfo = await _meterFlowQueryService.GetLatestFlowInfo2(meterReadingDetail.FlowImportedId);
                if (meterFlowInfo.RemovedDateTime is not null)
                {
                    throw new ReadingException(ExceptionLiterals.InvalidLatestMeterReadingWithExpireMeterFlow);
                }
                return (meterReadingDetail.CurrentDateJalali, meterReadingDetail.CurrentNumber, meterReadingDetail.CurrentCounterStateCode);
            }

            return (string.Empty, 0, 0);
        }
    }
}
