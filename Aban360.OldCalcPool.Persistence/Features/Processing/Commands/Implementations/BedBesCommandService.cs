using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Contracts;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Xml.Linq;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations
{
    internal sealed class BedBesCommandService : AbstractBaseConnection, IBedBesCommandService
    {
        private static string tableName = "BedBes";
        public BedBesCommandService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task Create(BedBesCreateDto input, int zoneId)
        {
            //string dbName=GetDbName(zoneId);
            string dbName = "Atlas";
            string BedBesCreateQueryString = GetBedBesCreateQuery(dbName);

            await _sqlReportConnection.ExecuteAsync(BedBesCreateQueryString, input);
        }
        public async Task Create(ICollection<BedBesCreateDto> input)
        {
            //string dbName = GetDbName((int)input.FirstOrDefault().Town);
            string dbName = "Atlas";

            using (var connection = _sqlReportConnection)
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await connection.ExecuteAsync(GetBedBesCreateQuery(dbName), input, transaction: transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new IWaterCalculationAddException(ExceptionLiterals.UnSuccessfulToSave(tableName));
                    }
                }

            }
        }
        public async Task Create(ICollection<BedBesCreateDto> input, int zoneId)
        {
            // string dbName=GetDbName(zoneId);
            string dbName = "Atlas";
            var dt = ToDataTable(input);

            using var connection = _sqlReportConnection;
            await connection.OpenAsync();

            using var bulk = new SqlBulkCopy(connection)
            {
                DestinationTableName = $"[{dbName}].dbo.bed_bes",
                BatchSize = 5000,
                BulkCopyTimeout = 0
            };

            foreach (DataColumn col in dt.Columns)
                bulk.ColumnMappings.Add(col.ColumnName, col.ColumnName);

            await bulk.WriteToServerAsync(dt);
        }
        public async Task Delete(int id)
        {
            string query = GetDeleteQuery();
            int rowsAffected = await _sqlReportConnection.ExecuteAsync(query, new { id });

            if (rowsAffected == 0)
            {
                throw new ReadingException(ExceptionLiterals.NotFoundBillsToRemoved);
            }
        }
        public async Task UpdateDel(IEnumerable<BedBesUpdateDelDto> input)
        {
            //string dbName = GetDbName(input.FirstOrDefault().ZoneId);
            string dbName = "Atlas";
            string command = GetUpdateDelCommand(dbName);
            await _sqlReportConnection.ExecuteAsync(command, input);
        }

        public DataTable ToDataTable(IEnumerable<BedBesCreateDto> items)
        {
            DataTable table = new DataTable();

            table.Columns.Add("id", typeof(int));
            table.Columns.Add("town", typeof(decimal));
            table.Columns.Add("radif", typeof(decimal));
            table.Columns.Add("eshtrak", typeof(string));
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
            table.Columns.Add("tmp_date_bed", typeof(string));
            table.Columns.Add("tmp_pri_date", typeof(string));
            table.Columns.Add("tmp_today_date", typeof(string));
            table.Columns.Add("tmp_mohlat", typeof(string));
            table.Columns.Add("tmp_taviz_date", typeof(string));
            table.Columns.Add("C90", typeof(decimal));
            table.Columns.Add("C101", typeof(decimal));
            table.Columns.Add("Khali_s", typeof(decimal));
            table.Columns.Add("edareh_k", typeof(bool));
            table.Columns.Add("Avarez", typeof(decimal));
            //table.Columns.Add("TrackNumber", typeof(long));

            foreach (var x in items)
            {
                var row = table.NewRow();

                row["id"] = x.Id;
                row["town"] = x.Town;
                row["radif"] = x.Radif;
                row["eshtrak"] = x.Eshtrak;
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
                row["tmp_date_bed"] = x.TmpDateBed;
                row["tmp_pri_date"] = x.TmpPriDate;
                row["tmp_today_date"] = x.TmpTodayDate;
                row["tmp_mohlat"] = x.TmpMohlat;
                row["tmp_taviz_date"] = x.TmpTavizDate;
                row["C90"] = x.C90;
                row["C101"] = x.C101;
                row["Khali_s"] = x.KhaliS;
                row["edareh_k"] = x.EdarehK;
                row["Avarez"] = x.Avarez;

                table.Rows.Add(row);

            }
            return table;
        }
        private string GetBedBesCreateQuery(string dbName)
        {
            return @$"USE [{dbName}]
                    INSERT INTO bed_bes(
                        town, radif, eshtrak, barge, pri_no, today_no, pri_date, today_date,
                        abon_fas, fas_baha, ab_baha, ztadil, masraf, shahrdari, modat, date_bed,
                        jalase_no, mohlat, baha, abon_ab, pard, jam, cod_vas, ghabs, del, [type],
                        cod_enshab, enshab, elat, serial, ser, zaribfasl, ab_10, ab_20, tedad_vahd,
                        ted_khane, tedad_mas, tedad_tej, noe_va, jarime, masjar, sabt, rate,
                        operator, mamor, taviz_date, zarib_cntr, zabresani, zarib_d, tafavot,
                        kasr_ha, fix_mas, sh_ghabs1, sh_pard1, TAB_ABN_A, TAB_ABN_F, TABS_FA,
                        NEWAB, NEWFA, bodjeh, group1, MAS_FAS, FAZ, CHK_KARBARI, C200,
                        Ab_sevom, Ab_sevom1, Khali_s, edareh_k,Avarez
                    )
                    VALUES (
                        @town, @radif, @eshtrak, @barge, @prino, @todayno, @pridate, @todaydate,
                        @abonfas, @fasbaha, @abbaha, @ztadil, @masraf, @shahrdari, @modat, @datebed,
                        @jalaseno, @mohlat, @baha, @abonab, @pard, @jam, @codvas, @ghabs, @del, @type,
                        @codenshab, @enshab, @elat, @serial, @ser, @zaribfasl, @ab10, @ab20, @tedadvahd,
                        @tedkhane, @tedadmas, @tedadtej, @noeva, @jarime, @masjar, @sabt, @rate,
                        @operator, @mamor, @tavizdate, @zaribcntr, @zabresani, @zaribd, @tafavot,
                        @kasrha, @fixmas, @shghabs1, @shpard1, @TABABNA, @TABABNF, @TABSFA,
                        @NEWAB, @NEWFA, @bodjeh, @group1, @MASFAS, @FAZ, @CHKKARBARI, @C200,
                        @Absevom, @Absevom1, @Khalis, @edarehk,@Avarez
                    )";
        }
        private string GetDeleteQuery()
        {
            return @"Delete FROM [Atlas].dbo.bed_bes
                    Where Id=@id";
        }
        private string GetUpdateDelCommand(string dbName)
        {
            return $@"Update [{dbName}].dbo.bed_bes
                    Set del=@del
                    Where id=@id";
        }
    }
}
