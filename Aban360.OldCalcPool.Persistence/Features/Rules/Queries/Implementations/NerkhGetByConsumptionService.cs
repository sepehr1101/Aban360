using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Implementations
{
    internal sealed class NerkhGetByConsumptionService : AbstractBaseConnection, INerkhGetByConsumptionService
    {
        private readonly IZaribByDateAndZoneIdQueryService _zaribByDateAndZoneIdService;
        public NerkhGetByConsumptionService(
            IConfiguration configuration,
            IZaribByDateAndZoneIdQueryService zaribByDateAndZoneIdService)
            : base(configuration)
        {
            _zaribByDateAndZoneIdService = zaribByDateAndZoneIdService;
            _zaribByDateAndZoneIdService.NotNull(nameof(zaribByDateAndZoneIdService));
        }

        public async Task<(IEnumerable<NerkhGetDto>, IEnumerable<AbAzadGetDto>,IEnumerable<ZaribGetDto>)> Get(NerkhByConsumptionInputDto input)
        {
            string nerkhTableIdQueryString = GetNerkhTableIdQuery();
            int nerkhTableId = await _sqlReportConnection.QueryFirstOrDefaultAsync<int>(nerkhTableIdQueryString, new { zoneId = GetMergedZoneId(input.ZoneId) });

            string nerkhGetQueryString = GetNerkhGetQuery(nerkhTableId);
            var @params = new
            {
                usageId = input.UsageId,
                previousDateJalali = input.PreviousDateJalali,
                input.CurrentDateJalali,
                averageConsumption = input.AverageConsumption,
            };
            IEnumerable<NerkhGetDto> nerkh = await _sqlReportConnection.QueryAsync<NerkhGetDto>(nerkhGetQueryString, @params);
            IEnumerable<AbAzadGetDto> abAzad = await GetAbAzad(nerkh, nerkhTableId);
            IEnumerable<ZaribGetDto> zarib = await GetZarib(nerkh, input.ZoneId);
            return (nerkh,abAzad,zarib);
        }
        private async Task<IEnumerable<ZaribGetDto>> GetZarib(IEnumerable<NerkhGetDto> nerkh,int zoneId)
        {
            ICollection<ZaribGetDto> zaribs=new List<ZaribGetDto>();
            foreach (NerkhGetDto item in nerkh)
            {
                zaribs.Add(await _zaribByDateAndZoneIdService.Get(new ZaribInputDto(zoneId, item.Date1, item.Date2)));
            }
            return zaribs.ToList();
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
                        n.Id,
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
                        n.bodjeh_new AS Bodjeh_new,
                        n.vaj_faz AS VajFaz
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
                        date1 < @toDate
                        AND date2 >= @fromDate";///
        }

    }
}
