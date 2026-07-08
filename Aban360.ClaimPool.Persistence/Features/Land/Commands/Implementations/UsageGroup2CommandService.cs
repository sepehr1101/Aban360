using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Constants.Literals;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Dapper;
using System.Data;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations
{
    public class UsageGroup2CommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public UsageGroup2CommandService(
            IDbConnection sqlRonnection,
            IDbTransaction transaction)
        {
            _connection = sqlRonnection;
            _connection.NotNull(nameof(sqlRonnection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task Insert(UsageGroup2InsertDto inputDto)
        {
            string command = GetInsertCommand();
            int recordCount = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidInsertUsageGroup2);
            }
        }
        public async Task Update(UsageGroup2UpdateDto inputDto)
        {
            string command = GetUpdateCommand();
            int recordCount = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidUpdateUsageGroup2);
            }
        }
        public async Task Remove(short id)
        {
            string command = GetRemoveCommand(false);
            int recordCount = await _connection.ExecuteAsync(command, new { id }, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidRemoveUsageGroup2);
            }
        }
        public async Task Remove(IEnumerable<short> id)
        {
            string command = GetRemoveCommand(true);
            int recordCount = await _connection.ExecuteAsync(command, new { id }, _transaction);
            if (recordCount != (id?.Count() ?? 0))
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidRemoveUsageGroup2);
            }
        }

        private string GetInsertCommand()
        {
            return @"INSERT INTO [Db70].dbo.UsageGroup2(Title,Group1Id)
                    VALUES(@Title,@Group1Id)";
        }
        private string GetUpdateCommand()
        {
            return $@"UPDATE [Db70].dbo.UsageGroup2 
                    SET Title=@Title , Group1Id=@Group1Id
                    WHERE Id = @Id;";
        }
        private string GetRemoveCommand(bool isList)
        {
            string condition = isList ? " IN " : " = ";
            return $@"Delete [Db70].dbo.UsageGroup2 
                    WHERE Id {condition} @Id; ";
        }
    }
}
