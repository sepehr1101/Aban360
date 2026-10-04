using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.CalculationPool.Persistence.Features.ServiceLink.Qeuries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.Features.ServiceLink.Qeuries.Implementations
{
    internal sealed class OtherExpensesItemsQueryService : AbstractBaseConnection, IOtherExpensesItemsQueryService
    {
        public OtherExpensesItemsQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<OtherExpensesItemsGetDto>> Get()
        {
            string query = GetAllQuery();
            IEnumerable<OtherExpensesItemsGetDto> result = await _sqlReportConnection.QueryAsync<OtherExpensesItemsGetDto>(query);
            return result;
        }
        public async Task<OtherExpensesItemsGetDto> Get(int id)
        {
            string query = GetByIdQuery();
            OtherExpensesItemsGetDto? result = await _sqlReportConnection.QueryFirstOrDefaultAsync<OtherExpensesItemsGetDto>(query, new { id });
            if (result == null)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidId);
            }
            return result;
        }
        public async Task<OtherExpensesItemsGetDto> GetByItemId(int id)
        {
            string query = GetByIdQuery();
            OtherExpensesItemsGetDto? result = await _sqlReportConnection.QueryFirstOrDefaultAsync<OtherExpensesItemsGetDto>(query, new { id });
            if (result == null)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidId);
            }
            return result;
        }

        private string GetAllQuery()
        {
            return $@"";
        }
        private string GetByIdQuery()
        {
            return $@"";
        }
    }
}
