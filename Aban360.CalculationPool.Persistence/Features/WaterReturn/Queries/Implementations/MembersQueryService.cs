using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.CalculationPool.Persistence.Features.WaterReturn.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.Features.WaterReturn.Queries.Implementations
{
    internal sealed class MembersQueryService : AbstractBaseConnection, IMembersQueryService
    {
        public MembersQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<MemberGetDto> Get(string billId)
        {
            int zoneId = await GetZoneId(billId);
            string dbName = GetDbName(zoneId);
            string query = GetQuery(dbName);

            MemberGetDto article11 = await _sqlReportConnection.QueryFirstOrDefaultAsync<MemberGetDto>(query, new { billId });

            return article11;
        }

        private async Task<int> GetZoneId(string billId)
        {
            string query = GetZoneId();
            int zoneId = await _sqlReportConnection.QueryFirstOrDefaultAsync<int>(query, new { billId });
            if (zoneId == 0)
            {
                throw new InvalidBillIdException(ExceptionLiterals.BillIdNotFound);
            }

            return zoneId;
        }
        private string GetQuery(string dbName)
        {
            return @$"Select 
                    	town as ZoneId,
	                    radif as CustomerNumber,
                    	eshtrak as ReadingNumber,
                        cod_enshab as UsageId,
                    	noe_va as BranchTypeId,
                    	enshab as MeterDiamterId,
                    	serial_co as BodySerial,
                    	tedad_tej as CommertialUnit,
                    	tedad_mas as DomesticUnit,
                    	tedad_vahd as OtherUnit,
                    	ted_khane as HouseholdNumber,
                    	Khali_s as EmptyUnit
                    From [{dbName}].dbo.members
                    Where bill_id=@billId";
        }
        private string GetZoneId()
        {
            return @"Select ZoneId	
                    From [CustomerWarehouse].dbo.Clients 
                    Where 
                    	BillId=@billId AND
                    	ToDayJalali IS NULL";
        }
    }
}
