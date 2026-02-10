using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.Common.Timing;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using DotNetDBF;
using FluentValidation;
using System.Collections.Generic;
using System.Threading;
using static Aban360.Common.Extensions.IoExtensions;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Implementations
{
    internal sealed class MeterReadingFileCreateHandler : IMeterReadingFileCreateHandler
    {
        const string _dbfPath = @"AppData\Dbfs";
        const string _reportTitle = "آپلود و محاسبه اولیه";

        private readonly IMeterReadingDetailService _meterReadingFileService;
        private readonly IMeterFlowService _meterFlowService;
        private readonly ICustomerInfoService _customerInfoService;
        private readonly IMeterReadingDetailService _meterReadingDetailService;
        private readonly IMeterFlowValidationGetHandler _meterFlowValidationGetHandler;
        private readonly IOldTariffEngine _tariffEngine;
        private readonly IValidator<MeterReadingFileCreateDto> _validator;

        public MeterReadingFileCreateHandler(
            IMeterReadingDetailService meterReadingFileService,
            IMeterFlowService meterFlowService,
            ICustomerInfoService customerInfoService,
            IMeterReadingDetailService meterReadingDetailService,
            IOldTariffEngine tariffEngine,
            IMeterFlowValidationGetHandler meterFlowValidationGetHandler,
            IValidator<MeterReadingFileCreateDto> validator)
        {
            _meterReadingFileService = meterReadingFileService;
            _meterReadingFileService.NotNull(nameof(_meterReadingFileService));

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
                    MeterImaginaryInputDto meterImaginary = GetMeterImaginary(readingDetail);
                    AbBahaCalculationDetails abBahaCalc = await _tariffEngine.Handle(meterImaginary, cancellationToken);
                    readingDetailsCreate.Add(GetMeterReadingDetailByAbBahaValue(readingDetail, abBahaCalc, false));
                }
                else
                {
                    readingDetailsCreate.Add(GetMeterReadingDetailByAbBahaValue(readingDetail, null, true));
                }
            }
            await _meterReadingDetailService.Insert(readingDetailsCreate);

            int firstFlowId = readingDetailsCreate.FirstOrDefault().FlowImportedId;
            int zoneId = readingDetailsCreate.FirstOrDefault().ZoneId;
            await CompleteMeterFlow(firstFlowId, zoneId, input.ReadingFile.FileName, appUser, input.Description);

            return GetReturnData(readingDetailsCreate);
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
                Ruined=data.Count(r=>r.CurrentCounterStateCode==1)
            };
            ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCreateDto> result = new(_reportTitle, header, data);

            return result;
        }
        private async Task CompleteMeterFlow(int latestFlowId, int ZoneId, string fileName, IAppUser appUser, string? description)
        {
            MeterFlowUpdateDto meterFlowUpdate = new(latestFlowId, appUser.UserId, DateTime.Now);
            _meterFlowService.Update(meterFlowUpdate);

            MeterFlowCreateDto newMeterFlow = new()
            {
                MeterFlowStepId = MeterFlowStepEnum.Calculated,
                ZoneId = ZoneId,
                FileName = fileName,
                InsertByUserId = appUser.UserId,
                InsertDateTime = DateTime.Now,
                Description = description
            };
            await _meterFlowService.Create(newMeterFlow);
        }
        private async Task<IEnumerable<MeterReadingDetailCreateDto>> GetMeterReadingDetails(MeterReadingFileCreateDto meterFile, string filePath, Guid userId)
        {
            ICollection<MeterReadingFileDetail> meterReadings = ReadDb(filePath, userId);
            MeterReadingFileDetail firstMeterDetail = meterReadings.FirstOrDefault();

            MeterFlowCreateDto importedMeterFlow = CreateImportedMeterFlowStep(meterFile.ReadingFile.FileName, firstMeterDetail.ZoneId, userId, meterFile.Description);
            int meterFlowId = await _meterFlowService.Create(importedMeterFlow);

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
        private MeterFlowCreateDto CreateImportedMeterFlowStep(string fileName, int zoneId, Guid userId, string description)
        {
            return new MeterFlowCreateDto()
            {
                MeterFlowStepId = MeterFlowStepEnum.Imported,
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

            //if(counterStateCode==6 && previousNumber!=currentNumber)
            //{
            //    return false;
            //}
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
        private MeterReadingDetailCreateDto GetMeterReadingDetailByAbBahaValue(MeterReadingDetailCreateDto readingDetail, AbBahaCalculationDetails? abBahaCalc, bool hasZeroValue)
        {
            if (hasZeroValue)
            {
                readingDetail.SumItemsBeforeDiscount = 0;
                readingDetail.SumItems = 0;
                readingDetail.DiscountSum = 0;
                readingDetail.Consumption = 0;
                readingDetail.MonthlyConsumption = 0;
            }
            else
            {
                readingDetail.SumItemsBeforeDiscount = abBahaCalc.SumItemsBeforeDiscount;
                readingDetail.SumItems = abBahaCalc.SumItems;
                readingDetail.DiscountSum = abBahaCalc.DiscountSum;
                readingDetail.Consumption = abBahaCalc.Consumption;
                readingDetail.MonthlyConsumption = abBahaCalc.MonthlyConsumption;
            }
            return readingDetail;
        }
    }
}
