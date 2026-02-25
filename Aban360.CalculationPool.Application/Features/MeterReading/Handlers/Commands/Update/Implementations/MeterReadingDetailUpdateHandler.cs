using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Implementations;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Update.Implementations
{
    internal sealed class MeterReadingDetailUpdateHandler : AbstractBaseConnection, IMeterReadingDetailUpdateHandler
    {
        const int _conditionPayableAmount = 10000;
        const int _paymentDeadline = 7;

        private readonly IMeterReadingDetailQueryService _meterReadingDetailService;
        private readonly ICustomerInfoService _customerInfoService;
        private readonly IOldTariffEngine _oldTariffEngine;
        private readonly IValidator<MeterReadingDetailUpdateDto> _validator;
        public MeterReadingDetailUpdateHandler(
             IMeterReadingDetailQueryService meterReadingDetailService,
             ICustomerInfoService customerInfoService,
             IOldTariffEngine oldTariffEngine,
             IValidator<MeterReadingDetailUpdateDto> validator,
             IConfiguration configuration)
            : base(configuration)
        {
            _meterReadingDetailService = meterReadingDetailService;
            _meterReadingDetailService.NotNull(nameof(meterReadingDetailService));

            _customerInfoService = customerInfoService;
            _customerInfoService.NotNull(nameof(customerInfoService));

            _oldTariffEngine = oldTariffEngine;
            _oldTariffEngine.NotNull(nameof(oldTariffEngine));

            _validator = validator;
            _validator.NotNull(nameof(_validator));
        }

        public async Task Handle(MeterReadingDetailUpdateDto input, IAppUser appUser, CancellationToken cancellationToken)
        {
            await Validate(input, cancellationToken);
            MeterReadingDetailDataOutputDto previousMeterDetailDto = await _meterReadingDetailService.GetById(input.Id);
            AbBahaCalculationDetails abBahaResult = await CalcAbBahaTariff(input, previousMeterDetailDto, cancellationToken);
            //MeterReadingDetailCreateDuplicateDto readingCreateDuplicate = new(input.Id, input.CurrentCounterStateCode, input.CurrentDateJalali, input.CurrentNumber, appUser.UserId, DateTime.Now, abBahaResult.SumItems, abBahaResult.SumItemsBeforeDiscount, abBahaResult.DiscountSum, abBahaResult.Consumption, abBahaResult.MonthlyConsumption);
            MeterReadingDetailCreateDto meterReadingCreateDto = await GetMeterReadingDetailCreateDto(abBahaResult, input, previousMeterDetailDto, appUser);
            MeterReadingDetailDeleteDto readingDelete = new(input.Id, appUser.UserId, DateTime.Now);

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    MeterReadingDetailCommandService meterReadingDetailCommandService = new(connection, transaction);
                    //await meterReadingDetailCommandService.CreateDuplicateForLog(readingCreateDuplicate);
                    await meterReadingDetailCommandService.Insert(meterReadingCreateDto);

                    //remove previous
                    await meterReadingDetailCommandService.Delete(readingDelete);
                    transaction.Commit();
                }
            }

        }
        private async Task<MeterReadingDetailCreateDto> GetMeterReadingDetailCreateDto(AbBahaCalculationDetails abBahaCalc, MeterReadingDetailUpdateDto input, MeterReadingDetailDataOutputDto previousMeterDetailDto, IAppUser appUser)
        {
            MeterReadingDetailCreateDto meterDetailCreateDto = new MeterReadingDetailCreateDto();

            // MeterReadingDetailDataOutputDto previousMeterDetailDto = await _meterReadingDetailService.GetById(input.Id);
            CustomerInfoGetDto customerInfo = await _customerInfoService.Get(previousMeterDetailDto.ZoneId, previousMeterDetailDto.CustomerNumber);
            var (sumItems, jam, pard) = GetAmounts(customerInfo.MembersInfo.LatestDebtAmount, abBahaCalc?.SumItems ?? 0);
            string mohlatDateJalali = DateTime.Now.AddDays(_paymentDeadline).ToShortPersianDateString();


            meterDetailCreateDto.FlowImportedId = previousMeterDetailDto.FlowImportedId;
            meterDetailCreateDto.ZoneId = previousMeterDetailDto.ZoneId;
            meterDetailCreateDto.CustomerNumber = previousMeterDetailDto.CustomerNumber;
            meterDetailCreateDto.ReadingNumber = previousMeterDetailDto.ReadingNumber;
            meterDetailCreateDto.BillId = previousMeterDetailDto.BillId;
            meterDetailCreateDto.AgentCode = previousMeterDetailDto.AgentCode;
            meterDetailCreateDto.CurrentCounterStateCode = input.CurrentCounterStateCode ?? previousMeterDetailDto.CurrentCounterStateCode;
            meterDetailCreateDto.PreviousDateJalali = previousMeterDetailDto.PreviousDateJalali;
            meterDetailCreateDto.CurrentDateJalali = input.CurrentDateJalali ?? previousMeterDetailDto.PreviousDateJalali;
            meterDetailCreateDto.PreviousNumber = previousMeterDetailDto.PreviousNumber;
            meterDetailCreateDto.CurrentNumber = input.CurrentNumber ?? previousMeterDetailDto.CurrentNumber;
            meterDetailCreateDto.ExcludedByUserId = null;
            meterDetailCreateDto.ExcludedDateTime = null;
            meterDetailCreateDto.InsertByUserId = appUser.UserId;
            meterDetailCreateDto.InsertDateTime = DateTime.Now;
            meterDetailCreateDto.RemovedByUserId = null;
            meterDetailCreateDto.RemovedDateTime = null;
            meterDetailCreateDto.BranchTypeId = previousMeterDetailDto.BranchTypeId;
            meterDetailCreateDto.UsageId = previousMeterDetailDto.UsageId;
            meterDetailCreateDto.ConsumptionUsageId = previousMeterDetailDto.ConsumptionUsageId;
            meterDetailCreateDto.DomesticUnit = previousMeterDetailDto.DomesticUnit;
            meterDetailCreateDto.CommercialUnit = previousMeterDetailDto.CommercialUnit;
            meterDetailCreateDto.OtherUnit = previousMeterDetailDto.OtherUnit;
            meterDetailCreateDto.EmptyUnit = previousMeterDetailDto.EmptyUnit;
            meterDetailCreateDto.WaterInstallationDateJalali = previousMeterDetailDto.WaterInstallationDateJalali;
            meterDetailCreateDto.SewageInstallationDateJalali = previousMeterDetailDto.SewageInstallationDateJalali;
            meterDetailCreateDto.WaterRegisterDate = previousMeterDetailDto.WaterRegisterDate;
            meterDetailCreateDto.SewageRegisterDate = previousMeterDetailDto.SewageRegisterDate;
            meterDetailCreateDto.WaterCount = previousMeterDetailDto.WaterCount;
            meterDetailCreateDto.SewageCalcState = previousMeterDetailDto.SewageCalcState;
            meterDetailCreateDto.ContractualCapacity = previousMeterDetailDto.ContractualCapacity;
            meterDetailCreateDto.HouseholdDate = previousMeterDetailDto.HouseholdDate;
            meterDetailCreateDto.HouseholdNumber = previousMeterDetailDto.HouseholdNumber;
            meterDetailCreateDto.VillageId = previousMeterDetailDto.VillageId;
            meterDetailCreateDto.IsSpecial = previousMeterDetailDto.IsSpecial;
            meterDetailCreateDto.MeterDiameterId = previousMeterDetailDto.MeterDiameterId;
            meterDetailCreateDto.VirtualCategoryId = previousMeterDetailDto.VirtualCategoryId;
            meterDetailCreateDto.BodySerial = previousMeterDetailDto.BodySerial;
            meterDetailCreateDto.TavizDateJalali = customerInfo?.TavizInfo?.TavizDateJalali ?? null;
            meterDetailCreateDto.TavizNumber = customerInfo?.TavizInfo?.TavizNumber ?? null;
            meterDetailCreateDto.TavizCause = customerInfo?.TavizInfo?.TavizCause ?? null;
            meterDetailCreateDto.TavizRegisterDateJalali = customerInfo?.TavizInfo?.TavizRegisterDateJalali ?? null;
            meterDetailCreateDto.LastMeterDateJalali = previousMeterDetailDto.LastMeterDateJalali;
            meterDetailCreateDto.LastMeterNumber = previousMeterDetailDto.LastMeterNumber ?? 0;
            meterDetailCreateDto.LastConsumption = previousMeterDetailDto.LastConsumption ?? 0;
            meterDetailCreateDto.LastMonthlyConsumption = previousMeterDetailDto.LastMonthlyConsumption ?? 0;
            meterDetailCreateDto.LastCounterStateCode = previousMeterDetailDto.LastCounterStateCode ?? 0;
            meterDetailCreateDto.LastSumItems = previousMeterDetailDto.LastSumItems ?? 0;
            meterDetailCreateDto.SumItems = sumItems;//abBahaCalc.sumItems?
            meterDetailCreateDto.SumItemsBeforeDiscount = abBahaCalc.SumItemsBeforeDiscount;
            meterDetailCreateDto.DiscountSum = abBahaCalc.DiscountSum;
            meterDetailCreateDto.Consumption = abBahaCalc.Consumption;
            meterDetailCreateDto.MonthlyConsumption = abBahaCalc.MonthlyConsumption;

            meterDetailCreateDto.Barge = 0;
            meterDetailCreateDto.PriNo = meterDetailCreateDto.PreviousNumber;
            meterDetailCreateDto.TodayNo = meterDetailCreateDto.CurrentNumber;
            meterDetailCreateDto.PriDate = meterDetailCreateDto.PreviousDateJalali;
            meterDetailCreateDto.TodayDate = meterDetailCreateDto.CurrentDateJalali;
            meterDetailCreateDto.AbonAb = (decimal)(abBahaCalc?.AbonmanAbAmount ?? 0);
            meterDetailCreateDto.AbonFas = (decimal)(abBahaCalc?.AbonmanFazelabAmount ?? 0);
            meterDetailCreateDto.FasBaha = (decimal)(abBahaCalc?.FazelabAmount ?? 0);
            meterDetailCreateDto.AbBaha = (decimal)(abBahaCalc?.AbBahaAmount ?? 0);
            meterDetailCreateDto.Ztadil = 0;//todo
            meterDetailCreateDto.Masraf = (decimal)(abBahaCalc?.Consumption ?? 0);
            meterDetailCreateDto.Shahrdari = (decimal)(abBahaCalc?.MaliatAmount ?? 0);
            meterDetailCreateDto.Modat = abBahaCalc?.Duration ?? 0;
            meterDetailCreateDto.DateBed = DateTime.Now.ToShortPersianDateString();
            meterDetailCreateDto.JalaseNo = 0;//todo
            meterDetailCreateDto.Mohlat = mohlatDateJalali;
            meterDetailCreateDto.Baha = (decimal)sumItems;
            meterDetailCreateDto.Pard = (decimal)pard;
            meterDetailCreateDto.Jam = (decimal)jam;
            meterDetailCreateDto.CodVas = meterDetailCreateDto.CurrentCounterStateCode;
            meterDetailCreateDto.Ghabs = "1";
            meterDetailCreateDto.Del = false;
            meterDetailCreateDto.Type = "1";
            meterDetailCreateDto.CodEnshab = meterDetailCreateDto.UsageId;
            meterDetailCreateDto.Enshab = meterDetailCreateDto.MeterDiameterId;
            meterDetailCreateDto.Elat = 0;
            meterDetailCreateDto.Serial = 0;// string.IsNullOrWhiteSpace(meterReaing.BodySerial) ? 0 : int.Parse(meterReaing.BodySerial);//todo
            meterDetailCreateDto.Ser = 0;// string.IsNullOrWhiteSpace(meterReaing.BodySerial) ? 0 : int.Parse(meterReaing.BodySerial);//todo
            meterDetailCreateDto.ZaribFasl = (decimal)(abBahaCalc?.HotSeasonAbBahaAmount ?? 0);
            meterDetailCreateDto.Ab10 = 0;
            meterDetailCreateDto.Ab20 = 0;
            meterDetailCreateDto.TedadVahd = meterDetailCreateDto.OtherUnit;
            meterDetailCreateDto.TedKhane = meterDetailCreateDto.HouseholdNumber;
            meterDetailCreateDto.TedadMas = meterDetailCreateDto.DomesticUnit;
            meterDetailCreateDto.TedadTej = meterDetailCreateDto.CommercialUnit;
            meterDetailCreateDto.NoeVa = meterDetailCreateDto.BranchTypeId;
            meterDetailCreateDto.Jarime = 0;
            meterDetailCreateDto.Masjar = 0;
            meterDetailCreateDto.Sabt = 0;
            meterDetailCreateDto.Rate = (decimal)(abBahaCalc?.MonthlyConsumption ?? 0);
            meterDetailCreateDto.Operator = 0;//todo
            meterDetailCreateDto.Mamor = 0;//todo
            meterDetailCreateDto.TavizDate = "";//todo
            meterDetailCreateDto.ZaribCntr = 0;
            meterDetailCreateDto.Zabresani = 0;
            meterDetailCreateDto.ZaribD = 0;
            meterDetailCreateDto.Tafavot = 0;
            meterDetailCreateDto.KasrHa = (decimal)(abBahaCalc?.DiscountSum ?? 0);
            meterDetailCreateDto.FixMas = meterDetailCreateDto.ContractualCapacity;
            meterDetailCreateDto.ShGhabs1 = meterDetailCreateDto.BillId;
            meterDetailCreateDto.ShPard1 = "";//todo
            meterDetailCreateDto.TabAbnA = 0;
            meterDetailCreateDto.TabAbnF = 0;
            meterDetailCreateDto.TabsFa = 0;
            meterDetailCreateDto.NewAb = 0;
            meterDetailCreateDto.NewFa = 0;
            meterDetailCreateDto.Bodjeh = (decimal)(abBahaCalc?.SumBoodje ?? 0);
            meterDetailCreateDto.Group1 = meterDetailCreateDto.ConsumptionUsageId;
            meterDetailCreateDto.MasFas = 0;
            meterDetailCreateDto.Faz = (abBahaCalc?.FazelabAmount ?? 0) > 0;
            meterDetailCreateDto.ChkKarbari = 0;
            meterDetailCreateDto.C200 = 0;
            meterDetailCreateDto.AbSevom = 0;
            meterDetailCreateDto.AbSevom1 = 0;
            meterDetailCreateDto.C70 = 0;
            meterDetailCreateDto.C80 = 0;
            meterDetailCreateDto.C90 = 0;
            meterDetailCreateDto.C101 = 0;
            meterDetailCreateDto.KhaliS = meterDetailCreateDto.EmptyUnit;
            meterDetailCreateDto.EdarehK = meterDetailCreateDto.IsSpecial;
            meterDetailCreateDto.Avarez = (decimal)(abBahaCalc?.AvarezAmount ?? 0);

            //KasrHa Props
            meterDetailCreateDto.AbBahaDiscount = abBahaCalc?.AbBahaDiscount ?? 0;
            meterDetailCreateDto.HotSeasonDiscount= abBahaCalc?.HotSeasonDiscount ?? 0;
            meterDetailCreateDto.HotSeasonFazelabDiscount = abBahaCalc?.AbonmanFazelabDiscount ?? 0;
            meterDetailCreateDto.FazelabDiscount=abBahaCalc?.FazelabDiscount ?? 0;
            meterDetailCreateDto.AbonmanFazelabDiscount = abBahaCalc?.AbonmanFazelabDiscount ?? 0;
            meterDetailCreateDto.AbonmanAbDiscount=abBahaCalc?.AbonmanAbDiscount ?? 0;
            meterDetailCreateDto.AvarezDiscount=abBahaCalc?.AvarezDiscount ?? 0;
            meterDetailCreateDto.JavaniDiscount=abBahaCalc?.JavaniDiscount ?? 0;
            meterDetailCreateDto.BoodjeDiscount=abBahaCalc?.BoodjeDiscount ?? 0;
            meterDetailCreateDto.MaliatDiscount=abBahaCalc?.MaliatDiscount ?? 0;

            return meterDetailCreateDto;

        }
        private async Task Validate(MeterReadingDetailUpdateDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private async Task<AbBahaCalculationDetails> CalcAbBahaTariff(MeterReadingDetailUpdateDto meterReadingDetailUpdate, MeterReadingDetailDataOutputDto previousMeterDetailDto, CancellationToken cancellationToken)
        {
            MeterReadingDetailDataOutputDto meterReadingDetail = await _meterReadingDetailService.GetById(meterReadingDetailUpdate.Id);
            if (meterReadingDetail.CurrentCounterStateCode == 1 && meterReadingDetail.MonthlyConsumption.Value > 0 &&
                meterReadingDetailUpdate.CurrentCounterStateCode == 1 && meterReadingDetailUpdate.MonthlyAverage.HasValue)
            {
                MeterDateInfoWithMonthlyConsumptionOutputDto meterInfo = new MeterDateInfoWithMonthlyConsumptionOutputDto()
                {
                    BillId = meterReadingDetail.BillId,
                    CurrentDateJalali = meterReadingDetail.CurrentDateJalali,
                    MonthlyAverageConsumption = meterReadingDetailUpdate.MonthlyAverage.Value,
                    PreviousDateJalali = meterReadingDetail.PreviousDateJalali,
                };
                AbBahaCalculationDetails abBahaCalc = await _oldTariffEngine.Handle(meterInfo, cancellationToken);
                return abBahaCalc;
            }

            MeterImaginaryInputDto meterImaginary = GetMeterImaginary(meterReadingDetail, meterReadingDetailUpdate, previousMeterDetailDto);
            AbBahaCalculationDetails abBaha = await _oldTariffEngine.Handle(meterImaginary, cancellationToken);

            return abBaha;
        }
        private MeterImaginaryInputDto GetMeterImaginary(MeterReadingDetailDataOutputDto readingDetail, MeterReadingDetailUpdateDto meterReadingDetailUpdate, MeterReadingDetailDataOutputDto previousMeterDetailDto)
        {
            CustomerDetailInfoInputDto customerInfo = new()
            {
                ZoneId = readingDetail.ZoneId,
                Radif = readingDetail.CustomerNumber,
                BranchType = readingDetail.BranchTypeId,
                UsageId = readingDetail.UsageId,
                DomesticUnit = readingDetail.DomesticUnit,
                CommertialUnit = readingDetail.CommercialUnit,
                OtherUnit = readingDetail.OtherUnit,
                EmptyUnit = readingDetail.EmptyUnit,
                WaterInstallationDateJalali = readingDetail.WaterInstallationDateJalali,
                SewageInstallationDateJalali = readingDetail.SewageInstallationDateJalali,
                WaterRegisterDate = readingDetail.WaterRegisterDate,
                SewageRegisterDate = readingDetail.SewageRegisterDate,
                SewageCalcState = readingDetail.SewageCalcState,
                ContractualCapacity = readingDetail.ContractualCapacity,
                HouseholdDate = readingDetail.HouseholdDate,
                HouseholdNumber = readingDetail.HouseholdNumber,
                ReadingNumber = readingDetail.ReadingNumber,
                VillageId = readingDetail.VillageId,
                IsSpecial = readingDetail.IsSpecial,
                VirtualCategoryId = readingDetail.VirtualCategoryId,
                CounterStateCode = meterReadingDetailUpdate.CurrentCounterStateCode,
            };
            MeterInfoByPreviousDataInputDto meterInfo = new()
            {
                BillId = readingDetail.BillId,
                PreviousDateJalali = readingDetail.PreviousDateJalali,
                PreviousNumber = readingDetail.PreviousNumber,
                CurrentDateJalali = meterReadingDetailUpdate.CurrentDateJalali ?? previousMeterDetailDto.CurrentDateJalali,
                CurrentMeterNumber = meterReadingDetailUpdate.CurrentNumber ?? previousMeterDetailDto.CurrentNumber,
                CounterStateCode = meterReadingDetailUpdate.CurrentCounterStateCode
            };
            return new MeterImaginaryInputDto()
            {
                CustomerInfo = customerInfo,
                MeterPreviousData = meterInfo,
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
    }
}