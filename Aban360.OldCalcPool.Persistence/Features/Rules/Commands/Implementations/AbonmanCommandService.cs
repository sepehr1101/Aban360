using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Constants;
using Dapper;
using System.Data;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Implementations
{
    public sealed class AbonmanCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public AbonmanCommandService(
            IDbConnection connection,
            IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task Insert(AbonmanCreateDto input)
        {
            string command = GetCreateCommand();
            int effectedRecord = await _connection.ExecuteAsync(command, input, _transaction);
            if (effectedRecord <= 0)
            {
                throw new InvalidBillCommandException(Exceptionliterals.InvalidAbonmanInsert);
            }
        }
        public async Task Update(AbonmanUpdateDto input)
        {
            string command = GetUpdateCommand();
            int effectedRecord = await _connection.ExecuteAsync(command, input, _transaction);
            if (effectedRecord <= 0)
            {
                throw new InvalidBillCommandException(Exceptionliterals.InvalidAbonmanUpdate);
            }
        }

        private string GetCreateCommand()
        {
            return $@"Use [OldCalc]
                     Insert Into Abonman(date1,date2,vaj,cod,[desc])
                     Values(@date1,@date2,@vaj,@code,@desc)";
        }
        private string GetUpdateCommand()
        {
            return $@"Use [OldCalc]
                    Update [OldCalc].dbo.Abonman
                    Set 
                    	date1=@date1,
                    	date2=@date2,
                    	vaj=@vaj,
                    	cod=@code,
                    	[desc]=@desc
                    Where Id=@id";
        }
    }
}
