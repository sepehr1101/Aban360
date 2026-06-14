using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.Common.Db.Services
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
            MemberInfoGetDto data = await _sqlReportConnection.QueryFirstOrDefaultAsync<MemberInfoGetDto>(query, input);
            if (data is null || data.ZoneId <= 0)
            {
                throw new InvalidBillIdException(ExceptionLiterals.InvalidBillId);
            }
            MemberInfoGetDto result = await GetFromMoshtrak(data, dbName);
            return result;
        }
        public async Task<MemberInfoGetDto> GetFromMoshtrak(MemberInfoGetDto input, string dbName)
        {
            string query = GetMoshtrakInfoQuery(dbName);
            MoshtrakInfoGetDto moshtrakInfo = await _sqlReportConnection.QueryFirstOrDefaultAsync<MoshtrakInfoGetDto>(query, new { customerNumber = input.CustomerNumber });

            input.DiscountCount = moshtrakInfo?.DiscountCount ?? 0;
            input.DiscountId = moshtrakInfo?.DiscountId ?? 0;
            input.DiscountTitle = moshtrakInfo?.DiscountTitle ?? string.Empty;
            input.BlockCode = moshtrakInfo?.BlockCode ?? null;
            return input;
        }

        private string GetZoneIdAndCustomerNumberQuery()
        {
            return @"Select 
						ZoneId,
						ZoneTitle,
						CustomerNumber,
						DeletionStateId
                    From CustomerWarehouse.dbo.Clients	
                    Where 
                    	BillId=@billId AND
                    	ToDayJalali IS NULL";
        }
        private string GetMemeberInfoQuery(string dbName)
        {
            return $@"Select
						m.id,
						m.radif CustomerNumber,
						TRIM(m.bill_id) BillId,
						m.town ZoneId,
						t51.C2 ZoneTitle,
						t46.C0 RegionId,
						t46.C2 RegionTitle,
						TRIM(m.eshtrak) ReadingNumber,
						TRIM(m.name) FirstName,
						TRIM(m.family) AS Surname,
						TRIM(m.name)+' '+TRIM(m.family) FullName,
						TRIM(m.father_nam) FatherName,
						m.enshab MeterDiameterId,
						t5.C2 MeterDiameterTitle,
						m.cod_enshab UsageId,
                    	m.group1 UsageConsumptionId,
						t41.C1 UsageTitle,
						m.tedad_vahd AS OtherUnit,
						m.tedad_tej AS CommercialUnit,
						m.ted_khane AS HouseholdNumber,
						m.tedad_mas AS DomesticUnit,
						m.date_sabt AS RegisterDateJalali,
						m.arse AS Premises,
						m.aian AS OverallImprovement,
						m.aian_tej AS CommercialImprovement,
						m.aian_mas AS DomesticImprovement,
						TRIM(m.address) Address ,
						'' AS HousePlate,
						m.pelak Plaque,
						m.edareh_k AS IsSpecial,
						m.hasf DeletionStateId,
						m.noe_va AS UseStateId,
						t7.C1 UseStateTitle,
						m.master_sif AS MainSiphon,
						m.sif_1 AS Siphon100,
						m.sif_2 AS Siphon125,
						m.sif_3 AS Siphon150,
						m.sif_4 AS Siphon200,
						m.sif_5 AS Siphon5,
						m.sif_6 AS Siphon6,
						m.sif_7 AS Siphon7,
						m.sif_8 AS Siphon8,
						m.sif_mosh_1 AS CommonSiphon1,
						m.fix_mas AS ContractualCapacity,
						m.serial_co AS BodySerial,
						TRIM(m.G_inst_ab) AS MeterInstalltionRegisterDateJalali,
						TRIM(m.G_inst_fas) AS SiphonInstalltionRegisterDateJalali,
						TRIM(m.ask_ab) AS MeterRequestDateJalali,
						TRIM(m.inst_ab) AS MeterInstallationDateJalali,
						TRIM(m.ask_fas) AS SiphonRequestDateJalali,--
						TRIM(m.inst_fas) AS SiphonInstallationDateJalali,

						TRIM(m.POST_COD) PostalCode,
						TRIM(m.PHONE_NO ) AS PhoneNumber,
						TRIM(m.MOBILE) AS MobileNumber,
						TRIM(m.MELI_COD) AS NationalCode,
						0 AS MOJAVZ,
						m.VillageId VillageId,
						m.VillageName VillageName,
						x AS X,
						y AS Y,
						m.Khali_s AS EmptyUnit,
						m.operator AS Operator,
						m.Senf AS Guild,
						TRIM(m.date_KHANE) HouseholdDateJalali ,
						bed_bes DebtAmount
					From [{dbName}].dbo.members m
					Left Join [Db70].dbo.T51 t51
						ON m.town=t51.C0
					Left Join [Db70].dbo.T46 t46
						ON t51.C1=t46.C0
					Left Join [Db70].dbo.T41 t41
						ON m.cod_enshab=t41.C0
					Left Join [Db70].dbo.T5 t5
						ON m.enshab=t5.C0
					Left Join [Db70].dbo.T7 t7
						ON m.noe_va=t7.C0
					Where
						m.town=@ZoneId AND
						m.radif=@CustomerNumber";
        }
        private string GetMoshtrakInfoQuery(string dbName)
        {
            return $@"Select 
						m.BLOCK_COD BlockCode,
						m.ted_takh DiscountCount ,
						m.cod_takh DiscountId ,
						t15.C1 DiscountTitle
					From [131301].dbo.moshtrak m
					Left Join [Db70].dbo.T15 t15
						ON m.cod_takh=t15.C0
					where m.radif=@customerNumber";
        }
    }
}
