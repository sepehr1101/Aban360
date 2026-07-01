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
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
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

        private readonly IMeterReadingCreateBaseHandler _meterReadingCreateBaseHandler;
        private readonly IMeterFlowQueryService _meterFlowService;
        private readonly ICustomerInfoService _customerInfoService;
        private readonly IValidator<MeterReadingFileCreateDto> _validator;
        private readonly IBedBesQueryService _bedBesQueryService;

        public MeterReadingFileCreateHandler(
            IMeterReadingCreateBaseHandler meterReadingCreateBaseHandler,
            IMeterFlowQueryService meterFlowService,
            ICustomerInfoService customerInfoService,
            IValidator<MeterReadingFileCreateDto> validator,
            IBedBesQueryService bedBesQueryService,
            IConfiguration configuration)
            : base(configuration)
        {
            _meterReadingCreateBaseHandler = meterReadingCreateBaseHandler;
            _meterReadingCreateBaseHandler.NotNull(nameof(meterReadingCreateBaseHandler));

            _meterFlowService = meterFlowService;
            _meterFlowService.NotNull(nameof(_meterFlowService));

            _customerInfoService = customerInfoService;
            _customerInfoService.NotNull(nameof(_customerInfoService));

            _validator = validator;
            _validator.NotNull(nameof(_validator));

            _bedBesQueryService = bedBesQueryService;
            _bedBesQueryService.NotNull(nameof(bedBesQueryService));
        }

        public async Task<ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCreateDto>> Handle(MeterReadingFileCreateDto input, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(input, cancellationToken);
            await _meterReadingCreateBaseHandler.CheckDuplicateFile(input.ReadingFile.FileName, cancellationToken);

            string filePath = await SaveToDisk(input.ReadingFile, _dbfPath);
            IEnumerable<MeterReadingDetailCreateDto> readingDetails = await GetMeterReadingDetails(input, filePath, appUser.UserId);
            FileCreateDto fileCreateInfo = new(input.ReadingFile.FileName, filePath, input.Description);
            ICollection<MeterReadingDetailCreateDto> readingDetailsCreate = await _meterReadingCreateBaseHandler.GetReadingDetailCreateFinal(readingDetails, fileCreateInfo, appUser, cancellationToken);

            await _meterReadingCreateBaseHandler.ExecSql(readingDetailsCreate, fileCreateInfo, appUser);
            return _meterReadingCreateBaseHandler.GetReturnData(readingDetailsCreate, _reportTitle);
        }
        private async Task<IEnumerable<MeterReadingDetailCreateDto>> GetMeterReadingDetails(MeterReadingFileCreateDto meterFile, string filePath, Guid userId)
        {
            ICollection<MeterReadingFileDetail> meterReadings = ReadDb(filePath, userId);
            MeterReadingFileDetail firstMeterDetail = meterReadings.FirstOrDefault();
            MeterFlowCreateDto importedMeterFlow = _meterReadingCreateBaseHandler.GetMeterFlowCreateDto(MeterFlowStepEnum.Imported, meterFile.ReadingFile.FileName, firstMeterDetail.ZoneId, userId, meterFile.Description);
            CustomersInfoGetDto customersInfo;
            IEnumerable<ZoneIdAndCustomerNumber> customersByInvalidPreviousBedBes = new List<ZoneIdAndCustomerNumber>();
            int meterFlowId = 0;

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    MeterFlowCommandService meterflowCommandService = new(connection, transaction);
                    meterFlowId = await meterflowCommandService.Insert(importedMeterFlow);
                    customersInfo = await _customerInfoService.GetByBulkCopy(connection, transaction, firstMeterDetail.ZoneId, meterReadings.Select(m => m.CustomerNumber).ToList());
                    customersByInvalidPreviousBedBes = await _bedBesQueryService.GetPreviousDateAndNumberWithSqlBulk(connection, transaction, firstMeterDetail.ZoneId, meterReadings.Select(m => m.CustomerNumber).ToList());
                    transaction.Commit();
                }
            }
            foreach (var item in customersInfo.BedBesInfo)
            {
                if (customersByInvalidPreviousBedBes.Select(c => c.CustomerNumber).Contains(item.CustomerNumber))
                {
                    BedBesPreviousNumberAndDateOutputDto previousInfo = await _bedBesQueryService.GetPreviousDateAndNumber(new ZoneIdAndCustomerNumber(item.ZoneId, item.CustomerNumber), item.BillId);
                    item.LastMeterNumber = previousInfo.PreviousNumber;
                    item.LastMeterDateJalali = previousInfo.PreviousDateJalali;
                }
            }
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

                MeterReadingFileDetail meterDetail = _meterReadingCreateBaseHandler.CreateMeterReading(zoneId, customerNumber, readingNumber, agentCode, counterStateCode, previousDay, currentDay, previousNumber, currentNumber, userId);
                meterReadingFileDetail.Add(meterDetail);
            }

            return meterReadingFileDetail;
        }
        private async Task InputValidate(MeterReadingFileCreateDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
    }
}
