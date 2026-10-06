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
            IEnumerable<OtherExpensesItemsDataDto> result = await _sqlConnection.QueryAsync<OtherExpensesItemsDataDto>(query);
            return result;
        }
        public async Task<OtherExpensesItemsDataDto> Get(int id)
        {
            string query = GetByIdQuery();
            OtherExpensesItemsDataDto? result = await _sqlConnection.QueryFirstOrDefaultAsync<OtherExpensesItemsDataDto>(query, new { id });
            if (result == null)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidId);
            }
            return result;
        }
        public async Task<OtherExpensesItemsDataDto?> Get(OtherExpensesItemsGetDto inputDto, bool hasException)
        {
            string query = GetByCustomerInfoQuery();
            OtherExpensesItemsDataDto? result = await _sqlConnection.QueryFirstOrDefaultAsync<OtherExpensesItemsDataDto>(query, inputDto);
            if (result is null && hasException)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidId);
            }
            return result;
        }

        private string GetAllQuery()
        {
            return $@"Select 
                    	Id,
                    	ServiceId,
                    	ServiceTitle,
                    	ZoneId,
                    	ZoneTitle,
                    	UsageId,
                    	UsageTitle,
                    	Amount,
                    	InsertBy,
                    	InsertDateTime,
                    	RemoveBy,
                    	RemoveDateTime
                    From Aban360.CalculationPool.OtherExpensesItmes
                    Where RemoveBy IS NULL";
        }
        private string GetByIdQuery()
        {
            return $@"Select 
                    	Id,
                    	ServiceId,
                    	ServiceTitle,
                    	ZoneId,
                    	ZoneTitle,
                    	UsageId,
                    	UsageTitle,
                    	Amount,
                    	InsertBy,
                    	InsertDateTime,
                    	RemoveBy,
                    	RemoveDateTime
                    From Aban360.CalculationPool.OtherExpensesItmes
                    Where 
                        Id = @Id AND
                        RemoveBy IS NULL";
        }
        private string GetByCustomerInfoQuery()
        {
            return $@"Select 
                    	Id,
                    	ServiceId,
                    	ServiceTitle,
                    	ZoneId,
                    	ZoneTitle,
                    	UsageId,
                    	UsageTitle,
                    	Amount,
                    	InsertBy,
                    	InsertDateTime,
                    	RemoveBy,
                    	RemoveDateTime
                    From Aban360.CalculationPool.OtherExpensesItmes
                    Where 
                       RemoveBy IS NULL AND
	                   ZoneId = @ZoneId AND
	                   UsageId = @UsageId AND
	                   ServiceId = @ServiceId ";
        }
    }
}
