using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Commands.Implementations;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Diagnostics;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Update.Implementations
{
    internal sealed class MeterReadingDetailUpdateHandler : AbstractBaseConnection, IMeterReadingDetailUpdateHandler
    {
        private readonly IMeterFlowQueryService _meterFlowQueryService;
        private readonly IMeterReadingDetailQueryService _meterReadingDetailService;
        private readonly ICustomerInfoService _customerInfoService;
        private readonly IMeterFlowQueryService _meterFlowService;
        private readonly IOldTariffEngine _oldTariffEngine;
        private readonly IMeterReadingValidateHandler _meterReadingValidateHandler;
        private readonly IValidator<MeterReadingDetailUpdateDto> _validator;
        static MeterFlowStepEnum[] _allowedUpdateFileStep = { MeterFlowStepEnum.Imported, MeterFlowStepEnum.Calculated, MeterFlowStepEnum.ConsumptionChecked };
        const double _maxAmount = 999_999_999_999;
        const int _conditionConsumption = 99_999_999;
        const int _conditionPayableAmount = 10000;
        const int _paymentDeadline = 7;
        const int _malfunctionMeterStateId = 1;
        private int[] _domesticUnits = { 1, 3 };

        public MeterReadingDetailUpdateHandler(
            IMeterFlowQueryService meterFlowQueryService,
             IMeterReadingDetailQueryService meterReadingDetailService,
             ICustomerInfoService customerInfoService,
             IMeterFlowQueryService meterFlowService,
             IOldTariffEngine oldTariffEngine,
             IMeterReadingValidateHandler meterReadingValidateHandler,
             IValidator<MeterReadingDetailUpdateDto> validator,
             IConfiguration configuration)
            : base(configuration)
        {
            _meterFlowQueryService = meterFlowQueryService;
            _meterFlowQueryService.NotNull(nameof(meterFlowQueryService));

            _meterReadingDetailService = meterReadingDetailService;
            _meterReadingDetailService.NotNull(nameof(meterReadingDetailService));

            _customerInfoService = customerInfoService;
            _customerInfoService.NotNull(nameof(customerInfoService));

            _meterFlowService = meterFlowService;
            _meterFlowService.NotNull(nameof(meterFlowService));

            _oldTariffEngine = oldTariffEngine;
            _oldTariffEngine.NotNull(nameof(oldTariffEngine));

            _meterReadingValidateHandler = meterReadingValidateHandler;
            _meterReadingValidateHandler.NotNull(nameof(meterReadingValidateHandler));

            _validator = validator;
            _validator.NotNull(nameof(_validator));
        }

        public async Task<MeterReadingDetailCheckedDto> Handle(MeterReadingDetailUpdateDto input, IAppUser appUser, CancellationToken cancellationToken)
        {
            MeterReadingDetailDataOutputDto previousMeterDetailDto = await _meterReadingDetailService.GetById(input.Id);
            await Validate(input, previousMeterDetailDto, cancellationToken);

            CustomerInfoGetDto customerInfo = await _customerInfoService.Get(previousMeterDetailDto.ZoneId, previousMeterDetailDto.CustomerNumber);
            AbBahaCalculationDetails abBahaResult = await CalcAbBahaTariff(input, previousMeterDetailDto, customerInfo, cancellationToken);
            //MeterReadingDetailCreateDuplicateDto readingCreateDuplicate = new(input.Id, input.CurrentCounterStateCode, input.CurrentDateJalali, input.CurrentNumber, appUser.UserId, DateTime.Now, abBahaResult.SumItems, abBahaResult.SumItemsBeforeDiscount, abBahaResult.DiscountSum, abBahaResult.Consumption, abBahaResult.MonthlyConsumption);
            MeterReadingDetailCreateDto meterReadingCreateDto = await GetMeterReadingDetailCreateDto(abBahaResult, input, previousMeterDetailDto, customerInfo, appUser);
            MeterReadingDetailDeleteDto readingDeleteDto = new(input.Id, appUser.UserId, DateTime.Now, MeterReadingDetailRemovedType.EditRecord);
            if (abBahaResult.SumItems > _maxAmount)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidDisallowedAmount(previousMeterDetailDto.BillId, _maxAmount));
            }

            int meterReadingDetailUpdatedId = await ExecSql(meterReadingCreateDto, readingDeleteDto);
            MeterReadingDetailDataOutputDto meterReadingUpdatedInfo = await _meterReadingDetailService.GetById(meterReadingDetailUpdatedId);
            MeterFlowStepEnum latestFlowStep = (await _meterFlowService.GetLatestFlowInfo(meterReadingUpdatedInfo.FlowImportedId)).MeterFlowStepId;
            return GetResult(meterReadingUpdatedInfo, latestFlowStep);
        }
        private async Task<int> ExecSql(MeterReadingDetailCreateDto meterReadingCreateDto, MeterReadingDetailDeleteDto readingDeleteDto)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    MeterReadingDetailCommandService meterReadingDetailCommandService = new(connection, transaction);

                    int meterReadingDetailUpdatedId = await meterReadingDetailCommandService.Insert(meterReadingCreateDto);
                    await meterReadingDetailCommandService.Delete(readingDeleteDto);//remove previous

                    transaction.Commit();
                    return meterReadingDetailUpdatedId;
                }
            }
        }
        private MeterReadingDetailCheckedDto GetResult(MeterReadingDetailDataOutputDto updatedInfo, MeterFlowStepEnum latestFlowStep)
        {
            return new MeterReadingDetailCheckedDto()
            {
                Id = updatedInfo.Id,
                FlowImportedId = updatedInfo.FlowImportedId,
                ZoneId = updatedInfo.ZoneId,
                CustomerNumber = updatedInfo.CustomerNumber,
                ReadingNumber = updatedInfo.ReadingNumber,
                BillId = updatedInfo.BillId,
                AgentCode = updatedInfo.AgentCode,
                CurrentCounterStateCode = updatedInfo.CurrentCounterStateCode,
                PreviousDateJalali = updatedInfo.PreviousDateJalali,
                CurrentDateJalali = updatedInfo.CurrentDateJalali,
                PreviousNumber = updatedInfo.PreviousNumber,
                CurrentNumber = updatedInfo.CurrentNumber,
                InsertByUserId = updatedInfo.InsertByUserId,
                InsertDateTime = updatedInfo.InsertDateTime,
                BranchTypeId = updatedInfo.BranchTypeId,
                BranchTypeTitle = updatedInfo.BranchTypeTitle,
                UsageId = updatedInfo.UsageId,
                UsageTitle = updatedInfo.UsageTitle,
                ConsumptionUsageId = updatedInfo.ConsumptionUsageId,
                DomesticUnit = updatedInfo.DomesticUnit,
                CommercialUnit = updatedInfo.CommercialUnit,
                OtherUnit = updatedInfo.OtherUnit,
                TotalUnit = updatedInfo.DomesticUnit + updatedInfo.CommercialUnit + updatedInfo.OtherUnit,
                EmptyUnit = updatedInfo.EmptyUnit,
                WaterInstallationDateJalali = updatedInfo.WaterInstallationDateJalali,
                SewageInstallationDateJalali = updatedInfo.SewageInstallationDateJalali,
                WaterRegisterDate = updatedInfo.WaterRegisterDate,
                SewageRegisterDate = updatedInfo.SewageRegisterDate,
                WaterCount = updatedInfo.WaterCount,
                SewageCalcState = updatedInfo.SewageCalcState,
                ContractualCapacity = updatedInfo.ContractualCapacity,
                HouseholdNumber = updatedInfo.HouseholdNumber,
                HouseholdDate = updatedInfo.HouseholdDate,
                VillageId = updatedInfo.VillageId,
                IsSpecial = updatedInfo.IsSpecial,
                MeterDiameterId = updatedInfo.MeterDiameterId,
                VirtualCategoryId = updatedInfo.VirtualCategoryId,
                BodySerial = updatedInfo.BodySerial,
                Duration = updatedInfo.Modat ?? 0,
                TavizDateJalali = updatedInfo.TavizDateJalali,
                TavizCause = updatedInfo.TavizCause,
                TavizRegisterDateJalali = updatedInfo.TavizRegisterDateJalali,
                TavizNumber = updatedInfo.TavizNumber,
                PreviousMeterDateJalali = updatedInfo.PreviousMeterDateJalali,
                PreviousMeterNumber = updatedInfo.PreviousMeterNumber,
                PreviousConsumption = updatedInfo.PreviousConsumption,
                PreviousMonthlyConsumption = updatedInfo.PreviousMonthlyConsumption,
                PreviousCounterStateCode = updatedInfo.PreviousCounterStateCode,
                PreviousSumItems = updatedInfo.PreviousSumItems,

                SumItems = updatedInfo.SumItems,
                SumItemsBeforeDiscount = updatedInfo.SumItemsBeforeDiscount,
                DiscountSum = updatedInfo.DiscountSum,
                WaterDebt = updatedInfo.WaterDebt,
                BeforDebt = updatedInfo.BeforDebt,
                Consumption = updatedInfo.Consumption,
                MonthlyConsumption = updatedInfo.MonthlyConsumption,
                AttentionState = _meterReadingValidateHandler.GetAttentionState(updatedInfo, latestFlowStep),
                HasAttentionCounterState = _meterReadingValidateHandler.IsAttentionCounterState(updatedInfo.CurrentCounterStateCode),
            };
        }
        private async Task<MeterReadingDetailCreateDto> GetMeterReadingDetailCreateDto(AbBahaCalculationDetails abBahaCalc, MeterReadingDetailUpdateDto input, MeterReadingDetailDataOutputDto previousMeterDetailDto, CustomerInfoGetDto customerInfo, IAppUser appUser)
        {
            MeterReadingDetailCreateDto meterDetailCreateDto = new MeterReadingDetailCreateDto();

            // MeterReadingDetailDataOutputDto previousMeterDetailDto = await _meterReadingDetailService.GetById(input.Id);
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
            meterDetailCreateDto.LastMeterDateJalali = previousMeterDetailDto.PreviousMeterDateJalali;
            meterDetailCreateDto.LastMeterNumber = previousMeterDetailDto.PreviousMeterNumber ?? 0;
            meterDetailCreateDto.LastConsumption = previousMeterDetailDto.PreviousConsumption ?? 0;
            meterDetailCreateDto.LastMonthlyConsumption = previousMeterDetailDto.PreviousMonthlyConsumption ?? 0;
            meterDetailCreateDto.LastCounterStateCode = previousMeterDetailDto.PreviousCounterStateCode ?? 0;
            meterDetailCreateDto.LastSumItems = previousMeterDetailDto.PreviousSumItems ?? 0;
            meterDetailCreateDto.SumItems = sumItems;//abBahaCalc.sumItems?
            meterDetailCreateDto.SumItemsBeforeDiscount = abBahaCalc?.SumItemsBeforeDiscount ?? 0;
            meterDetailCreateDto.DiscountSum = abBahaCalc?.DiscountSum ?? 0;
            meterDetailCreateDto.Consumption = Math.Round((abBahaCalc?.Consumption ?? 0), 2);

            double monthlyConsumption = abBahaCalc?.MonthlyConsumption ?? 0;
            int totalUnit = abBahaCalc?.Customer?.UnitAll ?? 0;
            int finalTotalUnit = totalUnit == 0 ? 1 : totalUnit;
            meterDetailCreateDto.MonthlyConsumption = Math.Round(monthlyConsumption);
            meterDetailCreateDto.MonthlyPerUnit = Math.Round((monthlyConsumption / finalTotalUnit), 2);

            meterDetailCreateDto.Barge = 0;
            meterDetailCreateDto.PriNo = meterDetailCreateDto.PreviousNumber;
            meterDetailCreateDto.TodayNo = meterDetailCreateDto.CurrentNumber;
            meterDetailCreateDto.PriDate = meterDetailCreateDto.PreviousDateJalali;
            meterDetailCreateDto.TodayDate = meterDetailCreateDto.CurrentDateJalali;
            meterDetailCreateDto.AbonAb = (decimal)(abBahaCalc?.AbonmanAbAmount ?? 0);
            meterDetailCreateDto.AbonFas = (decimal)(abBahaCalc?.AbonmanFazelabAmount ?? 0);
            meterDetailCreateDto.FasBaha = ((decimal)(abBahaCalc?.FazelabAmount ?? 0)) + ((decimal)(abBahaCalc?.HotSeasonFazelabAmount ?? 0));
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
            meterDetailCreateDto.WaterDebt = customerInfo.MembersInfo.LatestDebtAmount;

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
            meterDetailCreateDto.ZaribD = (decimal)(abBahaCalc?.JavaniAmount ?? 0);
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
            meterDetailCreateDto.HotSeasonDiscount = abBahaCalc?.HotSeasonDiscount ?? 0;
            meterDetailCreateDto.HotSeasonFazelabDiscount = abBahaCalc?.AbonmanFazelabDiscount ?? 0;
            meterDetailCreateDto.FazelabDiscount = abBahaCalc?.FazelabDiscount ?? 0;
            meterDetailCreateDto.AbonmanFazelabDiscount = abBahaCalc?.AbonmanFazelabDiscount ?? 0;
            meterDetailCreateDto.AbonmanAbDiscount = abBahaCalc?.AbonmanAbDiscount ?? 0;
            meterDetailCreateDto.AvarezDiscount = abBahaCalc?.AvarezDiscount ?? 0;
            meterDetailCreateDto.JavaniDiscount = abBahaCalc?.JavaniDiscount ?? 0;
            meterDetailCreateDto.BoodjeDiscount = abBahaCalc?.BoodjeDiscount ?? 0;
            meterDetailCreateDto.MaliatDiscount = abBahaCalc?.MaliatDiscount ?? 0;

            return meterDetailCreateDto;

        }
        private async Task Validate(MeterReadingDetailUpdateDto input, MeterReadingDetailDataOutputDto previousMeterDetailDto, CancellationToken cancellationToken)
        {
            await InputValidate(input, cancellationToken);
            await RemovedValidate(previousMeterDetailDto);
            CounterStateValidate(input);
            DataValidate(previousMeterDetailDto);
        }
        private async Task InputValidate(MeterReadingDetailUpdateDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private async Task RemovedValidate(MeterReadingDetailDataOutputDto previousMeterDetailDto)
        {
            if (previousMeterDetailDto.RemovedByUserId is not null)
            {
                throw new ReadingException(ExceptionLiterals.InvalidUpdateMeterReading);
            }
            MeterFlowGetDto latestFlowInfo = await _meterFlowQueryService.GetLatestFlowInfo(previousMeterDetailDto.FlowImportedId);
            if (latestFlowInfo.RemovedDateTime is not null)
            {
                throw new ReadingException(ExceptionLiterals.InvalidUpdateMeterReadingRemoved);
            }
            if (!_allowedUpdateFileStep.Contains(latestFlowInfo.MeterFlowStepId))
            {
                throw new ReadingException(ExceptionLiterals.InvalidUpdateMeterReadingFinished);
            }
        }
        private void CounterStateValidate(MeterReadingDetailUpdateDto input)
        {
            if (input.CurrentCounterStateCode == _malfunctionMeterStateId && input.MonthlyAverage is null)
            {
                throw new ReadingException(ExceptionLiterals.InvalidMonthlyAverageWithMalfunctionState);
            }
        }
        private void DataValidate(MeterReadingDetailDataOutputDto previousMeterDetailDto)
        {
            if (!_domesticUnits.Contains(previousMeterDetailDto.UsageId) && (previousMeterDetailDto.ContractualCapacity <= 0))
            {
                throw new ReadingException(ExceptionLiterals.InvalidContractualCapacity);
            }
        }
        private async Task<AbBahaCalculationDetails> CalcAbBahaTariff(MeterReadingDetailUpdateDto meterReadingDetailUpdate, MeterReadingDetailDataOutputDto previousMeterDetailDto, CustomerInfoGetDto customerInfo, CancellationToken cancellationToken)
        {
            AbBahaCalculationDetails abBaha;

            MeterReadingDetailDataOutputDto meterReadingDetail = await _meterReadingDetailService.GetById(meterReadingDetailUpdate.Id);
            if (meterReadingDetailUpdate.CurrentCounterStateCode == (int)CounterStateCodeEnum.Malfunction && meterReadingDetailUpdate.MonthlyAverage.HasValue)
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
            else if (meterReadingDetailUpdate.CurrentCounterStateCode == (int)CounterStateCodeEnum.Change)
            {
                if (string.IsNullOrWhiteSpace(customerInfo.TavizInfo.TavizDateJalali) ||
                    customerInfo.TavizInfo.TavizDateJalali.CompareTo(meterReadingDetailUpdate.CurrentDateJalali) > 0 ||
                     customerInfo.TavizInfo.TavizDateJalali.CompareTo(customerInfo.BedBesInfo.LastMeterDateJalali) < 0)
                {
                    throw new ReadingException(ExceptionLiterals.InvalidCalculation);
                }
                else
                {
                    MeterImaginaryInputDto meterImaginary = GetMeterImaginary(meterReadingDetail, meterReadingDetailUpdate, previousMeterDetailDto, customerInfo.TavizInfo.TavizDateJalali, true);
                    AbBahaCalculationDetails abBahaCalcTmp = await _oldTariffEngine.Handle(meterImaginary, cancellationToken);
                    MeterDateInfoWithMonthlyConsumptionOutputDto meterInfo = new MeterDateInfoWithMonthlyConsumptionOutputDto()
                    {
                        BillId = customerInfo.MembersInfo.BillId,
                        CurrentDateJalali = meterReadingDetailUpdate.CurrentDateJalali,
                        MonthlyAverageConsumption = abBahaCalcTmp.MonthlyConsumption,
                        PreviousDateJalali = previousMeterDetailDto.PreviousDateJalali,
                    };

                    abBaha = await _oldTariffEngine.Handle(meterInfo, cancellationToken);
                    if (abBaha.SumItems > _maxAmount)
                    {
                        throw new InvalidBillCommandException(ExceptionLiterals.InvalidDisallowedAmount(customerInfo.MembersInfo.BillId, _maxAmount));
                    }
                }
            }
            else if (meterReadingDetailUpdate.CurrentCounterStateCode == (int)CounterStateCodeEnum.Close)
            {
                abBaha = GetAbBahaCalcWithZeroValues(meterReadingDetailUpdate, customerInfo);
            }
            else
            {
                MeterImaginaryInputDto meterImaginary = GetMeterImaginary(meterReadingDetail, meterReadingDetailUpdate, previousMeterDetailDto, null, false);
                abBaha = await _oldTariffEngine.Handle(meterImaginary, cancellationToken);
            }

            return abBaha;
        }
        private AbBahaCalculationDetails GetAbBahaCalcWithZeroValues(MeterReadingDetailUpdateDto inputDto, CustomerInfoGetDto customerInfo)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            inputDto.CurrentNumber = 0;
            string previousDate = customerInfo.BedBesInfo?.LastMeterDateJalali ?? customerInfo.MembersInfo.WaterInstallationDateJalali;
            int previousNumber = customerInfo.BedBesInfo?.LastMeterNumber ?? 0;
            int finalUnit = GetFinalDomesticUnit(customerInfo, inputDto.CurrentDateJalali);
            ConsumptionInfo consumptionInfo = new(previousDate, inputDto.CurrentDateJalali, 0, GetDuration(previousDate, inputDto.CurrentDateJalali), 0, finalUnit);
            MeterInfoOutputDto meterInfo = new(previousDate, inputDto.CurrentDateJalali, previousNumber, 0, inputDto.CurrentCounterStateCode);

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
                CounterStateCode = inputDto.CurrentCounterStateCode,
            };
            MeterInfoByPreviousDataInputDto previousMeterInfo = new()
            {
                BillId = customerInfo.MembersInfo.BillId,
                PreviousDateJalali = previousDate,
                PreviousNumber = previousNumber,
                CurrentDateJalali = inputDto.CurrentDateJalali,
                CurrentMeterNumber = inputDto.CurrentNumber ?? 0,
                CounterStateCode = inputDto.CurrentCounterStateCode,
            };
            MeterImaginaryInputDto meterImaginaryDto = new() { CustomerInfo = customerDetailInfo, MeterPreviousData = previousMeterInfo };
            CustomerInfoOutputDto customerInfoOutputDto = new(meterImaginaryDto);
            stopWatch.Stop();


            AbBahaCalculationDetails abBahaCalcResult = new AbBahaCalculationDetails(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            new List<NerkhGetDto>(), new List<AbAzadFormulaDto>(), new List<ZaribGetDto>(),
            consumptionInfo, meterInfo, customerInfoOutputDto, stopWatch.ElapsedMilliseconds, 0);

            return abBahaCalcResult;
        }
        private MeterImaginaryInputDto GetMeterImaginary(MeterReadingDetailDataOutputDto readingDetail, MeterReadingDetailUpdateDto meterReadingDetailUpdate, MeterReadingDetailDataOutputDto previousMeterDetailDto, string? meterChangeDateJalali, bool isChangeCounterState)
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
            MeterInfoByPreviousDataInputDto changeMeterInfo = new()
            {
                BillId = readingDetail.BillId,
                PreviousDateJalali = meterChangeDateJalali,
                PreviousNumber = 0,
                CurrentDateJalali = meterReadingDetailUpdate.CurrentDateJalali ?? previousMeterDetailDto.CurrentDateJalali,
                CurrentMeterNumber = meterReadingDetailUpdate.CurrentNumber ?? previousMeterDetailDto.CurrentNumber,
                CounterStateCode = meterReadingDetailUpdate.CurrentCounterStateCode
            };
            return new MeterImaginaryInputDto()
            {
                CustomerInfo = customerInfo,
                MeterPreviousData = isChangeCounterState ? changeMeterInfo : meterInfo,
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
            if (readingDateGregorian.Value < expireHouseHoldGregorian.Value)//تاریخ قرائت قبل از تاریخ ثبت خانوار
            {
                return 0;
            }
            if (expireHouseHoldGregorian.Value.AddYears(1) < readingDateGregorian.Value)// تاریخ قرائت بعد از تاریخ انقضای خانوار
            {
                return 0;
            }
            return householdUnit;
        }

    }
}