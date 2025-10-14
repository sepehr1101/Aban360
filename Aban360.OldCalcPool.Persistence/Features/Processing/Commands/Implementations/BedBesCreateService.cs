using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Contracts;
using Dapper;
using LiteDB;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

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
                prino = input.PriNo,
                todayno = input.TodayNo,
                pridate = input.PriDate,
                todaydate = input.TodayDate,
                abonfas = input.AbonFas,
                fasbaha = input.FasBaha,
                abbaha = input.AbBaha,
                ztadil = input.Ztadil,
                masraf = input.Masraf,
                shahrdari = input.Shahrdari,
                modat = input.Modat,
                datebed = input.DateBed,
                jalaseno = input.JalaseNo,
                mohlat = input.Mohlat,
                baha = input.Baha,
                abonab = input.AbonAb,
                pard = input.Pard,
                jam = input.Jam,
                codvas = input.CodVas,
                ghabs = input.Ghabs,
                del = input.Del,
                type = input.Type,
                cod_enshab = input.CodEnshab,
                enshab = input.Enshab,
                elat = input.Elat,
                serial = input.Serial,
                ser = input.Ser,
                zaribfasl = input.ZaribFasl,
                ab10 = input.Ab10,
                ab20 = input.Ab20,
                tedadvahd = input.TedadVahd,
                tedkhane = input.TedKhane,
                tedadmas = input.TedadMas,
                tedadtej = input.TedadTej,
                noeva = input.NoeVa,
                jarime = input.Jarime,
                masjar = input.Masjar,
                sabt = input.Sabt,
                rate = input.Rate,
                @operator = input.Operator,
                mamor = input.Mamor,
                tavizdate = input.TavizDate,
                zaribcntr = input.ZaribCntr,
                zabresani = input.Zabresani,
                zaribd = input.ZaribD,
                tafavot = input.Tafavot,
                kasrha = input.KasrHa,
                fixmas = input.FixMas,
                shghabs1 = input.ShGhabs1,
                shpard1 = input.ShPard1,
                TABABNA = input.TabAbnA,
                TABABNF = input.TabAbnF,
                TABSFA = input.TabsFa,
                NEWAB = input.NewAb,
                NEWFA = input.NewFa,
                bodjeh = input.Bodjeh,
                group1 = input.Group1,
                MAS_FAS = input.MasFas,
                FAZ = input.Faz,
                CHK_KARBARI = input.ChkKarbari,
                input.C200,
                DATEINS = input.DateIns,
                Absevom = input.AbSevom,
                Absevom1 = input.AbSevom1,
                input.C70,
                input.C80,
                tmpdatebed = input.TmpDateBed,
                tmppridate = input.TmpPriDate,
                tmptodaydate = input.TmpTodayDate,
                tmpmohlat = input.TmpMohlat,
                tmptavizdate = input.TmpTavizDate,
                input.C90,
                input.C101,
                Khali_s = input.KhaliS,
                edarehk = input.EdarehK,
                TAFA402 = input.Tafa402,
                input.Avarez,
                input.TrackNumber
            };

            await _sqlReportConnection.ExecuteAsync(BedBesCreateQueryString, @params);
        }

        public async Task Create(ICollection<BedBesCreateDto> input)
        {
            using (var connection = _sqlReportConnection)
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await connection.ExecuteAsync(GetBedBesCreateQuery(), input, transaction: transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("خطا: " + ex.Message);
                        throw;
                    }
                }
            }
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
                        @town, @radif, @eshtrak, @barge, @prino, @todayno, @pridate, @todaydate,
                        @abonfas, @fasbaha, @abbaha, @ztadil, @masraf, @shahrdari, @modat, @datebed,
                        @jalaseno, @mohlat, @baha, @abonab, @pard, @jam, @codvas, @ghabs, @del, @type,
                        @codenshab, @enshab, @elat, @serial, @ser, @zaribfasl, @ab10, @ab20, @tedadvahd,
                        @tedkhane, @tedadmas, @tedadtej, @noeva, @jarime, @masjar, @sabt, @rate,
                        @operator, @mamor, @tavizdate, @zaribcntr, @zabresani, @zaribd, @tafavot,
                        @kasrha, @fixmas, @shghabs1, @shpard1, @TABABNA, @TABABNF, @TABSFA,
                        @NEWAB, @NEWFA, @bodjeh, @group1, @MASFAS, @FAZ, @CHKKARBARI, @C200, @DATEINS,
                        @Absevom, @Absevom1, @Khalis, @edarehk, @TAFA402,
                        @Avarez, @TrackNumber
                    )";
        }
    }
}
