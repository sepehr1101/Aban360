using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Implementations
{
    internal sealed class NerkhGetByConsumptionService : AbstractBaseConnection, INerkhGetByConsumptionService
    {
        public NerkhGetByConsumptionService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<(IEnumerable<NerkhGetDto>, IEnumerable<AbAzadGetDto>)> Get(NerkhByConsumptionInputDto input)
        {
            string nerkhTableIdQueryString = GetNerkhTableIdQuery();
            int nerkhTableId = await _sqlReportConnection.QueryFirstOrDefaultAsync<int>(nerkhTableIdQueryString, new { zoneId = input.ZoneId });

            string nerkhGetQueryString = GetNerkhGetQuery(nerkhTableId);
            var @params = new
            {
                usageId = input.UsageId,
                previousDateJalali = input.PreviousDateJalali,
                input.CurrentDateJalali,
                averageConsumption = input.AverageConsumption,
            };
            IEnumerable<NerkhGetDto> result = await _sqlReportConnection.QueryAsync<NerkhGetDto>(nerkhGetQueryString, @params);
            IEnumerable<AbAzadGetDto> abAzad = await GetAbAzad(result, nerkhTableId);
            return (result,abAzad);
        }
        private async Task<IEnumerable<AbAzadGetDto>> GetAbAzad(IEnumerable<NerkhGetDto> nerkh, int nerkhTableId)
        {
            string abAzadQueryString = GetAbAzadQuery(nerkhTableId);
            ICollection<AbAzadGetDto> abAzad = new List<AbAzadGetDto>();
            foreach (NerkhGetDto nerkhItem in nerkh)
            {
                var @abAzadParams = new
                {
                    @fromDate = nerkhItem.Date1,
                    @toDate = nerkhItem.Date2,
                };
                abAzad.Add(await _sqlReportConnection.QueryFirstOrDefaultAsync<AbAzadGetDto>(abAzadQueryString, @abAzadParams));
            }

            return abAzad.ToList();
        }
        private string GetNerkhGetQuery(int nerkh)
        {
            return @$"Select
                    	n.date1 AS Date1,
                    	n.date2 AS Date2,
                    	n.ebt AS Ebt,
                    	n.ent AS Ent,
                    	n.vaj AS Vaj,
                    	n.cod AS Cod,
                    	n.olgo AS Olgo,
                    	n.[desc] AS [Desc],
                    	n.o_vaj AS OVaj,
                    	n.o_vaj_faz AS OVajFaz,
                        n.bodjeh_new AS Bodjeh_new
                    From [OldCalc].dbo.nerkh_{nerkh} n
                    Where 
                    	(n.date1<=@currentDateJalali AND n.date2>@previousDateJalali)AND
                    	(@averageConsumption BETWEEN n.ebt AND n.ent) AND
                    	n.cod=@usageId";
        }

        private string GetNerkhTableIdQuery()
        {
            return @"Select t.olgo
                    From [OldCalc].dbo.table1 t
                    Where t.town=@zoneId";
        }
        private string GetAbAzadQuery(int nerkh)
        {
            return @$"SELECT
                        MAX(CASE WHEN cod = 39 THEN vaj END) AS Azad,
                        MAX(CASE WHEN cod = 8 THEN vaj END) AS Amozesh
                    FROM [OldCalc].dbo.nerkh_{nerkh}
                    WHERE
                        date1 >= @fromDate
                        AND date2 <= @toDate";
        }

    }
}
