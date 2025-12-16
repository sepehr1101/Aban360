using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;
using Aban360.OldCalcPools.Persistence.Features.WaterReturn.Command.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;

namespace Aban360.OldCalcPools.Persistence.Features.WaterReturn.Command.Implementations
{
    internal sealed class RepairCommandService : AbstractBaseConnection, IRepairCommandService
    {
        public RepairCommandService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task Create(RepairCreateDto input)
        {
            //string dbName = GetDbName((int)input.Town);
            string atlasQuery = CreateAtlasQuery();
            //string localQuery = CreateQuery(dbName);

            await _sqlReportConnection.ExecuteScalarAsync(atlasQuery, input);
            //await _sqlReportConnection.ExecuteScalarAsync(localQuery, input);
        }
        public async Task Create(IEnumerable<RepairCreateDto> input)
        {
            string dbName = GetDbName((int)input.FirstOrDefault().Town);
            string atlasQuery = CreateAtlasQuery();
            string regionDbName= CreateQuery(dbName);

            await _sqlReportConnection.ExecuteAsync(atlasQuery, input);
        }

        public async Task Update(RepairUpdateDto input)
        {
            ZoneIdAndBargeGetDto zoneIdAndBarge = await GetZoneIdAndBarege(input.Id);
            string dbName = GetDbName(zoneIdAndBarge.ZoneId);
            string atlasQuery = UpdateAtlasQuery();
            string localQuery = UpdateQuery(dbName);

            var localQueryParameters = new DynamicParameters(input);
            localQueryParameters.Add("barge", zoneIdAndBarge.Barge);
            localQueryParameters.Add("zoneId", zoneIdAndBarge.ZoneId);

            await _sqlReportConnection.ExecuteScalarAsync(atlasQuery, input);
            await _sqlReportConnection.ExecuteScalarAsync(localQuery, localQueryParameters);
        }
        public async Task Delete(RepairDeleteDto input)
        {
            ZoneIdAndBargeGetDto zoneIdAndBarge = await GetZoneIdAndBarege(input.Id);
            string dbName = GetDbName(zoneIdAndBarge.ZoneId);
            string query = DeleteQuery(dbName);
            string atlasQuery = DeleteAtlasQuery();

            var localQueryParameters = new DynamicParameters(input);
            localQueryParameters.Add("barge", zoneIdAndBarge.Barge);
            localQueryParameters.Add("zoneId", zoneIdAndBarge.ZoneId);

            await _sqlReportConnection.ExecuteScalarAsync(atlasQuery, input);
            await _sqlReportConnection.ExecuteScalarAsync(query, localQueryParameters);
        }
        private async Task<ZoneIdAndBargeGetDto> GetZoneIdAndBarege(int id)
        {
            string query = GetZoneIdAndBargeQuery();
            ZoneIdAndBargeGetDto zoneIdAndBarge = await _sqlConnection.QueryFirstOrDefaultAsync<ZoneIdAndBargeGetDto>(query, new { id = id });

            return zoneIdAndBarge;
        }

