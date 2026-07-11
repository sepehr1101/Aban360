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
            int effectedRecord = await _connection.ExecuteAsync(nerkhCreateQueryString, input, _transaction);
            if (effectedRecord <= 0)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidInsertNerkh);
            }
        }
        public async Task Update(NerkhUpdateDto input)
        {
            string nerkhUpdateQueryString = GetNerkhUpdateQuery();
            int effectedRecord = await _connection.ExecuteAsync(nerkhUpdateQueryString, input, _transaction);
            if (effectedRecord <= 0)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidUpdateNerkh);
            }
        }

        private string GetNerkhCreateQuery()
        {
            return @"use [OldCalc]
                     Insert Into Nerkh
                    (
                        date1,date2,
                        ebt,ent,
                        vaj,cod,olgo,
                        AllowedSewageFormula,DisallowedSewageFormula,
                        [desc],o_vaj,o_vaj_faz
                    )
                     Values
                    (
                        @date1,@date2,
                        @ebt,@ent,
                        @vaj,@cod,@olgo,
                        @AllowedSewageFormula,@DisallowedSewageFormula,
                        @desc,@oVaj,@oVajFaz
                    )";
        }
        private string GetNerkhUpdateQuery()
        {
            return @"use [OldCalc]
                    Update [OldCalc].dbo.Nerkh
                    Set 
                    	date1 = @date1 ,
                    	date2 = @date2 ,
                    	ebt = @ebt ,
                    	ent = @ent ,
                    	vaj = @vaj ,
                    	cod = @cod ,
                    	olgo = @olgo ,
                    	[desc] = @desc,
                    	o_vaj = @oVaj ,
                    	o_vaj_faz = @oVajFaz ,
                        AllowedSewageFormula = @AllowedSewageFormula ,
                        DisallowedSewageFormula = @DisallowedSewageFormula 
                    Where Id=@id";
        }
    }
}
