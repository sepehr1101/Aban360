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
						t51.C2 ZoneTitle,
	                    radif as CustomerNumber,
                    	TRIM(eshtrak) as ReadingNumber,
						(TRIM(name)+' '+TRIM(family)) FullName,
                        cod_enshab as UsageId,
						t41.C1 UsageTitle,
                    	noe_va as BranchTypeId,
                    	enshab as MeterDiamterId,
                    	serial_co as BodySerial,
                    	tedad_tej as CommertialUnit,
                    	tedad_mas as DomesticUnit,
                    	tedad_vahd as OtherUnit,
                    	ted_khane as HouseholdNumber,
                    	Khali_s as EmptyUnit,
						TRIM(PHONE_NO) PhoneNumber,
						TRIM(MOBILE) MobileNumber,
						TRIM(MELI_COD) NationalCode,
						bed_bes LatestDebt,
                        TRIM(bill_id) BillId
                    From [{dbName}].dbo.members
					Join [Db70].dbo.T51 t51
						ON town=t51.C0
					Join [Db70].dbo.T41 t41
						ON cod_enshab=t41.C0
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
