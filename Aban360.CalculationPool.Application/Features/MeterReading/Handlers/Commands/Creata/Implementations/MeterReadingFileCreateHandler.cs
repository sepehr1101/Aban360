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
using DNTPersianUtils.Core;
using DotNetDBF;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;
using static Aban360.Common.Extensions.IoExtensions;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Implementations
{
    internal sealed class MeterReadingFileCreateHandler : AbstractBaseConnection, IMeterReadingFileCreateHandler
    {
        const string _dbfPath = @"AppData\Dbfs";
        const string _reportTitle = "آپلود و محاسبه اولیه";
        const int _conditionPayableAmount = 10000;
        const int _paymentDeadline = 7;

        private readonly IMeterFlowQueryService _meterFlowService;
        private readonly ICustomerInfoService _customerInfoService;
        private readonly IMeterReadingDetailQueryService _meterReadingDetailService;
        private readonly IMeterFlowValidationGetHandler _meterFlowValidationGetHandler;
        private readonly IOldTariffEngine _tariffEngine;
        private readonly IValidator<MeterReadingFileCreateDto> _validator;
        private readonly IPreviousAverageHandler _previousAverageHandler;

        public MeterReadingFileCreateHandler(
              IMeterFlowQueryService meterFlowService,
            ICustomerInfoService customerInfoService,
             IMeterReadingDetailQueryService meterReadingDetailService,
            IOldTariffEngine tariffEngine,
            IMeterFlowValidationGetHandler meterFlowValidationGetHandler,
            IValidator<MeterReadingFileCreateDto> validator,
            IPreviousAverageHandler previousAverageHandler,
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
        }

        public async Task<ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCreateDto>> Handle(MeterReadingFileCreateDto input, IAppUser appUser, CancellationToken cancellationToken)
        {
            await Validate(input, cancellationToken);
            await CheckDuplicateFile(input.ReadingFile.FileName, cancellationToken);

            string filePath = await SaveToDisk(input.ReadingFile, _dbfPath);
            IEnumerable<MeterReadingDetailCreateDto> readingDetails = await GetMeterReadingDetails(input, filePath, appUser.UserId);

            ICollection<MeterReadingDetailCreateDto> readingDetailsCreate = new List<MeterReadingDetailCreateDto>();
            foreach (var readingDetail in readingDetails)
            {
                if (CounterStateValidation(readingDetail.CurrentCounterStateCode, readingDetail.CurrentNumber, readingDetail.PreviousNumber))
                {
                    if (readingDetail.CurrentCounterStateCode == 1)//xarab
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
                        AbBahaCalculationDetails abBahaCalc = await _tariffEngine.Handle(meterInfo, cancellationToken);
                        readingDetailsCreate.Add(await GetMeterReadingDetailByAbBahaValue(readingDetail, abBahaCalc, false));
                    }
                    else if (readingDetail.CurrentCounterStateCode == 2 && string.IsNullOrWhiteSpace(readingDetail.TavizDateJalali))
                    {
                        readingDetailsCreate.Add(await GetMeterReadingDetailByAbBahaValue(readingDetail, null, true));
                    }
                    else if (readingDetail.CurrentCounterStateCode == 2 && readingDetail.TavizDateJalali.CompareTo(readingDetail.CurrentDateJalali) > 0)
                    {
                        readingDetailsCreate.Add(await GetMeterReadingDetailByAbBahaValue(readingDetail, null, true));
                    }
                    else if (readingDetail.CurrentCounterStateCode == 2 && readingDetail.TavizDateJalali.CompareTo(readingDetail.PreviousDateJalali) < 0)
                    {
                        readingDetailsCreate.Add(await GetMeterReadingDetailByAbBahaValue(readingDetail, null, true));
                    }
                    else if (readingDetail.CurrentCounterStateCode == 2) //taviz
                    {
                        int previousNumber = readingDetail.PreviousNumber;
                        string previousDateJalali = readingDetail.PreviousDateJalali;

                        readingDetail.PreviousNumber = 0;
                        readingDetail.PreviousDateJalali = readingDetail.TavizDateJalali;//TODO: check has value
                        MeterImaginaryInputDto meterImaginaryTmp = GetMeterImaginary(readingDetail);
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
                        readingDetailsCreate.Add(await GetMeterReadingDetailByAbBahaValue(readingDetail, abBahaCalc, false));
                    }
                    else //not xarab, nor taviz
                    {
                        MeterImaginaryInputDto meterImaginary = GetMeterImaginary(readingDetail);
                        AbBahaCalculationDetails abBahaCalc = await _tariffEngine.Handle(meterImaginary, cancellationToken);
                        readingDetailsCreate.Add(await GetMeterReadingDetailByAbBahaValue(readingDetail, abBahaCalc, false));
                    }
                }
                else
                {
                    readingDetailsCreate.Add(await GetMeterReadingDetailByAbBahaValue(readingDetail, null, true));
                }
            }

            await InsertAndUpdate(readingDetailsCreate, input, appUser, filePath);
            return GetReturnData(readingDetailsCreate);
        }
        private async Task InsertAndUpdate(ICollection<MeterReadingDetailCreateDto> readingDetailsCreate, MeterReadingFileCreateDto input, IAppUser appUser, string filePath)
        {
            int firstFlowId = readingDetailsCreate.FirstOrDefault().FlowImportedId;
            int zoneId = readingDetailsCreate.FirstOrDefault().ZoneId;

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                IDbTransaction transaction = null;
                using (transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    MeterReadingDetailCommandService meterReadingDetailService = new(connection, transaction);
                    try
                    {
                        await meterReadingDetailService.Insert(readingDetailsCreate);
                        await MeterFlowCommands(connection, transaction, firstFlowId, zoneId, input.ReadingFile.FileName, appUser, input.Description);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        await meterReadingDetailService.Delete(new MeterReadingDetailDeleteDto(firstFlowId, appUser.UserId, DateTime.Now));//todo: notWork
                        DeleteFromDisk(filePath);//todo:Error
                    }
                }
            }

        }
        private ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCreateDto> GetReturnData(IEnumerable<MeterReadingDetailCreateDto> data)
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
                Ruined = data.Count(r => r.CurrentCounterStateCode == 1)
            };
            ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCreateDto> result = new(_reportTitle, header, data);

            return result;
        }
        private async Task MeterFlowCommands(IDbConnection connection, IDbTransaction transaction, int latestFlowId, int ZoneId, string fileName, IAppUser appUser, string? description)
        {
            MeterFlowCommandService meterFlowService = new(connection, transaction);

            MeterFlowUpdateDto meterFlowUpdate = new(latestFlowId, appUser.UserId, DateTime.Now);
            await meterFlowService.Update(meterFlowUpdate);

            MeterFlowCreateDto newMeterFlow = GetMeterFlowCreateDto(MeterFlowStepEnum.Calculated, fileName, ZoneId, appUser.UserId, description);
            await meterFlowService.Insert(newMeterFlow);
        }
        private async Task<IEnumerable<MeterReadingDetailCreateDto>> GetMeterReadingDetails(MeterReadingFileCreateDto meterFile, string filePath, Guid userId)
        {
            ICollection<MeterReadingFileDetail> meterReadings = ReadDb(filePath, userId);
            MeterReadingFileDetail firstMeterDetail = meterReadings.FirstOrDefault();
            MeterFlowCreateDto importedMeterFlow = GetMeterFlowCreateDto(MeterFlowStepEnum.Imported, meterFile.ReadingFile.FileName, firstMeterDetail.ZoneId, userId, meterFile.Description);
            int meterFlowId = 0;

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    MeterFlowCommandService meterflowCommandService = new(connection, transaction);
                    meterFlowId = await meterflowCommandService.Insert(importedMeterFlow);

                    transaction.Commit();
                }
            }

            CustomersInfoGetDto customersInfo = await _customerInfoService.Get(firstMeterDetail.ZoneId, meterReadings.Select(m => m.CustomerNumber).ToList());
            IEnumerable<MeterReadingDetailCreateDto> meterReadingsDetailCreate = GetReadingMeterDetails(meterReadings, customersInfo, meterFlowId);

            return meterReadingsDetailCreate;
        }
        private IEnumerable<MeterReadingDetailCreateDto> GetReadingMeterDetails(ICollection<MeterReadingFileDetail> meterReadings, CustomersInfoGetDto customersInfo, int meterFlowId)
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
                       PreviousDateJalali = meterReading.PreviousDateJalali,
                       CurrentDateJalali = meterReading.CurrentDateJalali,
                       PreviousNumber = meterReading.PreviousNumber,
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
        private MeterReadingFileDetail CreateMeterReading(int zoneId, int customerNumber, string readingNumber, int agentCode, short currentCounterStateCode, string previousDateJalali, string currentDateJalali, int previousNumber, int currentNumber, Guid userId)
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
        private ICollection<MeterReadingFileDetail> ReadDb(string filePath, Guid userId)
        {
            var result = new List<Dictionary<string, object>>();

            FileStream stream = File.OpenRead(filePath);
            DBFReader reader = new DBFReader(stream);

            int recordCount = reader.RecordCount;
            DBFField[] fields = reader.Fields;

            ICollection<MeterReadingFileDetail> meterReadingFileDetail = new List<MeterReadingFileDetail>();
            object[] rowObjects;
            while ((rowObjects = reader.NextRecord()) != null)
            {
                //radif=0 eshterak=1 pridate=2 currentday=3 prinu=4 currentnu=5 codvas-counterstate=6 mamorcode=7 town=13
                int customerNumber = (int)(decimal)rowObjects[0];
                string readingNumber = (string)rowObjects[1];
                string previousDay = (string)rowObjects[2];
                string currentDay = (string)rowObjects[3];
                int previousNumber = (int)(decimal)rowObjects[4];
                int currentNumber = (int)(decimal)rowObjects[5];
                short counterStateCode = (short)(decimal)rowObjects[6];
                int agentCode = (int)(decimal)rowObjects[7];
                int zoneId = (int)(decimal)rowObjects[13];

                MeterReadingFileDetail meterDetail = CreateMeterReading(zoneId, customerNumber, readingNumber, agentCode, counterStateCode, previousDay, currentDay, previousNumber, currentNumber, userId);
                meterReadingFileDetail.Add(meterDetail);
            }

            return meterReadingFileDetail;
        }
        private MeterFlowCreateDto GetMeterFlowCreateDto(MeterFlowStepEnum step, string fileName, int zoneId, Guid userId, string description)
        {
            return new MeterFlowCreateDto()
            {
                MeterFlowStepId = step,
                FileName = fileName,
                ZoneId = zoneId,
                InsertByUserId = userId,
                InsertDateTime = DateTime.Now,
                Description = description
            };
        }
        private async Task Validate(MeterReadingFileCreateDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private async Task CheckDuplicateFile(string fileName, CancellationToken cancellationToken)
        {
            string? insertDateTime = await _meterFlowService.GetInsertDateTime(fileName);
            if (insertDateTime is not null)
            {
                string insertDateJalali = ConvertDate.GregorianToJalali(insertDateTime);
                throw new ReadingException(ExceptionLiterals.InvalidDuplicateFileName(insertDateJalali));
            }
        }
        private bool CounterStateValidation(int counterStateCode, int currentNumber, int previousNumber)
        {
            int[] invalidCounterStateCode = [4, /*6,*/ 7, 8, 9, 10];

            if (counterStateCode == 6 && previousNumber != currentNumber)
            {
                return false;
            }
            if (invalidCounterStateCode.Contains(counterStateCode))
            {
                return false;
            }
            else if ((counterStateCode == 3 || counterStateCode == 5) && currentNumber > previousNumber)
            {
                return false;
            }
            return true;
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
        private async Task<MeterReadingDetailCreateDto> GetMeterReadingDetailByAbBahaValue(MeterReadingDetailCreateDto r, AbBahaCalculationDetails? abBahaCalc, bool hasZeroValue)
        {
            double preDebtAmount = await _customerInfoService.GetMembersBedBes(new ZoneIdAndCustomerNumber(r.ZoneId, r.CustomerNumber));//checkResult: changeDto
            var (sumItems, jam, pard) = GetAmounts(preDebtAmount, abBahaCalc?.SumItems ?? 0);
            string mohlatDateJalali = DateTime.Now.AddDays(_paymentDeadline).ToShortPersianDateString();

            r.Barge = 0;
            r.PriNo = r.PreviousNumber;
            r.TodayNo = r.CurrentNumber;
            r.PriDate = r.PreviousDateJalali;
            r.TodayDate = r.CurrentDateJalali;
            r.AbonAb = (decimal)(abBahaCalc?.AbonmanAbAmount ?? 0);
            r.AbonFas = (decimal)(abBahaCalc?.AbonmanFazelabAmount ?? 0);
            r.FasBaha = (decimal)(abBahaCalc?.FazelabAmount ?? 0);
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
            r.Operator = 0;//todo
            r.Mamor = 0;//todo
            r.TavizDate = "";//todo
            r.ZaribCntr = 0;
            r.Zabresani = 0;
            r.ZaribD = 0;
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

            r.AbBahaDiscount=abBahaCalc.AbBahaDiscount;
            r.HotSeasonDiscount=abBahaCalc.HotSeasonDiscount;
            r.HotSeasonFazelabDiscount = abBahaCalc.HotSeasonFazelabDiscount;
            r.FazelabDiscount=abBahaCalc.FazelabDiscount;
            r.AbonmanAbDiscount= abBahaCalc.AbonmanAbDiscount;
            r.AbonmanFazelabDiscount = abBahaCalc.AbonmanFazelabDiscount;
            r.AvarezDiscount=abBahaCalc.AvarezDiscount;
            r.JavaniDiscount=abBahaCalc.JavaniDiscount;
            r.BoodjeDiscount=abBahaCalc.BoodjeDiscount;
            r.MaliatDiscount=abBahaCalc.MaliatDiscount;

            if (hasZeroValue)
            {
                r.SumItemsBeforeDiscount = 0;
                r.SumItems = 0;
                r.DiscountSum = 0;
                r.Consumption = 0;
                r.MonthlyConsumption = 0;
                r.CurrentCounterStateCode = 4;
            }
            else
            {
                r.SumItemsBeforeDiscount = abBahaCalc.SumItemsBeforeDiscount;
                r.SumItems = abBahaCalc.SumItems;
                r.DiscountSum = abBahaCalc.DiscountSum;
                r.Consumption = abBahaCalc.Consumption;
                r.MonthlyConsumption = abBahaCalc.MonthlyConsumption;
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
    }
}
