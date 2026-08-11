using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.ClaimPool.Persistence.Features.Tracking.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Tracking.Queries.Implementations
{
    internal sealed class TrackingKartableQueryService : AbstractBaseConnection, ITrackingKartableQueryService
    {
        private static string _title = "پیگیری درخواست";
        public TrackingKartableQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<TrackingDisplayFlowHeaderOutputDto, TrackingDisplayFlowDateOutputDto>> Get(int trackNumber)
        {
            IEnumerable<TrackingDisplayFlowDateOutputDto> data = await GetDataByTrackNumber(trackNumber);
            string dbName = GetDbName(data.FirstOrDefault().ZoneId);
            TrackingDisplayFlowHeaderOutputDto header = await GetHeaderByTrackNumber(trackNumber, dbName);
            header.BillId = data.LastOrDefault().BillId;
            return new ReportOutput<TrackingDisplayFlowHeaderOutputDto, TrackingDisplayFlowDateOutputDto>(_title, header, data);
        }
        public async Task<string?> RepairBillId(int trackNumber)
        {
            TrackingBillIdRepairDto? trackingInfo = await _sqlReportConnection.QueryFirstOrDefaultAsync<TrackingBillIdRepairDto>(
                GetTrackingInfoForBillIdRepairQuery(), new { trackNumber });
            if (trackingInfo is null)
            {
                return null;
            }
            if(!string.IsNullOrWhiteSpace(trackingInfo.BillId) && trackingInfo.BillId.Length>6)
            {
                return trackingInfo.BillId;
            }

            string dbName = GetDbName(trackingInfo.ZoneId);
            string? billId = await _sqlReportConnection.QueryFirstOrDefaultAsync<string?>(
                GetBillIdFromGhestQuery(dbName), new { trackNumber });

            if (string.IsNullOrWhiteSpace(billId) || billId.Length < 6)
            {
                int? customerNumber = await _sqlReportConnection.QueryFirstOrDefaultAsync<int?>(
                    GetCustomerNumberFromMoshtrakQuery(dbName), new { trackNumber });
                if (customerNumber is null || customerNumber==0)
                {
                    return null;
                }

                billId = await _sqlReportConnection.QueryFirstOrDefaultAsync<string?>(
                    GetBillIdFromMembersQuery(dbName), new { customerNumber });
            }

            if (string.IsNullOrWhiteSpace(billId) || billId.Length<6)
            {
                return null;
            }

            await _sqlReportConnection.ExecuteAsync(
                GetTrackingBillIdUpdateCommand(), new { trackingInfo.TrackId, billId });
            return billId;
        }
        private async Task<IEnumerable<TrackingDisplayFlowDateOutputDto>> GetDataByTrackNumber(int trackNumber)
        {
            string query = GetDataQuery();
            IEnumerable<TrackingDisplayFlowDateOutputDto> data = await _sqlReportConnection.QueryAsync<TrackingDisplayFlowDateOutputDto>(query, new { trackNumber });
            if (!data.Any())
            {
                throw new InvalidTrackNumberException(ExceptionLiterals.InvalidTrackNumber);
            }

            return data;
        }
        private async Task<TrackingDisplayFlowHeaderOutputDto> GetHeaderByTrackNumber(int trackNumber, string dbName)
        {
            string query = GetHeaderQuery(dbName);
            TrackingDisplayFlowHeaderOutputDto header = await _sqlReportConnection.QueryFirstOrDefaultAsync<TrackingDisplayFlowHeaderOutputDto>(query, new { trackNumber });
            if (header is null || header.ZoneId < 0)
            {
                throw new InvalidTrackNumberException(ExceptionLiterals.InvalidTrackNumber);
            }
            header.Title = _title;
            header.ReportDateJalali = DateTime.Now.ToShortPersianDateString();

            return header;
        }
        private string GetDataQuery()
        {
            return @"Select 
	                    t.ZoneId,
                    	TrackID TrackingId,
                    	t.Status StatusId,
                    	s.SummaryDescription StatusTitle,
                    	t.DateTimeJalali RegisterDateJalali,
                    	Format(t.DateAndTime,'HH:mm') RegisterTime,
                    	u.DisplayName UserDisplayName,
                    	s.HasDetails,
                    	s.HasSms,
						t.Description,
                        t.BillId
                    From [AbAndFazelab].dbo.Tracking t
                    Join [AbAndFazelab].dbo.Status s
                    	On t.Status=s.StatusID
                    Left Join AuthDb.dbo.[Users] u
                    	On t.InserrtedBy=u.UserCode
                    Where trackNumber=@trackNumber
                    Order by t.DateAndTime ASC";
        }
        private string GetHeaderQuery(string dbName)
        {
            return $@"Select 
                    	m.town ZoneId,
                    	t51.C2 ZoneTitle,
                        IIF(mem.radif IS NULL or mem.radif=0, 0, 1) HasBillId,
                    	m.radif CustomerNumber,
                    	TRIM(m.name) FirstName,
                    	TRIM(m.family) Surname,
                    	m.C99 MobileNumber,
	                    m.trackingNumber TrackNumber
                    From [{dbName}].dbo.moshtrak m
                    Left join [Db70].dbo.t51 t51
                    	ON m.town=t51.C0
                    LEFT JOIN [{dbName}].dbo.members mem
						ON m.town=mem.town AND m.radif=mem.radif
                    where m.trackingNumber=@trackNumber	";
        }
        private string GetDuplicateQuery()
        {
            return @"Select top 1  
                    	ZoneID,
                    	TrackNumber
                    From AbAndFazelab.dbo.tracking 
                    where tracknumber=@trackNumber
                    Order by DateAndTime ";
        }

        private string GetTrackingInfoForBillIdRepairQuery()
        {
            return @"Select Top 1
                        TrackID,
                        ZoneID,
                        BillID BillId
                    From [AbAndFazelab].dbo.Tracking
                    Where
                        TrackNumber=@trackNumber AND
                        IsConsiderd=0
                    Order By DateAndTime Desc";
        }
        private string GetBillIdFromGhestQuery(string dbName)
        {
            return $@"Select Top 1 TRIM(sh_ghabs1)
                    From [{dbName}].dbo.ghest
                    Where
                        par_no=REPLICATE('0', 11-LEN(CAST(@trackNumber AS varchar(11))))+CAST(@trackNumber AS varchar(11)) AND
                        NULLIF(TRIM(sh_ghabs1), '') IS NOT NULL";
        }
        private string GetCustomerNumberFromMoshtrakQuery(string dbName)
        {
            return $@"Select Top 1 radif
                    From [{dbName}].dbo.moshtrak
                    Where par_no=REPLICATE('0', 11-LEN(CAST(@trackNumber AS varchar(11))))+CAST(@trackNumber AS varchar(11))";
        }
        private string GetBillIdFromMembersQuery(string dbName)
        {
            return $@"Select Top 1 TRIM(bill_id)
                    From [{dbName}].dbo.members
                    Where
                        radif=@customerNumber AND
                        NULLIF(TRIM(bill_id), '') IS NOT NULL";
        }
        private string GetTrackingBillIdUpdateCommand()
        {
            return @"Update [AbAndFazelab].dbo.Tracking
                    Set BillID=@billId
                    Where TrackID=@trackId";
        }

        private sealed record TrackingBillIdRepairDto
        {
            public Guid TrackId { get; init; }
            public int ZoneId { get; init; }
            public string? BillId { get; set; }
        }

    }
}
