using Aban360.Common.Db.Dapper;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;
using Aban360.MeterPool.Persistence.Features.Reading.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.MeterPool.Persistence.Features.Reading.Queries.Implementations
{
    internal sealed class BoundQueryService : AbstractBaseConnection, IBoundQueryService
    {
        public BoundQueryService(IConfiguration configuration)
                : base(configuration)
        {
        }

        public async Task<IEnumerable<BoundGetDto>> Get()
        {
            string query = GetQuery();
            IEnumerable<BoundGetDto> result = await _sqlReportConnection.QueryAsync<BoundGetDto>(query);
            return result;
        }
        public async Task<IEnumerable<BoundGetDto>> Get(Guid id)
        {
            string query = GetByIdQuery();
            IEnumerable<BoundGetDto> result = await _sqlReportConnection.QueryAsync<BoundGetDto>(query, new { id });
            return result;
        }

        private string GetQuery()
        {
            return $@"Select 
                    	Id,
                    	ReadingBoundId ,
                    	ReadingBoundTitle ,
                    	ZoneId ,
                    	ZoneTitle ,
                    	FromReadingNumber ,
                    	ToReadingNumber ,
                    	IsValidated ,
                    	BlockCount ,
                    	CustomerCount ,
                    	Year ,
                    	ReadingPeriodTypeId ,
                    	ReadingPeriodId ,
                    	InsertBy ,
                    	InsertDateTime 
                    From [Atlas].Reading.Bound
                    Where RemoveBy IS NULL ";
        }
        private string GetByIdQuery()
        {
            return $@"Select 
                    	Id,
                    	ReadingBoundId ,
                    	ReadingBoundTitle ,
                    	ZoneId ,
                    	ZoneTitle ,
                    	FromReadingNumber ,
                    	ToReadingNumber ,
                    	IsValidated ,
                    	BlockCount ,
                    	CustomerCount ,
                    	Year ,
                    	ReadingPeriodTypeId ,
                    	ReadingPeriodId ,
                    	InsertBy ,
                    	InsertDateTime 
                    From [Atlas].Reading.Bound
                    Where 
                    	RemoveBy IS NULL AND
                    	Id = @Id";
        }
    }
}
