﻿using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Implementations
{
    internal sealed class ZaribGetService : AbstractBaseConnection, IZaribGetService
    {
        public ZaribGetService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<IEnumerable<ZaribGetDto>> Get(int id)
        {
            string ZaribGetQueryString = GetZaribGetQuery();
            IEnumerable<ZaribGetDto> result = await _sqlReportConnection.QueryAsync<ZaribGetDto>(ZaribGetQueryString, new { id });

            return result;
        }

        private string GetZaribGetQuery()
        {
            return @$"Select 
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
                    Where z.Id=@zoneId";
        }
    }
}
