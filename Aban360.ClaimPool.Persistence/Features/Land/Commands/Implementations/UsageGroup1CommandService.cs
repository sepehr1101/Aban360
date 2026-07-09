using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Constants.Literals;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Dapper;
using System.Data;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations
{
    public class UsageGroup1CommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public UsageGroup1CommandService(
            IDbConnection sqlRonnection,
            IDbTransaction transaction)
        {
            _connection = sqlRonnection;
            _connection.NotNull(nameof(sqlRonnection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task Insert(UsageGroup1InsertDto inputDto)
        {
            string command = GetInsertCommand();
            int recordCount = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidInsertUsageGroup1);
            }
        }
        public async Task Update(UsageGroup1UpdateDto inputDto)
        {
            string command = GetUpdateCommand();
            int recordCount = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidUpdateUsageGroup1);
            }
        }
        public async Task Remove(short id)
        {
            string command = GetRemoveCommand();
            int recordCount = await _connection.ExecuteAsync(command, new { id }, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidRemoveUsageGroup1);
            }
        }

        private string GetInsertCommand()
        {
            return @"INSERT INTO [Db70].dbo.UsageGroup1(Title)
                    VALUES(@Title)";
        }
        private string GetUpdateCommand()
        {
            return $@"UPDATE [Db70].dbo.UsageGroup1 
                    SET Title=@Title
                    WHERE Id = @Id;";
        }
        private string GetRemoveCommand()
        {
            return $@"Delete [Db70].dbo.UsageGroup1 
                    WHERE Id = @Id; ";
        }
    }
}
