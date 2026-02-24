using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
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
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Implementations
{
    internal sealed class GenerateBillHandler : AbstractBaseConnection, IGenerateBillHandler
    {
        private readonly ICustomerInfoService _customerInfoService;
        private readonly IOldTariffEngine _tariffEngine;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly IValidator<GenerateBillInputDto> _validator;
        private readonly IVariabService _variabService;
        const int _paymentDeadline = 7;
        const int _conditionPayableAmount = 10000;

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
            await Validation(inputDto, cancellationToken);
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await GetZoneIdANdCustomerNumber(inputDto.BillId);

            CustomerInfoGetDto customerInfo = await _customerInfoService.Get(zoneIdAndCustomerNumber.ZoneId, zoneIdAndCustomerNumber.CustomerNumber);
            CounterStateValidation(inputDto.CounterStateCode, inputDto.MeterNumber, customerInfo.BedBesInfo.LastMeterNumber);

            MeterInfoByPreviousDataInputDto tariffMeterInfoByPreviousData = GetMeterInfoByPreviousData(customerInfo, inputDto);
            AbBahaCalculationDetails abBahaCalcResult = await _tariffEngine.Handle(tariffMeterInfoByPreviousData, cancellationToken);
            if (!inputDto.IsConfirm)
            {
                return abBahaCalcResult;
            }
            BedBesCreateDto bedBes = await GetBedBes(customerInfo, abBahaCalcResult, inputDto, zoneIdAndCustomerNumber);
            KasrHaDto kasrHa = GerKasrHa(customerInfo, abBahaCalcResult, inputDto);
            ContorUpdateDto contorUpdate = GetControUpdateDto(customerInfo, bedBes);
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

                        int bedBesRecordId = await bedBedCommandService.Insert(bedBes, zoneIdAndCustomerNumber.ZoneId);
                        if (abBahaCalcResult.DiscountSum > 0)
                        {
                            await kasrHasCommandService.Insert(kasrHa, zoneIdAndCustomerNumber.ZoneId);
                        }
                        await membersCommandService.UpdateBedbes(zoneIdAndCustomerNumber, (long)bedBes.Baha, dbName);
                        await waterDebtCommandService.UpdateAmount(bedBes.ShGhabs1, (long)bedBes.Baha);
                        await controCommandService.Update(contorUpdate, dbName, true);                                                                                                   //update contro
                        await billCommandService.InsertByBedBesId(zoneIdAndCustomerNumber, bedBesRecordId, dbName);

                        transaction.Commit();
                    }
                    catch (Exception es)
                    {
                        transaction.Rollback();
                        throw es;
                    }
                }
            }
            return abBahaCalcResult;
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
            return new MeterInfoByPreviousDataInputDto()
            {
                BillId = customerInfo.MembersInfo.BillId,
                PreviousDateJalali = customerInfo.BedBesInfo?.LastMeterDateJalali ?? customerInfo.MembersInfo.WaterInstallationDateJalali,
                PreviousNumber = customerInfo.BedBesInfo?.LastMeterNumber ?? 0,
                CurrentDateJalali = DateTime.Now.ToShortPersianDateString(),
                CurrentMeterNumber = generateBillInfo.MeterNumber,
                CounterStateCode = generateBillInfo.CounterStateCode
            };
        }
        private async Task<BedBesCreateDto> GetBedBes(CustomerInfoGetDto customerInfo, AbBahaCalculationDetails abBahaCalc, GenerateBillInputDto generateBillInfo, ZoneIdAndCustomerNumber zoneIdAndCustomerNumber)
        {
            double preDebtAmount = await _customerInfoService.GetMembersBedBes(zoneIdAndCustomerNumber);//checkResult: changeDto
            var (sumItems, jam, pard) = GetAmounts(preDebtAmount, abBahaCalc.SumItems);
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            string mohlatDateJalali = DateTime.Now.AddDays(_paymentDeadline).ToShortPersianDateString();
            string paymentId = TransactionIdGenerator.GeneratePaymentId((long)pard, abBahaCalc.Customer.BillId);
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
                CodVas = 0,
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
                Sabt = 1,
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
                MasFas = (decimal)abBahaCalc.Consumption,
                Faz = false,
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
            string paymentId = TransactionIdGenerator.GeneratePaymentId((long)abBahaCalc.SumItems, abBahaCalc.Customer.BillId);

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
        private async Task Validation(GenerateBillInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private void CounterStateValidation(int? counterStateCode, int currentNumber, int? previousNumber)
        {
            if ((counterStateCode is null || !IsChangedOrReverse(counterStateCode)) &&
                (previousNumber.HasValue) &&
                (currentNumber < previousNumber))
            {
                throw new TariffCalcException(ExceptionLiterals.CurrentNumberLessThanPreviousNumber);
            }

        }
        private bool IsChangedOrReverse(int? counterStateCode)
        {
            int changeCode = 3;
            int reverseCode = 5;
            return counterStateCode == changeCode || counterStateCode == reverseCode;
        }
    }
}
