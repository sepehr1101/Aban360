using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.ClaimPool.Persistence.Features.Tracking.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Aban360.ClaimPool.Persistence.Features.Tracking.Queries.Implementations
{
    internal sealed class TrackingQueryService : AbstractBaseConnection, ITrackingQueryService
    {
        private static string _title = "پیگیری درخواست";
        public TrackingQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<TrackingDisplayFlowHeaderOutputDto, TrackingDisplayFlowDateOutputDto>> Get(int trackNumber)
        {
            IEnumerable<TrackingDisplayFlowDateOutputDto> data = await GetDataByTrackNumber(trackNumber);
            string dbName = GetDbName(data.FirstOrDefault().ZoneId);
            TrackingDisplayFlowHeaderOutputDto header = await GetHeaderByTrackNumber(trackNumber, dbName);

            return new ReportOutput<TrackingDisplayFlowHeaderOutputDto, TrackingDisplayFlowDateOutputDto>(_title, header, data);
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
						t.Description
                    From [AbAndFazelab].dbo.Tracking t
                    Join [AbAndFazelab].dbo.Status s
                    	On t.Status=s.StatusID
                    Join AuthDb.dbo.[Users] u
                    	On t.InserrtedBy=u.UserCode
                    Where trackNumber=@trackNumber
                    Order by t.DateAndTime ASC";
        }
        private string GetHeaderQuery(string dbName)
        {
            return $@"Select 
                    	m.town ZoneId,
                    	t51.C2 ZoneTitle,
                    	m.radif CustomerNumber,
                    	TRIM(m.name) FirstName,
                    	TRIM(m.family) Surname,
                    	m.C99 MobileNumber,
	                    m.trackingNumber TrackNumber
                    From [{dbName}].dbo.moshtrak m
                    Left join [Db70].dbo.t51 t51
                    	ON m.town=t51.C0
                    where m.trackingNumber=@trackNumber	";
        }

    }
}
