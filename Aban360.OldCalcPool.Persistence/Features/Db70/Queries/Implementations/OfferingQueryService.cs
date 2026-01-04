using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Implementations
{
    internal sealed class OfferingQueryService : AbstractBaseConnection, IOfferingQueryService
    {
        public OfferingQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<NumericDictionary> Get(SearchByIdInput input)
        {
            string query = GetSingleQuery();
            NumericDictionary result = await _sqlReportConnection.QueryFirstOrDefaultAsync<NumericDictionary>(query, input);

            return result;
        }

        public async Task<IEnumerable<NumericDictionary>> Get()
        {
            string query = GetAllQuery();
            IEnumerable<NumericDictionary> result = await _sqlReportConnection.QueryAsync<NumericDictionary>(query, null);

            return result;
        }


        private string GetSingleQuery()
        {
            return @"Select	
                    	C0 Id,
                    	C1 Title
                    From Db70.dbo.T100
                    Where C0=@id";
        }
        private string GetAllQuery()
        {
            return @"Select	
                    	C0 Id,
                    	C1 Title
                    From Db70.dbo.T100";
        }
    }
}
