using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Implementations
{
    internal sealed class Table1UpdateService : AbstractBaseConnection, ITable1UpdateService
    {
        public Table1UpdateService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task Update(Table1UpdateDto input)
        {
            string Table1UpdateQueryString = GetTable1UpdateQuery();
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
            await _sqlReportConnection.ExecuteAsync(Table1UpdateQueryString, @params);
        }

        private string GetTable1UpdateQuery()
        {
            return @$"use [OldCalc]
                     Insert Into Table1(date1,date2,ebt,ent,vaj,cod,olgo,[desc],o_vaj,o_vaj_faz)
                     Values(@date1,@date2,@ebt,@ent,@vaj,@code,@olgo,@desc,@oVaj,@oVajFaz)";
        }
    }
}
