using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

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

        public async Task<(IEnumerable<NerkhGetDto>, IEnumerable<AbAzadFormulaDto>,IEnumerable<ZaribGetDto>, int)> Get(NerkhByConsumptionInputDto input)
        {
            string olgooQuery = GetOlgooQuery();
            int olgoo = await _sqlReportConnection.QueryFirstOrDefaultAsync<int>(olgooQuery, new { zoneId = GetMergedZoneId(input.ZoneId) });

            string nerkhQuery = GetNerkh_n_Query(olgoo);
            var @params = new
            {
                usageId = input.UsageId,
                previousDateJalali = input.PreviousDateJalali,
                input.CurrentDateJalali,
                averageConsumption = input.AverageConsumption,
            };
            IEnumerable<NerkhGetDto> nerkh = await _sqlReportConnection.QueryAsync<NerkhGetDto>(nerkhQuery, @params);
            IEnumerable<AbAzadFormulaDto> abAzad = await GetAbAzad(nerkh, olgoo);
            IEnumerable<ZaribGetDto> zarib = await GetZarib(nerkh, input.ZoneId);
            return (nerkh,abAzad,zarib, olgoo);
        }
        public async Task<(IEnumerable<NerkhGetDto>, IEnumerable<AbAzadFormulaDto>,IEnumerable<ZaribGetDto>, int, IEnumerable<NerkhGetDto>)> GetWithAggregatedNerkh(NerkhByConsumptionInputDto input)
        {
            string olgooQuery = GetOlgooQuery();
            int olgoo = await _sqlReportConnection.QueryFirstOrDefaultAsync<int>(olgooQuery, new { zoneId = GetMergedZoneId(input.ZoneId) });

            string nerkhGetQueryString = GetNerkhQuery(olgoo);
            string nerkh1403Query = GetNerkhQuery(14);
            var @params = new
            {
                usageId = input.UsageId,
                previousDateJalali = input.PreviousDateJalali,
                input.CurrentDateJalali,
                averageConsumption = (double)input.AverageConsumption,
                olgoo,
            };
            var @params1403 = new
            {
                usageId = input.UsageId,
                previousDateJalali = input.PreviousDateJalali,
                input.CurrentDateJalali,
                averageConsumption = (double)input.AverageConsumption,
                olgoo=14,
            };
            IEnumerable<NerkhGetDto> nerkh = await _sqlReportConnection.QueryAsync<NerkhGetDto>(nerkhGetQueryString, @params);
            IEnumerable<NerkhGetDto> nerkh1403 = await _sqlReportConnection.QueryAsync<NerkhGetDto>(nerkh1403Query, @params1403);
            IEnumerable<AbAzadFormulaDto> abAzad = await GetAbAzad(nerkh);
            IEnumerable<ZaribGetDto> zarib = await GetZarib(nerkh, input.ZoneId);
            return (nerkh,abAzad,zarib, olgoo, nerkh1403);
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
        private async Task<IEnumerable<AbAzadFormulaDto>> GetAbAzad(IEnumerable<NerkhGetDto> nerkh, int nerkhTableId)
        {
            string abAzadQueryString = GetAbAzadQuery(nerkhTableId);
            ICollection<AbAzadFormulaDto> abAzad = new List<AbAzadFormulaDto>();
            foreach (NerkhGetDto nerkhItem in nerkh)
            {
                var @abAzadParams = new
                {
                    @fromDate = nerkhItem.Date1,
                    @toDate = nerkhItem.Date2,
                };
                abAzad.Add(await _sqlReportConnection.QueryFirstOrDefaultAsync<AbAzadFormulaDto>(abAzadQueryString, @abAzadParams));
            }

            return abAzad.ToList();
        }
        private async Task<IEnumerable<AbAzadFormulaDto>> GetAbAzad(IEnumerable<NerkhGetDto> nerkh)
        {
            string abAzadQueryString = GetAbAzadQuery();
            ICollection<AbAzadFormulaDto> abAzad = new List<AbAzadFormulaDto>();
            foreach (NerkhGetDto nerkhItem in nerkh)
            {
                var @abAzadParams = new
                {
                    @fromDate = nerkhItem.Date1,
                    @toDate = nerkhItem.Date2,
                };
                abAzad.Add(await _sqlReportConnection.QueryFirstOrDefaultAsync<AbAzadFormulaDto>(abAzadQueryString, @abAzadParams));
            }

            return abAzad.ToList();
        }
        private string GetNerkh_n_Query(int nerkh)
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
        private string GetNerkhQuery(int olgoo)
        {
            return @"Select
                        n.Id,
                    	n.date1 AS Date1,
                    	n.date2 AS Date2,
                    	n.ebt AS Ebt,
                    	n.ent AS Ent,
                    	n.vaj AS Vaj,
                        n.AllowedFormula,
                        n.DisallowedFormula,
                    	n.cod AS Cod,
                    	n.olgo AS Olgo,
                    	n.[desc] AS [Desc],
                    	n.o_vaj AS OVaj,
                    	n.o_vaj_faz AS OVajFaz,
                        n.bodjeh_new AS Bodjeh_new,
                        n.vaj_faz AS VajFaz
                    From [OldCalc].dbo.Nerkh n
                    Where 
                    	(n.date1<@currentDateJalali AND n.date2>=@previousDateJalali)AND
                    	(@averageConsumption > n.ebt*@olgoo AND @averageConsumption <= n.ent*@olgoo) AND
                    	n.cod=@usageId
                    ORDER BY n.date1";
        }
        private string GetOlgooQuery()
        {
            return @"Select t.olgo
                    From [OldCalc].dbo.table1 t
                    Where t.town=@zoneId";
        }
        private string GetAbAzadQuery(int nerkh)
        {
            return @$"SELECT                        
                        MAX(vaj) AS Formula
                    FROM [OldCalc].dbo.nerkh_{nerkh}
                    WHERE
                        date1 < @toDate And
                        date2 >= @fromDate AND
                        cod = 39";
        }
        private string GetAbAzadQuery()
        {
            return @$"SELECT
                        MAX(vaj) AS Formula
                    FROM [OldCalc].dbo.Nerkh
                    WHERE
                        date1 < @toDate AND
                        date2 >= @fromDate AND
                        cod = 39";
        }
    }
}
