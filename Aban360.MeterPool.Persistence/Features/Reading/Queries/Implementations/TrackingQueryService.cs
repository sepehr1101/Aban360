using Aban360.Common.Db.Dapper;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;
using Aban360.MeterPool.Persistence.Features.Reading.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.MeterPool.Persistence.Features.Reading.Queries.Implementations
{
    internal sealed class TrackingQueryService : AbstractBaseConnection, ITrackingQueryService
    {
        public TrackingQueryService(IConfiguration configuration)
                : base(configuration)
        {
        }

        public async Task<IEnumerable<TrackingGetDto>> Get()
        {
            string query = GetQuery();
            IEnumerable<TrackingGetDto> result = await _sqlReportConnection.QueryAsync<TrackingGetDto>(query);
            return result;
        }
        public async Task<IEnumerable<TrackingGetDto>> Get(Guid id)
        {
            string query = GetByIdQuery();
            IEnumerable<TrackingGetDto> result = await _sqlReportConnection.QueryAsync<TrackingGetDto>(query, new { id });
            return result;
        }
        public async Task<IEnumerable<TrackingGetDto>> Get(Guid readerId, int flowStepId)
        {
            string query = GetByUserIdAndStepFlowIdQuery();
            IEnumerable<TrackingGetDto> result = await _sqlReportConnection.QueryAsync<TrackingGetDto>(query, new { readerId, flowStepId });
            return result;
        }

        private string GetQuery()
        {
            return $@"Select 
						Id,
						BlockId ,
						TrackNumber ,
						TbReadingProgramId,
						ZoneId ,
						ZoneTitle ,
						FlowStepId ,
						FromReadingNumber ,
						ToReadingNumber ,
						ListNumber ,
						InsertBy ,
						InsertDateJalali ,
						InsertDateTime ,
						Seen ,
						CounterReaderCode ,
						CounterReaderId ,
						CounterReaderName ,
						CounterReaderMobile ,
						IsBazdid ,
						IsRoosta,
						FromDate,
						ToDate,
						OverallCustomerCount,
						ItemCustomerCount,
						ProvisionallyPercent,
						ImagePercent,
						HasPreNumber,
						DisplayBillId,
						DisplayCustomerNumber,
						Description,
						X,
						Y,
						DisplayPreDate,
						DisplayMobile,
						HasImage,
						DisplayDebt,
						DisplayIcons
					From [Atlas].reading.Tracking";
        }
        private string GetByIdQuery()
        {
            return $@"Select 
						Id,
						BlockId ,
						TrackNumber ,
						TbReadingProgramId,
						ZoneId ,
						ZoneTitle ,
						FlowStepId ,
						FromReadingNumber ,
						ToReadingNumber ,
						ListNumber ,
						InsertBy ,
						InsertDateJalali ,
						InsertDateTime ,
						Seen ,
						CounterReaderCode ,
						CounterReaderId ,
						CounterReaderName ,
						CounterReaderMobile ,
						IsBazdid ,
						IsRoosta,
						FromDate,
						ToDate,
						OverallCustomerCount,
						ItemCustomerCount,
						ProvisionallyPercent,
						ImagePercent,
						HasPreNumber,
						DisplayBillId,
						DisplayCustomerNumber,
						Description,
						X,
						Y,
						DisplayPreDate,
						DisplayMobile,
						HasImage,
						DisplayDebt,
						DisplayIcons
					From [Atlas].reading.Tracking
					Where Id = @Id;";
        }
        private string GetByUserIdAndStepFlowIdQuery()
        {
            return $@"Select  
						Id,
						BlockId ,
						TrackNumber ,
						TbReadingProgramId,
						ZoneId ,
						ZoneTitle ,
						FlowStepId ,
						FromReadingNumber ,
						ToReadingNumber ,
						ListNumber ,
						InsertBy ,
						InsertDateJalali ,
						InsertDateTime ,
						Seen ,
						CounterReaderCode ,
						CounterReaderId ,
						CounterReaderName ,
						CounterReaderMobile ,
						IsBazdid ,
						IsRoosta,
						FromDate,
						ToDate,
						OverallCustomerCount,
						ItemCustomerCount,
						ProvisionallyPercent,
						ImagePercent,
						HasPreNumber,
						DisplayBillId,
						DisplayCustomerNumber,
						Description,
						X,
						Y,
						DisplayPreDate,
						DisplayMobile,
						HasImage,
						DisplayDebt,
						DisplayIcons
					From [Atlas].Reading.Tracking
					Where 
						CounterReaderId = @ReaderId AND
						FlowStepId = @FlowStepId AND
						Seen = 0
					Order By 
						TrackNumber Asc,
						InsertDateTime Asc ";
        }
    }
}
