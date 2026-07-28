using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Features.Tagging.Commands;
using Dapper;
using System.Data;

namespace Aban360.ReportPool.Persistence.Features.Tagging.Commands
{
    public class MainTagGroupCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public MainTagGroupCommandService(
                IDbConnection connection,
                IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task Insert(MainTagGroupInsertDto input)
        {
            int effectedRecords = await _connection.ExecuteAsync(GetInsertCommand(), input, _transaction);
            if (effectedRecords < 0)
            {
                throw new CustomValidationException(ExceptionLiterals.InvalidInsertMainTagGroup);
            }
        }
        public async Task Update(MainTagGroupUpdateDto input)
        {
            int effectedRecords = await _connection.ExecuteAsync(GetUpdateCommand(), input, _transaction);
            if (effectedRecords < 0)
            {
                throw new CustomValidationException(ExceptionLiterals.InvalidInsertMainTagGroup);
            }
        }
        public async Task Remove(MainTagGroupRemoveDto input)
        {
            int effectedRecords = await _connection.ExecuteAsync(GetRemoveCommand(), input, _transaction);
            if (effectedRecords < 0)
            {
                throw new CustomValidationException(ExceptionLiterals.InvalidInsertMainTagGroup);
            }

        }

        private string GetInsertCommand()
        {
            return @"Insert Into CustomerWarehouse.dbo.MainTagGroup(Title , CreateDateTime)
                    Values(@Title , @CreateDateTime)";
        }
        private string GetRemoveCommand()
        {
            return @"Update CustomerWarehouse.dbo.MainTagGroup
                    Set DeleteDateTime = @RemoveDateTime 
                    Where Id = @Id";
        }
        private string GetUpdateCommand()
        {
            return @"Update CustomerWarehouse.dbo.MainTagGroup
                    Set Title = @Title
                    Where Id = @Id";
        }
    }
}
