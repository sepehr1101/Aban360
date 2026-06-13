using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using DNTPersianUtils.Core;
using System.Diagnostics;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Implementations
{
    internal sealed class GenerateBillWithPreDebtHandler : IGenerateBillWithPreDebtHandler
    {
        private readonly ICommonZoneService _commonZoneService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly IBedBesQueryService _bedBesQueryService;
        private readonly ICustomerInfoService _customerInfoService;
        private const int _counterStateCode = 0;
        public GenerateBillWithPreDebtHandler(
            ICommonZoneService commonZoneService,
            ICommonMemberQueryService commonMemberQueryService,
            IBedBesQueryService bedBesQueryService,
            ICustomerInfoService customerInfoService)
        {
            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(bedBesQueryService));

            _bedBesQueryService = bedBesQueryService;
            _bedBesQueryService.NotNull(nameof(bedBesQueryService));

            _customerInfoService = customerInfoService;
            _customerInfoService.NotNull(nameof(customerInfoService));
        }

        public async Task<NewBillOutputDto> Handle(string billId, IAppUser appUser, CancellationToken cancellationToken)
        {
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(billId);
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(zoneIdAndCustomerNumber);
            await _commonZoneService.IsUserInZone(appUser, zoneIdAndCustomerNumber.ZoneId);
            CustomerInfoGetDto customerInfo = await _customerInfoService.Get(zoneIdAndCustomerNumber.ZoneId, zoneIdAndCustomerNumber.CustomerNumber);

            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            AbBahaCalculationDetails abBahaCalcResult = GetAbBahaCalcWithZeroValues(currentDateJalali, memberInfo, customerInfo,  memberInfo.DebtAmount ?? 0);
            BedBesPreviousNumberAndDateOutputDto previousMeterInfo = await _bedBesQueryService.GetPreviousDateAndNumber(zoneIdAndCustomerNumber, billId);
            NewBillOutputDto result = new()
            {
                AbBahaCalculationDetail = abBahaCalcResult,
                PreviousBillsInfo = await _bedBesQueryService.GetPreviousBillsInfo(zoneIdAndCustomerNumber),
                PreviousMeterChangeDateJalali = customerInfo.TavizInfo?.TavizDateJalali ?? string.Empty,
                PreviousMeterNumber = previousMeterInfo.PreviousNumber,//todo
                PreviousReadingDateJalali = previousMeterInfo.PreviousDateJalali
            };

            return result;
        }
        private AbBahaCalculationDetails GetAbBahaCalcWithZeroValues(string currentDateJalali, MemberInfoGetDto memberInfo, CustomerInfoGetDto customerInfo, long preDebtAmount)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            string previousDate = customerInfo.BedBesInfo?.LastMeterDateJalali ?? customerInfo.MembersInfo.WaterInstallationDateJalali;
            int previousNumber = customerInfo.BedBesInfo?.LastMeterNumber ?? 0;
            int finalUnit = GetFinalDomesticUnit(customerInfo, currentDateJalali);
            ConsumptionInfo consumptionInfo = new(previousDate, currentDateJalali, 0, GetDuration(previousDate, currentDateJalali), 0, finalUnit);
            MeterInfoOutputDto meterInfo = new(previousDate, currentDateJalali, 0, 0, _counterStateCode);

            CustomerDetailInfoInputDto customerDetailInfo = new()
            {
                ZoneId = customerInfo.MembersInfo.ZoneId,
                Radif = customerInfo.MembersInfo.CustomerNumber,
                BranchType = customerInfo.MembersInfo.BranchTypeId,
                UsageId = customerInfo.MembersInfo.UsageId,
                DomesticUnit = customerInfo.MembersInfo.DomesticUnit,
                CommertialUnit = customerInfo.MembersInfo.CommercialUnit,
                OtherUnit = customerInfo.MembersInfo.OtherUnit,
                EmptyUnit = customerInfo.MembersInfo.EmptyUnit,
                WaterInstallationDateJalali = customerInfo.MembersInfo.WaterInstallationDateJalali,
                SewageInstallationDateJalali = customerInfo.MembersInfo.SewageInstallationDateJalali,
                WaterRegisterDate = customerInfo.MembersInfo.WaterRegisterDate,
                SewageRegisterDate = customerInfo.MembersInfo.SewageRegisterDate,
                SewageCalcState = customerInfo.MembersInfo.SewageCalcState,
                ContractualCapacity = customerInfo.MembersInfo.ContractualCapacity,
                HouseholdNumber = customerInfo.MembersInfo.HouseholdNumber,
                HouseholdDate = customerInfo.MembersInfo.HouseholdDate,
                ReadingNumber = customerInfo.MembersInfo.ReadingNumber,
                VillageId = customerInfo.MembersInfo.VillageId,
                IsSpecial = customerInfo.MembersInfo.IsSpecial,
                VirtualCategoryId = customerInfo.MembersInfo.VirtualCategoryId,
                CounterStateCode = _counterStateCode,
            };
            MeterInfoByPreviousDataInputDto previousMeterInfo = new()
            {
                BillId = customerInfo.MembersInfo.BillId,
                PreviousDateJalali = previousDate,
                PreviousNumber = 0,
                CurrentDateJalali = currentDateJalali,
                CurrentMeterNumber = 0,
                CounterStateCode = _counterStateCode,
            };
            MeterImaginaryInputDto meterImaginaryDto = new() { CustomerInfo = customerDetailInfo, MeterPreviousData = previousMeterInfo };
            CustomerInfoOutputDto customerInfoOutputDto = new(meterImaginaryDto);
            customerInfoOutputDto.ZoneTitle = memberInfo.ZoneTitle;
            customerInfoOutputDto.UsageTitle = memberInfo.UsageTitle;
            customerInfoOutputDto.FullName = memberInfo.FullName;

            stopWatch.Stop();

            AbBahaCalculationDetails abBahaCalcResult = new()
            {
                SumItemsBeforeDiscount = preDebtAmount,
                SumItems = preDebtAmount,
                SumAbBahaAmount = 0,
                AbBahaAmount = 0,
                HotSeasonAbBahaAmount = 0,
                HotSeasonFazelabAmount = 0,
                AbonmanAbAmount = 0,
                FazelabAmount = 0,
                BoodjePart1Amount = 0,
                BoodjePart2Amount = 0,
                SumBoodje = 0,
                JavaniAmount = 0,
                MaliatAmount = 0,
                AbonmanFazelabAmount = 0,
                AvarezAmount = 0,
                AbBahaDiscount = 0,
                HotSeasonDiscount = 0,
                HotSeasonFazelabDiscount = 0,
                FazelabDiscount = 0,
                AbonmanAbDiscount = 0,
                AbonmanFazelabDiscount = 0,
                AvarezDiscount = 0,
                JavaniDiscount = 0,
                BoodjeDiscount = 0,
                MaliatDiscount = 0,
                DiscountSum = 0,
                Consumption = consumptionInfo.Consumption,
                MonthlyConsumption = consumptionInfo.MonthlyAverageConsumption,
                DailyConsumption = consumptionInfo.DailyAverageConsumption,
                Duration = consumptionInfo.Duration,
                Nerkh = new List<NerkhGetDto>(),
                AbAzad = new List<AbAzadFormulaDto>(),
                Zarib = new List<ZaribGetDto>(),
                MeterInfo = meterInfo,
                Customer = customerInfoOutputDto,
                StopWatch = stopWatch.ElapsedMilliseconds
            };
            return abBahaCalcResult;
        }
        private int GetFinalDomesticUnit(CustomerInfoGetDto customerInfo, string readingDateJalali)
        {
            if (IsGardenAndResidence(customerInfo.MembersInfo.UsageId))
            {
                return customerInfo.MembersInfo.DomesticUnit < 1 ? 1 : customerInfo.MembersInfo.DomesticUnit;//((/*customerInfo.OtherUnit + */customerInfo.DomesticUnit) == 0 ? 1 : /*customerInfo.OtherUnit + */ customerInfo.DomesticUnit);
            }
            int finalHousehold = GetHouseholdUnit(customerInfo.MembersInfo.HouseholdNumber, customerInfo.MembersInfo.HouseholdDate, readingDateJalali);
            if (finalHousehold > 1)
            {
                return customerInfo.MembersInfo.HouseholdNumber;//customerInfo.DomesticUnit;
            }
            return customerInfo.MembersInfo.DomesticUnit - customerInfo.MembersInfo.EmptyUnit < 1 ? 1 : customerInfo.MembersInfo.DomesticUnit - customerInfo.MembersInfo.EmptyUnit;
        }
        internal static bool IsGardenAndResidence(int usageId)
        {
            int[] s = [25, 34];
            return s.Contains(usageId);
        }
        private int GetHouseholdUnit(int householdUnit, string? householdDate, string readingDateJalali)
        {
            if (householdUnit <= 0)
            {
                return 0;
            }
            if (string.IsNullOrWhiteSpace(householdDate))
            {
                return 0;
            }
            DateTime? expireHouseHoldGregorian = householdDate.ToGregorianDateTime();
            if (!expireHouseHoldGregorian.HasValue)
            {
                return 0;
            }
            DateTime? readingDateGregorian = readingDateJalali.ToGregorianDateTime();
            if (!readingDateGregorian.HasValue)
            {
                throw new InvalidDateException(readingDateJalali);
            }
            if (expireHouseHoldGregorian.Value.AddYears(1) < readingDateGregorian.Value)
            {
                return 0;
            }
            return householdUnit;
        }
        private int GetDuration(string previousDate, string currentDate)
        {
            int thresholdDay = 1;
            var previousGregorian = previousDate.ToGregorianDateTime();
            var currentGregorian = currentDate.ToGregorianDateTime();
            int duration = (currentGregorian.Value - previousGregorian.Value).Days;
            if (duration < thresholdDay)
            {
                throw new InvalidBillIdException(ExceptionLiterals.InvalidDuration);
            }
            return duration;
        }
    }
}
