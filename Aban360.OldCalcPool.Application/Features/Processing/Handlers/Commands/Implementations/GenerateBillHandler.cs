using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Diagnostics;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Implementations
{
    internal sealed class GenerateBillHandler : AbstractBaseConnection, IGenerateBillHandler
    {
        private readonly ICustomerInfoService _customerInfoService;
        private readonly IOldTariffEngine _tariffEngine;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly IValidator<GenerateBillInputDto> _validator;
        private readonly IVariabService _variabService;
        static int[] _domesticUsage = { 1, 3 };//todo: IsTrue?
        static int[] _allowedZeroMeterNumberCounterState = { 4, 7 };
        const int _paymentDeadline = 7;
        const int _conditionPayableAmount = 10000;
        const float _domesticMaltiplier = 0.7f;
        const int _temporaryDeletionStateId = 5;
        const int _changeCodeCounterState = 3;
        const int _reverseCodeCounterSatate = 5;
        public GenerateBillHandler(
            ICustomerInfoService customerInfoService,
            IOldTariffEngine tariffEngine,
            ICommonMemberQueryService commonMemberQueryService,
            IConfiguration configuration,
            IValidator<GenerateBillInputDto> validator,
            IVariabService variabService)
            : base(configuration)
        {
            _customerInfoService = customerInfoService;
            _customerInfoService.NotNull(nameof(customerInfoService));

            _tariffEngine = tariffEngine;
            _tariffEngine.NotNull(nameof(tariffEngine));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

            _variabService = variabService;
            _variabService.NotNull(nameof(variabService));
        }

        public async Task<AbBahaCalculationDetails> Handle(GenerateBillInputDto inputDto, CancellationToken cancellationToken)
        {
            await InputValidation(inputDto, cancellationToken);
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await GetZoneIdANdCustomerNumber(inputDto.BillId);
            CustomerInfoGetDto customerInfo = await _customerInfoService.Get(zoneIdAndCustomerNumber.ZoneId, zoneIdAndCustomerNumber.CustomerNumber);
            await Validation(inputDto, zoneIdAndCustomerNumber, customerInfo);

            AbBahaCalculationDetails abBahaCalcResult;
            if (IsAllowedZeroMeterNumber(inputDto.CounterStateCode))
            {
                abBahaCalcResult = GetAbBahaCalcWithZeroValues(inputDto, customerInfo);
            }
            else
            {
                MeterInfoByPreviousDataInputDto tariffMeterInfoByPreviousData = GetMeterInfoByPreviousData(customerInfo, inputDto);
                abBahaCalcResult = await _tariffEngine.Handle(tariffMeterInfoByPreviousData, cancellationToken);
            }
            if (!inputDto.IsConfirm)
            {
                return abBahaCalcResult;
            }
            BedBesCreateDto bedBes = await GetBedBes(customerInfo, abBahaCalcResult, inputDto, zoneIdAndCustomerNumber, inputDto.CounterStateCode);
            KasrHaDto kasrHa = GerKasrHa(customerInfo, abBahaCalcResult, inputDto);
            ContorUpdateDto contorUpdate = GetControUpdateDto(customerInfo, bedBes);

            await SqlCommands(zoneIdAndCustomerNumber, bedBes, kasrHa, contorUpdate, abBahaCalcResult, inputDto.CounterStateCode);

            return abBahaCalcResult;
        }
        private AbBahaCalculationDetails GetAbBahaCalcWithZeroValues(GenerateBillInputDto inputDto, CustomerInfoGetDto customerInfo)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            inputDto.MeterNumber = 0;
            string previousDate = customerInfo.BedBesInfo?.LastMeterDateJalali ?? customerInfo.MembersInfo.WaterInstallationDateJalali;
            int previousNumber = customerInfo.BedBesInfo?.LastMeterNumber ?? 0;
            int finalUnit = GetFinalDomesticUnit(customerInfo, inputDto.CurrentDateJalali);
            ConsumptionInfo consumptionInfo = new(previousDate, inputDto.CurrentDateJalali, 0, GetDuration(previousDate, inputDto.CurrentDateJalali), 0, finalUnit);
            MeterInfoOutputDto meterInfo = new(previousDate, inputDto.CurrentDateJalali, previousNumber, 0, inputDto.CounterStateCode);

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
                CounterStateCode = inputDto.CounterStateCode,
            };
            MeterInfoByPreviousDataInputDto previousMeterInfo = new()
            {
                BillId = customerInfo.MembersInfo.BillId,
                PreviousDateJalali = previousDate,
                PreviousNumber = previousNumber,
                CurrentDateJalali = inputDto.CurrentDateJalali,
                CurrentMeterNumber = inputDto.MeterNumber,
                CounterStateCode = inputDto.CounterStateCode,
            };
            MeterImaginaryInputDto meterImaginaryDto = new() { CustomerInfo = customerDetailInfo, MeterPreviousData = previousMeterInfo };
            CustomerInfoOutputDto customerInfoOutputDto = new(meterImaginaryDto);
            stopWatch.Stop();


            AbBahaCalculationDetails abBahaCalcResult = new AbBahaCalculationDetails(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            new List<NerkhGetDto>(), new List<AbAzadFormulaDto>(), new List<ZaribGetDto>(),
            consumptionInfo, meterInfo, customerInfoOutputDto, stopWatch.ElapsedMilliseconds, 0);

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
        private async Task SqlCommands(ZoneIdAndCustomerNumber zoneIdAndCustomerNumber, BedBesCreateDto bedBes, KasrHaDto kasrHa, ContorUpdateDto contorUpdate, AbBahaCalculationDetails abBahaCalcResult, int? counterStateCode)
        {
            string dbName = GetDbName(zoneIdAndCustomerNumber.ZoneId);

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        BedBesCommandService bedBedCommandService = new BedBesCommandService(connection, transaction);
                        KasrHaCommandService kasrHasCommandService = new KasrHaCommandService(connection, transaction);
                        MembersCommandService membersCommandService = new MembersCommandService(connection, transaction);
                        WaterDebtCommandService waterDebtCommandService = new WaterDebtCommandService(connection, transaction);
                        BillCommandService billCommandService = new BillCommandService(connection, transaction);
                        ContorCommandService controCommandService = new ContorCommandService(connection, transaction);

                        int bedBesRecordId = await bedBedCommandService.Insert(bedBes, dbName);
                        BillByBedBedIdInsertDto billInsertByBedBesIdDto = new(zoneIdAndCustomerNumber.ZoneId, zoneIdAndCustomerNumber.CustomerNumber, GetTypeId(counterStateCode), bedBesRecordId);
                        await billCommandService.InsertByBedBesId(billInsertByBedBesIdDto, dbName);

                        if (abBahaCalcResult.DiscountSum > 0)
                        {
                            await kasrHasCommandService.Insert(kasrHa, dbName);
                        }
                        if (!IsAllowedZeroMeterNumber(counterStateCode))
                        {
                            await waterDebtCommandService.UpdateAmount(bedBes.ShGhabs1, (long)bedBes.Baha);
                            await membersCommandService.UpdateBedbes(zoneIdAndCustomerNumber, (long)bedBes.Baha, dbName);
                            await controCommandService.Update(contorUpdate, dbName, true);                                                                                                   //update contro

                        }

                        transaction.Commit();
                    }
                    catch (Exception es)
                    {
                        transaction.Rollback();
                        throw es;
                    }
                }
            }
        }
        private async Task<ZoneIdAndCustomerNumber> GetZoneIdANdCustomerNumber(string billId)
        {
            ZoneIdAndCustomerNumber result = await _commonMemberQueryService.Get(billId);
            if (result.DeletionStateId == 1)
            {
                throw new InvalidBillIdException(ExceptionLiterals.InvalidDeletionState);
            }
            return result;
        }
        private ContorUpdateDto GetControUpdateDto(CustomerInfoGetDto customerInfo, BedBesCreateDto bedBes)
        {
            return new ContorUpdateDto()
            {
                ZoneId = customerInfo.MembersInfo.ZoneId,
                CustomerNumber = customerInfo.MembersInfo.CustomerNumber,
                CurrentDateJalali = bedBes.TodayDate,
                CurrentNumber = (int)bedBes.TodayNo,
                Consumption = (int)bedBes.Masraf,
                ConsumptionAverage = (float)bedBes.Rate,
                MeterChangeDateJalali = customerInfo.TavizInfo?.TavizDateJalali ?? string.Empty,
                MeterChangeNumber = customerInfo.TavizInfo?.TavizNumber ?? 0
            };
        }
        private MeterInfoByPreviousDataInputDto GetMeterInfoByPreviousData(CustomerInfoGetDto customerInfo, GenerateBillInputDto generateBillInfo)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            if (!string.IsNullOrWhiteSpace(generateBillInfo.CurrentDateJalali))
            {
                currentDateJalali = generateBillInfo.CurrentDateJalali;
            }
            return new MeterInfoByPreviousDataInputDto()
            {
                BillId = customerInfo.MembersInfo.BillId,
                PreviousDateJalali = customerInfo.BedBesInfo?.LastMeterDateJalali ?? customerInfo.MembersInfo.WaterInstallationDateJalali,
                PreviousNumber = customerInfo.BedBesInfo?.LastMeterNumber ?? 0,
                CurrentDateJalali = currentDateJalali,
                CurrentMeterNumber = generateBillInfo.MeterNumber,
                CounterStateCode = generateBillInfo.CounterStateCode
            };
        }
        private async Task<BedBesCreateDto> GetBedBes(CustomerInfoGetDto customerInfo, AbBahaCalculationDetails abBahaCalc, GenerateBillInputDto generateBillInfo, ZoneIdAndCustomerNumber zoneIdAndCustomerNumber, int? counterSatetCode)
        {
            double preDebtAmount = await _customerInfoService.GetMembersBedBes(zoneIdAndCustomerNumber);//checkResult: changeDto
            var (sumItems, jam, pard) = GetAmounts(preDebtAmount, abBahaCalc.SumItems);
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            string mohlatDateJalali = DateTime.Now.AddDays(_paymentDeadline).ToShortPersianDateString();
            string paymentIdOption = $"1{currentDateJalali.Substring(5, 2)}";
            string paymentId = IsAllowedZeroMeterNumber(counterSatetCode) ?
                string.Empty :
                TransactionIdGenerator.GeneratePaymentId((long)pard, abBahaCalc.Customer.BillId, paymentIdOption);
            decimal barge = await _variabService.GetAndRenew(abBahaCalc.Customer.ZoneId);

            return new BedBesCreateDto()// ToDo :check
            {
                Town = customerInfo.MembersInfo.ZoneId,
                Radif = customerInfo.MembersInfo.CustomerNumber,
                Eshtrak = customerInfo.MembersInfo.ReadingNumber,
                Barge = barge,
                PriNo = (decimal)customerInfo.BedBesInfo.LastMeterNumber,
                TodayNo = generateBillInfo.MeterNumber,
                PriDate = customerInfo.BedBesInfo.LastMeterDateJalali,
                TodayDate = currentDateJalali,
                AbonFas = (decimal)abBahaCalc.AbonmanFazelabAmount,
                FasBaha = (decimal)abBahaCalc.FazelabAmount,
                AbBaha = (decimal)abBahaCalc.AbBahaAmount,
                Ztadil = 0,
                Masraf = (decimal)abBahaCalc.Consumption,
                Shahrdari = (decimal)abBahaCalc.MaliatAmount,
                Modat = abBahaCalc.Duration,
                DateBed = currentDateJalali,
                JalaseNo = 0,
                Mohlat = mohlatDateJalali,
                Baha = (decimal)sumItems,
                AbonAb = (decimal)abBahaCalc.AbonmanAbAmount,
                Pard = (decimal)pard,
                Jam = (decimal)jam,
                CodVas = abBahaCalc.MeterInfo.CounterStateCode ?? 0,
                Ghabs = "1",
                Del = false,
                Type = "1",
                CodEnshab = customerInfo.MembersInfo.UsageId,
                Enshab = customerInfo.MembersInfo.MeterDiameterId,
                Elat = 0,
                Serial = 0,
                Ser = 0,
                ZaribFasl = (decimal)abBahaCalc.HotSeasonAbBahaAmount,
                Ab10 = 0,
                Ab20 = 0,
                TedadVahd = customerInfo.MembersInfo.OtherUnit,
                TedKhane = customerInfo.MembersInfo.HouseholdNumber,
                TedadMas = customerInfo.MembersInfo.DomesticUnit,
                TedadTej = customerInfo.MembersInfo.CommercialUnit,
                NoeVa = abBahaCalc.Customer.BranchType,
                Jarime = 0,
                Masjar = 0,
                Sabt = 1,//todo
                Rate = (decimal)abBahaCalc.MonthlyConsumption,
                Operator = 5,//generate manual bill
                Mamor = 0,
                TavizDate = customerInfo?.TavizInfo?.TavizDateJalali ?? string.Empty,
                ZaribCntr = 0,
                Zabresani = 0,
                ZaribD = (decimal)abBahaCalc.JavaniAmount,
                Tafavot = 0,
                KasrHa = (decimal)abBahaCalc.DiscountSum,
                FixMas = customerInfo.MembersInfo.ContractualCapacity,
                ShGhabs1 = customerInfo.MembersInfo.BillId,
                ShPard1 = paymentId,
                TabAbnA = 0,
                TabAbnF = 0,
                TabsFa = 0,
                NewAb = 0,
                NewFa = 0,
                Bodjeh = (decimal)abBahaCalc.SumBoodje,
                Group1 = customerInfo.MembersInfo.ConsumptionUsageId,
                MasFas = GetSewageConsumption(abBahaCalc.Customer.UsageId, abBahaCalc.Consumption),
                Faz = customerInfo.MembersInfo.SewageCalcState >= 1,
                ChkKarbari = 0,
                C200 = 0,
                DateIns = currentDateJalali,
                AbSevom = 0,
                AbSevom1 = 0,
                C70 = 0,
                C80 = 0,
                TmpDateBed = "",
                TmpPriDate = "",
                TmpTodayDate = "",
                TmpMohlat = "",
                TmpTavizDate = "",
                C90 = 0,
                C101 = 0,
                KhaliS = customerInfo.MembersInfo.EmptyUnit,
                EdarehK = customerInfo.MembersInfo.IsSpecial,
                Tafa402 = 0,
                Avarez = (decimal)abBahaCalc.AvarezAmount,
                TrackNumber = 0
            };
        }
        private decimal GetSewageConsumption(int usageId, double consumption) => IsDomestic(usageId) ? (decimal)(consumption * _domesticMaltiplier) : (decimal)consumption;
        private (double, double, double) GetAmounts(double preDebt, double sumItems)
        {
            double jam = preDebt + sumItems;
            if (jam > _conditionPayableAmount)
            {
                long divideJam = (long)(jam / 1000);
                double payable = divideJam * 1000;
                double remained = sumItems - payable;
                return (sumItems, jam, payable);
            }
            else
            {
                return (sumItems, jam, 0);
            }
        }
        private KasrHaDto GerKasrHa(CustomerInfoGetDto customerInfo, AbBahaCalculationDetails abBahaCalc, GenerateBillInputDto generateBillInfo)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            string paymentId = string.Empty;//TransactionIdGenerator.GeneratePaymentId((long)abBahaCalc.SumItems, abBahaCalc.Customer.BillId);

            return new KasrHaDto()
            {
                Town = customerInfo.MembersInfo.ZoneId,
                IdBedbes = 0,
                Radif = customerInfo.MembersInfo.CustomerNumber,
                CodEnshab = customerInfo.MembersInfo.UsageId,
                Barge = 0,
                PriDate = customerInfo.BedBesInfo.LastMeterDateJalali,
                TodayDate = DateTime.Now.ToShortPersianDateString(),
                PriNo = (decimal)customerInfo.BedBesInfo.LastMeterNumber,
                TodayNo = generateBillInfo.MeterNumber,
                Masraf = (decimal)abBahaCalc.Consumption,
                AbBaha = (decimal)abBahaCalc.AbBahaDiscount,
                FasBaha = (decimal)abBahaCalc.FazelabDiscount,
                AbonAb = (decimal)abBahaCalc.AbonmanAbDiscount,
                AbonFas = (decimal)abBahaCalc.AbonmanFazelabDiscount,
                TabAbnA = 0,
                TabAbnF = 0,
                Ab10 = 0,
                Shahrdari = (decimal)abBahaCalc.MaliatDiscount,
                Rate = (decimal)abBahaCalc.MonthlyConsumption,
                Baha = (decimal)abBahaCalc.SumItems,
                ShGhabs = customerInfo.MembersInfo.BillId,
                ShPard = paymentId,
                DateBed = currentDateJalali,
                TmpDateBed = "",
                TmpTodayDate = "",
                TedVahd = customerInfo.MembersInfo.OtherUnit,
                TedKhane = customerInfo.MembersInfo.HouseholdNumber,
                TedadMas = customerInfo.MembersInfo.DomesticUnit,
                TedadTej = customerInfo.MembersInfo.CommercialUnit,
                ZaribFasl = 0,
                NoeVa = abBahaCalc.Customer.BranchType,
                Bodjeh = (decimal)abBahaCalc.BoodjeDiscount,
            };
        }
        private async Task Validation(GenerateBillInputDto inputDto, ZoneIdAndCustomerNumber zoneIdAndCustomerNumber, CustomerInfoGetDto customerInfo)
        {
            await DeletionStateValidation(zoneIdAndCustomerNumber);
            CounterStateValidation(inputDto.CounterStateCode, inputDto.MeterNumber, customerInfo.BedBesInfo.LastMeterNumber);
        }
        private async Task InputValidation(GenerateBillInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
            if (!string.IsNullOrWhiteSpace(inputDto.CurrentDateJalali))
            {
                DateOnly? currentDate = inputDto.CurrentDateJalali.ToGregorianDateOnly();
                if (!currentDate.HasValue)
                {
                    var message = string.Join("تاریخ ناصحیح");
                    throw new BaseException(message);
                }
            }
        }
        private async Task DeletionStateValidation(ZoneIdAndCustomerNumber input)
        {
            MemberInfoGetDto customerInfo = await _commonMemberQueryService.Get(input);
            if (customerInfo.DeletionStateId == _temporaryDeletionStateId)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidTemporaryDeletionState);
            }
        }
        private void CounterStateValidation(int? counterStateCode, int currentNumber, int? previousNumber)
        {
            if ((counterStateCode is null || !IsChangedOrReverse(counterStateCode)) &&
                (previousNumber.HasValue) &&
                !IsAllowedZeroMeterNumber(counterStateCode) &&
                (currentNumber < previousNumber))
            {
                throw new TariffCalcException(ExceptionLiterals.CurrentNumberLessThanPreviousNumber);
            }

        }
        private bool IsChangedOrReverse(int? counterStateCode) => counterStateCode == _changeCodeCounterState || counterStateCode == _reverseCodeCounterSatate;
        private bool IsDomestic(int usageId) => _domesticUsage.Contains(usageId);
        private bool IsAllowedZeroMeterNumber(int? counterStateCode) => _allowedZeroMeterNumberCounterState.Contains(counterStateCode ?? 0);
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
        private int GetTypeId(int? counterStateCode)
        {
            if (_allowedZeroMeterNumberCounterState.Contains(counterStateCode ?? 0))
            {
                return 8;
            }
            return 1;//todo: set other 
        }
    }
}
