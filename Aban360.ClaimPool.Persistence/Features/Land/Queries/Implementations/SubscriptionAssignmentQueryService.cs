using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Implementations
{
    internal sealed class SubscriptionAssignmentQueryService : AbstractBaseConnection, ISubscriptionAssignmentQueryService
    {
        public SubscriptionAssignmentQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<SubscriptionAssignmentGetDto> Get(string input)
        {
            string zoneIdQuery = GetZoneIdQuery();
            ZoneIdCustomerNumber data = await _sqlReportConnection.QueryFirstOrDefaultAsync<ZoneIdCustomerNumber>(zoneIdQuery, new { billId = input });
            if (data == null)
            {
                throw new BaseException(ExceptionLiterals.BillIdNotFound);
            }
            string subscriptionAssignmentQuery = GetSubscriptionAssignmentQuery(data.ZoneId.ToString());
            SubscriptionAssignmentGetDto subscriptionAssignmentGetDto = await _sqlReportConnection.QueryFirstAsync<SubscriptionAssignmentGetDto>(subscriptionAssignmentQuery, new { customerNumber = data.CustomerNumber  ,zoneId=data.ZoneId});

            return subscriptionAssignmentGetDto;
        }

        private string GetZoneIdQuery()
        {
            return @"Select 
                        c.ZoneId,
                        c.CustomerNumber
                     From  [CustomerWarehouse].dbo.Clients c
                     Where 
						c.BillId=@billId AND
						c.ToDayJalali IS NULL";
        }
        private string GetSubscriptionAssignmentQuery(string zoneId)
        {
            return @$"select
                        m.Id,
                        m.X,
                    	m.Y,
                    	TRIM(m.name) AS FirstName,
                    	TRIM(m.family) AS SurName,
                    	TRIM(m.address) AS Address,
                    	TRIM(m.eshtrak) AS ReadingNumber
                    from [{zoneId}].dbo.members m
                    where m.radif=@customerNumber and m.town=@zoneId";
        }

    }
}
