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
        public async Task<TrackingOutputDto> Get(Guid trackId)
        {
            string query = GetByTrackIdQuery();
            TrackingOutputDto? result = await _sqlReportConnection.QueryFirstOrDefaultAsync<TrackingOutputDto>(query, new { trackId });
            if (result is null)
            {
                throw new InvalidTrackNumberException(ExceptionLiterals.InvalidTrackNumber);
            }
            result.StringTrackNumber = result.TrackNumber.ToString().PadLeft(11, '0');
            return result;
        }
        public async Task<TrackingOutputDto> GetSecondToLatest(int trackNumber)
        {
            string query = GetTwoLatestByTrackNumberQuery();
            IEnumerable<TrackingOutputDto> result = await _sqlReportConnection.QueryAsync<TrackingOutputDto>(query, new { trackNumber });
            if (!result.Any() && result?.Count() != 2)
            {
                throw new InvalidTrackNumberException(ExceptionLiterals.InvalidTrackNumber);
            }
            TrackingOutputDto secondToLatest = result.OrderBy(x => x.InsertDateJalali).FirstOrDefault();
            secondToLatest.StringTrackNumber = trackNumber.ToString().PadLeft(11, '0');
            return secondToLatest;
        }
        public async Task<IEnumerable<TrackingKartableDataOutputDto>> GetAllOpenRequest(IEnumerable<int> zoneIds)
        {
            string query = GetAllOpenTrackingQuery();
            var @params = new { zoneIds };
            IEnumerable<TrackingKartableDataOutputDto> result = await _sqlReportConnection.QueryAsync<TrackingKartableDataOutputDto>(query, @params);            
            return result;
        }
        public async Task<IEnumerable<TrackingKartableDataOutputDto>> GetAllArchivedRequest()
        {
            string query = GetAllArchivedTrackingQuery();
            IEnumerable<TrackingKartableDataOutputDto> result = await _sqlReportConnection.QueryAsync<TrackingKartableDataOutputDto>(query, null);
            if (!result.Any())
            {
                throw new InvalidTrackNumberException(ExceptionLiterals.NotFoundAnyOpenTrack);
            }
            return result;
        }   
        public async Task<IEnumerable<UnconfirmedRequestDataOutputDto>> GetUnconfirmedRequestByZoneId(int zoneId)
        {
            string dbName=GetDbName(zoneId);
            string query = GetUnconfirmedRequestByZoneIdQuery(dbName);
            IEnumerable<UnconfirmedRequestDataOutputDto> result = await _sqlReportConnection.QueryAsync<UnconfirmedRequestDataOutputDto>(query, new { zoneId});
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
                    	t.NeighbourBillId,
						t.RequestOrigin RequestOriginId
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
        private string GetByTrackIdQuery()
        {
            return $@"Select
                    	t.TrackID,
                    	t.TrackNumber,
                        t.BillID,
						t46.C0 RegionId,
						t46.C2 RegionTitle,
                    	t.ZoneID, 
						t51.C2 ZoneTitle,
                    	t.DateTimeJalali InsertDateJalali,
                        t.DateAndTime InsertDateTimeGregorian,
                    	t.ServiceGroup_FK ServiceGroupId,
                    	sg.Title ServiceGroupTitle,
                    	t.Status StatusId,
                    	s.Description StatusTitle,
                    	t.InserrtedBy ,
                    	t.Description,
                    	t.NotificationMobile,
                    	t.NeighbourBillId,
						t.Caller,
						t.RequestOrigin RequestOriginId
                    From AbAndFazelab.dbo.Tracking t
                    Join AbAndFazelab.dbo.Status s
                    	ON t.Status=s.StatusID
                    Join AbAndFazelab.dbo.ServiceGroup sg
                    	ON t.ServiceGroup_FK=sg.Id
					Join Db70.dbo.T51 t51
						ON t.ZoneID=t51.C0
					Join Db70.dbo.T46 t46
						ON t51.C1=t46.C0
                    Where t.TrackID=@trackId";
        }
        private string GetLatestByTrackNumberQuery()
        {
            return $@"Select Top 1
                    	t.TrackID,
                    	t.TrackNumber,
                        t.BillID,
						t46.C0 RegionId,
						t46.C2 RegionTitle,
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
                    	t.NeighbourBillId,
						t.Caller,
						t.RequestOrigin RequestOriginId
                    From AbAndFazelab.dbo.Tracking t
                    Join AbAndFazelab.dbo.Status s
                    	ON t.Status=s.StatusID
                    Join AbAndFazelab.dbo.ServiceGroup sg
                    	ON t.ServiceGroup_FK=sg.Id
					Join Db70.dbo.T51 t51
						ON t.ZoneID=t51.C0
					Join Db70.dbo.T46 t46
						ON t51.C1=t46.C0
                    Where t.TrackNumber=@TrackNumber 
					Order By t.DateAndTime Desc";
        }
        private string GetTwoLatestByTrackNumberQuery()
        {
            return $@"Select Top 2
                    	t.TrackID,
                    	t.TrackNumber,
                        t.BillID,
						t46.C0 RegionId,
						t46.C2 RegionTitle,
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
                    	t.NeighbourBillId,
						t.Caller,
						t.RequestOrigin RequestOriginId
                    From AbAndFazelab.dbo.Tracking t
                    Join AbAndFazelab.dbo.Status s
                    	ON t.Status=s.StatusID
                    Join AbAndFazelab.dbo.ServiceGroup sg
                    	ON t.ServiceGroup_FK=sg.Id
					Join Db70.dbo.T51 t51
						ON t.ZoneID=t51.C0
					Join Db70.dbo.T46 t46
						ON t51.C1=t46.C0
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
                    	s.SummaryDescription StatusTitle,
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
                    	t.Status IN (0,15,20,50,60,65,70, /*75,110,150,*/ 90002) AND
                        t.ZoneId IN @zoneIds
                    Order by sg.Title,t.DateAndTime desc";
        }
        private string GetAllArchivedTrackingQuery()
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
                    	t.Status IN (90003)
                    Order by sg.Title,t.DateAndTime desc";
        }
        private string GetUnconfirmedRequestByZoneIdQuery(string dbName)
        {
            return $@";With AllTracking  As(
                    	Select 
                    		*,
                    		Rn=Row_Number() Over(Partition By t.TrackNumber Order By t.DateAndTime Desc)
                    	From AbAndFazelab.dbo.Tracking t
                    	Where t.ZoneID=@zoneId 
                    ),
                    Karts As(
                    	Select 
                    		k.par_no,
                    		MAX(k.total) Total
                    	From [{dbName}].dbo.kart k
                    	Group By k.par_no
                    )
                    Select 
                    	t46.C0 RegionId,
                    	t46.C2 RegionTitle,
                    	t.ZoneID,
                    	t51.C2 ZoneTitle,
                    	t.TrackNumber,
                    	t.BillID,
                    	k.Total Amount,
                    	m.radif CustomerNumber,
                    	TRIM(m.name) FirstName,
                    	TRIM(m.family) Surname,
                    	TRIM(m.name)+' '+TRIM(m.family) FullName,
                    	TRIM(m.meli_cod) NationalCode,
                    	TRIM(m.mobile) MobileNumber,
                    	TRIM(m.phone_no) PhoneNumber,
                    	TRIM(m.post_cod) PostalCode,
                    	TRIM(m.address) Address,
                    	m.cod_enshab UsageId,
                    	t41.C1 UsageTitle
                    From AllTracking t
                    Join [{dbName}].dbo.moshtrak m
                    	ON t.TrackNumber=m.TrackingNumber
                    Join karts k
                    	ON m.par_no=k.par_no
                    Join [Db70].dbo.T51 t51	
                    	ON t.ZoneID=t51.C0
                    Join [Db70].dbo.T46 t46	
                    	ON t51.C1=t46.C0
                    Join [Db70].dbo.T41 t41	
                    	ON m.cod_enshab=t41.C0
                    Where 
                    	t.Rn=1 And 
                    	t.Status=75";
        }
    }
}
