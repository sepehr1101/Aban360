using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class ConsumptionCheckedHandler : IConsumptionCheckedHandler
    {
        private readonly IMeterReadingDetailService _meterReadingDetailService;
        private readonly IMeterFlowService _meterFlowService;
        private const int _domesticConsumptionExpirePercent = 30;
        private const int _nonDomesticConsumptionExpirePercent = 30;
        public ConsumptionCheckedHandler(
            IMeterReadingDetailService meterReadingDetailService,
            IMeterFlowService meterFlowService)
        {
            _meterReadingDetailService = meterReadingDetailService;
            _meterReadingDetailService.NotNull(nameof(meterReadingDetailService));

            _meterFlowService = meterFlowService;
            _meterFlowService.NotNull(nameof(meterFlowService));
        }

        public async Task<IEnumerable<MeterReadingDetailCheckedDto>> Handle(int latestFlowId, IAppUser appUser, CancellationToken cancellationToken)
        {
            int firstFlowId = await _meterFlowService.GetFirstFlowId(latestFlowId);
            IEnumerable<MeterReadingDetailGetDto> meterReadings = await _meterReadingDetailService.Get(firstFlowId);
            IEnumerable<MeterReadingDetailCheckedDto> readingsCheck = GetReadingControl(meterReadings);
            await CreateConsumpitonCheckedFlow(latestFlowId, appUser);

            return readingsCheck;
        }
        private async Task CreateConsumpitonCheckedFlow(int latestFlowId, IAppUser appUser)
        {
            MeterFlowUpdateDto meterFlowUpdate = new(latestFlowId, appUser.UserId, DateTime.Now);
            _meterFlowService.Update(meterFlowUpdate);

            MeterFlowGetDto meterFlow = await _meterFlowService.Get(latestFlowId);
            MeterFlowCreateDto newMeterFlow = new()
            {
                MeterFlowStepId = MeterFlowStepEnum.ConsumptionChecked,
                ZoneId = meterFlow.ZoneId,
                FileName = meterFlow.FileName,
                InsertByUserId = appUser.UserId,
                InsertDateTime = DateTime.Now,
            };
            await _meterFlowService.Create(newMeterFlow);
        }
        private IEnumerable<MeterReadingDetailCheckedDto> GetReadingControl(IEnumerable<MeterReadingDetailGetDto> meterReadings)
        {
            ICollection<MeterReadingDetailCheckedDto> readingsControl = new List<MeterReadingDetailCheckedDto>();
            meterReadings.ForEach(mr =>
            {
                MeterReadingDetailCheckedDto readingControl = GetMeterReadingDetailControl(mr);
                readingsControl.Add(readingControl);
            });

            return readingsControl;
        }
        private MeterReadingDetailCheckedDto GetMeterReadingDetailControl(MeterReadingDetailGetDto input)
        {
            bool hasAttention = false;
            if (IsDomestic(input.UsageId))
            {
                //domestic
                hasAttention = GetHasAttention(_domesticConsumptionExpirePercent, input.LastMonthlyConsumption.Value, input.MonthlyConsumption.Value);
            }
            else
            {
                //nonDimestic
                hasAttention = GetHasAttention(_nonDomesticConsumptionExpirePercent, input.ContractualCapacity, input.MonthlyConsumption.Value);
            }


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

                UsageId = input.UsageId,
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


                TavizCause = input.TavizCause,
                TavizDateJalali = input.TavizDateJalali,
                TavizNumber = input.TavizNumber,
                TavizRegisterDateJalali = input.TavizRegisterDateJalali,

                LastCounterStateCode = input.LastCounterStateCode,
                LastMeterDateJalali = input.LastMeterDateJalali,
                LastMeterNumber = input.LastMeterNumber,
                LastConsumption = input.LastConsumption,
                LastMonthlyConsumption = input.LastMonthlyConsumption,
                LastSumItems=input.LastSumItems,

                SumItems = input.SumItems,
                SumItemsBeforeDiscount = input.SumItemsBeforeDiscount,
                DiscountSum = input.DiscountSum,
                Consumption = input.Consumption,
                MonthlyConsumption = input.MonthlyConsumption,

                HasAttention = hasAttention
            };
        }
        private bool IsDomestic(int usageId)
        {
            int[] domesticUsage = [0, 1, 3];
            return domesticUsage.Contains(usageId);
        }
        private bool GetHasAttention(int expirePercent, double lastMonthlyConsumption, double monthlyConsumption)
        {
            double expireValue = (double)(lastMonthlyConsumption * expirePercent / 100d);
            double minValue = (double)(lastMonthlyConsumption - expireValue);
            double maxValue = (double)(lastMonthlyConsumption + expireValue);

            return monthlyConsumption >= minValue && monthlyConsumption <= maxValue ?
                   false :
                   true;
        }
    }
}
