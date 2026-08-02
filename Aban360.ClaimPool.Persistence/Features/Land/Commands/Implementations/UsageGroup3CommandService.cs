using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Constants.Literals;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Dapper;
using System.Data;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations
{
    public class UsageGroup3CommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public UsageGroup3CommandService(
            IDbConnection sqlRonnection,
            IDbTransaction transaction)
        {
            _connection = sqlRonnection;
            _connection.NotNull(nameof(sqlRonnection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task<int> Insert(UsageGroup3InsertDto inputDto)
        {
            string command = GetInsertCommand();
            int effectedRecords = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (effectedRecords <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidInsertUsageGroup3);
            }
            return effectedRecords;
        }
        public async Task<int> InsertByParentId(short previousGroup2Id, short newGroup2Id)
        {
            string command = GetInsertDuplicateByParentIdCommand();
            int effectedRecords = await _connection.ExecuteAsync(command, new { newGroup2Id, previousGroup2Id }, _transaction);
            if (effectedRecords <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidInsertUsageGroup3);
            }
            return effectedRecords;
        }
        public async Task Update(UsageGroup3UpdateDto inputDto)
        {
            string command = GetUpdateCommand();
            int recordCount = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidUpdateUsageGroup3);
            }
        }
        public async Task<int> RemoveByUsageGroup2(short id, bool hasException)
        {
            string command = GetRemoveByUsageGroup2Command();
            int recordCount = await _connection.ExecuteAsync(command, new { id }, _transaction);
            if (recordCount <= 0 && hasException)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidRemoveUsageGroup3);
            }
            return recordCount;
        }
        public async Task Remove(short id)
        {
            string command = GetRemoveCommand(false);
            int recordCount = await _connection.ExecuteAsync(command, new { id }, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidRemoveUsageGroup3);
            }
        }
        public async Task Remove(IEnumerable<short> id)
        {
            string command = GetRemoveCommand(true);
            int recordCount = await _connection.ExecuteAsync(command, new { id }, _transaction);
            if (recordCount != (id?.Count() ?? 0))
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidRemoveUsageGroup3);
            }
        }

        private string GetInsertCommand()
        {
            return @"INSERT INTO [Db70].dbo.UsageGroup3(Group2Id,UsageId,UsageTitle)
                     SELECT 
                     	@Group2Id,
                     	t41.C0,
                     	t41.C1
                     FROM [Db70].dbo.T41 t41
                     Where t41.C0 In @UsageIds;";
        }
        private string GetInsertDuplicateByParentIdCommand()
        {
            return @"INSERT INTO [Db70].dbo.UsageGroup3(Group2Id,UsageId,UsageTitle)
                    Select @NewGroup2Id,UsageId,UsageTitle
                    From [Db70].dbo.UsageGroup3
                    Where Group2Id=@PreviousGroup2Id;";
        }
        private string GetUpdateCommand()
        {
            return $@"UPDATE u3
                    SET 
                    	Group2Id = @Group2Id,
                    	UsageId = t41.C0,
                    	UsageTitle = T41.C1
                    FROM [Db70].dbo.UsageGroup3 u3
                    JOIN [Db70].dbo.T41 t41
                    	ON t41.C0 = @UsageId
                    WHERE u3.Id = @Id;";
        }
        private string GetRemoveCommand(bool isList)
        {
            string condition = isList ? " IN " : " = ";
            return $@"Delete [Db70].dbo.UsageGroup3 
                    WHERE Id {condition} @Id; ";
        }
        private string GetRemoveByUsageGroup2Command()
        {
            return $@"Delete [Db70].dbo.UsageGroup3 
                    WHERE Group2Id = @Id; ";
        }
    }
}
