using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Dapper;
using System.Data;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Implementations
{
    public sealed class ZaribCCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public ZaribCCommandService(
            IDbConnection connection,
            IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task Update(ZaribCUpdateDto input)
        {
            string command = GetZaribCUpdateQuery();
            int effectedRecord = await _connection.ExecuteAsync(command, input, _transaction);
            if (effectedRecord <= 0)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidUpdateZaribC);
            }
        }
        public async Task Insert(ZaribCCreateDto input)
        {
            string command = GetZaribCCreateQuery();
            int effectedRecord = await _connection.ExecuteAsync(command, input, _transaction);
            if (effectedRecord <= 0)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidInsertZaribC);
            }
        }
        public async Task Delete(int id)
        {
            string command = GetZaribCDeleteQuery();
            int effectedRecord = await _connection.ExecuteAsync(command, new { id = id }, _transaction);
            if (effectedRecord <= 0)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidRemoveZaribC);
            }
        }

        private string GetZaribCUpdateQuery()
        {
            return @"Update [OldCalc].dbo.Zarib_C
                    Set 
                        FromDateJalali=@fromDateJalali, 
                        ToDateJalali=@toDateJalali ,
                        C=@c ,
                        ConditionGroup=@conditionGroup , 
                        IsDeleted=@isDeleted
                    Where Id=@id";
        }
        private string GetZaribCCreateQuery()
        {
            return @"Insert Into [OldCalc].dbo.Zarib_C
                    (
                        FromDateJalali,ToDateJalali,
                        C,ConditionGroup,IsDeleted
                    )
                    Values
                    (
                        @fromDateJalali,@toDateJalali,
                        @c,@conditionGroup,@isDeleted
                    );";
        }
        private string GetZaribCDeleteQuery()
        {
            return @"Update [OldCalc].dbo.Zarib_C
                    Set 
                        IsDeleted=1
                    Where Id=@id";
        }

    }
}
