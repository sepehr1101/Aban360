using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Implementations
{
    internal sealed class Table1CreateService : AbstractBaseConnection, ITable1CreateService
    {
        public Table1CreateService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task Create(Table1CreateDto input)
        {
            string Table1CreateQueryString = GetTable1CreateQuery();
            var @params = new
            {
                Id = input.Id,
                flag = input.Flag,
                town = input.Town,
                modidate = input.ModiDate,
                view_town = input.ViewTown,
                state = input.State,
                zone_state = input.ZoneState,
                z1 = input.Z1,
                z2 = input.Z2,
                olgo = input.Olgo,
                provinse = input.Provinse,
                zone1 = input.Zone1,
                zone2 = input.Zone2,
                darsa_gh = input.DarsaGh,
                ensh_mas = input.EnshMas,
                ensh_nmas = input.EnshNmas,
                fixtejari = input.FixTejari,
                tabs2 = input.Tabs2,
                arse_ab = input.ArseAb,
                arse_fa = input.ArseFa,
                aian_2 = input.Aian2,
                groupshahr = input.GroupShahr,
                server_nam = input.ServerNam,
                ready_ab = input.ReadyAb,
                ready_fa = input.ReadyFa,
                entegal_ab = input.EntegalAb,
                b_entg_ab = input.B_Entg_Ab,
                entegal_fa = input.EntegalFa,
                b_entg_fa = input.B_Entg_Fa,
                manba_ab = input.ManbaAb,
                b_manba_m = input.B_Manba_M,
                b_manba_t = input.B_Manba_T,
                codbank = input.CodBank,
                zarib_baha = input.ZaribBaha,
                tab_ab = input.TabAb,
                tab_fa = input.TabFa,
                tab_20 = input.Tab20,
                codmant = input.CodMant,
                z_student = input.ZStudent,
                z_school = input.ZSchool,
                abfar_tag = input.AbfarTag
            };

            await _sqlReportConnection.ExecuteAsync(Table1CreateQueryString, @params);
        }

        private string GetTable1CreateQuery()
        {
            return @$"use [OldCalc]
                     INSERT INTO x2 (
                        Id, flag, town, modidate, view_town, state, zone_state, 
                        z1, z2, olgo, provinse, zone1, zone2, darsa_gh, ensh_mas, ensh_nmas, 
                        fixtejari, tabs2, arse_ab, arse_fa, aian_2, groupshahr, server_nam, 
                        ready_ab, ready_fa, entegal_ab, b_entg_ab, entegal_fa, b_entg_fa, 
                        manba_ab, b_manba_m, b_manba_t, codbank, zarib_baha, 
                        tab_ab, tab_fa, tab_20, codmant, z_student, z_school, abfar_tag
                    )
                    VALUES (
                        @Id, @flag, @town, @modidate, @view_town, @state, @zone_state, 
                        @z1, @z2, @olgo, @provinse, @zone1, @zone2, @darsa_gh, @ensh_mas, @ensh_nmas, 
                        @fixtejari, @tabs2, @arse_ab, @arse_fa, @aian_2, @groupshahr, @server_nam, 
                        @ready_ab, @ready_fa, @entegal_ab, @b_entg_ab, @entegal_fa, @b_entg_fa, 
                        @manba_ab, @b_manba_m, @b_manba_t, @codbank, @zarib_baha, 
                        @tab_ab, @tab_fa, @tab_20, @codmant, @z_student, @z_school, @abfar_tag
                    );
                    ";
        }


    }
}
