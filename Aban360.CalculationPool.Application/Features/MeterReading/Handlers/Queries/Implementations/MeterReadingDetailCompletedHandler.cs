using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class MeterReadingDetailCompletedHandler : IMeterReadingDetailCompletedHandler
    {
        private readonly IMeterFlowQueryService _meterFlowQueryService;
        private readonly IMeterReadingDetailQueryService _meterReadingDetailQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private static int[] closedAndObstacleCounterState = { (int)CounterStateCodeEnum.Close, (int)CounterStateCodeEnum.Block, (int)CounterStateCodeEnum.NonRead };
        private static string _reportTitle = ReportLiterals.MeterReadingCompleted;
        public MeterReadingDetailCompletedHandler(
            IMeterFlowQueryService meterFlowQueryService,
            IMeterReadingDetailQueryService meterReadingDetailQueryService,
            ICommonZoneService commonZoneService)
        {
            _meterFlowQueryService = meterFlowQueryService;
            _meterFlowQueryService.NotNull(nameof(meterFlowQueryService));

            _meterReadingDetailQueryService = meterReadingDetailQueryService;
            _meterReadingDetailQueryService.NotNull(nameof(meterReadingDetailQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));
        }

        public async Task<ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCheckedDto>> Handle(int flowImportedId, IAppUser appUser, CancellationToken cancellationToken)
        {
            MeterFlowGetDto meterFlowInfo = await _meterFlowQueryService.Get(flowImportedId);
            await _commonZoneService.IsUserInZone(appUser, meterFlowInfo.ZoneId);
            IEnumerable<MeterReadingDetailDataOutputDto> detail = await _meterReadingDetailQueryService.Get(flowImportedId, null);
            IEnumerable<MeterReadingDetailCheckedDto> data = GetMeterReadingDetailControl(detail.Where(d => d.ExcludedByUserId is null));

            ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCheckedDto> result = GetResult(data, detail?.Where(d => d.ExcludedByUserId is not null).Count() ?? 0);
            return result;
        }
        private ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCheckedDto> GetResult(IEnumerable<MeterReadingDetailCheckedDto> data, int excludedCount)
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
                PureReading = data?.Count(r => !closedAndObstacleCounterState.Contains(r.CurrentCounterStateCode)) ?? 0,
                Malfunction = data?.Count(r => r.CurrentCounterStateCode == (int)CounterStateCodeEnum.Malfunction) ?? 0,
                Changed = data?.Count(r => r.CurrentCounterStateCode == (int)CounterStateCodeEnum.Change) ?? 0,
                NextRound = data?.Count(r => r.CurrentCounterStateCode == (int)CounterStateCodeEnum.NextRound) ?? 0,
                WithoutConsumption = data?.Count(r => r.CurrentCounterStateCode == (int)CounterStateCodeEnum.WithoutConsumption) ?? 0,
                Excluded = excludedCount
            };
            ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCheckedDto> result = new(_reportTitle, header, data.OrderByDescending(meter => meter.AttentionState));

            return result;
        }
        private IEnumerable<MeterReadingDetailCheckedDto> GetMeterReadingDetailControl(IEnumerable<MeterReadingDetailDataOutputDto> input)
        {
            return input.Select(data => new MeterReadingDetailCheckedDto()
            {
                Id = data.Id,
                FlowImportedId = data.FlowImportedId,
                ZoneId = data.ZoneId,
                CustomerNumber = data.CustomerNumber,
                ReadingNumber = data.ReadingNumber,
                BillId = data.BillId,
                AgentCode = data.AgentCode,
                CurrentCounterStateCode = data.CurrentCounterStateCode,
                PreviousDateJalali = data.PreviousDateJalali,
                CurrentDateJalali = data.CurrentDateJalali,
                PreviousNumber = data.PreviousNumber,
                CurrentNumber = data.CurrentNumber,
                InsertByUserId = data.InsertByUserId,
                InsertDateTime = data.InsertDateTime,
                Duration = data.Modat ?? 0,

                BranchTypeId = data.BranchTypeId,
                BranchTypeTitle = data.BranchTypeTitle,
                UsageId = data.UsageId,
                UsageTitle = data.UsageTitle,
                ConsumptionUsageId = data.ConsumptionUsageId,
                DomesticUnit = data.DomesticUnit,
                CommercialUnit = data.CommercialUnit,
                OtherUnit = data.OtherUnit,
                TotalUnit = data.DomesticUnit + data.CommercialUnit + data.OtherUnit,
                EmptyUnit = data.EmptyUnit,
                WaterInstallationDateJalali = data.WaterInstallationDateJalali,
                SewageInstallationDateJalali = data.SewageInstallationDateJalali,
                WaterRegisterDate = data.WaterRegisterDate,
                SewageRegisterDate = data.SewageRegisterDate,
                WaterCount = data.WaterCount,
                SewageCalcState = data.SewageCalcState,
                HouseholdDate = data.HouseholdDate,
                HouseholdNumber = data.HouseholdNumber,
                VillageId = data.VillageId,
                IsSpecial = data.IsSpecial,
                MeterDiameterId = data.MeterDiameterId,
                VirtualCategoryId = data.VirtualCategoryId,
                ContractualCapacity = data.ContractualCapacity,
                BodySerial = data.BodySerial,

                TavizCause = data.TavizCause,
                TavizDateJalali = data.TavizDateJalali,
                TavizNumber = data.TavizNumber,
                TavizRegisterDateJalali = data.TavizRegisterDateJalali,

                PreviousCounterStateCode = data.PreviousCounterStateCode,
                PreviousMeterDateJalali = data.PreviousMeterDateJalali,
                PreviousMeterNumber = data.PreviousMeterNumber,
                PreviousConsumption = data.PreviousConsumption,
                PreviousMonthlyConsumption = data.PreviousMonthlyConsumption,
                PreviousCounterStateTitle = data.PreviousCounterStateTitle,
                PreviousSumItems = data.PreviousSumItems,

                BeforDebt = data.BeforDebt,
                WaterDebt = data.WaterDebt,
                SumItems = data.SumItems,
                SumItemsBeforeDiscount = data.SumItemsBeforeDiscount,
                DiscountSum = data.DiscountSum,
                Consumption = data.Consumption,
                MonthlyConsumption = data.MonthlyConsumption,
                MonthlyPerUnit = data.MonthlyPerUnit,

                AttentionState = HighLowEnum.Zero,
                HasAttentionCounterState = false
            }).ToList();
        }
    }
}
