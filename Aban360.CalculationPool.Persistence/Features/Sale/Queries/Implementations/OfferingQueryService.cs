using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Exceptions;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.Features.Sale.Queries.Implementations
{
    internal sealed class OfferingQueryService : AbstractBaseConnection, IOfferingQueryService
    {
        public OfferingQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<NumericDictionary> Get(short id)
        {
            string query = GetByIdQuery();
            NumericDictionary? data = await _sqlReportConnection.QueryFirstOrDefaultAsync<NumericDictionary>(query, new { id = id });
            if (data is null)
            {
                throw new ReadingException(ExceptionLiterals.NotFoundOfferingId);
            }
            return data;
        }
        public async Task<IEnumerable<NumericDictionary>> Get(IEnumerable<short> ids)
        {
            string query = GetByIdQuery();
            IEnumerable<NumericDictionary> data = await _sqlReportConnection.QueryAsync<NumericDictionary>(query, new { ids = ids });
            return data;
        }

        private string GetByIdQuery()
        {
            return $@"Select 
                        C0 Id,
                        C1 Title 
                      From Db70.dbo.T100 
                      Where C0 = @id";
        }
        private string GetByIdsQuery()
        {
            return $@"Select 
                        C0 Id,
                        C1 Title
                      From Db70.dbo.T100 
                      Where C0 IN @ids";
        }
    }
}
