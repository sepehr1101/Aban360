using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Implementations
{
    internal sealed class NerkhGetAllService : AbstractBaseConnection, INerkhGetAllService
    {
        const string _minDate = "1402/12/29";
        public NerkhGetAllService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<IEnumerable<NerkhGetDto>> Get(int nerkh)
        {
            string nerkhGetQueryString = GetNerkhGetQuery(nerkh);
            IEnumerable<NerkhGetDto> result = await _sqlReportConnection.QueryAsync<NerkhGetDto>(nerkhGetQueryString);

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
                    Where n.date1>='{_minDate}'";
        }

    }
}
