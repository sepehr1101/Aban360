using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Persistence.Features.WaterReturn.Command.Contracts;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.WaterReturn.Command.Implementations
{
    internal sealed class AutoBackCommandService : AbstractBaseConnection, IAutoBackCommandService
    {
        public AutoBackCommandService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task Create(AutoBackCreateDto input)
        {
            //string dbName = GetDbName((int)input.Town);
            string dbName = "Atlas";
            string query = GetCreateQuery(dbName);

            await _sqlReportConnection.ExecuteScalarAsync(query, input);
        }
        public async Task Create(IEnumerable<AutoBackCreateDto> input)
        {
            //string dbName = GetDbName((int)input.First().Town);
            string dbName = "Atlas";
            string query = GetCreateQuery(dbName);

            await _sqlReportConnection.ExecuteAsync(query, input);
        }
        private string GetCreateQuery(string dbName)
        {
            return @"INSERT INTO Atlas.dbo.[autoback]
                    (
                        town, radif, eshtrak, barge, pri_no, today_no, pri_date, today_date,
                        abon_fas, fas_baha, ab_baha, ztadil, masraf, shahrdari, modat, date_bed,
                        jalase_no, mohlat, baha, abon_ab, pard, jam, cod_vas, ghabs, del, type,
                        cod_enshab, enshab, elat, serial, ser, zaribfasl, ab_10, ab_20,
                        tedad_vahd, ted_khane, tedad_mas, tedad_tej, noe_va, jarime, masjar,
                        sabt, rate, operator, mamor, taviz_date, zarib_cntr, zabresani,
                        zarib_d, tafavot, mas_hadar, ab_hadar, range_mas, taf_back, ted_ghabs,
                        TAB_ABN_A, TAB_ABN_F, TABS_FA, bodjeh, FAZ,
                        tmp_pri_date, tmp_today_date, tmp_date_bed, tmp_mohlat, tmp_taviz_date
                    )
                    VALUES
                    (   
                        @Town, @Radif, @Eshtrak, @Barge, @PriNo, @TodayNo, @PriDate, @TodayDate,
                        @AbonFas, @FasBaha, @AbBaha, @Ztadil, @Masraf, @Shahrdari, @Modat, @DateBed,
                        @JalaseNo, @Mohlat, @Baha, @AbonAb, @Pard, @Jam, @CodVas, @Ghabs, @Del, @Type,
                        @CodEnshab, @Enshab, @Elat, @Serial, @Ser, @ZaribFasl, @Ab10, @Ab20,
                        @TedadVahd, @TedKhane, @TedadMas, @TedadTej, @NoeVa, @Jarime, @Masjar,
                        @Sabt, @Rate, @Operator, @Mamor, @TavizDate, @ZaribCntr, @Zabresani,
                        @ZaribD, @Tafavot, @MasHadar, @AbHadar, @RangeMas, @TafBack, @TedGhabs,
                        @TabAbnA, @TabAbnF, @TabsFa, @Bodjeh, @Faz,
                        @TmpPriDate, @TmpTodayDate, @TmpDateBed, @TmpMohlat, @TmpTavizDate
                    );";
        }
    }
}