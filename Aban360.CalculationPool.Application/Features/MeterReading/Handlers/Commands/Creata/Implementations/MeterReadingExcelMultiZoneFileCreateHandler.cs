using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.ReportPool.Domain.Base;
using Dapper;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Excel = MiniExcelLibs;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Implementations
{
    /// <summary>
    /// Calculates an Excel meter-reading file containing multiple zones and writes only
    /// the final bill and discount rows to Atlas. This handler deliberately does not
    /// create MeterFlow or MeterReadingDetail records.
    /// </summary>
    internal sealed class MeterReadingExcelMultiZoneFileCreateHandler : AbstractBaseConnection, IMeterReadingExcelMultiZoneFileCreateHandler
    {
        private const string _atlasDatabaseName = "Atlas";
        private const int _closeMeterStateId = 4;
        private const int _paymentDeadline = 7;
        private const int _maxPaymentIdLength = 13;

        private readonly IMeterReadingCreateBaseHandler _meterReadingCreateBaseHandler;
        private readonly IValidator<MeterReadingExcelFileCreateDto> _validator;
        private readonly ICustomerInfoService _customerInfoService;
        private readonly IBedBesQueryService _bedBesQueryService;
        private readonly ICommonZoneService _commonZoneService;

        public MeterReadingExcelMultiZoneFileCreateHandler(
            IMeterReadingCreateBaseHandler meterReadingCreateBaseHandler,
            IValidator<MeterReadingExcelFileCreateDto> validator,
            ICustomerInfoService customerInfoService,
            IBedBesQueryService bedBesQueryService,
            ICommonZoneService commonZoneService,
            IConfiguration configuration)
            : base(configuration)
        {
            _meterReadingCreateBaseHandler = meterReadingCreateBaseHandler;
            _meterReadingCreateBaseHandler.NotNull(nameof(meterReadingCreateBaseHandler));

            _validator = validator;
            _validator.NotNull(nameof(validator));

            _customerInfoService = customerInfoService;
            _customerInfoService.NotNull(nameof(customerInfoService));

            _bedBesQueryService = bedBesQueryService;
            _bedBesQueryService.NotNull(nameof(bedBesQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));
        }

        public async Task<ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCreateDto>> Handle(
            MeterReadingExcelFileCreateDto input,
            IAppUser appUser,
            CancellationToken cancellationToken)
        {
            await InputValidate(input, cancellationToken);

            ICollection<MeterReadingFileDetail> meterReadings = ReadExcel(input, appUser.UserId, cancellationToken);
            await ValidateZoneAccess(meterReadings, appUser);

            IEnumerable<MeterReadingDetailCreateDto> readingDetails = await GetMeterReadingDetails(meterReadings, cancellationToken);
            ICollection<MeterReadingDetailCreateDto> calculatedReadings = await _meterReadingCreateBaseHandler.GetReadingDetailCreateFinal(
                readingDetails,
                appUser,
                cancellationToken);

            ICollection<MeterReadingDetailCreateDto> readingsToSave = calculatedReadings
                .Where(reading => reading.ExcludedByUserId is null)
                .ToList();

            if (readingsToSave.Count == 0)
            {
                throw new ReadingException(ExceptionLiterals.NotFoundBillsToConfirm);
            }

            var (bedBesRows, kasrHaRows) = CreateAtlasRows(readingsToSave);
            await SaveToAtlas(bedBesRows, kasrHaRows, cancellationToken);

            return _meterReadingCreateBaseHandler.GetReturnData(calculatedReadings, ReportLiterals.MeterReadingCreateFile);
        }

        private ICollection<MeterReadingFileDetail> ReadExcel(
            MeterReadingExcelFileCreateDto input,
            Guid userId,
            CancellationToken cancellationToken)
        {
            ICollection<MeterReadingFileDetail> meterReadings = new List<MeterReadingFileDetail>();
            string errorMessage = ExceptionLiterals.InvalidReadingFile;
            int rowNumber = 0;

            try
            {
                using Stream stream = input.ReadingFile.OpenReadStream();
                IEnumerable<dynamic> rows = Excel.MiniExcel.Query(
                    stream,
                    useHeaderRow: false,
                    sheetName: ExceptionLiterals.Page(1));

                foreach (dynamic item in rows.Skip(1))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    rowNumber++;
                    errorMessage = ExceptionLiterals.InvalidRecord(rowNumber);

                    try
                    {
                        var row = (IDictionary<string, object>)item;

                        int customerNumber = Convert.ToInt32(row.ElementAt(6).Value);
                        string readingNumber = row.ElementAt(8).Value?.ToString() ?? string.Empty;
                        string previousDateJalali = row.ElementAt(10).Value?.ToString() ?? string.Empty;
                        string currentDateJalali = row.ElementAt(1).Value?.ToString() ?? string.Empty;
                        int previousNumber = Convert.ToInt32(row.ElementAt(9).Value);
                        int currentNumber = Convert.ToInt32(row.ElementAt(0).Value);
                        short counterStateCode = Convert.ToInt16(row.ElementAt(2).Value);
                        int agentCode = Convert.ToInt32(row.ElementAt(3).Value);
                        int zoneId = Convert.ToInt32(row.ElementAt(4).Value);
                        int[] allowedZeroStates = [4, 6];

                        if (currentNumber == 0 && !allowedZeroStates.Contains(counterStateCode))
                        {
                            errorMessage = string.Join(" - ", errorMessage, ExceptionLiterals.InvalidZeroMeterNumber);
                            throw new ReadingException(errorMessage);
                        }

                        meterReadings.Add(_meterReadingCreateBaseHandler.CreateMeterReading(
                            zoneId,
                            customerNumber,
                            readingNumber,
                            agentCode,
                            counterStateCode,
                            previousDateJalali,
                            currentDateJalali,
                            previousNumber,
                            currentNumber,
                            userId));
                    }
                    catch (OperationCanceledException)
                    {
                        throw;
                    }
                    catch
                    {
                        throw new ReadingException(errorMessage);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (ReadingException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ReadingException(e.Message);
            }
            catch
            {
                throw new ReadingException(errorMessage);
            }

            if (meterReadings.Count == 0)
            {
                throw new ReadingException(ExceptionLiterals.InvalidReadingFile);
            }

            string duplicateRows = string.Join(", ", meterReadings
                .GroupBy(reading => new { reading.ZoneId, reading.CustomerNumber, reading.CurrentDateJalali })
                .Where(group => group.Count() > 1)
                .Select(group => $"ناحیه:{group.Key.ZoneId}- ردیف:{group.Key.CustomerNumber}- تاریخ قرائت:{group.Key.CurrentDateJalali}"));

            if (!string.IsNullOrWhiteSpace(duplicateRows))
            {
                throw new ReadingException($"{ExceptionLiterals.InvalidReadingFile} - ردیف‌های تکراری: {duplicateRows}");
            }

            return meterReadings;
        }

        private async Task ValidateZoneAccess(IEnumerable<MeterReadingFileDetail> meterReadings, IAppUser appUser)
        {
            HashSet<int> allowedZoneIds = (await _commonZoneService.GetMyZoneIds(appUser)).ToHashSet();
            int[] inaccessibleZoneIds = meterReadings
                .Select(reading => reading.ZoneId)
                .Distinct()
                .Where(zoneId => !allowedZoneIds.Contains(zoneId))
                .ToArray();

            if (inaccessibleZoneIds.Length > 0)
            {
                throw new AccessZoneException($"{ExceptionLiterals.NotAccessZone}: {string.Join(", ", inaccessibleZoneIds)}");
            }
        }

        private async Task<IEnumerable<MeterReadingDetailCreateDto>> GetMeterReadingDetails(
            ICollection<MeterReadingFileDetail> meterReadings,
            CancellationToken cancellationToken)
        {
            ICollection<MeterReadingDetailCreateDto> result = new List<MeterReadingDetailCreateDto>();

            foreach (IGrouping<int, MeterReadingFileDetail> zoneGroup in meterReadings.GroupBy(reading => reading.ZoneId))
            {
                cancellationToken.ThrowIfCancellationRequested();

                ICollection<MeterReadingFileDetail> zoneReadings = zoneGroup.ToList();
                CustomersInfoGetDto customersInfo = await GetCustomerInfo(zoneGroup.Key, zoneReadings, cancellationToken);

                IEnumerable<MeterReadingDetailCreateDto> zoneDetails = _meterReadingCreateBaseHandler.GetReadingMeterDetails(
                    zoneReadings,
                    customersInfo,
                    meterFlowId: 0);

                foreach (MeterReadingDetailCreateDto zoneDetail in zoneDetails)
                {
                    result.Add(zoneDetail);
                }
            }

            return result;
        }

        private async Task<CustomersInfoGetDto> GetCustomerInfo(
            int zoneId,
            ICollection<MeterReadingFileDetail> zoneReadings,
            CancellationToken cancellationToken)
        {
            CustomersInfoGetDto customersInfo;

            using (SqlConnection connection = _sqlReportConnection)
            {
                await connection.OpenAsync(cancellationToken);
                using SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);

                customersInfo = await _customerInfoService.GetByBulkCopy(
                    connection,
                    transaction,
                    zoneId,
                    zoneReadings.Select(reading => reading.CustomerNumber).Distinct().ToList());

                transaction.Commit();
            }

            HashSet<int> foundCustomerNumbers = customersInfo.MembersInfo
                .Where(member => member.ZoneId == zoneId)
                .Select(member => member.CustomerNumber)
                .ToHashSet();

            int[] missingCustomerNumbers = zoneReadings
                .Select(reading => reading.CustomerNumber)
                .Distinct()
                .Where(customerNumber => !foundCustomerNumbers.Contains(customerNumber))
                .ToArray();

            if (missingCustomerNumbers.Length > 0)
            {
                throw new ReadingException(
                    $"{ExceptionLiterals.NotFoundCustomer} ناحیه {zoneId}، ردیف: {string.Join(", ", missingCustomerNumbers)}");
            }

            foreach (LatestBedBesConsumptionInfo bedBesInfo in customersInfo.BedBesInfo)
            {
                cancellationToken.ThrowIfCancellationRequested();

                BedBesPreviousNumberAndDateOutputDto? previousInfo = await _bedBesQueryService.GetPreviousDateAndNumber(
                    new ZoneIdAndCustomerNumber(bedBesInfo.ZoneId, bedBesInfo.CustomerNumber),
                    bedBesInfo.BillId,
                    false);

                if (previousInfo is null)
                {
                    string installationDate = customersInfo.MembersInfo
                        .FirstOrDefault(member =>
                            member.ZoneId == bedBesInfo.ZoneId &&
                            member.CustomerNumber == bedBesInfo.CustomerNumber)
                        ?.WaterInstallationDateJalali ?? string.Empty;

                    if (string.IsNullOrWhiteSpace(installationDate))
                    {
                        throw new InvalidBillCommandException(
                            ExceptionLiterals.InvalidBedBesPreviousNumberAndDateAndInstallationDate(bedBesInfo.BillId));
                    }

                    bedBesInfo.LastMeterNumber = 0;
                    bedBesInfo.LastMeterDateJalali = installationDate;
                    bedBesInfo.LastCounterStateCode = 0;
                    bedBesInfo.LastConsumption = 0;
                    bedBesInfo.LastMonthlyConsumption = 0;
                }
                else
                {
                    bedBesInfo.LastMeterNumber = previousInfo.PreviousNumber;
                    bedBesInfo.LastMeterDateJalali = previousInfo.PreviousDateJalali;
                    bedBesInfo.LastCounterStateCode = previousInfo.CounterStateCode;
                    bedBesInfo.LastConsumption = previousInfo.Consumption;
                    bedBesInfo.LastMonthlyConsumption = previousInfo.ConsumptionAverage;
                }
            }

            return customersInfo;
        }

        private static (ICollection<BedBesCreateDto> BedBesRows, ICollection<KasrHaDto> KasrHaRows) CreateAtlasRows(
            IEnumerable<MeterReadingDetailCreateDto> calculatedReadings)
        {
            ICollection<BedBesCreateDto> bedBesRows = new List<BedBesCreateDto>();
            ICollection<KasrHaDto> kasrHaRows = new List<KasrHaDto>();
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            string paymentDeadlineJalali = DateTime.Now.AddDays(_paymentDeadline).ToShortPersianDateString();
            string paymentIdOption = $"{CommonLiterals.WaterPayIdUniqueCode}{currentDateJalali.Substring(5, 2)}";

            foreach (MeterReadingDetailCreateDto reading in calculatedReadings)
            {
                long payableAmount = Convert.ToInt64(decimal.Truncate(reading.Pard ?? 0));
                string paymentId = TransactionIdGenerator.GeneratePaymentId(payableAmount, reading.BillId, paymentIdOption);
                string storedPaymentId = paymentId.Length <= _maxPaymentIdLength ? paymentId : string.Empty;
                long trackNumber = long.TryParse(paymentId, out long parsedTrackNumber) ? parsedTrackNumber : 0;

                bedBesRows.Add(new BedBesCreateDto
                {
                    Town = reading.ZoneId,
                    Radif = reading.CustomerNumber,
                    Eshtrak = reading.ReadingNumber,
                    Barge = 0,
                    PriNo = reading.PreviousNumber,
                    TodayNo = reading.CurrentNumber,
                    PriDate = reading.PreviousDateJalali,
                    TodayDate = reading.CurrentDateJalali,
                    AbonFas = reading.AbonFas ?? 0,
                    FasBaha = reading.FasBaha ?? 0,
                    AbBaha = reading.AbBaha ?? 0,
                    Ztadil = reading.Ztadil ?? 0,
                    Masraf = (decimal)(reading.Consumption ?? 0),
                    Shahrdari = reading.Shahrdari ?? 0,
                    Modat = reading.Modat ?? 0,
                    DateBed = currentDateJalali,
                    JalaseNo = 0,
                    Mohlat = paymentDeadlineJalali,
                    Baha = (decimal)(reading.SumItems ?? 0),
                    AbonAb = reading.AbonAb ?? 0,
                    Pard = reading.Pard ?? 0,
                    Jam = reading.Jam ?? 0,
                    CodVas = reading.CurrentCounterStateCode,
                    Ghabs = "1",
                    Del = false,
                    Type = "1",
                    CodEnshab = reading.UsageId,
                    Enshab = reading.MeterDiameterId,
                    Elat = 0,
                    Serial = 0,
                    Ser = 0,
                    ZaribFasl = reading.ZaribFasl ?? 0,
                    Ab10 = 0,
                    Ab20 = 0,
                    TedadVahd = reading.OtherUnit,
                    TedKhane = reading.HouseholdNumber,
                    TedadMas = reading.DomesticUnit,
                    TedadTej = reading.CommercialUnit,
                    NoeVa = reading.BranchTypeId,
                    Jarime = 0,
                    Masjar = 0,
                    Sabt = 1,
                    Rate = (decimal)(reading.MonthlyConsumption ?? 0),
                    Operator = 666,
                    Mamor = reading.AgentCode,
                    TavizDate = reading.TavizDateJalali ?? string.Empty,
                    ZaribCntr = 0,
                    Zabresani = 0,
                    ZaribD = reading.ZaribD ?? 0,
                    Tafavot = 0,
                    KasrHa = (decimal)(reading.DiscountSum ?? 0),
                    FixMas = reading.ContractualCapacity,
                    ShGhabs1 = reading.BillId,
                    ShPard1 = storedPaymentId,
                    TabAbnA = 0,
                    TabAbnF = 0,
                    TabsFa = 0,
                    NewAb = 0,
                    NewFa = 0,
                    Bodjeh = reading.Bodjeh ?? 0,
                    Group1 = reading.ConsumptionUsageId,
                    MasFas = reading.MasFas ?? 0,
                    Faz = reading.Faz ?? false,
                    ChkKarbari = reading.ChkKarbari ?? 0,
                    C200 = 0,
                    DateIns = currentDateJalali,
                    AbSevom = 0,
                    AbSevom1 = 0,
                    C70 = 0,
                    C80 = 0,
                    TmpDateBed = string.Empty,
                    TmpPriDate = string.Empty,
                    TmpTodayDate = string.Empty,
                    TmpMohlat = string.Empty,
                    TmpTavizDate = string.Empty,
                    C90 = 0,
                    C101 = 0,
                    KhaliS = reading.EmptyUnit,
                    EdarehK = reading.IsSpecial,
                    Tafa402 = 0,
                    Avarez = reading.Avarez ?? 0,
                    TrackNumber = trackNumber
                });

                if ((reading.DiscountSum ?? 0) <= 0)
                {
                    continue;
                }

                kasrHaRows.Add(new KasrHaDto
                {
                    Town = reading.ZoneId,
                    IdBedbes = 0,
                    Radif = reading.CustomerNumber,
                    CodEnshab = reading.UsageId,
                    Barge = 0,
                    PriDate = reading.PreviousDateJalali,
                    TodayDate = reading.CurrentDateJalali,
                    PriNo = reading.PreviousNumber,
                    TodayNo = reading.CurrentNumber,
                    Masraf = (decimal)(reading.Consumption ?? 0),
                    AbBaha = (decimal)reading.AbBahaDiscount,
                    FasBaha = (decimal)(reading.FazelabDiscount + reading.HotSeasonFazelabDiscount),
                    AbonAb = (decimal)reading.AbonmanAbDiscount,
                    AbonFas = (decimal)reading.AbonmanFazelabDiscount,
                    TabAbnA = 0,
                    TabAbnF = 0,
                    Ab10 = 0,
                    Shahrdari = (decimal)reading.MaliatDiscount,
                    Rate = (decimal)(reading.MonthlyConsumption ?? 0),
                    Baha = (decimal)(reading.DiscountSum ?? 0),
                    ShGhabs = reading.BillId,
                    ShPard = storedPaymentId,
                    DateBed = currentDateJalali,
                    TmpDateBed = string.Empty,
                    TmpTodayDate = string.Empty,
                    TedVahd = reading.OtherUnit,
                    TedKhane = reading.HouseholdNumber,
                    TedadMas = reading.DomesticUnit,
                    TedadTej = reading.CommercialUnit,
                    ZaribFasl = 0,
                    NoeVa = reading.BranchTypeId,
                    Bodjeh = (decimal)reading.BoodjeDiscount
                });
            }

            return (bedBesRows, kasrHaRows);
        }

        private async Task SaveToAtlas(
            ICollection<BedBesCreateDto> bedBesRows,
            ICollection<KasrHaDto> kasrHaRows,
            CancellationToken cancellationToken)
        {
            using SqlConnection connection = _sqlReportConnection;
            await connection.OpenAsync(cancellationToken);
            using SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);

            await EnsureBillsAreNotDuplicate(connection, transaction, bedBesRows, cancellationToken);

            BedBesCommandService bedBesCommandService = new(connection, transaction);
            KasrHaCommandService kasrHaCommandService = new(connection, transaction);

            await bedBesCommandService.InsertByBulk(bedBesRows, _atlasDatabaseName);
            if (kasrHaRows.Count > 0)
            {
                await kasrHaCommandService.InsertByBulk(kasrHaRows, _atlasDatabaseName);
            }
            //transaction.Rollback();
            transaction.Commit();
        }

        private static async Task EnsureBillsAreNotDuplicate(
            SqlConnection connection,
            SqlTransaction transaction,
            ICollection<BedBesCreateDto> bedBesRows,
            CancellationToken cancellationToken)
        {
            string duplicatesInInput = string.Join(", ", bedBesRows
                .GroupBy(row => new { row.Town, row.Radif, row.ShGhabs1, row.TodayDate })
                .Where(group => group.Count() > 1)
                .Select(group => group.Key.ShGhabs1)
                .Distinct());

            if (!string.IsNullOrWhiteSpace(duplicatesInInput))
            {
                throw new ReadingException(ExceptionLiterals.InvalidDuplicateGenerateBill(duplicatesInInput));
            }

            const string createTempTable = @"
                CREATE TABLE #TempMultiZoneBills
                (
                    ZoneId DECIMAL(18, 0) NOT NULL,
                    CustomerNumber DECIMAL(18, 0) NOT NULL,
                    BillId NVARCHAR(20) NOT NULL,
                    TodayDate NVARCHAR(10) NOT NULL
                );";

            await connection.ExecuteAsync(createTempTable, transaction: transaction);

            DataTable table = new();
            table.Columns.Add("ZoneId", typeof(decimal));
            table.Columns.Add("CustomerNumber", typeof(decimal));
            table.Columns.Add("BillId", typeof(string));
            table.Columns.Add("TodayDate", typeof(string));

            foreach (BedBesCreateDto row in bedBesRows)
            {
                table.Rows.Add(row.Town, row.Radif, row.ShGhabs1, row.TodayDate);
            }

            using (SqlBulkCopy bulkCopy = new(connection, SqlBulkCopyOptions.Default, transaction))
            {
                bulkCopy.DestinationTableName = "#TempMultiZoneBills";
                bulkCopy.ColumnMappings.Add("ZoneId", "ZoneId");
                bulkCopy.ColumnMappings.Add("CustomerNumber", "CustomerNumber");
                bulkCopy.ColumnMappings.Add("BillId", "BillId");
                bulkCopy.ColumnMappings.Add("TodayDate", "TodayDate");
                await bulkCopy.WriteToServerAsync(table, cancellationToken);
            }

            const string duplicateQuery = @"
                SELECT DISTINCT t.BillId
                FROM #TempMultiZoneBills t
                INNER JOIN [Atlas].dbo.bed_bes b WITH (UPDLOCK, HOLDLOCK)
                    ON b.town = t.ZoneId
                    AND b.radif = t.CustomerNumber
                    AND b.sh_ghabs1 COLLATE Arabic_CI_AS = t.BillId COLLATE Arabic_CI_AS
                    AND b.today_date = t.TodayDate
                WHERE ISNULL(b.del, 0) = 0;";

            IEnumerable<string> duplicateBillIds = await connection.QueryAsync<string>(
                duplicateQuery,
                transaction: transaction);

            string duplicateBillIdsText = string.Join(", ", duplicateBillIds.Distinct());
            if (!string.IsNullOrWhiteSpace(duplicateBillIdsText))
            {
                throw new ReadingException(ExceptionLiterals.InvalidDuplicateGenerateBill(duplicateBillIdsText));
            }
        }

        private async Task InputValidate(MeterReadingExcelFileCreateDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                string message = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
    }
}
