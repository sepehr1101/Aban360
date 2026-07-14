using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Implementations
{
    internal sealed class CustomerInfoService : AbstractBaseConnection, ICustomerInfoService
    {
        public CustomerInfoService(IConfiguration configuration)
            : base(configuration)
        {
        }


        private async Task<ZoneIdAndCustomerNumberGetDto> GetZoneIdAndCustomerNumber(string billId)
        {
            string query = GetZoneIdAndCustomerNumberQuery();
            ZoneIdAndCustomerNumberGetDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<ZoneIdAndCustomerNumberGetDto>(query, new { billId });
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
            string memberQuery = GetAllMembers(dbName, false);
            string bedBesQuery = GetAllBedBesQuery(dbName, false);
            string tavizQuery = GetAllTavisQuery(dbName, false);

            IEnumerable<MembersInfo> membersInfo = await _sqlReportConnection.QueryAsync<MembersInfo>(memberQuery, new { zoneId, customerNumbers });
            IEnumerable<LatesTavizInfo> latestTavizInfo = await _sqlReportConnection.QueryAsync<LatesTavizInfo>(tavizQuery, new { zoneId, customerNumbers });
            IEnumerable<LatestBedBesConsumptionInfo> latestBedBesInfo = await _sqlReportConnection.QueryAsync<LatestBedBesConsumptionInfo>(bedBesQuery, new { zoneId, customerNumbers });

            return new CustomersInfoGetDto(membersInfo, latestBedBesInfo, latestTavizInfo);
        }
        public async Task<CustomersInfoGetDto> GetByBulkCopy(IDbConnection connection, IDbTransaction transaction, int zoneId, ICollection<int> customerNumbers)
        {
            var table = new DataTable();
            table.Columns.Add("ZoneId", typeof(int));
            table.Columns.Add("CustomerNumbers", typeof(int));
            foreach (var item in customerNumbers)
                table.Rows.Add(zoneId, item);

            string tempTableCreateCommand = "Create Table #TempCustomerNumbers" +
                                                "(ZoneId int Not Null," +
                                                "CustomerNumber int Not NUll)";
            await connection.ExecuteAsync(tempTableCreateCommand, null, transaction);
            using (var bulkCopy = new SqlBulkCopy((SqlConnection)connection, SqlBulkCopyOptions.Default, (SqlTransaction)transaction))
            {
                bulkCopy.DestinationTableName = "#TempCustomerNumbers";
                bulkCopy.BatchSize = 10000;
                await bulkCopy.WriteToServerAsync(table);
            }

            string dbName = GetDbName(zoneId);
            string memberQuery = GetAllMembers(dbName, true);
            string bedBesQuery = GetAllBedBesQuery(dbName, true);
            string tavizQuery = GetAllTavisQuery(dbName, true);

            IEnumerable<MembersInfo> membersInfo = await connection.QueryAsync<MembersInfo>(memberQuery, null, transaction);
            IEnumerable<LatesTavizInfo> latestTavizInfo = await connection.QueryAsync<LatesTavizInfo>(tavizQuery, null, transaction);
            IEnumerable<LatestBedBesConsumptionInfo> latestBedBesInfo = await connection.QueryAsync<LatestBedBesConsumptionInfo>(bedBesQuery, null, transaction);

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
        public async Task<double> GetMembersBedBes(ZoneIdAndCustomerNumber input)
        {
            string dbName = GetDbName(input.ZoneId);
            string query = GetMembersBedBesQueru(dbName);
            double bedBesAmount = await _sqlReportConnection.QueryFirstOrDefaultAsync<double>(query, input);
            return bedBesAmount;
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
						BillId=@billId ";
        }

        private string GetMembers(string dbName)
        {
            return @$"Select
						m.town as ZoneId,
						m.radif as CustomerNumber,
						Trim(m.bill_id) as BillId,
						m.noe_va as BranchTypeId,
						m.cod_enshab as UsageId,
						m.group1 ConsumptionUsageId,
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
						m.bed_bes LatestDebtAmount
					From [{dbName}].dbo.members m
					Where 
						m.town=@zoneId AND
						m.radif=@customerNumber;";
        }
        private string GetBedBesQuery(string dbName)
        {
            return $@"Select TOP 1	
						b.town ZoneId,
						b.radif as CustomerNumber,
						b.sh_ghabs1 BillId,
						b.today_date as LastMeterDateJalali ,
						b.today_no as LastMeterNumber,
						b.rate as LastConsumptionAverage,
						b.cod_vas as LastCounterStateCode
					From [{dbName}].dbo.bed_bes b
					Where
						b.town=@zoneId AND
						b.radif=@customerNumber  AND
						b.cod_vas NOT IN (4,7,8)
					Order By today_date DESC, Id DESC;";
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
						t.radif=@customerNumber
					Order by t.taviz_date Desc;";
        }


        private string GetAllMembers(string dbName, bool isSqlBulkCopy)
        {
            string conditionQuery = isSqlBulkCopy ?
                @"Join #TempCustomerNumbers t 
					ON 	m.town = t.ZoneId AND
						m.radif = t.CustomerNumber" :
                @"Where 
						m.town = @zoneId AND
						m.radif IN @customerNumbers";
            return @$"Select
						m.town as ZoneId,
						m.radif as CustomerNumber,
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
						Trim(m.serial_co) as BodySerial
					From [{dbName}].dbo.members m
					{conditionQuery};";
        }
        private string GetAllBedBesQuery(string dbName, bool isSqlBulkCopy)
        {
            string conditionQuery = isSqlBulkCopy ?
                @"Join #TempCustomerNumbers t 
					ON 	b.town = t.ZoneId AND
						b.radif = t.CustomerNumber" :
                @"Where 
						b.town = @zoneId AND
						b.radif IN @customerNumbers";

            return $@";With CTE AS (
						Select 
							b.town,
							b.radif,
							b.sh_ghabs1,
							b.today_date,
							b.today_no,
							b.rate,
							b.masraf,
							b.baha,
							b.cod_vas,
							b.del,
							RN= ROW_NUMBER() OVER(Partition By b.radif Order By b.today_date DESC)
						From [{dbName}].dbo.bed_bes b
						{conditionQuery}
					)
					Select
						c.town ZoneId,
						c.radif as CustomerNumber,
						c.sh_ghabs1 BillId,
						c.today_date as LastMeterDateJalali ,
						c.today_no as LastMeterNumber,
						c.masraf as LastConsumption,
						c.rate as LastMonthlyConsumption,
						c.cod_vas as LastCounterStateCode,
						c.baha as LastSumItems,
						c.del IsReturned
					From CTE c
					Where c.RN=1";
        }
        private string GetAllTavisQuery(string dbName, bool isSqlBulkCopy)
        {
            string conditionQuery = isSqlBulkCopy ?
                @"Join #TempCustomerNumbers tc 
					ON 	t.town = tc.ZoneId AND
						t.radif = tc.CustomerNumber" :
                @"Where 
						t.town = @zoneId AND
						t.radif IN @customerNumbers";
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
						{conditionQuery}
					)
					Select
						c.radif as CustomerNumber,
						c.taviz_date as TavizDateJalali,
						c.elat as TavizCause,
						c.date_sabt as TavizRegisterDateJalali,
						c.taviz_no as TavizNumber
					From CTE c
					Where c.RN=1";
        }
        private string GetMembersBedBesQueru(string dbName)
        {
            return $@"Select bed_bes
					From [{dbName}].dbo.members
					Where
						 town=@zoneId AND
						 radif=@customerNumber";
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
