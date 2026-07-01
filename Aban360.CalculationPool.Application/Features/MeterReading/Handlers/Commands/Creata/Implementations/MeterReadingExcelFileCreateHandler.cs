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
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Excel = MiniExcelLibs;
using System.Data;
using static Aban360.Common.Extensions.IoExtensions;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Implementations
{
    internal sealed class MeterReadingExcelFileCreateHandler : AbstractBaseConnection, IMeterReadingExcelFileCreateHandler
    {
        const string _dbfPath = @"AppData\Dbfs";
        const string _reportTitle = "آپلود و محاسبه اولیه";

        private readonly IMeterReadingCreateBaseHandler _meterReadingCreateBaseHandler;
        private readonly IMeterFlowQueryService _meterFlowService;
        private readonly ICustomerInfoService _customerInfoService;
        private readonly IValidator<MeterReadingExcelFileCreateDto> _validator;
        private readonly IBedBesQueryService _bedBesQueryService;

        public MeterReadingExcelFileCreateHandler(
            IMeterReadingCreateBaseHandler meterReadingCreateBaseHandler,
            IMeterFlowQueryService meterFlowService,
            ICustomerInfoService customerInfoService,
            IValidator<MeterReadingExcelFileCreateDto> validator,
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

        public async Task<ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCreateDto>> Handle(MeterReadingExcelFileCreateDto input, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(input, cancellationToken);
            //await _meterReadingCreateBaseHandler.CheckDuplicateFile(input.ReadingFile.FileName, cancellationToken);

            string filePath = await SaveToDisk(input.ReadingFile, _dbfPath);
            IEnumerable<MeterReadingDetailCreateDto> readingDetails = await GetMeterReadingDetails(input, filePath, appUser.UserId);
            FileCreateDto fileCreateInfo = new(input.ReadingFile.FileName, filePath, input.Description);
            ICollection<MeterReadingDetailCreateDto> readingDetailsCreate = await _meterReadingCreateBaseHandler.GetReadingDetailCreateFinal(readingDetails, fileCreateInfo, appUser, cancellationToken);

            await _meterReadingCreateBaseHandler.ExecSql(readingDetailsCreate, fileCreateInfo, appUser);
            return _meterReadingCreateBaseHandler.GetReturnData(readingDetailsCreate, _reportTitle);
        }
        private async Task<IEnumerable<MeterReadingDetailCreateDto>> GetMeterReadingDetails(MeterReadingExcelFileCreateDto meterFile, string filePath, Guid userId)
        {
            ICollection<MeterReadingFileDetail> meterReadings = ReadExcel(filePath, userId);
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
        private ICollection<MeterReadingFileDetail> ReadExcel(string filePath, Guid userId)
        {
            var result = new List<Dictionary<string, object>>();

            var rows = Excel.MiniExcel.Query(filePath, useHeaderRow: false);
            ICollection<MeterReadingFileDetail> meterReadingFileDetail = new List<MeterReadingFileDetail>();
            foreach (var item in rows.Skip(1))
            {
                var row = (IDictionary<string, object>)item;

                //0:CurrentNumber 1:CurretnDate 2:CurrentCounterState 3:AgentCode 4:ZoneId 5:ZoneTitle
                //6:CustomerNumber 7:BillId 8:ReadingNumber 9:PriNumber 10:PriDate 
                int customerNumber = Convert.ToInt32(row.ElementAt(6).Value);
                string readingNumber = row.ElementAt(8).Value.ToString();
                string previousDay = row.ElementAt(10).Value.ToString();
                string currentDay = row.ElementAt(1).Value.ToString();
                int previousNumber = Convert.ToInt32(row.ElementAt(9).Value);
                int currentNumber = Convert.ToInt32(row.ElementAt(0).Value);
                short counterStateCode = Convert.ToInt16(row.ElementAt(2).Value);
                int agentCode = Convert.ToInt32(row.ElementAt(3).Value);
                int zoneId = Convert.ToInt32(row.ElementAt(4).Value);

                MeterReadingFileDetail meterDetail = _meterReadingCreateBaseHandler.CreateMeterReading(zoneId, customerNumber, readingNumber, agentCode, counterStateCode, previousDay, currentDay, previousNumber, currentNumber, userId);
                meterReadingFileDetail.Add(meterDetail);
            }

            return meterReadingFileDetail;
        }
        private async Task InputValidate(MeterReadingExcelFileCreateDto input, CancellationToken cancellationToken)
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
