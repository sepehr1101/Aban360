using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Implementations
{
    internal sealed class ZaribCreateService : AbstractBaseConnection, IZaribCreateService
    {
        public ZaribCreateService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task Create(ZaribCreateDto input)
        {
            string zaribCreateQueryString = GetZaribCreateQuery();
            string zoneTitleQueryString = GetZoneTitleQuery();
            string zoneTitle = await _sqlReportConnection.QueryFirstOrDefaultAsync<string>(zoneTitleQueryString, new { id = input.ZoneId1 });

            var @params = new
            {
                town = input.Town,
                zone1 = zoneTitle,
                zone2=zoneTitle,
                zarib_baha = input.Zarib_baha,
                date1 = input.Date1,
                date2 = input.Date2,
                zb = input.Zb,
                zb1 = input.Zb1,
                zb2 = input.Zb2,
                zb3 = input.Zb3,
                zb4 = input.Zb4,
                zb5 = input.Zb5,
                zb6 = input.Zb6,
                zb7 = input.Zb7,
                zb8 = input.Zb8,
                zb_r = input.Zb_r
            };
            await _sqlReportConnection.ExecuteAsync(zaribCreateQueryString, @params);
        }

        private string GetZaribCreateQuery()
        {
            return @"INSERT INTO [OldCalc].dbo.Zarib (town,zone1,zone2 zarib_baha, date1, date2, zb, zb1, zb2, zb3, zb4, zb5, zb6, zb7, zb8, zb_r) 
                    Values(@town,@zone1,@zone2, @zarib_baha, @date1, @date2, @zb, @zb1, @zb2, @zb3, @zb4, @zb5, @zb6, @zb7, @zb8, @zb_r)";
        }
        private string GetZoneTitleQuery()
        {
            return @"Select t.C2
                    From [Db70].dbo.T51 t
                    Where t.C0=@zoneId";
        }
    }
}
