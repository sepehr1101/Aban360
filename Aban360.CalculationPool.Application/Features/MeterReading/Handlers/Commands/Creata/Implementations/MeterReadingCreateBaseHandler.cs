using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Implementations;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.Common.Timing;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Queries.Implementations;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.ReportPool.Domain.Base;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Implementations
{
    internal sealed class MeterReadingCreateBaseHandler : AbstractBaseConnection, IMeterReadingCreateBaseHandler
    {
        private readonly IMeterFlowQueryService _meterFlowService;
        private readonly ICustomerInfoService _customerInfoService;
        private readonly IMeterReadingDetailQueryService _meterReadingDetailService;
        private readonly IMeterFlowValidationGetHandler _meterFlowValidationGetHandler;
        private readonly IOldTariffEngine _tariffEngine;
        private readonly IValidator<MeterReadingFileCreateDto> _validator;
        private readonly IPreviousAverageHandler _previousAverageHandler;
        private readonly IBedBesQueryService _bedBesQueryService;
        const int _conditionPayableAmount = 10000;
        const int _paymentDeadline = 7;
        const double _maxAmount = 999_999_999_999;
        const int _commonMeterStateId = 0;
        const int _malfunctionMeterStateId = 1;
        const int _changeCounterStateId = 2;
        const int _reverseCounterState = 3;
        const int _closeMeterStateId = 4;
        const int _nextRoundCounterSatateId = 5;
        const int _withoutConsumptionMeterStateId = 6;
        const int _blockMeterStateId = 7;
        const int _noReadMeterStateId = 8;
        const int _desolateUnitMeterStateId = 9;//todo: rename
        const int _disconnectionMeterStateId = 10;
        public MeterReadingCreateBaseHandler(
            IMeterFlowQueryService meterFlowService,
            ICustomerInfoService customerInfoService,
            IMeterReadingDetailQueryService meterReadingDetailService,
            IOldTariffEngine tariffEngine,
            IMeterFlowValidationGetHandler meterFlowValidationGetHandler,
            IValidator<MeterReadingFileCreateDto> validator,
            IPreviousAverageHandler previousAverageHandler,
            IBedBesQueryService bedBesQueryService,
            IConfiguration configuration)
            : base(configuration)
        {
            _meterFlowService = meterFlowService;
            _meterFlowService.NotNull(nameof(_meterFlowService));

            _customerInfoService = customerInfoService;
            _customerInfoService.NotNull(nameof(_customerInfoService));

            _meterReadingDetailService = meterReadingDetailService;
            _meterReadingDetailService.NotNull(nameof(_meterReadingDetailService));

            _tariffEngine = tariffEngine;
            _tariffEngine.NotNull(nameof(tariffEngine));

            _meterFlowValidationGetHandler = meterFlowValidationGetHandler;
            _meterFlowValidationGetHandler.NotNull(nameof(meterFlowValidationGetHandler));

            _validator = validator;
            _validator.NotNull(nameof(_validator));

            _previousAverageHandler = previousAverageHandler;
            _previousAverageHandler.NotNull(nameof(_previousAverageHandler));

            _bedBesQueryService = bedBesQueryService;
            _bedBesQueryService.NotNull(nameof(bedBesQueryService));
        }

        public async Task<ICollection<MeterReadingDetailCreateDto>> GetReadingDetailCreateFinal(IEnumerable<MeterReadingDetailCreateDto> readingDetails, FileCreateDto fileInfo, IAppUser appUser, CancellationToken cancellationToken)
        {
            ICollection<MeterReadingDetailCreateDto> readingDetailsCreate = new List<MeterReadingDetailCreateDto>();
            foreach (var readingDetail in readingDetails)
            {
                var (isValid, hasExclude) = CounterStateValidation(readingDetail.CurrentCounterStateCode, readingDetail.CurrentNumber, readingDetail.PreviousNumber);
                if (isValid)
                {
                    if (readingDetail.CurrentCounterStateCode == _malfunctionMeterStateId)//xarab
                    {
                        float previousAverage = await _previousAverageHandler.HandleByPreviousYear(readingDetail.ZoneId, readingDetail.CustomerNumber, readingDetail.PreviousDateJalali, readingDetail.CurrentDateJalali) ??
                        await _previousAverageHandler.HandleByLatestReading(readingDetail.ZoneId, readingDetail.CustomerNumber, readingDetail.PreviousDateJalali, readingDetail.CurrentDateJalali);
                        MeterDateInfoWithMonthlyConsumptionOutputDto meterInfo = new MeterDateInfoWithMonthlyConsumptionOutputDto()
                        {
                            BillId = readingDetail.BillId,
                            CurrentDateJalali = readingDetail.CurrentDateJalali,
                            MonthlyAverageConsumption = previousAverage,
                            PreviousDateJalali = readingDetail.PreviousDateJalali,
                        };
                        try
                        {

                            AbBahaCalculationDetails abBahaCalc = await _tariffEngine.Handle(meterInfo, cancellationToken);
                            if (abBahaCalc.SumItems > _maxAmount)
                            {
                                throw new InvalidBillCommandException(ExceptionLiterals.InvalidDisallowedAmount(readingDetail.BillId, _maxAmount));
                            }
                            readingDetailsCreate.Add(await GetMeterReadingDetailByAbBahaValue(readingDetail, abBahaCalc, false, null));
                        }
                        catch (Exception ex) when (IsInException(ex))
                        {
                            readingDetailsCreate.Add(await GetMeterReadingDetailByAbBahaValue(readingDetail, null, true, appUser.UserId));
                        }
                    }
                    else if (readingDetail.CurrentCounterStateCode == _changeCounterStateId && string.IsNullOrWhiteSpace(readingDetail.TavizDateJalali))
                    {
                        readingDetailsCreate.Add(await GetMeterReadingDetailByAbBahaValue(readingDetail, null, true, appUser.UserId));
                    }
                    else if (readingDetail.CurrentCounterStateCode == _changeCounterStateId && readingDetail.TavizDateJalali.CompareTo(readingDetail.CurrentDateJalali) > 0)
                    {
                        readingDetailsCreate.Add(await GetMeterReadingDetailByAbBahaValue(readingDetail, null, true, appUser.UserId));
                    }
                    else if (readingDetail.CurrentCounterStateCode == _changeCounterStateId && readingDetail.TavizDateJalali.CompareTo(readingDetail.PreviousDateJalali) < 0)
                    {
                        readingDetailsCreate.Add(await GetMeterReadingDetailByAbBahaValue(readingDetail, null, true, appUser.UserId));
                    }
                    else if (readingDetail.CurrentCounterStateCode == _changeCounterStateId) //taviz
                    {
                        int previousNumber = readingDetail.PreviousNumber;
                        string previousDateJalali = readingDetail.PreviousDateJalali;

                        readingDetail.PreviousNumber = 0;
                        readingDetail.PreviousDateJalali = readingDetail.TavizDateJalali;//TODO: check has value
                        MeterImaginaryInputDto meterImaginaryTmp = GetMeterImaginary(readingDetail);
                        try
                        {

                            AbBahaCalculationDetails abBahaCalcTmp = await _tariffEngine.Handle(meterImaginaryTmp, cancellationToken);
                            readingDetail.PreviousNumber = previousNumber;
                            readingDetail.PreviousDateJalali = previousDateJalali;
                            MeterDateInfoWithMonthlyConsumptionOutputDto meterInfo = new MeterDateInfoWithMonthlyConsumptionOutputDto()
                            {
                                BillId = readingDetail.BillId,
                                CurrentDateJalali = readingDetail.CurrentDateJalali,
                                MonthlyAverageConsumption = abBahaCalcTmp.MonthlyConsumption,
                                PreviousDateJalali = readingDetail.PreviousDateJalali,
                            };


                            AbBahaCalculationDetails abBahaCalc = await _tariffEngine.Handle(meterInfo, cancellationToken);
                            if (abBahaCalc.SumItems > _maxAmount)
                            {
                                throw new InvalidBillCommandException(ExceptionLiterals.InvalidDisallowedAmount(readingDetail.BillId, _maxAmount));
                            }
                            readingDetailsCreate.Add(await GetMeterReadingDetailByAbBahaValue(readingDetail, abBahaCalc, false, null));
                        }
                        catch (Exception ex) when (IsInException(ex))
                        {
                            readingDetailsCreate.Add(await GetMeterReadingDetailByAbBahaValue(readingDetail, null, true, appUser.UserId));
                        }
                    }
                    else //not xarab, nor taviz
                    {
                        MeterImaginaryInputDto meterImaginary = GetMeterImaginary(readingDetail);
                        try
                        {
                            AbBahaCalculationDetails abBahaCalc = await _tariffEngine.Handle(meterImaginary, cancellationToken);
                            if (abBahaCalc.SumItems > _maxAmount)
                            {
                                throw new InvalidBillCommandException(ExceptionLiterals.InvalidDisallowedAmount(readingDetail.BillId, _maxAmount));
                            }
                            readingDetailsCreate.Add(await GetMeterReadingDetailByAbBahaValue(readingDetail, abBahaCalc, false, null));
                        }
                        catch (Exception ex) when (IsInException(ex))
                        {
                            readingDetailsCreate.Add(await GetMeterReadingDetailByAbBahaValue(readingDetail, null, true, appUser.UserId));
                        }
                    }
                }
                else
                {
                    Guid? excludedUserId = hasExclude ? appUser.UserId : null;
                    readingDetailsCreate.Add(await GetMeterReadingDetailByAbBahaValue(readingDetail, null, true, excludedUserId));
                }
            }

            return readingDetailsCreate;
        }
        public async Task<ICollection<MeterReadingDetailCreateDto>> GetReadingDetailCreateFinalNonRead(IEnumerable<MeterReadingDetailCreateDto> readingDetails, FileCreateDto fileInfo, IAppUser appUser, CancellationToken cancellationToken)
        {
            ICollection<MeterReadingDetailCreateDto> readingDetailsCreate = new List<MeterReadingDetailCreateDto>();
            foreach (var readingDetail in readingDetails)
            {
                try
                {
                    MeterDateInfoWithMonthlyConsumptionOutputDto meterInfo = new MeterDateInfoWithMonthlyConsumptionOutputDto()
                    {
                        BillId = readingDetail.BillId,
                        CurrentDateJalali = readingDetail.CurrentDateJalali,
                        MonthlyAverageConsumption = readingDetail.MonthlyConsumption ?? 0,
                        PreviousDateJalali = readingDetail.PreviousDateJalali,
                    };

                    AbBahaCalculationDetails abBahaCalc = await _tariffEngine.Handle(meterInfo, cancellationToken);
                    if (abBahaCalc.SumItems > _maxAmount)
                    {
                        throw new InvalidBillCommandException(ExceptionLiterals.InvalidDisallowedAmount(readingDetail.BillId, _maxAmount));
                    }
                    readingDetailsCreate.Add(await GetMeterReadingDetailByAbBahaValue(readingDetail, abBahaCalc, false, null));
                }
                catch (Exception ex) when (IsInException(ex))
                {
                    readingDetailsCreate.Add(await GetMeterReadingDetailByAbBahaValue(readingDetail, null, true, appUser.UserId));
                }
            }
            return readingDetailsCreate;
        }
        public async Task<(CustomersInfoGetDto, int)> GetCustomerInfoAndFirstFlowId(ICollection<MeterReadingFileDetail> meterReadings, string fileName, string filePath, string? description, Guid userId)
        {
            MeterReadingFileDetail firstMeterDetail = meterReadings.FirstOrDefault();
            string fromReadingNumber = meterReadings?.Min(m => m.ReadingNumber) ?? string.Empty;
            string toReadingNumber = meterReadings?.Max(m => m.ReadingNumber) ?? string.Empty;
            MeterFlowCreateDto importedMeterFlow = GetMeterFlowCreateDto(MeterFlowStepEnum.Imported, fileName, firstMeterDetail.ZoneId, fromReadingNumber, toReadingNumber, userId, description);
            IEnumerable<ZoneIdAndCustomerNumber> customersByInvalidPreviousBedBes = new List<ZoneIdAndCustomerNumber>();
            CustomersInfoGetDto customersInfo;
            int meterFlowId = 0;

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    MeterFlowCommandService meterflowCommandService = new(connection, transaction);

                    meterFlowId = await meterflowCommandService.Insert(importedMeterFlow);
                    customersInfo = await _customerInfoService.GetByBulkCopy(connection, transaction, firstMeterDetail.ZoneId, meterReadings.Select(m => m.CustomerNumber).ToList());
                    transaction.Commit();
                }
            }
            foreach (var item in customersInfo.BedBesInfo)
            {
                if (item.IsReturned)
                {
                    BedBesPreviousNumberAndDateOutputDto previousInfo = await _bedBesQueryService.GetPreviousDateAndNumber(new ZoneIdAndCustomerNumber(item.ZoneId, item.CustomerNumber), item.BillId);
                    item.LastMeterNumber = previousInfo.PreviousNumber;
                    item.LastMeterDateJalali = previousInfo.PreviousDateJalali;
                }
            }

            return (customersInfo, meterFlowId);
        }
        public IEnumerable<MeterReadingDetailCreateDto> GetReadingMeterDetails(ICollection<MeterReadingFileDetail> meterReadings, CustomersInfoGetDto customersInfo, int meterFlowId)
        {
            return from meterReading in meterReadings
                   join members in customersInfo.MembersInfo
                       on meterReading.CustomerNumber equals members.CustomerNumber
                       into membersJoin
                   from members in membersJoin.DefaultIfEmpty()
                   join bedbes in customersInfo.BedBesInfo
                       on members.CustomerNumber equals bedbes.CustomerNumber
                       into bedbesJoin
                   from bedbes in bedbesJoin.DefaultIfEmpty()
                   join taviz in customersInfo.TavizInfo
                       on members.CustomerNumber equals taviz.CustomerNumber
                       into tavizJoin
                   from taviz in tavizJoin.DefaultIfEmpty()
                   select new MeterReadingDetailCreateDto()
                   {
                       FlowImportedId = meterFlowId,
                       ZoneId = meterReading.ZoneId,
                       CustomerNumber = meterReading.CustomerNumber,
                       ReadingNumber = meterReading.ReadingNumber,
                       BillId = members.BillId,
                       AgentCode = meterReading.AgentCode,
                       CurrentCounterStateCode = meterReading.CurrentCounterStateCode,
                       PreviousDateJalali = bedbes is null ? members.WaterInstallationDateJalali : bedbes.LastMeterDateJalali,
                       CurrentDateJalali = meterReading.CurrentDateJalali,
                       PreviousNumber = bedbes?.LastMeterNumber ?? 0,
                       CurrentNumber = meterReading.CurrentNumber,
                       InsertByUserId = meterReading.InsertByUserId,
                       InsertDateTime = meterReading.InsertDateTime,

                       BranchTypeId = members.BranchTypeId,
                       UsageId = members.UsageId,
                       ConsumptionUsageId = members.ConsumptionUsageId,
                       DomesticUnit = members.DomesticUnit,
                       CommercialUnit = members.CommercialUnit,
                       OtherUnit = members.OtherUnit,
                       EmptyUnit = members.EmptyUnit,
                       WaterInstallationDateJalali = members.WaterInstallationDateJalali,
                       SewageInstallationDateJalali = members.SewageInstallationDateJalali,
                       WaterRegisterDate = members.WaterRegisterDate,
                       SewageRegisterDate = members.WaterRegisterDate,
                       WaterCount = members.WaterCount,
                       SewageCalcState = members.SewageCalcState,
                       ContractualCapacity = members.ContractualCapacity,
                       HouseholdDate = members.HouseholdDate,
                       HouseholdNumber = members.HouseholdNumber,
                       VillageId = members.VillageId,
                       IsSpecial = members.IsSpecial,
                       MeterDiameterId = members.MeterDiameterId,
                       VirtualCategoryId = members.VirtualCategoryId,
                       BodySerial = members.BodySerial,

                       TavizCause = taviz?.TavizCause,
                       TavizDateJalali = taviz?.TavizDateJalali,
                       TavizNumber = taviz?.TavizNumber,
                       TavizRegisterDateJalali = taviz?.TavizRegisterDateJalali,

                       LastMeterDateJalali = bedbes is null ? members.WaterInstallationDateJalali : bedbes.LastMeterDateJalali,
                       LastMeterNumber = bedbes?.LastMeterNumber ?? 0,
                       LastConsumption = bedbes?.LastConsumption ?? 0,
                       LastMonthlyConsumption = bedbes?.LastMonthlyConsumption ?? 0,
                       LastCounterStateCode = bedbes?.LastCounterStateCode ?? 0,
                       LastSumItems = bedbes?.LastSumItems ?? 0
                   };
        }
        public async Task ExecSql(ICollection<MeterReadingDetailCreateDto> readingDetailsCreate, FileCreateDto fileInfo, IAppUser appUser)
        {
            int firstFlowId = readingDetailsCreate.FirstOrDefault().FlowImportedId;
            int zoneId = readingDetailsCreate.FirstOrDefault().ZoneId;
            string fromReadingNumber = readingDetailsCreate?.Min(m => m.ReadingNumber) ?? string.Empty;
            string toReadingNumber = readingDetailsCreate?.Max(m => m.ReadingNumber) ?? string.Empty;

            MeterFlowDeleteDto meterFlowDeleteDto = new(firstFlowId, appUser.UserId, DateTime.Now);

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    MeterReadingDetailCommandService meterReadingDetailService = new(connection, transaction);
                    MeterFlowCommandService meterFlowCommand = new(connection, transaction);

                    try
                    {
                        await meterReadingDetailService.Insert(readingDetailsCreate);
                        await MeterFlowCommands(connection, transaction, firstFlowId, zoneId, fromReadingNumber, toReadingNumber, fileInfo.FileName, appUser, fileInfo.Description);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        await meterFlowCommand.Delete(meterFlowDeleteDto);
                        throw ex;
                        //DeleteFromDisk(filePath);//todo:Error
                    }
                }
            }
        }
        public ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCreateDto> GetReturnData(IEnumerable<MeterReadingDetailCreateDto> data, string title)
        {
            int[] closedAndObstacleCounterState = { 4, 7, 8 };
            MeterReadingDetailHeaderOutputDto header = new MeterReadingDetailHeaderOutputDto()
            {
                Amount = data.Sum(m => m.SumItems) ?? 0,
                Consumption = data.Sum(m => m.Consumption) ?? 0,
                RecordCount = data.Count(),
                Closed = data.Count(r => r.CurrentCounterStateCode == 4),
                Obstacle = data.Count(r => r.CurrentCounterStateCode == 7),
                Temporarily = data.Count(r => r.CurrentCounterStateCode == 8),
                PureReading = data.Count(r => !closedAndObstacleCounterState.Contains(r.CurrentCounterStateCode)),
                Malfunction = data.Count(r => r.CurrentCounterStateCode == 1)
            };
            ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCreateDto> result = new(title, header, data);

            return result;
        }
        private async Task MeterFlowCommands(IDbConnection connection, IDbTransaction transaction, int latestFlowId, int ZoneId, string fromReadingNumber, string toReadingNumber, string fileName, IAppUser appUser, string? description)
        {
            MeterFlowCommandService meterFlowService = new(connection, transaction);

            MeterFlowUpdateDto meterFlowUpdate = new(latestFlowId, appUser.UserId, DateTime.Now);
            await meterFlowService.Update(meterFlowUpdate);

            MeterFlowCreateDto newMeterFlow = GetMeterFlowCreateDto(MeterFlowStepEnum.Calculated, fileName, ZoneId, fromReadingNumber, toReadingNumber, appUser.UserId, description);
            await meterFlowService.Insert(newMeterFlow);
        }
        public MeterReadingFileDetail CreateMeterReading(int zoneId, int customerNumber, string readingNumber, int agentCode, short currentCounterStateCode, string previousDateJalali, string currentDateJalali, int previousNumber, int currentNumber, Guid userId)
        {
            return new MeterReadingFileDetail(
                zoneId: zoneId,
                customerNumber: customerNumber,
                readingNumber: readingNumber,
                agentCode: agentCode,
                currentCounterStateCode: currentCounterStateCode,
                previousDateJalali: previousDateJalali,
                currentDateJalali: currentDateJalali,
                previousNumber: previousNumber,
                currentNumber: currentNumber,
                insertByUserId: userId,
                insertDateTime: DateTime.Now
            );
        }
        public MeterFlowCreateDto GetMeterFlowCreateDto(MeterFlowStepEnum step, string fileName, int zoneId, string fromReadingNumber, string toReadingNumber, Guid userId, string description)
        {
            return new MeterFlowCreateDto()
            {
                MeterFlowStepId = step,
                FileName = fileName,
                ZoneId = zoneId,
                FromReadingNumber = fromReadingNumber,
                ToReadingNumber = toReadingNumber,
                InsertByUserId = userId,
                InsertDateTime = DateTime.Now,
                Description = description
            };
        }
        public async Task CheckDuplicateFile(string fileName, CancellationToken cancellationToken)
        {
            string? insertDateTime = await _meterFlowService.GetInsertDateTime(fileName);
            if (insertDateTime is not null)
            {
                string insertDateJalali = ConvertDate.GregorianToJalali(insertDateTime);
                throw new ReadingException(ExceptionLiterals.InvalidDuplicateFileName(insertDateJalali));
            }
        }
        private (bool, bool) CounterStateValidation(int counterStateCode, int currentNumber, int previousNumber)
        {
            int[] invalidCounterStateCode = [_closeMeterStateId, /*_withoutConsumptionMeterTypeId,*/ _blockMeterStateId, _noReadMeterStateId, _desolateUnitMeterStateId, _disconnectionMeterStateId];

            if (counterStateCode == _commonMeterStateId && previousNumber > currentNumber)
            {
                return (false, true);
            }
            if (counterStateCode == _withoutConsumptionMeterStateId && previousNumber != currentNumber)
            {
                return (false, true);
            }
            if (invalidCounterStateCode.Contains(counterStateCode))
            {
                return (false, false);
            }
            else if ((counterStateCode == _changeCounterStateId || counterStateCode == _reverseCounterState || counterStateCode == _nextRoundCounterSatateId) && currentNumber > previousNumber)
            {
                return (false, true);
            }
            return (true, false);//(IsValid,HasExclude)
        }
        private MeterImaginaryInputDto GetMeterImaginary(MeterReadingDetailCreateDto readingDetail)
        {
            CustomerDetailInfoInputDto customerInfo = new()
            {
                ZoneId = readingDetail.ZoneId,
                Radif = readingDetail.CustomerNumber,
                BranchType = readingDetail.BranchTypeId,//todo
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
                CounterStateCode = readingDetail.CurrentCounterStateCode,
            };
            MeterInfoByPreviousDataInputDto meterInfo = new()
            {
                BillId = readingDetail.BillId,
                PreviousDateJalali = readingDetail.PreviousDateJalali,
                PreviousNumber = readingDetail.PreviousNumber,
                CurrentDateJalali = readingDetail.CurrentDateJalali,
                CurrentMeterNumber = readingDetail.CurrentNumber,
                CounterStateCode = readingDetail.CurrentCounterStateCode
            };
            return new MeterImaginaryInputDto()
            {
                CustomerInfo = customerInfo,
                MeterPreviousData = meterInfo,
            };
        }
        private async Task<MeterReadingDetailCreateDto> GetMeterReadingDetailByAbBahaValue(MeterReadingDetailCreateDto r, AbBahaCalculationDetails? abBahaCalc, bool hasZeroValue, Guid? userIdExclude)
        {
            double preDebtAmount = await _customerInfoService.GetMembersBedBes(new ZoneIdAndCustomerNumber(r.ZoneId, r.CustomerNumber));//checkResult: changeDto
            var (sumItems, jam, pard) = GetAmounts(preDebtAmount, abBahaCalc?.SumItems ?? 0);
            string mohlatDateJalali = DateTime.Now.AddDays(_paymentDeadline).ToShortPersianDateString();

            r.ExcludedByUserId = userIdExclude;
            r.ExcludedDateTime = userIdExclude.HasValue ? DateTime.Now : null;
            r.ExcludedCauseId = userIdExclude.HasValue ? (int)ExcludedCauseEnum.Error : null;
            r.ExcludedCauseTitle = userIdExclude.HasValue ? ReportLiterals.Error : null;
            r.Barge = 0;
            r.PriNo = r.PreviousNumber;
            r.TodayNo = r.CurrentNumber;
            r.PriDate = r.PreviousDateJalali;
            r.TodayDate = r.CurrentDateJalali;
            r.AbonAb = (decimal)(abBahaCalc?.AbonmanAbAmount ?? 0);
            r.AbonFas = (decimal)(abBahaCalc?.AbonmanFazelabAmount ?? 0);
            r.FasBaha = ((decimal)(abBahaCalc?.FazelabAmount ?? 0)) + ((decimal)(abBahaCalc?.HotSeasonFazelabAmount ?? 0));
            r.AbBaha = (decimal)(abBahaCalc?.AbBahaAmount ?? 0);
            r.Ztadil = 0;//todo
            r.Masraf = (decimal)(abBahaCalc?.Consumption ?? 0);
            r.Shahrdari = (decimal)(abBahaCalc?.MaliatAmount ?? 0);
            r.Modat = abBahaCalc?.Duration ?? 0;
            r.DateBed = DateTime.Now.ToShortPersianDateString();
            r.JalaseNo = 0;//todo
            r.Mohlat = mohlatDateJalali;
            r.Baha = (decimal)sumItems;
            r.Pard = (decimal)pard;
            r.Jam = (decimal)jam;
            r.CodVas = r.CurrentCounterStateCode;
            r.Ghabs = "1";
            r.Del = false;
            r.Type = "1";
            r.CodEnshab = r.UsageId;
            r.Enshab = r.MeterDiameterId;
            r.Elat = 0;
            r.Serial = 0;// string.IsNullOrWhiteSpace(meterReaing.BodySerial) ? 0 : int.Parse(meterReaing.BodySerial);//todo
            r.Ser = 0;// string.IsNullOrWhiteSpace(meterReaing.BodySerial) ? 0 : int.Parse(meterReaing.BodySerial);//todo
            r.ZaribFasl = (decimal)(abBahaCalc?.HotSeasonAbBahaAmount ?? 0);
            r.Ab10 = 0;
            r.Ab20 = 0;
            r.TedadVahd = r.OtherUnit;
            r.TedKhane = r.HouseholdNumber;
            r.TedadMas = r.DomesticUnit;
            r.TedadTej = r.CommercialUnit;
            r.NoeVa = r.BranchTypeId;
            r.Jarime = 0;
            r.Masjar = 0;
            r.Sabt = 0;
            r.Rate = (decimal)(abBahaCalc?.MonthlyConsumption ?? 0);
            r.Operator = 666;
            r.Mamor = r.AgentCode;
            r.TavizDate = "";//todo
            r.ZaribCntr = 0;
            r.Zabresani = 0;
            r.ZaribD = (decimal)(abBahaCalc?.JavaniAmount ?? 0);
            r.Tafavot = 0;
            r.KasrHa = (decimal)(abBahaCalc?.DiscountSum ?? 0);
            r.FixMas = r.ContractualCapacity;
            r.ShGhabs1 = r.BillId;
            r.ShPard1 = "";//todo
            r.TabAbnA = 0;
            r.TabAbnF = 0;
            r.TabsFa = 0;
            r.NewAb = 0;
            r.NewFa = 0;
            r.Bodjeh = (decimal)(abBahaCalc?.SumBoodje ?? 0);
            r.Group1 = r.ConsumptionUsageId;
            r.MasFas = 0;
            r.Faz = (abBahaCalc?.FazelabAmount ?? 0) > 0;
            r.ChkKarbari = 0;
            r.C200 = 0;
            r.AbSevom = 0;
            r.AbSevom1 = 0;
            r.C70 = 0;
            r.C80 = 0;
            r.C90 = 0;
            r.C101 = 0;
            r.KhaliS = r.EmptyUnit;
            r.EdarehK = r.IsSpecial;
            r.Avarez = (decimal)(abBahaCalc?.AvarezAmount ?? 0);

            r.AbBahaDiscount = abBahaCalc?.AbBahaDiscount ?? 0;
            r.HotSeasonDiscount = abBahaCalc?.HotSeasonDiscount ?? 0;
            r.HotSeasonFazelabDiscount = abBahaCalc?.HotSeasonFazelabDiscount ?? 0;
            r.FazelabDiscount = abBahaCalc?.FazelabDiscount ?? 0;
            r.AbonmanAbDiscount = abBahaCalc?.AbonmanAbDiscount ?? 0;
            r.AbonmanFazelabDiscount = abBahaCalc?.AbonmanFazelabDiscount ?? 0;
            r.AvarezDiscount = abBahaCalc?.AvarezDiscount ?? 0;
            r.JavaniDiscount = abBahaCalc?.JavaniDiscount ?? 0;
            r.BoodjeDiscount = abBahaCalc?.BoodjeDiscount ?? 0;
            r.MaliatDiscount = abBahaCalc?.MaliatDiscount ?? 0;

            if (hasZeroValue)
            {
                r.SumItemsBeforeDiscount = 0;
                r.SumItems = 0;
                r.DiscountSum = 0;
                r.Consumption = 0;
                r.MonthlyConsumption = 0;
            }
            else
            {
                r.SumItemsBeforeDiscount = abBahaCalc?.SumItemsBeforeDiscount ?? 0;
                r.SumItems = abBahaCalc?.SumItems ?? 0;
                r.DiscountSum = abBahaCalc?.DiscountSum ?? 0;
                r.Consumption = abBahaCalc?.Consumption ?? 0;
                r.MonthlyConsumption = abBahaCalc?.MonthlyConsumption ?? 0;
            }
            return r;
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
        private bool IsInException(Exception ex)
        {
            return ex is InvalidDateException ||
            ex is InvalidBillIdException ||
            ex is TariffDateException ||
            ex is InvalidBillCommandException ||
            ex is TariffCalcException;
        }
    }
}
