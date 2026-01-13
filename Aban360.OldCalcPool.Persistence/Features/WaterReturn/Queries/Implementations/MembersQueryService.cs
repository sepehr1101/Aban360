using Aban360.OldCalcPools.Persistence.Features.WaterReturn.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Dapper;
using Microsoft.Extensions.Configuration;
using Aban360.OldCalcPools.WaterReturn.Dto.Queries;

namespace Aban360.OldCalcPools.Persistence.Features.WaterReturn.Queries.Implementations
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

            MemberGetDto data = await _sqlReportConnection.QueryFirstOrDefaultAsync<MemberGetDto>(query, new { billId });
            if (data is null || data.ZoneId <= 0)
            {
                throw new InvalidBillIdException(ExceptionLiterals.InvalidBillId);
            }

            return data;
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
                    Where TRIM(bill_id)=@billId";
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
