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
        const int _domesticConsumptionLimitPercent = 30;
        const int _nonDomesticConsumptionLimitPercent = 30;
        const int _consumptionLimit = 50;
        const long _dailyDomesticAmount = 700_000;
        const long _dailyNonDomesticAmount = 1_000_000;
        int[] _misReadCounterStateCode = [4, 7, 8];
        int[] _specialCounterStateCode = [1, 2, 3, 5];

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
                Amount = data?.Sum(m => m.SumItems) ?? 0,
                Consumption = data?.Sum(m => m.Consumption) ?? 0,
                RecordCount = data?.Count() ?? 0,
                FromReadingNumber = data?.Min(m => m.ReadingNumber) ?? string.Empty,
                ToReadingNumber = data?.Max(m => m.ReadingNumber) ?? string.Empty,

                Closed = data?.Count(r => r.CurrentCounterStateCode == 4) ?? 0,
                Obstacle = data?.Count(r => r.CurrentCounterStateCode == 7) ?? 0,
                Temporarily = data?.Count(r => r.CurrentCounterStateCode == 8) ?? 0,
                PureReading = data?.Count(r => !closedAndObstacleCounterState.Contains(r.CurrentCounterStateCode)) ?? 0,
                Ruined = data?.Count(r => r.CurrentCounterStateCode == 1) ?? 0
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
                TotalUnit = input.DomesticUnit + input.CommercialUnit + input.OtherUnit,
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

                AttentionState = attentionState,
                HasAttentionCounterState = _specialCounterStateCode.Contains(input.CurrentCounterStateCode)
            };
        }
        private HighLowEnum GetConsumptionOrAmountAttention(MeterReadingDetailDataOutputDto meterReading, MeterFlowStepEnum latestFlowStep)
        {
            return latestFlowStep switch
            {
                MeterFlowStepEnum.Calculated => ConsumptionAttention(meterReading),
                MeterFlowStepEnum.ConsumptionChecked => GetAmountAttention(meterReading),
                _ => throw new ReadingException(ExceptionLiterals.InvalidFlowStep)
            };
        }
        private HighLowEnum ConsumptionAttention(MeterReadingDetailDataOutputDto input)
        {
            if (IsDomestic(input.UsageId))
            {
                return GetConsumptionAttention(_domesticConsumptionLimitPercent, input.LastMonthlyConsumption.Value, input.MonthlyConsumption.Value, input.CurrentCounterStateCode, input.UsageId);
            }
            else
            {
                return GetConsumptionAttention(_nonDomesticConsumptionLimitPercent, input.ContractualCapacity, input.MonthlyConsumption.Value, input.CurrentCounterStateCode, input.UsageId);
            }
        }
        private HighLowEnum GetAmountAttention(MeterReadingDetailDataOutputDto input)
        {
            if (_misReadCounterStateCode.Contains(input.CurrentCounterStateCode)) return HighLowEnum.Normal;
            if (input.SumItems == 0) return HighLowEnum.Zero;

            int totalUnit = input.DomesticUnit + input.CommercialUnit + input.OtherUnit;
            int duration = (int)(!input.Modat.HasValue || input.Modat <= 0 ? 1 : input.Modat);
            double perUnitAmount = input.SumItems.Value / (totalUnit == 0 ? 1 : totalUnit);
            double dailyPerUnitAmount = perUnitAmount / duration;

            if (IsDomestic(input.UsageId) && dailyPerUnitAmount > _dailyDomesticAmount) return HighLowEnum.High;
            if (!IsDomestic(input.UsageId) && dailyPerUnitAmount > _dailyNonDomesticAmount) return HighLowEnum.High;

            return HighLowEnum.Normal;
        }
        private HighLowEnum GetConsumptionAttention(int limitPercent, double previousItem, double consumption, int counterStateCode, int usageId)
        {
            if (_misReadCounterStateCode.Contains(counterStateCode)) return HighLowEnum.Normal;
            if (consumption == 0) return HighLowEnum.Zero;

            double limitValue = (double)(previousItem * limitPercent / 100d);
            double minValue = (double)(previousItem - limitValue);
            double maxValue = (double)(previousItem + limitValue);

            if (consumption < minValue) return HighLowEnum.Low;
            if (consumption > maxValue) return HighLowEnum.High;
            if (IsDomestic(usageId) && consumption > _consumptionLimit) return HighLowEnum.High;
            return HighLowEnum.Normal;
        }
        private bool IsDomestic(int usageId)
        {
            int[] domesticUsage = [1, 3];
            return domesticUsage.Contains(usageId);
        }
    }
}
