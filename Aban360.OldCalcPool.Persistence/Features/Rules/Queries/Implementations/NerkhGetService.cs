using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Implementations
{
    internal sealed class NerkhGetService : AbstractBaseConnection, INerkhGetService
    {
        public NerkhGetService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<NerkhGetDto> Get(int id, int nerkh)
        {
            string nerkhGetQueryString = GetNerkhGetQuery(nerkh);
            NerkhGetDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<NerkhGetDto>(nerkhGetQueryString, new { id });

            return result;
        }
        public async Task<NerkhGetDto> Get(int id)
        {
            string nerkhGetQueryString = GetNerkhGetQuery();
            NerkhGetDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<NerkhGetDto>(nerkhGetQueryString, new { id });
            return result;
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
						n.ztadil AS ZaribTadil,
						n.tabsare2 AS Tabsare2,
						n.zaribfasl AS ZaribFasl,
						n.zarib_d AS ZaribBodje,
                        n.vaj_faz AS VajFaz
                	From [OldCalc].dbo.nerkh_{nerkh} n
                	Where n.Id=@id";
        }
        private string GetNerkhGetQuery()
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
						n.ztadil AS ZaribTadil,
						n.tabsare2 AS Tabsare2,
						n.zaribfasl AS ZaribFasl,
						n.zarib_d AS ZaribBodje,
                        n.vaj_faz AS VajFaz
                	From [OldCalc].dbo.Nerkh n
                	Where n.Id=@id";
        }
    }
}
