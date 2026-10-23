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
        public async Task<TrackingGetDto> Get(long trackNumber)
        {
            string query = GetByTrackNumberQuery();
            TrackingGetDto? result = await _sqlReportConnection.QueryFirstOrDefaultAsync<TrackingGetDto>(query, new { trackNumber });
            if (result is null)
            {
                throw new ReadingException(ExceptionLiterals.InvalidTrackNumber);
            }
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
					From [Atlas].reading.Tracking
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
					Where t.TrackNumber = @TrackNumbers
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
    }
}
