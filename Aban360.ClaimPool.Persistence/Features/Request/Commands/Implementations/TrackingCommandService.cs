using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations
{
    internal sealed class TrackingCommandService : AbstractBaseConnection, ITrackingCommandService
    {
        public TrackingCommandService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task Insert(TrackingInsertDto inputDto)
        {
			string dbName = "AbAndFazelab";
            //string dbName = "--";
            string command = GetInsertCommand(dbName);
            await _sqlReportConnection.ExecuteAsync(command, inputDto);
        }
        public async Task UpdateIsConsiderdLatest(int trackNumber, bool isConsiderd)
        {
			string dbName = "AbAndFazelab";
            //string dbName = "--";
            string command = GetUpdateIsConsiderdCommand(dbName);
            await _sqlReportConnection.ExecuteAsync(command, new { trackNumber ,isConsiderd});
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
						110 Status,
						t.RequestOrigin,
						0 IsConsiderd,
						t.InserrtedBy,
						' --' Description,
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
        private string GetUpdateIsConsiderdCommand(string dbName)
        {
            return $@"Update [{dbName}].dbo.Tracking
					Set IsConsiderd=@isConsiderd
					Where TrackNumber=@trachNumber
					Order By DateAndTime DESC";
        }
    }
}
