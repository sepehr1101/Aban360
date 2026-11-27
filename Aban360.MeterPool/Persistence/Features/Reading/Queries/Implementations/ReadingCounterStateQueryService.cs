using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;
using Aban360.MeterPool.Persistence.Features.Reading.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.MeterPool.Persistence.Features.Reading.Queries.Implementations
{
    internal sealed class ReadingCounterStateQueryService : AbstractBaseConnection, IReadingCounterStateQueryService
    {
        public ReadingCounterStateQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<ReadingCounterStateGetDto>> Get()
        {
            string query = GetQuery();
            IEnumerable<ReadingCounterStateGetDto> data = await _sqlReportConnection.QueryAsync<ReadingCounterStateGetDto>(query);
            return data;
        }
        public async Task<ReadingCounterStateGetDto> Get(int id,bool hasException)
        {
            string query = GetByIdQuery();
            ReadingCounterStateGetDto? data = await _sqlReportConnection.QueryFirstOrDefaultAsync<ReadingCounterStateGetDto>(query, new { id });
            if (data is null && hasException)
            {
                throw new ReadingException(ExceptionLiterals.InvalidCounterStateId);
            }
            return data;
        }

        private string GetQuery()
        {
            return $@"Select 
                    	Id,
                    	Title,
                    	CanEnterNumber,
                    	IsClose,
                    	CanPriGTCurrent,
                    	IsChange,
                    	ShouldNumber,
                    	IsMalfunction,
                    	HasMeter,
                    	HasImage
                    From Atlas.reading.CounterStatus";
        }
        private string GetByIdQuery()
        {
            return $@"Select 
                    	Id,
                    	Title,
                    	CanEnterNumber,
                    	IsClose,
                    	CanPriGTCurrent,
                    	IsChange,
                    	ShouldNumber,
                    	IsMalfunction,
                    	HasMeter,
                    	HasImage
                    From Atlas.reading.CounterStatus
                    Where Id = @Id";
        }
    }
}
