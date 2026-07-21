using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class CheckedListGetHandler : ICheckedListGetHandler
    {
        const string _reportTitle = " مشاهده لیست کنترل ";
        const int _malfunctionMeterStateId = 1;
        const int _changeCounterStateId = 2;
        const int _closeMeterStateId = 4;
        const int _nextRoundCounterSatateId = 5;
        const int _withoutConsumptionMeterStateId = 6;
        const int _blockMeterStateId = 7;
        const int _noReadMeterStateId = 8;

        private readonly IMeterFlowValidationGetHandler _meterFlowValidationGetHandler;
        private readonly IMeterFlowQueryService _meterFlowService;
        private readonly IMeterReadingDetailQueryService _meterReadingDetailService;
        private readonly IMeterReadingValidateHandler _meterReadingValidateHandler;
        public CheckedListGetHandler(
            IMeterFlowValidationGetHandler meterFlowValidationGetHandler,
            IMeterFlowQueryService meterFlowService,
            IMeterReadingDetailQueryService meterReadingDetailService,
            IMeterReadingValidateHandler meterReadingValidateHandler)
        {
            _meterFlowValidationGetHandler = meterFlowValidationGetHandler;
            _meterFlowValidationGetHandler.NotNull(nameof(meterFlowValidationGetHandler));

            _meterFlowService = meterFlowService;
            _meterFlowService.NotNull(nameof(meterFlowService));

            _meterReadingDetailService = meterReadingDetailService;
            _meterReadingDetailService.NotNull(nameof(meterReadingDetailService));

            _meterReadingValidateHandler = meterReadingValidateHandler;
            _meterReadingValidateHandler.NotNull(nameof(meterReadingValidateHandler));
        }
        public async Task<ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCheckedDto>> Handle(int latestFlowId, IAppUser appUser, CancellationToken cancellationToken)
        {
            await _meterFlowValidationGetHandler.Handle(latestFlowId, cancellationToken);
            int firstFlowId = await _meterFlowService.GetFirstFlowId(latestFlowId);
            MeterFlowStepEnum latestFlowStep = (await _meterFlowService.Get(latestFlowId)).MeterFlowStepId;

            IEnumerable<MeterReadingDetailDataOutputDto> meterReadings = await _meterReadingDetailService.Get(firstFlowId, null);
            IEnumerable<MeterReadingDetailCheckedDto> readingsCheck = GetReadingControl(meterReadings, latestFlowStep);

            return GetResult(readingsCheck, meterReadings, latestFlowStep);

        }
        private ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCheckedDto> GetResult(IEnumerable<MeterReadingDetailCheckedDto> data, IEnumerable<MeterReadingDetailDataOutputDto> meterReadingsPrevioudControl, MeterFlowStepEnum latestFlowStep)
        {
            int[] closedAndObstacleCounterState = { 4, 7, 8 };
            MeterReadingDetailHeaderOutputDto header = new MeterReadingDetailHeaderOutputDto()
            {
                Amount = data?.Sum(m => m.SumItems) ?? 0,
                Consumption = data?.Sum(m => m.Consumption) ?? 0,
                RecordCount = data?.Count() ?? 0,
                FromReadingNumber = data?.Min(m => m.ReadingNumber) ?? string.Empty,
                ToReadingNumber = data?.Max(m => m.ReadingNumber) ?? string.Empty,

                Closed = data?.Count(r => r.CurrentCounterStateCode == _closeMeterStateId) ?? 0,
                Obstacle = data?.Count(r => r.CurrentCounterStateCode == _blockMeterStateId) ?? 0,
                Temporarily = data?.Count(r => r.CurrentCounterStateCode == _noReadMeterStateId) ?? 0,
                PureReading = data?.Count(r => !closedAndObstacleCounterState.Contains(r.CurrentCounterStateCode)) ?? 0,
                Malfunction = data?.Count(r => r.CurrentCounterStateCode == _malfunctionMeterStateId) ?? 0,
                Changed = data?.Count(r => r.CurrentCounterStateCode == _changeCounterStateId) ?? 0,
                NextRound = data?.Count(r => r.CurrentCounterStateCode == _nextRoundCounterSatateId) ?? 0,
                WithoutConsumption = data?.Count(r => r.CurrentCounterStateCode == _withoutConsumptionMeterStateId) ?? 0,
                Excluded = meterReadingsPrevioudControl?.Count(mr => mr.ExcludedByUserId is not null) ?? 0,
            };
            ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCheckedDto> result = new(_reportTitle, header, data.OrderByDescending(meter => meter.AttentionState));

            return result;
        }
        private IEnumerable<MeterReadingDetailCheckedDto> GetReadingControl(IEnumerable<MeterReadingDetailDataOutputDto> meterReadings, MeterFlowStepEnum latestFlowStep)
        {
            ICollection<MeterReadingDetailCheckedDto> readingsControl = new List<MeterReadingDetailCheckedDto>();
            meterReadings.Where(mr => mr.ExcludedByUserId is null).ForEach(mr =>
            {
                HighLowEnum attentionState = _meterReadingValidateHandler.GetAttentionState(mr, latestFlowStep);
                //HighLowEnum attentionState = GetConsumptionOrAmountAttention(mr, latestFlowStep);
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
                Duration = input.Modat ?? 0,

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

                PreviousCounterStateCode = input.PreviousCounterStateCode,
                PreviousMeterDateJalali = input.PreviousMeterDateJalali,
                PreviousMeterNumber = input.PreviousMeterNumber,
                PreviousConsumption = input.PreviousConsumption,
                PreviousMonthlyConsumption = input.PreviousMonthlyConsumption,
                PreviousCounterStateTitle = input.PreviousCounterStateTitle,
                PreviousSumItems = input.PreviousSumItems,

                BeforDebt = input.BeforDebt,
                WaterDebt = input.WaterDebt,
                SumItems = input.SumItems,
                SumItemsBeforeDiscount = input.SumItemsBeforeDiscount,
                DiscountSum = input.DiscountSum,
                Consumption = input.Consumption,
                MonthlyConsumption = input.MonthlyConsumption,

                AttentionState = attentionState,
                HasAttentionCounterState = _meterReadingValidateHandler.IsAttentionCounterState(input.CurrentCounterStateCode)
            };
        }
    }
}
