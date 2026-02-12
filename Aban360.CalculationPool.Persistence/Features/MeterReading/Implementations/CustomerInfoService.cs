using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Implementations
{
    internal sealed class CustomerInfoService : AbstractBaseConnection, ICustomerInfoService
    {
        public CustomerInfoService(IConfiguration configuration)
            : base(configuration)
        {
        }


        public async Task<ZoneIdAndCustomerNumberGetDto> GetZoneIdAndCustomerNumber(string billId)
        {
            string query = GetZoneIdAndCustomerNumberQuery();
            ZoneIdAndCustomerNumberGetDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<ZoneIdAndCustomerNumberGetDto>(query, new { billId = billId });
            if (result is null)
            {
                throw new InvalidBillIdException(ExceptionLiterals.BillIdNotFound);
            }

            return result;
        }
        public async Task<CustomerInfoGetDto> Get(int zoneId, int customerNumber)
        {
            string dbName = GetDbName(zoneId);
            string memberQuery = GetMembers(dbName);
            string bedBesQuery = GetBedBesQuery(dbName);
            string tavizQuery = GetTavisQuery(dbName);

            MembersInfo membersInfo = await _sqlReportConnection.QueryFirstOrDefaultAsync<MembersInfo>(memberQuery, new { zoneId, customerNumber });
            LatesTavizInfo latestTavizInfo = await _sqlReportConnection.QueryFirstOrDefaultAsync<LatesTavizInfo>(tavizQuery, new { zoneId, customerNumber });
            LatestBedBesConsumptionInfo latestBedBesInfo = await _sqlReportConnection.QueryFirstOrDefaultAsync<LatestBedBesConsumptionInfo>(bedBesQuery, new { zoneId, customerNumber });

            return new CustomerInfoGetDto(membersInfo, latestBedBesInfo, latestTavizInfo);
        }
        public async Task<CustomersInfoGetDto> Get(int zoneId, ICollection<int> customerNumbers)
        {
            string dbName = GetDbName(zoneId);
            string memberQuery = GetAllMembers(dbName);
            string bedBesQuery = GetAllBedBesQuery(dbName);
            string tavizQuery = GetAllTavisQuery(dbName);

            IEnumerable<MembersInfo> membersInfo = await _sqlReportConnection.QueryAsync<MembersInfo>(memberQuery, new { zoneId, customerNumbers });
            IEnumerable<LatesTavizInfo> latestTavizInfo = await _sqlReportConnection.QueryAsync<LatesTavizInfo>(tavizQuery, new { zoneId, customerNumbers });
            IEnumerable<LatestBedBesConsumptionInfo> latestBedBesInfo = await _sqlReportConnection.QueryAsync<LatestBedBesConsumptionInfo>(bedBesQuery, new { zoneId, customerNumbers });

            return new CustomersInfoGetDto(membersInfo, latestBedBesInfo, latestTavizInfo);
        }
        public async Task<CustomerGeneralInfoGetDto> Get(string billId)
        {
            ZoneIdAndCustomerNumberGetDto zoneCustomerNumber = await GetZoneIdAndCustomerNumber(billId);
            string dbName = GetDbName(zoneCustomerNumber.ZoneId);
            string query = GetCustomerGeneralInfoQuery(dbName);

            CustomerGeneralInfoGetDto data = await _sqlReportConnection.QueryFirstOrDefaultAsync<CustomerGeneralInfoGetDto>(query, new { zoneCustomerNumber.ZoneId, zoneCustomerNumber.CustomerNumber });
            if (data is null)
            {
                throw new InvalidBillIdException(ExceptionLiterals.BillIdNotFound);
            }
			return data;
        }

        private string GetZoneIdAndCustomerNumberQuery()
        {
            return @"Select 
						ZoneId,
						ZoneTitle,
						CustomerNumber
					From CustomerWarehouse.dbo.Clients 
					Where
						ToDayJalali IS NULL AND
						BillId=@billId";
        }

        private string GetMembers(string dbName)
        {
            return @$"Select
						m.town as ZoneId,
						m.radif as Radif,
						m.bill_id as BillId,
						Trim(m.bill_id) as BillId,
						m.noe_va as BranchType,
						m.cod_enshab as UsageId,
						m.tedad_mas as DomesticUnit,
						m.tedad_tej as CommercialUnit,
						m.tedad_vahd as OtherUnit,
						m.inst_ab as WaterInstallationDateJalali,
						m.inst_fas as SewageInstallationDateJalali,
					    m.g_inst_ab WaterRegisterDate,
					    m.g_inst_fas SewageRegisterDate,
						m.n_ab as WaterCount,
						m.n_faz as SewageCalcState,
						m.fix_mas as ContractualCapacity,
					    m.ted_khane as HouseholdNumber,
					    m.date_KHANE as HouseholdDate,
					    m.EJUCA as VirtualCategoryId,
						m.eshtrak as ReadingNumber,
					    TRIM(m.VillageId) as VillageId,
						m.edareh_k as IsSpecial,
						m.enshab as MeterDiameterId,
						m.Khali_s as EmptyUnit
					From [{dbName}].dbo.members m
					Where 
						m.town=@zoneId AND
						m.radif=@customerNumber;";
        }
        private string GetBedBesQuery(string dbName)
        {
            return $@"Select TOP 1
						b.today_date as LastMeterDateJalali ,
						b.today_no as LastMeterNumber,
						b.rate as LastConsumptionAverage,
						b.cod_vas as LastCounterStateCode
					From [{dbName}].dbo.bed_bes b
					Where
						b.town=@zoneId AND
						b.radif=@customerNumber
					Order By today_date DESC;";
        }
        private string GetTavisQuery(string dbName)
        {
            return $@"Select 
						t.taviz_date as TavizDateJalali,
						t.elat as TavizCause,
						t.date_sabt as TavizRegisterDateJalali,
						t.taviz_no as TavizNumber
					From [{dbName}].dbo.taviz t
					Where
						t.town=@zoneId AND
						t.radif=@customerNumber;";
        }


        private string GetAllMembers(string dbName)
        {

            return @$"Select
						m.town as ZoneId,
						m.radif as CustomerNumber,
						m.bill_id as BillId,
						Trim(m.bill_id) as BillId,
						m.noe_va as BranchTypeId,
						m.cod_enshab as UsageId,
						m.group1 as ConsumptionUsageId,
						m.tedad_mas as DomesticUnit,
						m.tedad_tej as CommercialUnit,
						m.tedad_vahd as OtherUnit,
						m.inst_ab as WaterInstallationDateJalali,
						m.inst_fas as SewageInstallationDateJalali,
					    m.g_inst_ab WaterRegisterDate,
					    m.g_inst_fas SewageRegisterDate,
						m.n_ab as WaterCount,
						m.n_faz as SewageCalcState,
						m.fix_mas as ContractualCapacity,
					    m.ted_khane as HouseholdNumber,
					    m.date_KHANE as HouseholdDate,
					    m.EJUCA as VirtualCategoryId,
						m.eshtrak as ReadingNumber,
					    TRIM(m.VillageId) as VillageId,
						m.edareh_k as IsSpecial,
						m.enshab as MeterDiameterId,
						m.Khali_s as EmptyUnit,
						m.serial_co as BodySerial
					From [{dbName}].dbo.members m
					Where 
						m.town=@zoneId AND
						m.radif IN @customerNumbers;";
        }
        private string GetAllBedBesQuery(string dbName)
        {
            return $@";With CTE AS (
						Select 
							b.town,
							b.radif,
							b.today_date,
							b.today_no,
							b.rate,
							b.masraf,
							b.baha,
							b.cod_vas,
							RN= ROW_NUMBER() OVER(Partition By b.radif Order By b.today_date DESC)
						From [{dbName}].dbo.bed_bes b
						where b.radif in @customerNumbers
					)
					Select
						c.radif as CustomerNumber,
						c.today_date as LastMeterDateJalali ,
						c.today_no as LastMeterNumber,
						c.masraf as LastConsumption,
						c.rate as LastMonthlyConsumption,
						c.cod_vas as LastCounterStateCode,
						c.baha as LastSumItems
					From CTE c
					Where
						c.town=@zoneId AND
						c.radif IN @customerNumbers AND
						c.RN=1";
        }
        private string GetAllTavisQuery(string dbName)
        {
            return $@";With CTE AS (
						Select 
							t.town,
							t.radif,
							t.taviz_date,
							t.elat,
							t.date_sabt,
							t.taviz_no,
							RN= ROW_NUMBER() OVER(Partition By t.radif Order By t.taviz_date DESC)
						From [{dbName}].dbo.taviz t
						where t.radif in @customerNumbers
					)
					Select
						c.radif as CustomerNumber,
						c.taviz_date as TavizDateJalali,
						c.elat as TavizCause,
						c.date_sabt as TavizRegisterDateJalali,
						c.taviz_no as TavizNumber
					From CTE c
					Where
						c.town=@zoneId AND
						c.radif IN @customerNumbers AND
						c.RN=1";
        }

        private string GetCustomerGeneralInfoQuery(string dbnName)
        {
            return $@"Select 
						t51.C2 ZoneTitle,
						m.bill_id BillId,
						TRIM(m.eshtrak) ReadingNumber,
						TRIM(m.name) FirstName,
						TRIM(m.family) Surname,
						TRIM(m.father_nam) FatherName,
						TRIM(m.Address) Address,
						t41.C1 UsageTitle,
						TRIM(m.PHONE_NO) PhoneNumber,
						TRIM(m.MOBILE) MobileNumber
					From [{dbnName}].dbo.members m
					Left Join [Db70].dbo.T51 t51
						ON m.town=t51.C0
					Left Join [Db70].dbo.T41 t41
						ON m.cod_enshab=t41.C0
					Where 
						m.town=@zoneId AND
						m.radif=@customerNumber";
        }
    }
}
