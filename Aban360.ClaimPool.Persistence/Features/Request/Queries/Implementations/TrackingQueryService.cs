using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Implementations
{
    internal sealed class TrackingQueryService : AbstractBaseConnection, ITrackingQueryService
    {
        public TrackingQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<TrackingOutputDto> GetFirstStep(int trackNumber)
        {
            string query = GetNewRequestByTrackNumberQuery();
            TrackingOutputDto? result = await _sqlReportConnection.QueryFirstOrDefaultAsync<TrackingOutputDto>(query, new { trackNumber });
            if (result is null)
            {
                throw new InvalidTrackNumberException(ExceptionLiterals.InvalidTrackNumber);
            }
            result.StringTrackNumber = trackNumber.ToString().PadLeft(11, '0');
            return result;
        }
        public async Task<TrackingOutputDto> GetLatest(int trackNumber)
        {
            string query = GetLatestByTrackNumberQuery();
            TrackingOutputDto? result = await _sqlReportConnection.QueryFirstOrDefaultAsync<TrackingOutputDto>(query, new { trackNumber });
            if (result is null)
            {
                throw new InvalidTrackNumberException(ExceptionLiterals.InvalidTrackNumber);
            }
            result.StringTrackNumber = trackNumber.ToString().PadLeft(11, '0');
            return result;
        }
        public async Task<IEnumerable<TrackingKartableDataOutputDto>> GetAllOpenRequest()
        {
            string query = GetAllOpenTrackingQuery();
            IEnumerable<TrackingKartableDataOutputDto> result = await _sqlReportConnection.QueryAsync<TrackingKartableDataOutputDto>(query, null);
            if (!result.Any())
            {
                throw new InvalidTrackNumberException(ExceptionLiterals.NotFoundAnyOpenTrack);
            }
            return result;
        }

        private string GetNewRequestByTrackNumberQuery()
        {
            return $@"Select	
                    	t.TrackID,
                    	t.TrackNumber,
                        t.BillID,
                    	t.ZoneID, 
						t51.C2 ZoneTitle,
                    	t.DateTimeJalali InsertDateJalali,
                    	t.ServiceGroup_FK ServiceGroupId,
                    	sg.Title ServiceGroupTitle,
                    	t.Status StatusId,
                    	s.Description StatusTitle,
                    	t.InserrtedBy ,
                    	t.Description,
                    	t.NotificationMobile,
                    	t.NeighbourBillId
                    From AbAndFazelab.dbo.Tracking t
                    Join AbAndFazelab.dbo.Status s
                    	ON t.Status=s.StatusID
                    Join AbAndFazelab.dbo.ServiceGroup sg
                    	ON t.ServiceGroup_FK=sg.Id
					Join Db70.dbo.T51 t51
						ON t.ZoneID=t51.C0
                    Where 
                    	t.TrackNumber=@TrackNumber AND
                    	t.Status=0";
        }
        private string GetLatestByTrackNumberQuery()
        {
            return $@"Select Top 1
                    	t.TrackID,
                    	t.TrackNumber,
                        t.BillID,
                    	t.ZoneID, 
						t51.C2 ZoneTitle,
                    	t.DateTimeJalali InsertDateJalali,
                    	t.ServiceGroup_FK ServiceGroupId,
                    	sg.Title ServiceGroupTitle,
                    	t.Status StatusId,
                    	s.Description StatusTitle,
                    	t.InserrtedBy ,
                    	t.Description,
                    	t.NotificationMobile,
                    	t.NeighbourBillId
                    From AbAndFazelab.dbo.Tracking t
                    Join AbAndFazelab.dbo.Status s
                    	ON t.Status=s.StatusID
                    Join AbAndFazelab.dbo.ServiceGroup sg
                    	ON t.ServiceGroup_FK=sg.Id
					Join Db70.dbo.T51 t51
						ON t.ZoneID=t51.C0
                    Where t.TrackNumber=@TrackNumber 
					Order By t.DateAndTime Desc";
        }
        private string GetAllOpenTrackingQuery()
        {
            return $@"Select 
                    	t.TrackID,
                    	t.ZoneID,
                    	T51.C2 ZoneTitle,
                    	t.TrackNumber,
                    	t.BillId,
                    	t.NeighbourBillId,
                    	t.Status StatusId,
                    	s.Description StatusTitle,
                    	t.ServiceGroup_Fk ServiceGroupId,
                    	sg.Title ServiceGroupTitle,
                    	IIF( (format(GETDATE(),'yyyy-MM-dd')) = (Format(t.DateAndTime,'yyyy-MM-dd')),0,1) HasAttention,
                    	t.DateTimeJalali RequestDateJalali
                    From [AbAndFazelab].dbo.Tracking t
                    Join [Db70].dbo.T51 T51
                    	ON t.ZoneID=T51.C0
                    Join [AbAndFazelab].dbo.ServiceGroup sg
                    	ON t.ServiceGroup_Fk=sg.Id
                    Join AbAndFazelab.dbo.Status s
                    	ON t.Status=s.StatusID
                    Where 
                    	t.IsConsiderd=0 AND 
                     	t.DateTimeJalali>='1404/07/01' AND
                    	t.Status IN (0,10,15,20,50,60,65,70,75,110,150,90002)
                    Order by sg.Title,t.DateAndTime desc";
        }
    }
}
