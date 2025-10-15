using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations
{
    internal sealed class KasrHaService : AbstractBaseConnection, IKasrHaService
    {
        private static string tableName = "KasrHa";
        public KasrHaService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task Create(ICollection<KasrHaDto> input)
        {
            using (var connection = _sqlReportConnection)
            {
                await connection.OpenAsync();

                using (var transaction=connection.BeginTransaction())
                {
                    try
                    {
                        await connection.ExecuteAsync(GetCreateQuery(), input, transaction: transaction);
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

        private string GetCreateQuery()
        {
            return @"USE [OldCalc]
                    INSERT INTO kasr_ha (
                        TOWN, Id_bedbes, radif,
                        cod_enshab, barge, pri_date, today_date,
                        pri_no, today_no, masraf, ab_baha,
                        fas_baha, abon_ab, abon_fas, TAB_ABN_A,
                        TAB_ABN_F, ab_10, shahrdari, rate,
                        baha, SH_GHABS, SH_PARD, date_bed,
                        tmp_date_bed, tmp_today_date, ted_vahd, tedad_tej,
                        ted_khane, tedad_mas, ZARIBFASL, NOE_VA,
                        bodjeh, TrackNumber
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
                        @Bodjeh, @TrackNumber
                    );";
        }
    }
}
