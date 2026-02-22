using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.Common.Db.QueryServices
{
    public interface ICommonMemberQueryService
    {
        Task<ZoneIdAndCustomerNumber> Get(string billId);
        Task<MemberInfoGetDto> Get(ZoneIdAndCustomerNumber input);
    }
    public sealed class CommonMemberQueryService : AbstractBaseConnection, ICommonMemberQueryService
    {
        public CommonMemberQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ZoneIdAndCustomerNumber> Get(string billId)
        {
            string query = GetZoneIdAndCustomerNumberQuery();
            ZoneIdAndCustomerNumber result = await _sqlReportConnection.QueryFirstOrDefaultAsync<ZoneIdAndCustomerNumber>(query, new { billId });
            if (result == null || result.ZoneId <= 0)
            {
                throw new InvalidBillIdException(ExceptionLiterals.InvalidBillId);
            }
            return result;
        }
        public async Task<MemberInfoGetDto> Get(ZoneIdAndCustomerNumber input)
        {
            string dbName = GetDbName(input.ZoneId);
            string query = GetMemeberInfoQuery(dbName);
            MemberInfoGetDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<MemberInfoGetDto>(query, input);
            if (result is null || result.ZoneId <= 0)
            {
                throw new InvalidBillIdException(ExceptionLiterals.InvalidBillId);
            }
			return result;
        }

        private string GetZoneIdAndCustomerNumberQuery()
        {
            return @"Select ZoneId,CustomerNumber
                    From CustomerWarehouse.dbo.Clients	
                    Where 
                    	BillId=@billId AND
                    	ToDayJalali IS NULL";
        }
        private string GetMemeberInfoQuery(string dbName)
        {
            return $@"Select
						m.radif CustomerNumber,
						m.bill_id BillId,
						m.town ZoneId,
						m.eshtrak ReadingNumber,
						m.name FirstName,
						m.family AS Surname,
						m.father_nam FatherName,
						m.enshab WaterDiameterId,
						m.cod_enshab UsageId,
						m.tedad_vahd AS OtherUnit,
						m.tedad_tej AS CommercialUnit,
						m.ted_khane AS HouseholdNumber,
						m.tedad_mas AS DomesticUnit,
						m.date_sabt AS RegisterDateJalali,
						m.arse AS Premises,
						m.aian AS OverallImprovement,
						m.aian_tej AS CommercialImprovement,
						m.aian_mas AS DomesticImprovement,
						m.ask_ab AS MeterRequestDateJalali,
						m.inst_ab AS MeterInstallationDateJalali,
						m.ask_fas AS SiphonRequestDateJalali,--
						m.inst_fas AS SiphonInstallationDateJalali,
						m.address Address ,
						'' AS HousePlate,
						m.edareh_k AS IsSpecial,
						m.hasf DeletionStateId,
						m.noe_va AS UseStateId,
						m.master_sif AS MainSiphon,
						m.sif_1 AS Siphon1,
						m.sif_2 AS Siphon2,
						m.sif_3 AS Siphon3,
						m.sif_4 AS Siphon4,
						m.sif_5 AS Siphon5,
						m.sif_6 AS Siphon6,
						m.sif_7 AS Siphon7,
						m.sif_8 AS Siphon8,
						m.sif_mosh_1 AS CommonSiphon1,
						m.fix_mas AS ContractualCapacity,
						m.serial_co AS BodySerial,
						m.G_inst_ab AS WaterInstalltionRegistareDate,
						m.G_inst_fas AS SewageInstalltionRegistareDate,
						m.POST_COD PostalCode,
						m.PHONE_NO  AS PhoneNumber,
						m.MOBILE AS MobileNumber,
						m.MELI_COD AS NationalCode,
						0 AS MOJAVZ,
						m.VillageId VillageId,
						m.VillageName VillageName,
						x AS X,
						y AS Y,
						m.Khali_s AS EmptyUnit,
						m.operator AS Operator,
						m.Senf AS Guild,
						m.date_KHANE HouseholdDateJalali 
					From [{dbName}].dbo.members m
					Where
						m.town=@ZoneId AND
						m.radif=@CustomerNumber";
        }
    }
}
