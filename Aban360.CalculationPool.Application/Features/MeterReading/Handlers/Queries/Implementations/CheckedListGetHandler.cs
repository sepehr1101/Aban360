using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class CheckedListGetHandler : ICheckedListGetHandler
    {
        const string _reportTitle = " مشاهده لیست کنترل ";
        const int _domesticConsumptionExpirePercent = 30;
        const int _nonDomesticConsumptionExpirePercent = 30;
        private const int _amountExpirePercent = 50;


        private readonly IMeterFlowValidationGetHandler _meterFlowValidationGetHandler;
        private readonly IMeterFlowQueryService _meterFlowService;
        private readonly IMeterReadingDetailQueryService _meterReadingDetailService;
        public CheckedListGetHandler(
            IMeterFlowValidationGetHandler meterFlowValidationGetHandler,
            IMeterFlowQueryService meterFlowService,
            IMeterReadingDetailQueryService meterReadingDetailService)
        {
            _meterFlowValidationGetHandler = meterFlowValidationGetHandler;
            _meterFlowValidationGetHandler.NotNull(nameof(meterFlowValidationGetHandler));

            _meterFlowService = meterFlowService;
            _meterFlowService.NotNull(nameof(meterFlowService));

            _meterReadingDetailService = meterReadingDetailService;
            _meterReadingDetailService.NotNull(nameof(meterReadingDetailService));

        }
        public async Task<ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCheckedDto>> Handle(int latestFlowId, IAppUser appUser, CancellationToken cancellationToken)
        {
            await _meterFlowValidationGetHandler.Handle(latestFlowId, cancellationToken);
            int firstFlowId = await _meterFlowService.GetFirstFlowId(latestFlowId);
            MeterFlowStepEnum latestFlowStep = (await _meterFlowService.Get(latestFlowId)).MeterFlowStepId;

            IEnumerable<MeterReadingDetailDataOutputDto> meterReadings = await _meterReadingDetailService.Get(firstFlowId);
            IEnumerable<MeterReadingDetailCheckedDto> readingsCheck = GetReadingControl(meterReadings, latestFlowStep);

            return GetResult(readingsCheck, latestFlowStep);

        }
        private ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCheckedDto> GetResult(IEnumerable<MeterReadingDetailCheckedDto> data, MeterFlowStepEnum latestFlowStep)
        {
            int[] closedAndObstacleCounterState = { 4, 7, 8 };
            MeterReadingDetailHeaderOutputDto header = new MeterReadingDetailHeaderOutputDto()
            {
                Amount = data.Sum(m => m.SumItems) ?? 0,
                Consumption = data.Sum(m => m.Consumption) ?? 0,
                RecordCount = data.Count(),
                Closed = data.Count(r => r.CurrentCounterStateCode == 4),
                Obstacle = data.Count(r => r.CurrentCounterStateCode == 7),
                Temporarily = data.Count(r => r.CurrentCounterStateCode == 8),
                PureReading = data.Count(r => !closedAndObstacleCounterState.Contains(r.CurrentCounterStateCode)),
                Ruined = data.Count(r => r.CurrentCounterStateCode == 1)
            };
            ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCheckedDto> result = new(_reportTitle, header, data.OrderByDescending(meter => meter.AttentionState));

            return result;
        }
        private IEnumerable<MeterReadingDetailCheckedDto> GetReadingControl(IEnumerable<MeterReadingDetailDataOutputDto> meterReadings, MeterFlowStepEnum latestFlowStep)
        {
            ICollection<MeterReadingDetailCheckedDto> readingsControl = new List<MeterReadingDetailCheckedDto>();
            meterReadings.ForEach(mr =>
            {
                HighLowEnum attentionState = GetConsumptionOrAmountAttention(mr, latestFlowStep);
                MeterReadingDetailCheckedDto readingControl = GetMeterReadingDetailControl(mr, attentionState);
                readingsControl.Add(readingControl);
            });

            return readingsControl;
        }
        private HighLowEnum GetConsumptionOrAmountAttention(MeterReadingDetailDataOutputDto meterReading, MeterFlowStepEnum latestFlowStep)
        {
            return latestFlowStep switch
            {
                MeterFlowStepEnum.Calculated => ConsumptionAttention(meterReading),
                MeterFlowStepEnum.ConsumptionChecked => AmountAttention(meterReading),
                _ => throw new ReadingException(ExceptionLiterals.InvalidFlowStep)
            };
        }
        private HighLowEnum ConsumptionAttention(MeterReadingDetailDataOutputDto input)
        {
            if (IsDomestic(input.UsageId))
            {
                return GetHasAttention(_domesticConsumptionExpirePercent, input.LastMonthlyConsumption.Value, input.MonthlyConsumption.Value);
            }
            else
            {
                return GetHasAttention(_nonDomesticConsumptionExpirePercent, input.ContractualCapacity, input.MonthlyConsumption.Value);
            }
        }
        private HighLowEnum AmountAttention(MeterReadingDetailDataOutputDto input)
        {
            return GetHasAttention(_amountExpirePercent, input.LastSumItems.Value, input.SumItems.Value);
        }
        private MeterReadingDetailCheckedDto GetMeterReadingDetailControl(MeterReadingDetailDataOutputDto input, HighLowEnum attentionState)
        {
            return new MeterReadingDetailCheckedDto()
            {
                Id = input.Id,
                FlowImportedId = input.FlowImportedId,
                ZoneId = input.ZoneId,
                CustomerNumber = input.CustomerNumber,
                ReadingNumber = input.ReadingNumber,
                BillId = input.BillId,
                AgentCode = input.AgentCode,
                CurrentCounterStateCode = input.CurrentCounterStateCode,
                PreviousDateJalali = input.PreviousDateJalali,
                CurrentDateJalali = input.CurrentDateJalali,
                PreviousNumber = input.PreviousNumber,
                CurrentNumber = input.CurrentNumber,
                InsertByUserId = input.InsertByUserId,
                InsertDateTime = input.InsertDateTime,

                BranchTypeId = input.BranchTypeId,
                BranchTypeTitle = input.BranchTypeTitle,
                UsageId = input.UsageId,
                UsageTitle = input.UsageTitle,
                ConsumptionUsageId = input.ConsumptionUsageId,
                DomesticUnit = input.DomesticUnit,
                CommercialUnit = input.CommercialUnit,
                OtherUnit = input.OtherUnit,
                EmptyUnit = input.EmptyUnit,
                WaterInstallationDateJalali = input.WaterInstallationDateJalali,
                SewageInstallationDateJalali = input.SewageInstallationDateJalali,
                WaterRegisterDate = input.WaterRegisterDate,
                SewageRegisterDate = input.SewageRegisterDate,
                WaterCount = input.WaterCount,
                SewageCalcState = input.SewageCalcState,
                HouseholdDate = input.HouseholdDate,
                HouseholdNumber = input.HouseholdNumber,
                VillageId = input.VillageId,
                IsSpecial = input.IsSpecial,
                MeterDiameterId = input.MeterDiameterId,
                VirtualCategoryId = input.VirtualCategoryId,
                ContractualCapacity = input.ContractualCapacity,
                BodySerial = input.BodySerial,

                TavizCause = input.TavizCause,
                TavizDateJalali = input.TavizDateJalali,
                TavizNumber = input.TavizNumber,
                TavizRegisterDateJalali = input.TavizRegisterDateJalali,

                LastCounterStateCode = input.LastCounterStateCode,
                LastMeterDateJalali = input.LastMeterDateJalali,
                LastMeterNumber = input.LastMeterNumber,
                LastConsumption = input.LastConsumption,
                LastMonthlyConsumption = input.LastMonthlyConsumption,
                LastSumItems = input.LastSumItems,

                SumItems = input.SumItems,
                SumItemsBeforeDiscount = input.SumItemsBeforeDiscount,
                DiscountSum = input.DiscountSum,
                Consumption = input.Consumption,
                MonthlyConsumption = input.MonthlyConsumption,

                AttentionState = attentionState
            };
        }
        private HighLowEnum GetHasAttention(int expirePercent, double lastItem, double currentItem)
        {
            if (currentItem == 0)
            {
                return HighLowEnum.Zero;
            }
            else
            {
                double expireValue = (double)(lastItem * expirePercent / 100d);
                double minValue = (double)(lastItem - expireValue);
                double maxValue = (double)(lastItem + expireValue);

                return currentItem >= minValue && currentItem <= maxValue ? HighLowEnum.Low : HighLowEnum.High;
            }
        }
        private bool IsDomestic(int usageId)
        {
            int[] domesticUsage = [0, 1, 3];
            return domesticUsage.Contains(usageId);
        }
    }
}
