using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations;
using Aban360.Common.Db.Services;
using Aban360.ReportPool.Domain.Base;
using Aban360.CalculationPool.Application.Features.MeterReading.Services;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Services
{
    public interface IMeterReadingCheckedListService
    {
        Task<ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCheckedDto>> CheckedListGetAsync(int latestFlowId, IAppUser appUser, CancellationToken cancellationToken);
        MeterReadingDetailCheckedDto CheckedListGetGetReadingControlAsync(MeterReadingDetailDataOutputDto meterReading, MeterFlowStepEnum latestFlowStep);
        Task<ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCheckedDto>> MeterReadingDetailCompletedAsync(int flowImportedId, IAppUser appUser, CancellationToken cancellationToken);
    }

    internal sealed class MeterReadingCheckedListService : IMeterReadingCheckedListService
    {
        const string _CheckedListGetreportTitle = " مشاهده لیست کنترل ";
        private readonly IMeterFlowService _meterFlowValidationGetHandler;
        private readonly IMeterFlowQueryService _meterFlowService;
        private readonly IMeterReadingDetailQueryService _meterReadingDetailService;
        private readonly IMeterReadingValidateHandler _meterReadingValidateHandler;
        private static int[] _CheckedListGetclosedAndObstacleCounterState =
        {
            (int)CounterStateCodeEnum.Close,
            (int)CounterStateCodeEnum.Block,
            (int)CounterStateCodeEnum.NonRead
        };
        private readonly ICommonZoneService _commonZoneService;
        private static int[] _MeterReadingDetailCompletedclosedAndObstacleCounterState =
        {
            (int)CounterStateCodeEnum.Close,
            (int)CounterStateCodeEnum.Block,
            (int)CounterStateCodeEnum.NonRead
        };
        private static string _MeterReadingDetailCompletedreportTitle = ReportLiterals.MeterReadingCompleted;
        public MeterReadingCheckedListService(IMeterFlowService meterFlowValidationGetHandler, IMeterFlowQueryService meterFlowService, IMeterReadingDetailQueryService meterReadingDetailService, IMeterReadingValidateHandler meterReadingValidateHandler, ICommonZoneService commonZoneService)
        {
            _meterFlowValidationGetHandler = meterFlowValidationGetHandler;
            _meterFlowValidationGetHandler.NotNull(nameof(meterFlowValidationGetHandler));
            _meterFlowService = meterFlowService;
            _meterFlowService.NotNull(nameof(meterFlowService));
            _meterReadingDetailService = meterReadingDetailService;
            _meterReadingDetailService.NotNull(nameof(meterReadingDetailService));
            _meterReadingValidateHandler = meterReadingValidateHandler;
            _meterReadingValidateHandler.NotNull(nameof(meterReadingValidateHandler));
            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));
        }

        public async Task<ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCheckedDto>> CheckedListGetAsync(int latestFlowId, IAppUser appUser, CancellationToken cancellationToken)
        {
            await _meterFlowValidationGetHandler.MeterFlowValidationGetAsync(latestFlowId, cancellationToken);
            int firstFlowId = await _meterFlowService.GetFirstFlowId(latestFlowId);
            MeterFlowStepEnum latestFlowStep = (await _meterFlowService.Get(latestFlowId)).MeterFlowStepId;
            IEnumerable<MeterReadingDetailDataOutputDto> meterReadings = await _meterReadingDetailService.Get(firstFlowId, null);
            IEnumerable<MeterReadingDetailCheckedDto> readingsCheck = CheckedListGetGetReadingControlAsync(meterReadings, latestFlowStep);
            return CheckedListGetGetResult(readingsCheck, meterReadings, latestFlowStep);
        }

        private ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCheckedDto> CheckedListGetGetResult(IEnumerable<MeterReadingDetailCheckedDto> data, IEnumerable<MeterReadingDetailDataOutputDto> meterReadingsPrevioudControl, MeterFlowStepEnum latestFlowStep)
        {
            MeterReadingDetailHeaderOutputDto header = new MeterReadingDetailHeaderOutputDto()
            {
                Amount = data?.Sum(m => m.SumItems) ?? 0,
                Consumption = data?.Sum(m => m.Consumption) ?? 0,
                RecordCount = data?.Count() ?? 0,
                FromReadingNumber = data?.Min(m => m.ReadingNumber) ?? string.Empty,
                ToReadingNumber = data?.Max(m => m.ReadingNumber) ?? string.Empty,
                Closed = data?.Count(r => r.CurrentCounterStateCode == (int)CounterStateCodeEnum.Close) ?? 0,
                Obstacle = data?.Count(r => r.CurrentCounterStateCode == (int)CounterStateCodeEnum.Block) ?? 0,
                Temporarily = data?.Count(r => r.CurrentCounterStateCode == (int)CounterStateCodeEnum.NonRead) ?? 0,
                PureReading = data?.Count(r => !_CheckedListGetclosedAndObstacleCounterState.Contains(r.CurrentCounterStateCode)) ?? 0,
                Malfunction = data?.Count(r => r.CurrentCounterStateCode == (int)CounterStateCodeEnum.Malfunction) ?? 0,
                Changed = data?.Count(r => r.CurrentCounterStateCode == (int)CounterStateCodeEnum.Change) ?? 0,
                NextRound = data?.Count(r => r.CurrentCounterStateCode == (int)CounterStateCodeEnum.NextRound) ?? 0,
                WithoutConsumption = data?.Count(r => r.CurrentCounterStateCode == (int)CounterStateCodeEnum.WithoutConsumption) ?? 0,
                Excluded = meterReadingsPrevioudControl?.Count(mr => mr.ExcludedByUserId is not null) ?? 0,
            };
            ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCheckedDto> result = new(_CheckedListGetreportTitle, header, data.OrderByDescending(meter => meter.AttentionState));
            return result;
        }

        private IEnumerable<MeterReadingDetailCheckedDto> CheckedListGetGetReadingControlAsync(IEnumerable<MeterReadingDetailDataOutputDto> meterReadings, MeterFlowStepEnum latestFlowStep)
        {
            ICollection<MeterReadingDetailCheckedDto> readingsControl = new List<MeterReadingDetailCheckedDto>();
            meterReadings.Where(mr => mr.ExcludedByUserId is null).ForEach(mr =>
            {
                HighLowEnum attentionState = _meterReadingValidateHandler.GetAttentionState(mr, latestFlowStep);
                //HighLowEnum attentionState = GetConsumptionOrAmountAttention(mr, latestFlowStep);
                MeterReadingDetailCheckedDto readingControl = CheckedListGetGetMeterReadingDetailControl(mr, attentionState);
                readingsControl.Add(readingControl);
            });
            return readingsControl;
        }

        public MeterReadingDetailCheckedDto CheckedListGetGetReadingControlAsync(MeterReadingDetailDataOutputDto meterReading, MeterFlowStepEnum latestFlowStep)
        {
            MeterReadingDetailCheckedDto readingControl = new MeterReadingDetailCheckedDto();
            if (meterReading.ExcludedByUserId is null)
            {
                HighLowEnum attentionState = _meterReadingValidateHandler.GetAttentionState(meterReading, latestFlowStep);
                readingControl = CheckedListGetGetMeterReadingDetailControl(meterReading, attentionState);
            }

            return readingControl;
        }

        private MeterReadingDetailCheckedDto CheckedListGetGetMeterReadingDetailControl(MeterReadingDetailDataOutputDto input, HighLowEnum attentionState)
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
                MonthlyPerUnit = input.MonthlyPerUnit,
                AttentionState = attentionState,
                HasAttentionCounterState = _meterReadingValidateHandler.IsAttentionCounterState(input.CurrentCounterStateCode)
            };
        }

        public async Task<ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCheckedDto>> MeterReadingDetailCompletedAsync(int flowImportedId, IAppUser appUser, CancellationToken cancellationToken)
        {
            MeterFlowGetDto meterFlowInfo = await _meterFlowService.Get(flowImportedId);
            await _commonZoneService.IsUserInZone(appUser, meterFlowInfo.ZoneId);
            IEnumerable<MeterReadingDetailDataOutputDto> detail = await _meterReadingDetailService.Get(flowImportedId, null);
            IEnumerable<MeterReadingDetailCheckedDto> data = MeterReadingDetailCompletedGetMeterReadingDetailControl(detail.Where(d => d.ExcludedByUserId is null));
            ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCheckedDto> result = MeterReadingDetailCompletedGetResult(data, detail?.Where(d => d.ExcludedByUserId is not null).Count() ?? 0);
            return result;
        }

        private ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCheckedDto> MeterReadingDetailCompletedGetResult(IEnumerable<MeterReadingDetailCheckedDto> data, int excludedCount)
        {
            MeterReadingDetailHeaderOutputDto header = new MeterReadingDetailHeaderOutputDto()
            {
                Amount = data?.Sum(m => m.SumItems) ?? 0,
                Consumption = data?.Sum(m => m.Consumption) ?? 0,
                RecordCount = data?.Count() ?? 0,
                FromReadingNumber = data?.Min(m => m.ReadingNumber) ?? string.Empty,
                ToReadingNumber = data?.Max(m => m.ReadingNumber) ?? string.Empty,
                Closed = data?.Count(r => r.CurrentCounterStateCode == (int)CounterStateCodeEnum.Close) ?? 0,
                Obstacle = data?.Count(r => r.CurrentCounterStateCode == (int)CounterStateCodeEnum.Block) ?? 0,
                Temporarily = data?.Count(r => r.CurrentCounterStateCode == (int)CounterStateCodeEnum.NonRead) ?? 0,
                PureReading = data?.Count(r => !_MeterReadingDetailCompletedclosedAndObstacleCounterState.Contains(r.CurrentCounterStateCode)) ?? 0,
                Malfunction = data?.Count(r => r.CurrentCounterStateCode == (int)CounterStateCodeEnum.Malfunction) ?? 0,
                Changed = data?.Count(r => r.CurrentCounterStateCode == (int)CounterStateCodeEnum.Change) ?? 0,
                NextRound = data?.Count(r => r.CurrentCounterStateCode == (int)CounterStateCodeEnum.NextRound) ?? 0,
                WithoutConsumption = data?.Count(r => r.CurrentCounterStateCode == (int)CounterStateCodeEnum.WithoutConsumption) ?? 0,
                Excluded = excludedCount
            };
            ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCheckedDto> result = new(_MeterReadingDetailCompletedreportTitle, header, data.OrderByDescending(meter => meter.AttentionState));
            return result;
        }

        private IEnumerable<MeterReadingDetailCheckedDto> MeterReadingDetailCompletedGetMeterReadingDetailControl(IEnumerable<MeterReadingDetailDataOutputDto> input)
        {
            return input.Select(data => new MeterReadingDetailCheckedDto() { Id = data.Id, FlowImportedId = data.FlowImportedId, ZoneId = data.ZoneId, CustomerNumber = data.CustomerNumber, ReadingNumber = data.ReadingNumber, BillId = data.BillId, AgentCode = data.AgentCode, CurrentCounterStateCode = data.CurrentCounterStateCode, PreviousDateJalali = data.PreviousDateJalali, CurrentDateJalali = data.CurrentDateJalali, PreviousNumber = data.PreviousNumber, CurrentNumber = data.CurrentNumber, InsertByUserId = data.InsertByUserId, InsertDateTime = data.InsertDateTime, Duration = data.Modat ?? 0, BranchTypeId = data.BranchTypeId, BranchTypeTitle = data.BranchTypeTitle, UsageId = data.UsageId, UsageTitle = data.UsageTitle, ConsumptionUsageId = data.ConsumptionUsageId, DomesticUnit = data.DomesticUnit, CommercialUnit = data.CommercialUnit, OtherUnit = data.OtherUnit, TotalUnit = data.DomesticUnit + data.CommercialUnit + data.OtherUnit, EmptyUnit = data.EmptyUnit, WaterInstallationDateJalali = data.WaterInstallationDateJalali, SewageInstallationDateJalali = data.SewageInstallationDateJalali, WaterRegisterDate = data.WaterRegisterDate, SewageRegisterDate = data.SewageRegisterDate, WaterCount = data.WaterCount, SewageCalcState = data.SewageCalcState, HouseholdDate = data.HouseholdDate, HouseholdNumber = data.HouseholdNumber, VillageId = data.VillageId, IsSpecial = data.IsSpecial, MeterDiameterId = data.MeterDiameterId, VirtualCategoryId = data.VirtualCategoryId, ContractualCapacity = data.ContractualCapacity, BodySerial = data.BodySerial, TavizCause = data.TavizCause, TavizDateJalali = data.TavizDateJalali, TavizNumber = data.TavizNumber, TavizRegisterDateJalali = data.TavizRegisterDateJalali, PreviousCounterStateCode = data.PreviousCounterStateCode, PreviousMeterDateJalali = data.PreviousMeterDateJalali, PreviousMeterNumber = data.PreviousMeterNumber, PreviousConsumption = data.PreviousConsumption, PreviousMonthlyConsumption = data.PreviousMonthlyConsumption, PreviousCounterStateTitle = data.PreviousCounterStateTitle, PreviousSumItems = data.PreviousSumItems, BeforDebt = data.BeforDebt, WaterDebt = data.WaterDebt, SumItems = data.SumItems, SumItemsBeforeDiscount = data.SumItemsBeforeDiscount, DiscountSum = data.DiscountSum, Consumption = data.Consumption, MonthlyConsumption = data.MonthlyConsumption, MonthlyPerUnit = data.MonthlyPerUnit, AttentionState = HighLowEnum.Zero, HasAttentionCounterState = false }).ToList();
        }
    }
}
