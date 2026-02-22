using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Exceptions;
using Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Implementations
{
    internal sealed class ChangeMeterCauseQueryService : AbstractBaseConnection, IChangeMeterCauseQueryService
    {
        public ChangeMeterCauseQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<NumericDictionary> Get(int id)
        {
            string query = GetSingleQuery();
            NumericDictionary result = await _sqlReportConnection.QueryFirstOrDefaultAsync<NumericDictionary>(query, new { id });
            if (result is null)
            {
                throw new InvalidIdException();
            }

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
                    	Id,Title
                    From Db70.dbo.MeterCause
                    Where Id=@id";
        }
        private string GetAllQuery()
        {
                return @"Select 
                    	    Id,Title
                        From Db70.dbo.MeterCause";
        }
    }
}
