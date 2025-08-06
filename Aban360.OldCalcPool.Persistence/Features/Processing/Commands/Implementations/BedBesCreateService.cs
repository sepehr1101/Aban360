using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations
{
    internal sealed class BedBesCreateService : AbstractBaseConnection, IBedBesCreateService
    {
        public BedBesCreateService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task Create(BedBesCreateDto input)
        {
            string BedBesCreateQueryString = GetBedBesCreateQuery();
            var @params = new
            {
                town = input.Town,
                radif = input.Radif,
                eshtrak = input.Eshtrak,
                barge = input.Barge,
                pri_no = input.PriNo,
                today_no = input.TodayNo,
                pri_date = input.PriDate,
                today_date = input.TodayDate,
                abon_fas = input.AbonFas,
                fas_baha = input.FasBaha,
                ab_baha = input.AbBaha,
                ztadil = input.Ztadil,
                masraf = input.Masraf,
                shahrdari = input.Shahrdari,
                modat = input.Modat,
                date_bed = input.DateBed,
                jalase_no = input.JalaseNo,
                mohlat = input.Mohlat,
                baha = input.Baha,
                abon_ab = input.AbonAb,
                pard = input.Pard,
                jam = input.Jam,
                cod_vas = input.CodVas,
                ghabs = input.Ghabs,
                del = input.Del,
                type = input.Type,
                cod_enshab = input.CodEnshab,
                enshab = input.Enshab,
                elat = input.Elat,
                serial = input.Serial,
                ser = input.Ser,
                zaribfasl = input.ZaribFasl,
                ab_10 = input.Ab10,
                ab_20 = input.Ab20,
                tedad_vahd = input.TedadVahd,
                ted_khane = input.TedKhane,
                tedad_mas = input.TedadMas,
                tedad_tej = input.TedadTej,
                noe_va = input.NoeVa,
                jarime = input.Jarime,
                masjar = input.Masjar,
                sabt = input.Sabt,
                rate = input.Rate,
                @operator = input.Operator,
                mamor = input.Mamor,
                taviz_date = input.TavizDate,
                zarib_cntr = input.ZaribCntr,
                zabresani = input.Zabresani,
                zarib_d = input.ZaribD,
                tafavot = input.Tafavot,
                kasr_ha = input.KasrHa,
                fix_mas = input.FixMas,
                sh_ghabs1 = input.ShGhabs1,
                sh_pard1 = input.ShPard1,
                TAB_ABN_A = input.TabAbnA,
                TAB_ABN_F = input.TabAbnF,
                TABS_FA = input.TabsFa,
                NEWAB = input.NewAb,
                NEWFA = input.NewFa,
                bodjeh = input.Bodjeh,
                group1 = input.Group1,
                MAS_FAS = input.MasFas,
                FAZ = input.Faz,
                CHK_KARBARI = input.ChkKarbari,
                input.C200,
                DATEINS = input.DateIns,
                Ab_sevom = input.AbSevom,
                Ab_sevom1 = input.AbSevom1,
                input.C70,
                input.C80,
                tmp_date_bed = input.TmpDateBed,
                tmp_pri_date = input.TmpPriDate,
                tmp_today_date = input.TmpTodayDate,
                tmp_mohlat = input.TmpMohlat,
                tmp_taviz_date = input.TmpTavizDate,
                input.C90,
                input.C101,
                Khali_s = input.KhaliS,
                edareh_k = input.EdarehK,
                TAFA_402 = input.Tafa402,
                input.Avarez,
                input.TrackNumber
            };

            await _sqlReportConnection.ExecuteAsync(BedBesCreateQueryString, @params);
        }

        private string GetBedBesCreateQuery()
        {
            return @$"USE [OldCalc]
                    INSERT INTO bed_bes(
                        town, radif, eshtrak, barge, pri_no, today_no, pri_date, today_date,
                        abon_fas, fas_baha, ab_baha, ztadil, masraf, shahrdari, modat, date_bed,
                        jalase_no, mohlat, baha, abon_ab, pard, jam, cod_vas, ghabs, del, [type],
                        cod_enshab, enshab, elat, serial, ser, zaribfasl, ab_10, ab_20, tedad_vahd,
                        ted_khane, tedad_mas, tedad_tej, noe_va, jarime, masjar, sabt, rate,
                        operator, mamor, taviz_date, zarib_cntr, zabresani, zarib_d, tafavot,
                        kasr_ha, fix_mas, sh_ghabs1, sh_pard1, TAB_ABN_A, TAB_ABN_F, TABS_FA,
                        NEWAB, NEWFA, bodjeh, group1, MAS_FAS, FAZ, CHK_KARBARI, C200, DATEINS,
                        Ab_sevom, Ab_sevom1, Khali_s, edareh_k, TAFA_402,
                        Avarez, TrackNumber
                    )
                    VALUES (
                        @town, @radif, @eshtrak, @barge, @pri_no, @today_no, @pri_date, @today_date,
                        @abon_fas, @fas_baha, @ab_baha, @ztadil, @masraf, @shahrdari, @modat, @date_bed,
                        @jalase_no, @mohlat, @baha, @abon_ab, @pard, @jam, @cod_vas, @ghabs, @del, @type,
                        @cod_enshab, @enshab, @elat, @serial, @ser, @zaribfasl, @ab_10, @ab_20, @tedad_vahd,
                        @ted_khane, @tedad_mas, @tedad_tej, @noe_va, @jarime, @masjar, @sabt, @rate,
                        @operator, @mamor, @taviz_date, @zarib_cntr, @zabresani, @zarib_d, @tafavot,
                        @kasr_ha, @fix_mas, @sh_ghabs1, @sh_pard1, @TAB_ABN_A, @TAB_ABN_F, @TABS_FA,
                        @NEWAB, @NEWFA, @bodjeh, @group1, @MAS_FAS, @FAZ, @CHK_KARBARI, @C200, @DATEINS,
                        @Ab_sevom, @Ab_sevom1,@Khali_s, @edareh_k, @TAFA_402,
                        @Avarez, @TrackNumber
                    )";
        }
    }
}
