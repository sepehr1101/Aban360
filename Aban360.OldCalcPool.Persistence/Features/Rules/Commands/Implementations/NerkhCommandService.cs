using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Dapper;
using System.Data;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Implementations
{
    public sealed class NerkhCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public NerkhCommandService(
            IDbConnection connection,
            IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task Insert(NerkhCreateDto input)
        {
            string nerkhCreateQueryString = GetNerkhCreateQuery();
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
            int effectedRecord = await _connection.ExecuteAsync(nerkhCreateQueryString, @params, _transaction);
            if (effectedRecord <= 0)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidInsertNerkh);
            }
        }
        public async Task Update(NerkhUpdateDto input)
        {
            string nerkhUpdateQueryString = GetNerkhUpdateQuery();
            var @params = new
            {
                id = input.Id,
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
            int effectedRecord = await _connection.ExecuteAsync(nerkhUpdateQueryString, @params, _transaction);
            if (effectedRecord <= 0)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidUpdateNerkh);
            }
        }
        public async Task Create(NerkhCreateDto input, int switchNerkh)
        {
            string nerkhCreateQueryString = GetNerkhCreateQuery(switchNerkh);
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
            await _connection.ExecuteAsync(nerkhCreateQueryString, @params, _transaction);
        }
        public async Task Update(NerkhUpdateDto input, int nerkh)
        {
            string nerkhUpdateQueryString = GetNerkhUpdateQuery(nerkh);
            var @params = new
            {
                id = input.Id,
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
            await _connection.ExecuteAsync(nerkhUpdateQueryString, @params, _transaction);
        }

        private string GetNerkhCreateQuery(int nerkhName)
        {
            return @$"use [OldCalc]
                     Insert Into nerkh_{nerkhName}(date1,date2,ebt,ent,vaj,cod,olgo,[desc],o_vaj,o_vaj_faz)
                     Values(@date1,@date2,@ebt,@ent,@vaj,@code,@olgo,@desc,@oVaj,@oVajFaz)";
        }
        private string GetNerkhCreateQuery()
        {
            return @"use [OldCalc]
                     Insert Into Nerkh(date1,date2,ebt,ent,vaj,cod,olgo,[desc],o_vaj,o_vaj_faz)
                     Values(@date1,@date2,@ebt,@ent,@vaj,@code,@olgo,@desc,@oVaj,@oVajFaz)";
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
                    Where Id=@id";
        }
        private string GetNerkhUpdateQuery()
        {
            return @"use [OldCalc]
                    Update [OldCalc].dbo.Nerkh
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
                    Where Id=@id";
        }
    }
}
