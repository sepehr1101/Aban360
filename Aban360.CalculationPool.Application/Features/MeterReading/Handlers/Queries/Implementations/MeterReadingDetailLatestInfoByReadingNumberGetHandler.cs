using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
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
        private readonly ICheckedListGetHandler _checkedListGetHandler;
        public MeterReadingDetailLatestInfoByReadingNumberGetHandler(
            IBillTransactionDetailsGetHandler billTransactionDetailsGetHandler,
            ICommonMemberQueryService commonMemberQueryService,
            IMeterFlowQueryService meterFlowQueryService,
            IMeterReadingDetailQueryService meterReadingDetailQueryService,
            ICommonZoneService commonZoneService,
            ICheckedListGetHandler checkedListGetHandler)
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

            _checkedListGetHandler = checkedListGetHandler;
            _checkedListGetHandler.NotNull(nameof(checkedListGetHandler));
        }

        public async Task<ReportOutput<BillTransactionDetailWithLastReadingDataHeaderOutputDto, BillTransactionDetailDataOutputDto>> Handle(string readingNumber, IAppUser appUser, CancellationToken cancellationToken)
        {
            IEnumerable<int> myZoneIds = await _commonZoneService.GetMyZoneIds(appUser);
            IEnumerable<ZoneIdAndCustomerNumberAndBillId> customersInfo = await _commonMemberQueryService.GetFromClient(new ZoneIdsAndReadingNumber(myZoneIds, readingNumber), false);

            if (!customersInfo.Any())
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidReadingNumber);
            }
            if ((customersInfo?.Count() ?? 0) > 1)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidMoreThan1ReadingNumber);
            }
            ReportOutput<BillTransactionDetailHeaderOutputDto, BillTransactionDetailDataOutputDto> transactionInfo = await _billTransactionDetailsGetHandler.Handle(customersInfo.First().BillId, appUser, cancellationToken);
            var (meterReadingDetail, meterFlowStepEnum) = await GetMeterReadingData(transactionInfo.ReportHeader.BillId);
            BillTransactionDetailWithLastReadingDataHeaderOutputDto header = GetHeader(transactionInfo.ReportHeader, meterReadingDetail, meterFlowStepEnum);

            return new ReportOutput<BillTransactionDetailWithLastReadingDataHeaderOutputDto, BillTransactionDetailDataOutputDto>(transactionInfo.Title, header, transactionInfo.ReportData);
        }
        private async Task<(MeterReadingDetailDataOutputDto, MeterFlowStepEnum)> GetMeterReadingData(string billId)
        {
            MeterReadingDetailDataOutputDto? meterReadingDetail = await _meterReadingDetailQueryService.Get(billId);
            MeterFlowStepEnum meterFlowStepId = 0;
            if (meterReadingDetail is not null)
            {
                MeterFlowGetDto meterFlowInfo = await _meterFlowQueryService.GetLatestFlowInfo2(meterReadingDetail.FlowImportedId);
                if (meterFlowInfo.RemovedDateTime is not null)
                {
                    throw new ReadingException(ExceptionLiterals.InvalidLatestMeterReadingWithExpireMeterFlow);
                }
                meterFlowStepId = meterFlowInfo.MeterFlowStepId;
            }

            return (meterReadingDetail, meterFlowStepId);
        }
        private BillTransactionDetailWithLastReadingDataHeaderOutputDto GetHeader(BillTransactionDetailHeaderOutputDto preHeader, MeterReadingDetailDataOutputDto meterReadingDetail, MeterFlowStepEnum flowEnum)
        {
            MeterReadingDetailCheckedDto meterReadingWithCheck = _checkedListGetHandler.GetReadingControl(meterReadingDetail, flowEnum);
            return new BillTransactionDetailWithLastReadingDataHeaderOutputDto()
            {
                ZoneId = preHeader.ZoneId,
                ZoneTitle = preHeader.ZoneTitle,
                CustomerNumber = preHeader.CustomerNumber,
                BillId = preHeader.BillId,
                FirstName = preHeader.FirstName,
                Surname = preHeader.Surname,
                FullName = preHeader.FullName,
                Title = preHeader.Title,
                RecordCount = preHeader.RecordCount,
                LatestMeterChangeDateJalali = preHeader.LatestMeterChangeDateJalali,
                PreviousMeterDateJalali = preHeader.PreviousMeterDateJalali,
                PreviousMeterNumber = preHeader.PreviousMeterNumber,
                Id = meterReadingWithCheck.Id,
                CurrentMeterDeteJalali = meterReadingWithCheck.CurrentDateJalali,
                CurrentMeterNumber = meterReadingWithCheck.CurrentNumber,
                CurrentCounterStateCode = meterReadingWithCheck.CurrentCounterStateCode,
                UsageId = meterReadingWithCheck.UsageId,
                UsageTitle = meterReadingWithCheck.UsageTitle,
                ReadingNumber = meterReadingWithCheck.ReadingNumber,
                Amount = meterReadingWithCheck.SumItems ?? 0,
                CommercialUnit = meterReadingWithCheck.CommercialUnit,
                DomesticUnit = meterReadingWithCheck.DomesticUnit,
                OtherUnit = meterReadingWithCheck.OtherUnit,
                Consumption = meterReadingWithCheck.Consumption,
                ConsumptionAverage = meterReadingWithCheck.MonthlyConsumption,
                HasAttentionCounterState = meterReadingWithCheck.HasAttentionCounterState,
            };
        }
    }
}
