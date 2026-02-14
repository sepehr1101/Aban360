using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Contracts;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations
{
    public sealed class KasrHaCommandService : IKasrHaCommandService
    {
        private readonly SqlConnection _connection;
        private readonly IDbTransaction _transaction;
        private static string tableName = "KasrHa";
        //public KasrHaService(IConfiguration configuration)
        //    : base(configuration)
        //{
        //}
        public KasrHaCommandService(
                SqlConnection connection,
                IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task Create(KasrHaDto input, int zoneId)
        {
            //string dbName = GetDbName(zoneId);
            string dbName = "Atlas";
            string query = GetCreateQuery(dbName);

            await _connection.ExecuteAsync(query, input);
        }
        public async Task Create(ICollection<KasrHaDto> input)
        {
            //string dbName = GetDbName((int)input.FirstOrDefault().Town);
            string dbName = "Atlas";

            using (var connection = _connection)
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await connection.ExecuteAsync(GetCreateQuery(dbName), input, transaction: transaction);
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
        public async Task Create(ICollection<KasrHaDto> input, int zoneId)
        {
            //var dbName=GetDbName(zoneId);   
            var dbName = "Atlas";
            DataTable table = GetDataTable(input);

            using var connection = _connection;
            await connection.OpenAsync();

            using var bulk = new SqlBulkCopy(connection)
            {
                DestinationTableName = $"[{dbName}].dbo.kasr_ha",
                BatchSize = 5000,
                BulkCopyTimeout = 0
            };

            foreach (DataColumn col in table.Columns)
                bulk.ColumnMappings.Add(col.ColumnName, col.ColumnName);

            await bulk.WriteToServerAsync(table);
        }
        public async Task Delete(RemoveBillDataInputDto input)
        {
            string command = GetDeleteCommand();
            await _connection.ExecuteAsync(command, input);
        }

        private DataTable GetDataTable(ICollection<KasrHaDto> input)
        {
            DataTable table = new DataTable();

            table.Columns.Add("TOWN", typeof(decimal));
            table.Columns.Add("Id_bedbes", typeof(decimal));
            table.Columns.Add("radif", typeof(decimal));
            table.Columns.Add("cod_enshab", typeof(decimal));
            table.Columns.Add("barge", typeof(decimal));
            table.Columns.Add("pri_date", typeof(string));
            table.Columns.Add("today_date", typeof(string));
            table.Columns.Add("pri_no", typeof(decimal));
            table.Columns.Add("today_no", typeof(decimal));
            table.Columns.Add("masraf", typeof(decimal));
            table.Columns.Add("ab_baha", typeof(decimal));
            table.Columns.Add("fas_baha", typeof(decimal));
            table.Columns.Add("abon_ab", typeof(decimal));
            table.Columns.Add("abon_fas", typeof(decimal));
            table.Columns.Add("TAB_ABN_A", typeof(decimal));
            table.Columns.Add("TAB_ABN_F", typeof(decimal));
            table.Columns.Add("ab_10", typeof(decimal));
            table.Columns.Add("shahrdari", typeof(decimal));
            table.Columns.Add("rate", typeof(decimal));
            table.Columns.Add("baha", typeof(decimal));
            table.Columns.Add("SH_GHABS", typeof(string));
            table.Columns.Add("SH_PARD", typeof(string));
            table.Columns.Add("date_bed", typeof(string));
            table.Columns.Add("tmp_date_bed", typeof(string));
            table.Columns.Add("tmp_today_date", typeof(string));
            table.Columns.Add("ted_vahd", typeof(int));
            table.Columns.Add("ted_khane", typeof(int));
            table.Columns.Add("tedad_mas", typeof(int));
            table.Columns.Add("tedad_tej", typeof(int));
            table.Columns.Add("ZARIBFASL", typeof(decimal));
            table.Columns.Add("NOE_VA", typeof(decimal));
            table.Columns.Add("bodjeh", typeof(decimal));
            table.Columns.Add("del", typeof(decimal));
            table.Columns.Add("baha2", typeof(decimal));
            table.Columns.Add("ab_baha2", typeof(decimal));

            foreach (var item in input)
            {
                var row = table.NewRow();

                row["TOWN"] = item.Town;
                row["Id_bedbes"] = item.IdBedbes;
                row["radif"] = item.Radif;
                row["cod_enshab"] = item.CodEnshab;
                row["barge"] = item.Barge;
                row["pri_date"] = item.PriDate ?? (object)DBNull.Value;
                row["today_date"] = item.TodayDate ?? (object)DBNull.Value;
                row["pri_no"] = item.PriNo;
                row["today_no"] = item.TodayNo;
                row["masraf"] = item.Masraf;
                row["ab_baha"] = item.AbBaha;
                row["fas_baha"] = item.FasBaha;
                row["abon_ab"] = item.AbonAb;
                row["abon_fas"] = item.AbonFas;
                row["TAB_ABN_A"] = item.TabAbnA;
                row["TAB_ABN_F"] = item.TabAbnF;
                row["ab_10"] = item.Ab10;
                row["shahrdari"] = item.Shahrdari;
                row["rate"] = item.Rate;
                row["baha"] = item.Baha;
                row["SH_PARD"] = item.ShPard ?? (object)DBNull.Value;
                row["SH_GHABS"] = item.ShGhabs ?? (object)DBNull.Value;
                row["date_bed"] = item.DateBed ?? (object)DBNull.Value;
                row["tmp_date_bed"] = item.TmpDateBed ?? (object)DBNull.Value;
                row["tmp_today_date"] = item.TmpTodayDate ?? (object)DBNull.Value;
                row["ted_vahd"] = item.TedVahd;
                row["tedad_tej"] = item.TedadTej;
                row["ted_khane"] = item.TedKhane;
                row["tedad_mas"] = item.TedadMas;
                row["ZARIBFASL"] = item.ZaribFasl;
                row["NOE_VA"] = item.NoeVa;
                row["bodjeh"] = item.Bodjeh;
                row["del"] = 0;
                row["baha2"] = 0;
                row["ab_baha2"] = 0;

                table.Rows.Add(row);
            }
            return table;
        }
        private string GetCreateQuery(string dbName)
        {
            return @$"USE [{dbName}]
                    INSERT INTO kasr_ha (
                        TOWN, Id_bedbes, radif,
                        cod_enshab, barge, pri_date, today_date,
                        pri_no, today_no, masraf, ab_baha,
                        fas_baha, abon_ab, abon_fas, TAB_ABN_A,
                        TAB_ABN_F, ab_10, shahrdari, rate,
                        baha, SH_GHABS, SH_PARD, date_bed,
                        tmp_date_bed, tmp_today_date, ted_vahd, tedad_tej,
                        ted_khane, tedad_mas, ZARIBFASL, NOE_VA,
                        bodjeh
                    )
                    VALUES (
                        @Town, @IdBedbes, @Radif,
                        @CodEnshab, @Barge, @PriDate, @TodayDate,
                        @PriNo, @TodayNo, @Masraf, @AbBaha,
                        @FasBaha, @AbonAb, @AbonFas, @TabAbnA,
                        @TabAbnF, @Ab10, @Shahrdari, @Rate,
                        @Baha, @ShGhabs, @ShPard, @DateBed,
                        @TmpDateBed, @TmpTodayDate, @TedVahd, @TedadTej,
                        @TedKhane, @TedadMas, @ZaribFasl, @NoeVa,
                        @Bodjeh
                    );";
        }
        private string GetDeleteCommand()
        {
            return @"Delete From Atlas.dbo.kasr_ha
                    Where 
                    	TOWN=@ZoneId AND
                    	radif=@CustomerNumber AND
                    	barge=@Barge AND
                    	Pri_date=@PrviousDateJalali AND
                    	today_date=@CurrentDateJalali AND
                    	pri_no=@PreviousNumber AND
                    	today_no=@CurrentNumber AND
                    	rate=@Consumption AND
                    	SH_GHABS=@BillId AND
                    	SH_PARD=@PaymentId AND
                    	date_bed=@RegisterDateJalali ";
        }
    }
}
