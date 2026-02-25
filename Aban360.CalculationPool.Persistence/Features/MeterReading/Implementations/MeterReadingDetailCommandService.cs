using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Implementations
{
    public sealed class MeterReadingDetailCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public MeterReadingDetailCommandService(
            IDbConnection connection,
            IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task Insert(MeterReadingDetailCreateDto input)
        {
            string command = GetInsertCommand();
            int rowEffect = await _connection.ExecuteAsync(command, input, _transaction);
            if (rowEffect <= 0)
            {
                throw new ReadingException(ExceptionLiterals.InvalidUpdate);
            }
        }
        public async Task Insert(IEnumerable<MeterReadingDetailCreateDto> input)
        {
            var dataTable = ToDataTable(input);

            using (var bulkCopy = new SqlBulkCopy((SqlConnection)_connection, SqlBulkCopyOptions.Default, (SqlTransaction)_transaction))
            {
                bulkCopy.DestinationTableName = "[Atlas].dbo.MeterReadingDetail";

                foreach (DataColumn col in dataTable.Columns)
                {
                    bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                }

                bulkCopy.BatchSize = 10000;
                bulkCopy.BulkCopyTimeout = 0;

                await bulkCopy.WriteToServerAsync(dataTable);
            }
        }
        public async Task Update(IEnumerable<MeterReadingWithAbBahaResultUpdateDto> items)
        {
            var dt = ToDataTable(items);

            string createTemp = @"
                    CREATE TABLE #TempMeterReadingDetail
                    (
                        Id INT PRIMARY KEY,
                        SumItems FLOAT(53) NULL,
                        SumItemsBeforeDiscount FLOAT(53) NULL,
                        DiscountSum FLOAT(53) NULL,
                        Consumption FLOAT(53) NULL,
                        MonthlyConsumption FLOAT(53) NULL
                    );";

            await new SqlCommand(createTemp, (SqlConnection)_connection, (SqlTransaction)_transaction).ExecuteNonQueryAsync();

            using (var bulk = new SqlBulkCopy((SqlConnection)_connection, SqlBulkCopyOptions.Default, (SqlTransaction)_transaction))
            {
                bulk.DestinationTableName = "#TempMeterReadingDetail";
                await bulk.WriteToServerAsync(dt);
            }

            string mergeSql = @"
                    MERGE Atlas.dbo.MeterReadingDetail AS target
                    USING #TempMeterReadingDetail AS source
                    ON target.Id = source.Id
                    WHEN MATCHED THEN 
                        UPDATE SET
                            target.SumItems = source.SumItems,
                            target.SumItemsBeforeDiscount = source.SumItemsBeforeDiscount,
                            target.DiscountSum = source.DiscountSum,
                            target.Consumption = source.Consumption,
                            target.MonthlyConsumption = source.MonthlyConsumption;";

            await new SqlCommand(mergeSql, (SqlConnection)_connection, (SqlTransaction)_transaction).ExecuteNonQueryAsync();
        }
        public async Task Delete(MeterReadingDetailDeleteDto input)
        {
            string query = GetDeleteCommands();
            int rowEffect = await _connection.ExecuteAsync(query, input, _transaction);
            if (rowEffect <= 0)
            {
                throw new ReadingException(ExceptionLiterals.InvalidUpdate);
            }
        }
        public async Task CreateDuplicateForLog(MeterReadingDetailCreateDuplicateDto input)
        {
            string query = GetCreateDuplicateForLogCommand();
            await _connection.ExecuteAsync(query, input);
        }
        public async Task Exclude(MeterReadingDetailExcludedDto input)
        {
            string query = GetExcludeCommand();
            await _connection.ExecuteAsync(query, input);
        }
        private DataTable ToDataTable(IEnumerable<MeterReadingDetailCreateDto> input)
        {
            var table = new DataTable();

            table.Columns.Add("FlowImportedId", typeof(int));
            table.Columns.Add("ZoneId", typeof(int));
            table.Columns.Add("CustomerNumber", typeof(int));
            table.Columns.Add("ReadingNumber", typeof(string));
            table.Columns.Add("BillId", typeof(string));
            table.Columns.Add("AgentCode", typeof(int));
            table.Columns.Add("CurrentCounterStateCode", typeof(short));
            table.Columns.Add("PreviousDateJalali", typeof(string));
            table.Columns.Add("CurrentDateJalali", typeof(string));
            table.Columns.Add("PreviousNumber", typeof(int));
            table.Columns.Add("CurrentNumber", typeof(int));
            table.Columns.Add("InsertByUserId", typeof(Guid));
            table.Columns.Add("InsertDateTime", typeof(DateTime));
            table.Columns.Add("RemovedByUserId", typeof(Guid));
            table.Columns.Add("RemovedDateTime", typeof(DateTime));
            table.Columns.Add("ExcludedByUserId", typeof(Guid));
            table.Columns.Add("ExcludedDateTime", typeof(DateTime));

            table.Columns.Add("BranchTypeId", typeof(int));
            table.Columns.Add("UsageId", typeof(int));
            table.Columns.Add("ConsumptionUsageId", typeof(int));
            table.Columns.Add("DomesticUnit", typeof(int));
            table.Columns.Add("CommercialUnit", typeof(int));
            table.Columns.Add("OtherUnit", typeof(int));
            table.Columns.Add("EmptyUnit", typeof(int));
            table.Columns.Add("WaterInstallationDateJalali", typeof(string));
            table.Columns.Add("SewageInstallationDateJalali", typeof(string));
            table.Columns.Add("WaterRegisterDate", typeof(string));
            table.Columns.Add("SewageRegisterDate", typeof(string));
            table.Columns.Add("WaterCount", typeof(int));
            table.Columns.Add("SewageCalcState", typeof(int));
            table.Columns.Add("ContractualCapacity", typeof(int));
            table.Columns.Add("HouseholdNumber", typeof(int));
            table.Columns.Add("HouseholdDate", typeof(string));
            table.Columns.Add("VillageId", typeof(string));
            table.Columns.Add("IsSpecial", typeof(bool));
            table.Columns.Add("MeterDiameterId", typeof(short));
            table.Columns.Add("VirtualCategoryId", typeof(int));
            table.Columns.Add("BodySerial", typeof(string));

            table.Columns.Add("TavizDateJalali", typeof(string));
            table.Columns.Add("TavizCause", typeof(string));
            table.Columns.Add("TavizRegisterDateJalali", typeof(string));
            table.Columns.Add("TavizNumber", typeof(int));

            table.Columns.Add("LastMeterDateJalali", typeof(string));
            table.Columns.Add("LastMeterNumber", typeof(int));
            table.Columns.Add("LastConsumption", typeof(float));
            table.Columns.Add("LastMonthlyConsumption", typeof(float));
            table.Columns.Add("LastCounterStateCode", typeof(int));
            table.Columns.Add("LastSumItems", typeof(double));

            table.Columns.Add("SumItems", typeof(double));
            table.Columns.Add("SumItemsBeforeDiscount", typeof(double));
            table.Columns.Add("DiscountSum", typeof(double));
            table.Columns.Add("Consumption", typeof(double));
            table.Columns.Add("MonthlyConsumption", typeof(double));

            //BedBesProps
            table.Columns.Add("barge", typeof(decimal));
            table.Columns.Add("pri_no", typeof(decimal));
            table.Columns.Add("today_no", typeof(decimal));
            table.Columns.Add("pri_date", typeof(string));
            table.Columns.Add("today_date", typeof(string));
            table.Columns.Add("abon_fas", typeof(decimal));
            table.Columns.Add("fas_baha", typeof(decimal));
            table.Columns.Add("ab_baha", typeof(decimal));
            table.Columns.Add("ztadil", typeof(decimal));
            table.Columns.Add("masraf", typeof(decimal));
            table.Columns.Add("shahrdari", typeof(decimal));
            table.Columns.Add("modat", typeof(decimal));
            table.Columns.Add("date_bed", typeof(string));
            table.Columns.Add("jalase_no", typeof(decimal));
            table.Columns.Add("mohlat", typeof(string));
            table.Columns.Add("baha", typeof(decimal));
            table.Columns.Add("abon_ab", typeof(decimal));
            table.Columns.Add("pard", typeof(decimal));
            table.Columns.Add("jam", typeof(decimal));
            table.Columns.Add("cod_vas", typeof(decimal));
            table.Columns.Add("ghabs", typeof(string));
            table.Columns.Add("del", typeof(bool));
            table.Columns.Add("type", typeof(string));
            table.Columns.Add("cod_enshab", typeof(decimal));
            table.Columns.Add("enshab", typeof(decimal));
            table.Columns.Add("elat", typeof(decimal));
            table.Columns.Add("serial", typeof(decimal));
            table.Columns.Add("ser", typeof(decimal));
            table.Columns.Add("zaribfasl", typeof(decimal));
            table.Columns.Add("ab_10", typeof(decimal));
            table.Columns.Add("ab_20", typeof(decimal));
            table.Columns.Add("tedad_vahd", typeof(decimal));
            table.Columns.Add("ted_khane", typeof(decimal));
            table.Columns.Add("tedad_mas", typeof(decimal));
            table.Columns.Add("tedad_tej", typeof(decimal));
            table.Columns.Add("noe_va", typeof(decimal));
            table.Columns.Add("jarime", typeof(decimal));
            table.Columns.Add("masjar", typeof(decimal));
            table.Columns.Add("sabt", typeof(decimal));
            table.Columns.Add("rate", typeof(decimal));
            table.Columns.Add("operator", typeof(decimal));
            table.Columns.Add("mamor", typeof(decimal));
            table.Columns.Add("taviz_date", typeof(string));
            table.Columns.Add("zarib_cntr", typeof(decimal));
            table.Columns.Add("zabresani", typeof(decimal));
            table.Columns.Add("zarib_d", typeof(decimal));
            table.Columns.Add("kasr_ha", typeof(decimal));
            table.Columns.Add("fix_mas", typeof(decimal));
            table.Columns.Add("sh_ghabs1", typeof(string));
            table.Columns.Add("sh_pard1", typeof(string));
            table.Columns.Add("TAB_ABN_A", typeof(decimal));
            table.Columns.Add("TAB_ABN_F", typeof(decimal));
            table.Columns.Add("TABS_FA", typeof(decimal));
            table.Columns.Add("NEWAB", typeof(decimal));
            table.Columns.Add("NEWFA", typeof(decimal));
            table.Columns.Add("BODJEH", typeof(decimal));
            table.Columns.Add("group1", typeof(decimal));
            table.Columns.Add("MAS_FAS", typeof(decimal));
            table.Columns.Add("FAZ", typeof(bool));
            table.Columns.Add("CHK_KARBARI", typeof(decimal));
            table.Columns.Add("C200", typeof(decimal));
            table.Columns.Add("Ab_sevom", typeof(decimal));
            table.Columns.Add("Ab_sevom1", typeof(decimal));
            table.Columns.Add("C70", typeof(decimal));
            table.Columns.Add("C80", typeof(decimal));
            table.Columns.Add("C90", typeof(decimal));
            table.Columns.Add("C101", typeof(decimal));
            table.Columns.Add("Khali_s", typeof(decimal));
            table.Columns.Add("edareh_k", typeof(bool));
            table.Columns.Add("Avarez", typeof(decimal));
            table.Columns.Add("tafavot", typeof(decimal));

            //KasrHa Props
            table.Columns.Add("AbBahaDiscount", typeof(decimal));
            table.Columns.Add("HotSeasonDiscount", typeof(decimal));
            table.Columns.Add("HotSeasonFazelabDiscount", typeof(decimal));
            table.Columns.Add("FazelabDiscount", typeof(decimal));
            table.Columns.Add("AbonmanAbDiscount", typeof(decimal));
            table.Columns.Add("AbonmanFazelabDiscount", typeof(decimal));
            table.Columns.Add("AvarezDiscount", typeof(decimal));
            table.Columns.Add("JavaniDiscount", typeof(decimal));
            table.Columns.Add("BoodjeDiscount", typeof(decimal));
            table.Columns.Add("MaliatDiscount", typeof(decimal));

            foreach (var x in input)
            {
                var row = table.NewRow();

                row["FlowImportedId"] = x.FlowImportedId;
                row["ZoneId"] = x.ZoneId;
                row["CustomerNumber"] = x.CustomerNumber;
                row["ReadingNumber"] = x.ReadingNumber;
                row["BillId"] = x.BillId;
                row["AgentCode"] = x.AgentCode;
                row["CurrentCounterStateCode"] = x.CurrentCounterStateCode;
                row["PreviousDateJalali"] = x.PreviousDateJalali;
                row["CurrentDateJalali"] = x.CurrentDateJalali;
                row["PreviousNumber"] = x.PreviousNumber;
                row["CurrentNumber"] = x.CurrentNumber;

                row["InsertByUserId"] = x.InsertByUserId;
                row["InsertDateTime"] = x.InsertDateTime;
                row["RemovedByUserId"] = x.RemovedByUserId ?? (object)DBNull.Value;
                row["RemovedDateTime"] = x.RemovedDateTime ?? (object)DBNull.Value;
                row["ExcludedByUserId"] = x.ExcludedByUserId ?? (object)DBNull.Value;
                row["ExcludedDateTime"] = x.ExcludedDateTime ?? (object)DBNull.Value;

                row["BranchTypeId"] = x.BranchTypeId;
                row["UsageId"] = x.UsageId;
                row["ConsumptionUsageId"] = x.ConsumptionUsageId;
                row["DomesticUnit"] = x.DomesticUnit;
                row["CommercialUnit"] = x.CommercialUnit;
                row["OtherUnit"] = x.OtherUnit;
                row["EmptyUnit"] = x.EmptyUnit;
                row["WaterInstallationDateJalali"] = x.WaterInstallationDateJalali;
                row["SewageInstallationDateJalali"] = x.SewageInstallationDateJalali;
                row["WaterRegisterDate"] = x.WaterRegisterDate;
                row["SewageRegisterDate"] = x.SewageRegisterDate;
                row["WaterCount"] = x.WaterCount;
                row["SewageCalcState"] = x.SewageCalcState;
                row["ContractualCapacity"] = x.ContractualCapacity;
                row["HouseholdNumber"] = x.HouseholdNumber;
                row["HouseholdDate"] = x.HouseholdDate;
                row["VillageId"] = x.VillageId ?? (object)DBNull.Value;
                row["IsSpecial"] = x.IsSpecial;
                row["MeterDiameterId"] = x.MeterDiameterId;
                row["VirtualCategoryId"] = x.VirtualCategoryId;

                row["TavizDateJalali"] = x.TavizDateJalali ?? (object)DBNull.Value;
                row["TavizCause"] = x.TavizCause ?? (object)DBNull.Value;
                row["TavizRegisterDateJalali"] = x.TavizRegisterDateJalali ?? (object)DBNull.Value;
                row["TavizNumber"] = x.TavizNumber ?? (object)DBNull.Value;

                row["LastMeterDateJalali"] = x.LastMeterDateJalali;
                row["LastMeterNumber"] = x.LastMeterNumber;
                row["LastConsumption"] = x.LastConsumption;
                row["LastMonthlyConsumption"] = x.LastMonthlyConsumption;
                row["LastCounterStateCode"] = x.LastCounterStateCode;
                row["LastSumItems"] = x.LastSumItems;

                row["SumItems"] = x.SumItems ?? (object)DBNull.Value;
                row["SumItemsBeforeDiscount"] = x.SumItemsBeforeDiscount ?? (object)DBNull.Value;
                row["DiscountSum"] = x.DiscountSum ?? (object)DBNull.Value;
                row["Consumption"] = x.Consumption ?? (object)DBNull.Value;
                row["MonthlyConsumption"] = x.MonthlyConsumption ?? (object)DBNull.Value;

                //BedBesProps
                row["barge"] = x.Barge;
                row["pri_no"] = x.PriNo;
                row["today_no"] = x.TodayNo;
                row["pri_date"] = x.PriDate;
                row["today_date"] = x.TodayDate;
                row["abon_fas"] = x.AbonFas;
                row["fas_baha"] = x.FasBaha;
                row["ab_baha"] = x.AbBaha;
                row["ztadil"] = x.Ztadil;
                row["masraf"] = x.Masraf;
                row["shahrdari"] = x.Shahrdari;
                row["modat"] = x.Modat;
                row["date_bed"] = x.DateBed;
                row["jalase_no"] = x.JalaseNo;
                row["mohlat"] = x.Mohlat;
                row["baha"] = x.Baha;
                row["abon_ab"] = x.AbonAb;
                row["pard"] = x.Pard;
                row["jam"] = x.Jam;
                row["cod_vas"] = x.CodVas;
                row["ghabs"] = x.Ghabs;
                row["del"] = x.Del;
                row["type"] = x.Type;
                row["cod_enshab"] = x.CodEnshab;
                row["enshab"] = x.Enshab;
                row["elat"] = x.Elat;
                row["serial"] = x.Serial;
                row["ser"] = x.Ser;
                row["zaribfasl"] = x.ZaribFasl;
                row["ab_10"] = x.Ab10;
                row["ab_20"] = x.Ab20;
                row["tedad_vahd"] = x.TedadVahd;
                row["ted_khane"] = x.TedKhane;
                row["tedad_mas"] = x.TedadMas;
                row["tedad_tej"] = x.TedadTej;
                row["noe_va"] = x.NoeVa;
                row["jarime"] = x.Jarime;
                row["masjar"] = x.Masjar;
                row["sabt"] = x.Sabt;
                row["rate"] = x.Rate;
                row["operator"] = x.Operator;
                row["mamor"] = x.Mamor;
                row["taviz_date"] = x.TavizDate;
                row["zarib_cntr"] = x.ZaribCntr;
                row["zabresani"] = x.Zabresani;
                row["zarib_d"] = x.ZaribD;
                row["kasr_ha"] = x.KasrHa;
                row["fix_mas"] = x.FixMas;
                row["sh_ghabs1"] = x.ShGhabs1;
                row["sh_pard1"] = x.ShPard1;
                row["TAB_ABN_A"] = x.TabAbnA;
                row["TAB_ABN_F"] = x.TabAbnF;
                row["TABS_FA"] = x.TabsFa;
                row["NEWAB"] = x.NewAb;
                row["NEWFA"] = x.NewFa;
                row["BODJEH"] = x.Bodjeh;
                row["group1"] = x.Group1;
                row["MAS_FAS"] = x.MasFas;
                row["FAZ"] = x.Faz;
                row["CHK_KARBARI"] = x.ChkKarbari;
                row["C200"] = x.C200;
                row["Ab_sevom"] = x.AbSevom;
                row["Ab_sevom1"] = x.AbSevom1;
                row["C70"] = x.C70;
                row["C80"] = x.C80;
                row["C90"] = x.C90;
                row["C101"] = x.C101;
                row["Khali_s"] = x.KhaliS;
                row["edareh_k"] = x.EdarehK;
                row["Avarez"] = x.Avarez;
                row["tafavot"] = x.Tafavot;

                //KasrHa Props
                row["AbBahaDiscount"] = x.AbBahaDiscount;
                row["HotSeasonFazelabDiscount"] = x.HotSeasonFazelabDiscount;
                row["HotSeasonDiscount"] = x.HotSeasonDiscount;
                row["FazelabDiscount"] = x.FazelabDiscount;
                row["AbonmanAbDiscount"] = x.AbonmanAbDiscount;
                row["AbonmanFazelabDiscount"] = x.AbonmanFazelabDiscount;
                row["AvarezDiscount"] = x.AvarezDiscount;
                row["JavaniDiscount"] = x.JavaniDiscount;
                row["BoodjeDiscount"] = x.BoodjeDiscount;
                row["MaliatDiscount"] = x.MaliatDiscount;

                table.Rows.Add(row);
            }
            return table;
        }
        public DataTable ToDataTable(IEnumerable<MeterReadingWithAbBahaResultUpdateDto> items)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("SumItems", typeof(double));
            dt.Columns.Add("SumItemsBeforeDiscount", typeof(double));
            dt.Columns.Add("DiscountSum", typeof(double));
            dt.Columns.Add("Consumption", typeof(double));
            dt.Columns.Add("MonthlyConsumption", typeof(double));

            foreach (var item in items)
            {
                dt.Rows.Add(
                    item.Id,
                    item.SumItems,
                    item.SumItemsBeforeDiscount,
                    item.DiscountSum,
                    item.Consumption,
                    item.MonthlyConsumption
                );
            }

            return dt;
        }

        private string GetInsertCommand()
        {
            return $@"INSERT INTO [Atlas].dbo.MeterReadingDetail (
                        FlowImportedId, ZoneId, CustomerNumber, ReadingNumber, BillId, AgentCode,
                        CurrentCounterStateCode, PreviousDateJalali, CurrentDateJalali, PreviousNumber, CurrentNumber, ExcludedByUserId,
                        ExcludedDateTime, InsertByUserId, InsertDateTime, RemovedByUserId, RemovedDateTime, BranchTypeId,
                        UsageId, ConsumptionUsageId, DomesticUnit, CommercialUnit, OtherUnit, EmptyUnit,
                        WaterInstallationDateJalali, SewageInstallationDateJalali, WaterRegisterDate, SewageRegisterDate, WaterCount, SewageCalcState,
                        ContractualCapacity, HouseholdNumber, HouseholdDate, VillageId, IsSpecial, MeterDiameterId,
                        VirtualCategoryId, BodySerial, TavizDateJalali, TavizCause, TavizRegisterDateJalali, TavizNumber,
                        LastMeterDateJalali, LastMeterNumber, LastMonthlyConsumption, LastConsumption, LastCounterStateCode, LastSumItems,
                        SumItems, SumItemsBeforeDiscount, DiscountSum, Consumption, MonthlyConsumption,
                        
                        barge, pri_no, today_no, pri_date, today_date, abon_fas,
                        fas_baha, ab_baha, ztadil, masraf, shahrdari, modat,
                        date_bed, jalase_no, mohlat, baha, abon_ab, pard,
                        jam, cod_vas, ghabs, del, type, cod_enshab,
                        enshab, elat, serial, ser, zaribfasl, ab_10,
                        ab_20, tedad_vahd, ted_khane, tedad_mas, tedad_tej, noe_va,
                        jarime, masjar, sabt, rate, operator, mamor,
                        taviz_date, zarib_cntr, zabresani, zarib_d, tafavot, kasr_ha,
                        fix_mas, sh_ghabs1, sh_pard1, TAB_ABN_A, TAB_ABN_F, TABS_FA,
                        NEWAB, NEWFA, BODJEH, group1, MAS_FAS, FAZ,
                        CHK_KARBARI, C200, Ab_sevom, Ab_sevom1, C70, C80,
                        C90, C101, Khali_s, edareh_k, Avarez,

                        AbBahaDiscount,HotSeasonDiscount,HotSeasonFazelabDiscount,FazelabDiscount,AbonmanAbDiscount,
                        AbonmanFazelabDiscount,AvarezDiscount,JavaniDiscount,BoodjeDiscount,MaliatDiscount

                    ) VALUES (
                        @FlowImportedId, @ZoneId, @CustomerNumber, @ReadingNumber, @BillId, @AgentCode,
                        @CurrentCounterStateCode, @PreviousDateJalali, @CurrentDateJalali, @PreviousNumber, @CurrentNumber, @ExcludedByUserId,
                        @ExcludedDateTime, @InsertByUserId, @InsertDateTime, @RemovedByUserId, @RemovedDateTime, @BranchTypeId,
                        @UsageId, @ConsumptionUsageId, @DomesticUnit, @CommercialUnit, @OtherUnit, @EmptyUnit,
                        @WaterInstallationDateJalali, @SewageInstallationDateJalali, @WaterRegisterDate, @SewageRegisterDate, @WaterCount, @SewageCalcState,
                        @ContractualCapacity, @HouseholdNumber, @HouseholdDate, @VillageId, @IsSpecial, @MeterDiameterId,
                        @VirtualCategoryId, @BodySerial, @TavizDateJalali, @TavizCause, @TavizRegisterDateJalali, @TavizNumber,
                        @LastMeterDateJalali, @LastMeterNumber, @LastMonthlyConsumption, @LastConsumption, @LastCounterStateCode, @LastSumItems,
                        @SumItems, @SumItemsBeforeDiscount, @DiscountSum, @Consumption, @MonthlyConsumption,
                        
                        @Barge, @PriNo, @TodayNo, @PriDate, @TodayDate, @AbonFas,
                        @FasBaha, @AbBaha, @Ztadil, @Masraf, @Shahrdari, @Modat,
                        @DateBed, @JalaseNo, @Mohlat, @Baha, @AbonAb, @Pard,
                        @Jam, @CodVas, @Ghabs, @Del, @Type, @CodEnshab,
                        @Enshab, @Elat, @Serial, @Ser, @ZaribFasl, @Ab10,
                        @Ab20, @TedadVahd, @TedKhane, @TedadMas, @TedadTej, @NoeVa,
                        @Jarime, @Masjar, @Sabt, @Rate, @Operator, @Mamor,
                        @TavizDate, @ZaribCntr, @Zabresani, @ZaribD, @Tafavot, @KasrHa,
                        @FixMas, @ShGhabs1, @ShPard1, @TabAbnA, @TabAbnF, @TabsFa,
                        @NewAb, @NewFa, @Bodjeh, @Group1, @MasFas, @Faz,
                        @ChkKarbari, @C200, @AbSevom, @AbSevom1, @C70, @C80,
                        @C90, @C101, @KhaliS, @EdarehK, @Avarez,

                        @AbBahaDiscount,@HotSeasonDiscount,@HotSeasonFazelabDiscount,@FazelabDiscount,@AbonmanAbDiscount,
                        @AbonmanFazelabDiscount,@AvarezDiscount,@JavaniDiscount,@BoodjeDiscount,@MaliatDiscount
                    )";
        }
        private string GetDeleteCommands()
        {
            return @"Update Atlas.dbo.MeterReadingDetail	
                    Set 
                    	RemovedByUserId=@RemovedByUserId ,
                    	RemovedDateTime=@RemovedDateTime
                    Where Id=@Id";
        }
        private string GetCreateDuplicateForLogCommand()
        {
            return @"INSERT INTO atlas.dbo.MeterReadingDetail
                    (
                        FlowImportedId,
                        ZoneId,
                        CustomerNumber,
                        ReadingNumber,
                        BillId,
                        AgentCode,
                        CurrentCounterStateCode,
                        PreviousDateJalali,
                        CurrentDateJalali,
                        PreviousNumber,
                        CurrentNumber,
                    
                        ExcludedByUserId,
                        ExcludedDateTime,
                    
                        InsertByUserId,
                        InsertDateTime,
                        RemovedByUserId,
                        RemovedDateTime,
                    
                        BranchTypeId,
                        UsageId,
						ConsumptionUsageId,
                        DomesticUnit,
                        CommercialUnit,
                        OtherUnit,
                        EmptyUnit,
                        WaterInstallationDateJalali,
                        SewageInstallationDateJalali,
                        WaterRegisterDate,
                        SewageRegisterDate,
                        WaterCount,
                        SewageCalcState,
                        ContractualCapacity,
                        HouseholdNumber,
                        HouseholdDate,
                        VillageId,
                        IsSpecial,
                        MeterDiameterId,
                        VirtualCategoryId,
                    
                        TavizDateJalali,
                        TavizCause,
                        TavizRegisterDateJalali,
                        TavizNumber,
                    
                        LastMeterDateJalali,
                        LastMeterNumber,
                        lastMonthlyConsumption,
                        lastConsumption,
                        LastCounterStateCode,
                        LastSumItems,                     

                        SumItems,
                        SumItemsBeforeDiscount,
                        DiscountSum,
                        Consumption,
                        MonthlyConsumption
                    )
                    SELECT
                        FlowImportedId,
                        ZoneId,
                        CustomerNumber,
                        ReadingNumber,
                        BillId,
                        AgentCode,
                        @CurrentCounterStateCode,
                        PreviousDateJalali,
                        @CurrentDateJalali,
                        PreviousNumber,
                        @CurrentNumber,
                    
                        ExcludedByUserId,
                        ExcludedDateTime,
                    
                        @InsertByUserId,      
                        @InsertDateTime,      
                        Null,
                        NUll,
                    
                        BranchTypeId,
                        UsageId,
						ConsumptionUsageId,
                        DomesticUnit,
                        CommercialUnit,
                        OtherUnit,
                        EmptyUnit,
                        WaterInstallationDateJalali,
                        SewageInstallationDateJalali,
                        WaterRegisterDate,
                        SewageRegisterDate,
                        WaterCount,
                        SewageCalcState,
                        ContractualCapacity,
                        HouseholdNumber,
                        HouseholdDate,
                        VillageId,
                        IsSpecial,
                        MeterDiameterId,
                        VirtualCategoryId,
                    
                        TavizDateJalali,
                        TavizCause,
                        TavizRegisterDateJalali,
                        TavizNumber,
                    
                        LastMeterDateJalali,
                        LastMeterNumber,
                        lastMonthlyConsumption,
                        lastConsumption,
                        LastCounterStateCode,
                        LastSumItems,

                        @SumItems,
                        @SumItemsBeforeDiscount,
                        @DiscountSum,
                        @Consumption,
                        @MonthlyConsumption
                    FROM atlas.dbo.MeterReadingDetail
                    WHERE Id = @Id;";
        }
        private string GetExcludeCommand()
        {
            return @"Update Atlas.dbo.MeterReadingDetail	
                    Set 
                    	ExcludedByUserId=@ExcludedByUserId ,
                    	ExcludedDateTime=@ExcludedDateTime
                    Where Id=@Id";
        }
    }
}