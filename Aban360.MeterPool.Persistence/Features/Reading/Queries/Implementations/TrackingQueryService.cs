using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
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
        public async Task<TrackingGetDto> Get(Guid id)
        {
            string query = GetByIdQuery();
            TrackingGetDto? result = await _sqlReportConnection.QueryFirstOrDefaultAsync<TrackingGetDto>(query, new { id });
            if (result is null)
            {
                throw new ReadingException(ExceptionLiterals.InvalidTrackingId);
            }
            return result;
        }
        public async Task<TrackingGetDto> GetLatest(long trackNumber)
        {
            string query = GetByTrackNumberQuery();
            TrackingGetDto? result = await _sqlReportConnection.QueryFirstOrDefaultAsync<TrackingGetDto>(query, new { trackNumber });
            if (result is null)
            {
                throw new ReadingException(ExceptionLiterals.InvalidTrackNumber);
            }
            return result;
        }
        public async Task<IEnumerable<TrackingDataDto>> GetFlow(long trackNumber)
        {
            string query = GetFlowByTrackNumberQuery();
            IEnumerable<TrackingDataDto> result = await _sqlReportConnection.QueryAsync<TrackingDataDto>(query, new { trackNumber });

            return result;
        }
        public async Task<IEnumerable<TrackingGetDto>> Get(IEnumerable<long> trackNumbers)
        {
            string query = GetByTrackNumbersQuery();
            IEnumerable<TrackingGetDto> result = await _sqlReportConnection.QueryAsync<TrackingGetDto>(query, new { trackNumbers });
            return result;
        }
        public async Task<IEnumerable<TrackingGetDto>> Get(Guid readerId, int flowStepId)
        {
            string query = GetByUserIdAndStepFlowIdQuery();
            IEnumerable<TrackingGetDto> result = await _sqlReportConnection.QueryAsync<TrackingGetDto>(query, new { readerId, flowStepId });
            return result;
        }
        public async Task<IEnumerable<MeterReadingCartableDataOutputDto>> Get(MeterReadingCartableGetDto input)
        {
            string query = GetCartableQuery();
            IEnumerable<MeterReadingCartableDataOutputDto> result = await _sqlReportConnection.QueryAsync<MeterReadingCartableDataOutputDto>(query, input);
            return result;
        }

        private string GetQuery()
        {
            return $@"Select 
						t.Id,
						t.FirstId,
						t.BlockId ,
						t.TrackNumber ,
						t.TbReadingProgramId,
						t.ZoneId ,
						t.ZoneTitle ,
						t.FlowStepId ,
						s.Title FlowStepTitle,
						t.FromReadingNumber ,
						t.ToReadingNumber ,
						t.ListNumber ,
						t.InsertBy ,
						t.InsertDateJalali ,
						t.InsertDateTime ,
						t.Seen ,
						t.CounterReaderCode ,
						t.CounterReaderId ,
						t.CounterReaderName ,
						t.CounterReaderMobile ,
						t.IsBazdid ,
						t.IsRoosta,
						t.FromDate,
						t.ToDate,
						t.OverallCustomerCount,
						t.ItemCustomerCount,
						t.ProvisionallyPercent,
						t.ImagePercent,
						t.HasPreNumber,
						t.DisplayBillId,
						t.DisplayCustomerNumber,
						t.Description,
						t.X,
						t.Y,
						t.DisplayPreDate,
						t.DisplayMobile,
						t.HasImage,
						t.DisplayDebt,
						t.DisplayIcons
					From [Atlas].reading.Tracking t
					Join [Atlas].reading.FlowStep s
						ON t.FlowStepId=s.Id
					Order By 
						t.TrackNumber Asc,
						t.InsertDateTime Asc";
        }
        private string GetByIdQuery()
        {
            return $@"Select 
						t.Id,
						t.FirstId,
						t.BlockId ,
						t.TrackNumber ,
						t.TbReadingProgramId,
						t.ZoneId ,
						t.ZoneTitle ,
						t.FlowStepId ,
						s.Title FlowStepTitle,
						t.FromReadingNumber ,
						t.ToReadingNumber ,
						t.ListNumber ,
						t.InsertBy ,
						t.InsertDateJalali ,
						t.InsertDateTime ,
						t.Seen ,
						t.CounterReaderCode ,
						t.CounterReaderId ,
						t.CounterReaderName ,
						t.CounterReaderMobile ,
						t.IsBazdid ,
						t.IsRoosta,
						t.FromDate,
						t.ToDate,
						t.OverallCustomerCount,
						t.ItemCustomerCount,
						t.ProvisionallyPercent,
						t.ImagePercent,
						t.HasPreNumber,
						t.DisplayBillId,
						t.DisplayCustomerNumber,
						t.Description,
						t.X,
						t.Y,
						t.DisplayPreDate,
						t.DisplayMobile,
						t.HasImage,
						t.DisplayDebt,
						t.DisplayIcons
					From [Atlas].reading.Tracking t
					Join [Atlas].reading.FlowStep s
						ON t.FlowStepId=s.Id
					Where t.Id = @Id;";
        }
        private string GetByTrackNumberQuery()
        {
            return $@"Select Top 1
						t.Id,
						t.FirstId,
						t.BlockId ,
						t.TrackNumber ,
						t.TbReadingProgramId,
						t.ZoneId ,
						t.ZoneTitle ,
						t.FlowStepId ,
						s.Title FlowStepTitle,
						t.FromReadingNumber ,
						t.ToReadingNumber ,
						t.ListNumber ,
						t.InsertBy ,
						t.InsertDateJalali ,
						t.InsertDateTime ,
						t.Seen ,
						t.CounterReaderCode ,
						t.CounterReaderId ,
						t.CounterReaderName ,
						t.CounterReaderMobile ,
						t.IsBazdid ,
						t.IsRoosta,
						t.FromDate,
						t.ToDate,
						t.OverallCustomerCount,
						t.ItemCustomerCount,
						t.ProvisionallyPercent,
						t.ImagePercent,
						t.HasPreNumber,
						t.DisplayBillId,
						t.DisplayCustomerNumber,
						t.Description,
						t.X,
						t.Y,
						t.DisplayPreDate,
						t.DisplayMobile,
						t.HasImage,
						t.DisplayDebt,
						t.DisplayIcons
					From [Atlas].reading.Tracking t
					Join [Atlas].reading.FlowStep s
						ON t.FlowStepId=s.Id
					Where t.TrackNumber = @TrackNumber
					Order By t.InsertDateTime Desc;";
        }
        private string GetFlowByTrackNumberQuery()
        {
            return $@"Select
						t.Id,
						t.FirstId,
						t.BlockId ,
						t.TrackNumber ,
						t.ZoneId ,
						t.ZoneTitle ,
						t.FlowStepId ,
						s.Title FlowStepTitle,
						t.FromReadingNumber ,
						t.ToReadingNumber ,
						t.InsertBy ,
						t.InsertDateJalali ,
						t.InsertDateTime ,
						t.CounterReaderCode ,
						t.CounterReaderId ,
						t.CounterReaderName ,
						t.CounterReaderMobile ,
						LAG(InsertDateTime) OVER(partition by FirstId Order by InsertDateTime) PreviousInsertDate,
						DATEDIFF(HOUR,lag(InsertDateTime) OVER(partition by FirstId Order by InsertDateTime),InsertDateTime) DistanceWithPreviousStep
					From [Atlas].reading.Tracking t
					Join [Atlas].reading.FlowStep s
						ON t.FlowStepId=s.Id
					Where t.TrackNumber = @TrackNumber
					Order By t.InsertDateTime ASC;";
        }
        private string GetByTrackNumbersQuery()
        {
            return $@"Select Top 1
						t.Id,
						t.FirstId,
						t.BlockId ,
						t.TrackNumber ,
						t.TbReadingProgramId,
						t.ZoneId ,
						t.ZoneTitle ,
						t.FlowStepId ,
						s.Title FlowStepTitle,
						t.FromReadingNumber ,
						t.ToReadingNumber ,
						t.ListNumber ,
						t.InsertBy ,
						t.InsertDateJalali ,
						t.InsertDateTime ,
						t.Seen ,
						t.CounterReaderCode ,
						t.CounterReaderId ,
						t.CounterReaderName ,
						t.CounterReaderMobile ,
						t.IsBazdid ,
						t.IsRoosta,
						t.FromDate,
						t.ToDate,
						t.OverallCustomerCount,
						t.ItemCustomerCount,
						t.ProvisionallyPercent,
						t.ImagePercent,
						t.HasPreNumber,
						t.DisplayBillId,
						t.DisplayCustomerNumber,
						t.Description,
						t.X,
						t.Y,
						t.DisplayPreDate,
						t.DisplayMobile,
						t.HasImage,
						t.DisplayDebt,
						t.DisplayIcons
					From [Atlas].reading.Tracking t
					Join [Atlas].reading.FlowStep s
						ON t.FlowStepId=s.Id
					Where t.TrackNumber In @TrackNumbers
          Order By t.InsertDateTime Desc;";
        }
        private string GetByUserIdAndStepFlowIdQuery()
        {
            return $@"Select  
						t.Id,
						t.FirstId,
						t.BlockId ,
						t.TrackNumber ,
						t.TbReadingProgramId,
						t.ZoneId ,
						t.ZoneTitle ,
						t.FlowStepId ,
						s.Title FlowStepTitle,
						t.FromReadingNumber ,
						t.ToReadingNumber ,
						t.ListNumber ,
						t.InsertBy ,
						t.InsertDateJalali ,
						t.InsertDateTime ,
						t.Seen ,
						t.CounterReaderCode ,
						t.CounterReaderId ,
						t.CounterReaderName ,
						t.CounterReaderMobile ,
						t.IsBazdid ,
						t.IsRoosta,
						t.FromDate,
						t.ToDate,
						t.OverallCustomerCount,
						t.ItemCustomerCount,
						t.ProvisionallyPercent,
						t.ImagePercent,
						t.HasPreNumber,
						t.DisplayBillId,
						t.DisplayCustomerNumber,
						t.Description,
						t.X,
						t.Y,
						t.DisplayPreDate,
						t.DisplayMobile,
						t.HasImage,
						t.DisplayDebt,
						t.DisplayIcons
					From [Atlas].Reading.Tracking t
					Join [Atlas].reading.FlowStep s
						ON t.FlowStepId=s.Id
					Where 
						t.CounterReaderId = @ReaderId AND
						t.FlowStepId = @FlowStepId AND
						t.Seen = 0
					Order By 
						t.TrackNumber Asc,
						t.InsertDateTime Asc ";
        }
        private string GetCartableQuery()
        {
            return $@"Select 
						t.Id,
						t.FirstId FirstTrackingId,
						t.BlockId,
						t.TrackNumber,
						t.ZoneId,
						t51.C2 ZoneTitle,
						t.FromReadingNumber,
						t.ToReadingNumber,
						firstT.InsertDateJalali TrackingStartDateJalali,
						firstT.InsertDateTime TrackingStartDateTime,
						t.CounterReaderCode,
						t.CounterReaderName,
						t.CounterReaderId,
						t.ItemCustomerCount,
						t.OverallCustomerCount
					From Atlas.reading.Tracking t
					Join Atlas.reading.Tracking firstT
						ON t.FirstId=firstT.FirstId AND firstT.FlowStepId=1
					Join Atlas.reading.FlowStep ft
						ON t.FlowStepId=ft.Id
					Join Db70.dbo.T51 t51	
						On t.ZoneId=t51.C0
					Where 
						t.Seen = 0 AND
						t.ZoneId IN @ZoneIds AND
						@FromReadingNumber IS NULL OR
						@ToReadingNumber IS NULL OR
						(@ToReadingNumber>t.FromReadingNumber AND @FromReadingNumber<t.ToReadingNumber)";
        }
    }
}
