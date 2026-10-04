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

        public async Task<IEnumerable<OtherExpensesItemsDataDto>> Get()
        {
            string query = GetAllQuery();
            IEnumerable<OtherExpensesItemsDataDto> result = await _sqlReportConnection.QueryAsync<OtherExpensesItemsDataDto>(query);
            return result;
        }
        public async Task<OtherExpensesItemsDataDto> Get(int id)
        {
            string query = GetByIdQuery();
            OtherExpensesItemsDataDto? result = await _sqlReportConnection.QueryFirstOrDefaultAsync<OtherExpensesItemsDataDto>(query, new { id });
            if (result == null)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidId);
            }
            return result;
        }
        public async Task<OtherExpensesItemsDataDto> Get(OtherExpensesItemsGetDto inputDto)
        {
            string query = GetByIdQuery();
            OtherExpensesItemsDataDto? result = await _sqlReportConnection.QueryFirstOrDefaultAsync<OtherExpensesItemsDataDto>(query, inputDto);
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
