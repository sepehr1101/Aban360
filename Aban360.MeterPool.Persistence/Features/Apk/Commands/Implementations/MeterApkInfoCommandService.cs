using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.MeterPool.Domain.Features.Apk.Commands;
using Dapper;
using System.Data;

namespace Aban360.MeterPool.Persistence.Features.Apk.Commands.Implementations
{
    public sealed class MeterApkInfoCommandService
    {
        private readonly IDbConnection _sqlConnection;
        private readonly IDbTransaction _transaction;
        public MeterApkInfoCommandService(
            IDbConnection sqlRonnection,
            IDbTransaction transaction)
        {
            _sqlConnection = sqlRonnection;
            _sqlConnection.NotNull(nameof(sqlRonnection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task Insert(ApkInfoInsertDto inputDto)
        {
            string command = GetInsertQuery();
            int recordCount = await _sqlConnection.ExecuteAsync(command, inputDto, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidInsertMeterApkFile);
            }
        }
        public async Task Remove(ApkInfoRemoveDto inputDto)
        {
            string command = GetRemoveQuery();
            int recordCount = await _sqlConnection.ExecuteAsync(command, inputDto, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidRemoveMeterApkFile);
            }
        }
        private string GetInsertQuery()
        {
            return @"Insert Into [Aban360].MeterPool.ApkInfo
                    (
                        Name, Version, 
                        FileContent, Description, 
                        InsertedBy, InsertedDateTime
                    )
                    Values
                    (
                        @Name, @Version, 
                        @FileContent, @Description, 
                        @InsertedBy, @InsertedDateTime
                    );";
        }
        private string GetRemoveQuery()
        {
            return $@"Update [Aban360].MeterPool.ApkInfo
                    Set RemovedBy = @RemovedBy, RemovedDateTime = @RemovedDateTime
                    Where Id = @Id";
        }
    }
}