        private string CreateAtlasQuery()
        {
            return @"USE [Atlas];
                    INSERT INTO [Atlas].dbo.REPAIR (
                        town, radif, eshtrak, barge, pri_no, today_no, pri_date, today_date,
                        abon_fas, fas_baha, ab_baha, ztadil, masraf, shahrdari, modat, date_bed,
                        jalase_no, mohlat, baha, abon_ab, pard, jam, cod_vas, ghabs, del, [type],
                        cod_enshab, enshab, elat, serial, ser, zaribfasl, ab_10, ab_20, tedad_vahd,
                        ted_khane, tedad_mas, tedad_tej, noe_va, jarime, masjar, sabt, rate,
                        operator, mamor, taviz_date, zarib_cntr, zabresani, zarib_d, tafavot,
                        mas_hadar, ab_hadar, range_mas, taf_back, ted_ghabs, TAB_ABN_A, TAB_ABN_F,
                        TABS_FA, bodjeh, group1, FAZ, CHK_KARBARI, C200, tmp_pri_date,
                        tmp_today_date, tmp_mohlat, tmp_taviz_date, tmp_date_bed, edareh_k,
                         date_sbt, Avarez
                    )
                    VALUES (
                       @Town, @Radif, @Eshtrak, @Barge, @PriNo, @TodayNo, @PriDate, @TodayDate,
                       @AbonFas, @FasBaha, @AbBaha, @Ztadil, @Masraf, @Shahrdari, @Modat, @DateBed,
                       @JalaseNo, @Mohlat, @Baha, @AbonAb, @Pard, @Jam, @CodVas, @Ghabs, @Del, @Type,
                       @CodEnshab, @Enshab, @Elat, @Serial, @Ser, @ZaribFasl, @Ab10, @Ab20, @TedadVahd,
                       @TedKhane, @TedadMas, @TedadTej, @NoeVa, @Jarime, @Masjar, @Sabt, @Rate,
                       @Operator, @Mamor, @TavizDate, @ZaribCntr, @Zabresani, @ZaribD, @Tafavot,
                       @MasHadar, @AbHadar, @RangeMas, @TafBack, @TedGhabs, @TabAbnA, @TabAbnF,
                       @TabsFa, @Bodjeh, @Group1, @Faz, @ChkKarbari, @C200, @TmpPriDate,
                       @TmpTodayDate, @TmpMohlat, @TmpTavizDate, @TmpDateBed, @EdarehK,
                       @DateSbt, @Avarez
                    );";
        }
        private string CreateQuery(string dbName)
        {
            return @$"USE [{dbName}];
                    INSERT INTO [{dbName}].dbo.REPAIR (
                        town, radif, eshtrak, barge, pri_no, today_no, pri_date, today_date,
                        abon_fas, fas_baha, ab_baha, ztadil, masraf, shahrdari, modat, date_bed,
                        jalase_no, mohlat, baha, abon_ab, pard, jam, cod_vas, ghabs, del, [type],
                        cod_enshab, enshab, elat, serial, ser, zaribfasl, ab_10, ab_20, tedad_vahd,
                        ted_khane, tedad_mas, tedad_tej, noe_va, jarime, masjar, sabt, rate,
                        operator, mamor, taviz_date, zarib_cntr, zabresani, zarib_d, tafavot,
                        mas_hadar, ab_hadar, range_mas, taf_back, ted_ghabs, TAB_ABN_A, TAB_ABN_F,
                        TABS_FA, bodjeh, group1, FAZ, CHK_KARBARI, C200, tmp_pri_date,
                        tmp_today_date, tmp_mohlat, tmp_taviz_date, tmp_date_bed, edareh_k,
                        date_sbt, Avarez
                    )
                    VALUES (
                       @Town, @Radif, @Eshtrak, @Barge, @PriNo, @TodayNo, @PriDate, @TodayDate,
                       @AbonFas, @FasBaha, @AbBaha, @Ztadil, @Masraf, @Shahrdari, @Modat, @DateBed,
                       @JalaseNo, @Mohlat, @Baha, @AbonAb, @Pard, @Jam, @CodVas, @Ghabs, @Del, @Type,
                       @CodEnshab, @Enshab, @Elat, @Serial, @Ser, @ZaribFasl, @Ab10, @Ab20, @TedadVahd,
                       @TedKhane, @TedadMas, @TedadTej, @NoeVa, @Jarime, @Masjar, @Sabt, @Rate,
                       @Operator, @Mamor, @TavizDate, @ZaribCntr, @Zabresani, @ZaribD, @Tafavot,
                       @MasHadar, @AbHadar, @RangeMas, @TafBack, @TedGhabs, @TabAbnA, @TabAbnF,
                       @TabsFa, @Bodjeh, @Group1, @Faz, @ChkKarbari, @C200, @TmpPriDate,
                       @TmpTodayDate, @TmpMohlat, @TmpTavizDate, @TmpDateBed, @EdarehK,
                       @DateSbt, @Avarez
                    );";
        }
        private string UpdateAtlasQuery()
        {
            return @"USE [Atlas];
                    UPDATE [Atlas].dbo.REPAIR
                    SET
                        abon_fas = @AbonFas, fas_baha = @FasBaha, ab_baha = @AbBaha, ztadil = @Ztadil,
                        shahrdari = @Shahrdari, date_bed = @DateBed, jalase_no = @JalaseNo, baha = @Baha,
                        abon_ab = @AbonAb, pard = @Pard, jam = @Jam, elat = @Elat,
                        zaribfasl = @ZaribFasl, ab_10 = @Ab10, ab_20 = @Ab20, zabresani = @Zabresani,
                        zarib_d = @ZaribD, TAB_ABN_A = @TabAbnA, TAB_ABN_F = @TabAbnF, TABS_FA = @TabsFa,
                        bodjeh = @Bodjeh, FAZ = @Faz, date_sbt = @DateSbt, Avarez = @Avarez
                    WHERE Id = @Id;";
        }
        private string UpdateQuery(string dbName)
        {
            return @$"USE [{dbName}];
                    UPDATE [{dbName}].REPAIR
                    SET
                        abon_fas = @AbonFas, fas_baha = @FasBaha, ab_baha = @AbBaha, ztadil = @Ztadil,
                        shahrdari = @Shahrdari, date_bed = @DateBed, jalase_no = @JalaseNo, baha = @Baha,
                        abon_ab = @AbonAb, pard = @Pard, jam = @Jam, elat = @Elat,
                        zaribfasl = @ZaribFasl, ab_10 = @Ab10, ab_20 = @Ab20, zabresani = @Zabresani,
                        zarib_d = @ZaribD, TAB_ABN_A = @TabAbnA, TAB_ABN_F = @TabAbnF, TABS_FA = @TabsFa,
                        bodjeh = @Bodjeh, FAZ = @Faz, date_sbt = @DateSbt, Avarez = @Avarez
                    WHERE 
                        town=@zoneId,   
                        barge=@barge";
        }
        private string DeleteAtlasQuery()
        {
            return @"USE [Atlas]
                    Delete From [Atlas].dbo.REPAIR
                    Where Id=@Id";
        }
        private string DeleteQuery(string dbName)
        {
            return @$"USE [{dbName}]
                    Delete From [{dbName}].dbo.REPAIR
                    Where
                        town=@zoneId AND  
                        barge=@barge";
        }
        private string GetZoneIdAndBargeQuery()
        {
            return @"Use [Atlas]
                     Select 
                         town as ZoneId,
                         barge as Barge
                     From [Atlas].dbo.REPAIR
                     Where id=@Id";
        }
    }
}
