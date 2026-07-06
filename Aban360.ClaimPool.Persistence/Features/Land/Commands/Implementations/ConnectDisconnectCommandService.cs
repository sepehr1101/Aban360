using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Constants.Literals;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Dapper;
using System.Data;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations
{
    public class ConnectDisconnectCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public ConnectDisconnectCommandService(
            IDbConnection sqlRonnection,
            IDbTransaction transaction)
        {
            _connection = sqlRonnection;
            _connection.NotNull(nameof(sqlRonnection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task Insert(ConnectDisconnectInsertDto inputDto)
        {
            string command = GetInsertQuery();
            int recordCount = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidInsertConnectDisconnect);
            }
        }
        public async Task Update(ConnectDisconnectUpdateDto inputDto)
        {
            string command = GetUpdateQuery();
            int recordCount = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidUpdateConnectDisconnect);
            }
        }
        public async Task Remove(ConnectDisconnectRemoveDto inputDto)
        {
            string command = GetRemoveQuery();
            int recordCount = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidRemoveConnectDisconnect);
            }
        }

        private string GetInsertQuery()
        {
            return @"INSERT INTO [CustomerWarehouse].dbo.ConnectDisconnect 
                    (
                        ZoneId,ZoneTitle,BillId,WaterDebt,
                        CommandDateTime,CommandBy,CommandCauseId,CommandCauseTitle,
                        ResultDateTime,ResultBy,ResultId,ResultTitle,
                        MeterDiameterId,MeterDiameterTitle,CompanyId,CompanyTitle,
                        PersonnelId,PersonnelName,TypeId,TypeTitle,Description
                    )
                    VALUES (
                        @ZoneId,@ZoneTitle,@BillId,@WaterDebt,
                        @CommandDateTime,@CommandBy,@CommandCauseId,@CommandCauseTitle,
                        @ResultDateTime,@ResultBy,@ResultId,@ResultTitle,
                        @MeterDiameterId,@MeterDiameterTitle,@CompanyId,@CompanyTitle,
                        @PersonnelId,@PersonnelName,@TypeId,@TypeTitle,@Description
                    )";
        }
        private string GetUpdateQuery()
        {
            return $@"Update [CustomerWarehouse].dbo.ConnectDisconnect 
                    Set  
                        ResultDateTime = @ResultDateTime ,
                        ResultBy = @ResultBy ,
                        ResultId = @ResultId ,
                        ResultTitle = @ResultTitle ,
                        JudicialNoticeId = @JudicialNoticeId ,
                        Description = @Description
                    Where Id=@Id";
        }
        private string GetRemoveQuery()
        {
            return $@"Update [CustomerWarehouse].dbo.ConnectDisconnect 
                    Set  
                        RemovedDateTime=@RemovedDateTime,
                        RemovedBy=@RemovedBy,
                        Description=@Description
                    Where Id=@Id";
        }

    }
}
