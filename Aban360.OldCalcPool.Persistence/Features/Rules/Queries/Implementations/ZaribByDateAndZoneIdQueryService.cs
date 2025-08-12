using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Implementations
{
    internal sealed class ZaribByDateAndZoneIdQueryService : AbstractBaseConnection, IZaribByDateAndZoneIdQueryService
    {
        public ZaribByDateAndZoneIdQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ZaribGetDto> Get(ZaribInputDto input)
        {
            string zaribQueryService = GetZaribQueryService();
            var @params = new
            {
                zoneId = GetMergedZoneId(GetMergedZoneId(input.ZoneId)),
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
            };
            ZaribGetDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<ZaribGetDto>(zaribQueryService, @params);

            return result;
        }

        private string GetZaribQueryService()
        {
            return @$"Select Top 1
                        z.id AS Id,
                    	z.town AS Town,
                    	z.zone1 AS ZoneTitle1,
                    	z.zone2 AS ZoneTitle2,
                    	z.zarib_baha AS Zarib_baha,
                    	z.date1 AS Date1,
                    	z.date2 AS Date2,
                    	z.zb AS Zb,
                    	z.zb1 AS Zb1,
                    	z.zb2 AS Zb2,
                    	z.zb3 AS Zb3,
                    	z.zb4 AS Zb4,
                    	z.zb5 AS Zb5,
                    	z.zb6 AS Zb6,
                    	z.zb7 AS Zb7,
                    	z.zb8 AS Zb8,
                    	z.zb_r AS Zb_r 
                    From [OldCalc].dbo.zarib z
                    Where 
	                    z.date1<@toDate AND z.date2>=@fromDate AND
	                    z.town=@zoneId
                    Order By z.date1 DESC";
        }
    }
}
