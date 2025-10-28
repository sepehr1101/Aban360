using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Exceptions;
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

        public async Task<string> Get(short id)
        {
            string Sql() => @"Select Title
                From Aban360.CalculationPool.Offering
                Where Id=@id";

            string? title=await _sqlConnection.QueryFirstOrDefaultAsync<string>(Sql(), new {id=id});
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new InvalidIdException();
            }
            return title;
        }
    }
}
