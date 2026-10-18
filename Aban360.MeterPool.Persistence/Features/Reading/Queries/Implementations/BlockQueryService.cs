using Aban360.Common.Db.Dapper;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;
using Aban360.MeterPool.Persistence.Features.Reading.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.MeterPool.Persistence.Features.Reading.Queries.Implementations
{
    internal sealed class BlockQueryService : AbstractBaseConnection, IBlockQueryService
    {
        public BlockQueryService(IConfiguration configuration)
                : base(configuration)
        {
        }

        public async Task<IEnumerable<BlockGetDto>> Get()
        {
            string query = GetQuery();
            IEnumerable<BlockGetDto> result = await _sqlReportConnection.QueryAsync<BlockGetDto>(query);
            return result;
        }
        public async Task<IEnumerable<BlockGetDto>> Get(Guid id)
        {
            string query = GetByIdQuery();
            IEnumerable<BlockGetDto> result = await _sqlReportConnection.QueryAsync<BlockGetDto>(query, new { id });
            return result;
        }

        private string GetQuery()
        {
            return $@"Select 
                    	Id,
                    	BoundId ,
                    	ReadingBlockId ,
                    	ReadingBlockTitle ,
                    	FromReadingNumber ,
                    	ToReadingNumber ,
                    	CustomerCount ,
                    	OrderDigit ,
                    	OrderPersian 
                    From [Atlas].reading.Block
                    Where RemoveBy IS NULL ";
        }
        private string GetByIdQuery()
        {
            return $@"Select 
                    	Id,
                    	BoundId ,
                    	ReadingBlockId ,
                    	ReadingBlockTitle ,
                    	FromReadingNumber ,
                    	ToReadingNumber ,
                    	CustomerCount ,
                    	OrderDigit ,
                    	OrderPersian 
                    From [Atlas].reading.Block
                    Where
                    	RemoveBy IS NULL AND
                    	Id = @Id ";
        }
    }
}
