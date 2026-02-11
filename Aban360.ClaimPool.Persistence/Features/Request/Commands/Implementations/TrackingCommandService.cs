using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using Aban360.Common.Extensions;
using Aban360.Common.Exceptions;
using Aban360.ClaimPool.Persistence.Constants.Literals;

namespace Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations
{
    public sealed class TrackingCommandService //: AbstractBaseConnection, ITrackingCommandService
    {
        private readonly SqlConnection _sqlConnection;
        private readonly IDbTransaction _transaction;
        public TrackingCommandService(
            SqlConnection sqlConnection,
            IDbTransaction transaction)
        {
            _sqlConnection = sqlConnection;
            _sqlConnection.NotNull(nameof(sqlConnection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task Insert(TrackingInsertDto inputDto)
        {
            string dbName = "AbAndFazelab";
            //string dbName = "--";
            string command = GetInsertCommand(dbName);
            int recordCount = await _sqlConnection.ExecuteAsync(command, inputDto);
            if (recordCount < 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidInsertTracking);
            }
        }
        public async Task UpdateIsConsiderdLatest(int trackNumber, bool isConsiderd)
        {
            string dbName = "AbAndFazelab";
            //string dbName = "--";
            string command = GetUpdateIsConsiderdCommand(dbName);
            int recordCount = await _sqlConnection.ExecuteAsync(command, new { trackNumber, isConsiderd });
        }


        private string GetInsertCommand(string dbName)//remove dbName:forTest
        {
            return $@"Insert [{dbName}].dbo.Tracking
					Select Top 1
						NEWID() TrackId,
						t.TrackNumber,
						t.ZoneId,
						GETDATE() DateAndTime,
						@currentDateJalali DateTimeJalali,
						t.BillId,
						t.ServiceGroup_FK,
						@StatusId Status,
						t.RequestOrigin,
						0 IsConsiderd,
						@UserId InserrtedBy,
						@Description Description,
						t.Caller,
						t.NotificationMobile,
						t.NeighbourBillId,
						t.AbBahaServiceId,
						AbBahaServiceTitle,
						t.C17,
						t.C18,
						t.C19,
						t.C20,
						t.C21,
						t.C22,
						t.C23,
						t.C24
					From AbAndFazelab.dbo.Tracking t
					Where t.TrackNumber=@trackNumber
					Order By t.DateAndTime DESC";
        }
        private string GetUpdateIsConsiderdCommand(string dbName)//todo : use DateAndTime insteadof DateTimeJalali
        {
            return $@"WITH FirstRecord AS (
                        SELECT TOP 1 IsConsiderd
                        FROM[{dbName}].dbo.Tracking
                        WHERE TrackNumber = @trackNumber
                        ORDER BY DateTimeJalali DESC
                    )
                    UPDATE FirstRecord
                    SET IsConsiderd = @isConsiderd";
        }
    }
}
