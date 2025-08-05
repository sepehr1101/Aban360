using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Implementations
{
    internal sealed class ZaribUpdateService : AbstractBaseConnection, IZaribUpdateService
    {
        public ZaribUpdateService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task Update(ZaribUpdateDto input)
        {
            string zaribUpdateQueryString = GetZaribUpdateQuery();
            string zoneTitleQueryString = GetZoneTitleQuery();
            string zoneTitle = await _sqlReportConnection.QueryFirstOrDefaultAsync<string>(zoneTitleQueryString, new { id = input.ZoneId1 });
          
            var @params = new
            {
                Id=input.Id,
                town = input.Town,
                zone1=zoneTitle,
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

            await _sqlReportConnection.ExecuteAsync(zaribUpdateQueryString, @params);
        }

        private string GetZaribUpdateQuery()
        {
            return @"UPDATE [OldCalc].dbo.zarib 
                    SET 
                    	town = @town, 
                        zone1 = @zone1,       
                        zone2 = @zone2,       
                    	zarib_baha = @zarib_baha,
                    	date1 = @date1, 
                    	date2 = @date2, 
                    	zb = @zb, 
                    	zb1 = @zb1, 
                    	zb2 = @zb2, 
                    	zb3 = @zb3,
                    	zb4 = @zb4,
                    	zb5 = @zb5,
                    	zb6 = @zb6,
                    	zb7 = @zb7,
                    	zb8 = @zb8,
                    	zb_r = @zb_r 
                    WHERE Id = @Id;";
        }
        private string GetZoneTitleQuery()
        {
            return @"Select t.C2
                    From [Db70].dbo.T51 t
                    Where t.C0=@id";
        }
    }
}
