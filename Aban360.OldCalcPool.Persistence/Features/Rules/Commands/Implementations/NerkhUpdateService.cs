using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Implementations
{
    internal sealed class NerkhUpdateService : AbstractBaseConnection, INerkhUpdateService
    {
        public NerkhUpdateService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task Update(NerkhUpdateDto input, int nerkh)
        {
            string nerkhUpdateQueryString = GetNerkhUpdateQuery(nerkh);
            var @params = new
            {
                date1 = input.Date1,
                date2 = input.Date2,
                ebt = input.Ebt,
                ent = input.Ent,
                vaj = input.Vaj,
                code = input.Cod,
                olgo = input.Olgo,
                desc = input.Desc,
                oVaj = input.OVaj,
                oVajFaz = input.OVajFaz,
            };
            await _sqlReportConnection.ExecuteAsync(nerkhUpdateQueryString, @params);
        }

        private string GetNerkhUpdateQuery(int nerkh)
        {
            return @$"use [OldCalc]
                    Update [OldCalc].dbo.nerkh_{nerkh}
                    Set 
                    	date1=@date1,
                    	date2=@date2,
                    	ebt=@ebt,
                    	ent=@ent,
                    	vaj=@vaj,
                    	cod=@code,
                    	olgo=@olgo,
                    	[desc]=@desc,
                    	o_vaj=@oVaj,
                    	o_vaj_faz=@oVajFaz
                    Where Id=2";
        }
    }
}
