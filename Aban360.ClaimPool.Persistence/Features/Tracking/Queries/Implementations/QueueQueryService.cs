using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.ClaimPool.Persistence.Features.Sms.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Tracking.Queries.Implementations
{
    internal sealed class QueueQueryService : AbstractBaseConnection, IQueueQueryService
    {
        private static string _title = "پیامک های ارسال شده";
        public QueueQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<TrackingSmsHeaderOutputDto, TrackingSmsDataOutputDto>> GetByTrackId(Guid trackId)
        {
            string query = GetByTrackNumberQuery();

            IEnumerable<TrackingSmsDataOutputDto> data = await _sqlReportConnection.QueryAsync<TrackingSmsDataOutputDto>(query, new { trackId });
            TrackingSmsHeaderOutputDto header = new(_title, data.Count());
            ReportOutput<TrackingSmsHeaderOutputDto, TrackingSmsDataOutputDto> result = new(_title, header, data);

            return result;
        }
        public async Task<TrackingSmsDataOutputDto> Get(Guid id)
        {
            string query = GetByQueueIdQuery();

            TrackingSmsDataOutputDto data = await _sqlReportConnection.QueryFirstOrDefaultAsync<TrackingSmsDataOutputDto>(query, new { id });
            return data;
        }
        private string GetByTrackNumberQuery()
        {
            return @"Select
                        q.Id,
                    	q.Text Message,
                    	q.FinalDeliveryState DeliverySatateId,
                    	m.Title DeliverySatateTitle,
	                    q.InsertDateJalali ,
	                    FORMAT(q.InsertDateTime,'HH:mm') InsertTime,    
                        q.Receiver
                    From [Sms].dbo.Queue q
                    Join [Sms].dbo.MagfaDeliveryState m
                    	 On q.FinalDeliveryState=m.Id
                    Where q.TrackingId=@trackId";
        }
        private string GetByQueueIdQuery()
        {
            return @"Select
                        q.Id,
                    	q.Text Message,
                    	q.FinalDeliveryState DeliverySatateId,
                    	m.Title DeliverySatateTitle,
	                    q.InsertDateJalali ,
	                    FORMAT(q.InsertDateTime,'HH:mm') InsertTime,    
                        q.Receiver
                    From [Sms].dbo.Queue q
                    Join [Sms].dbo.MagfaDeliveryState m
                    	 On q.FinalDeliveryState=m.Id
                    Where q.Id=@Id";
        }
    }
}
