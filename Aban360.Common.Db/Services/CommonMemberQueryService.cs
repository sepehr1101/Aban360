using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services.Dtos;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.Common.Db.Services
{
    public interface ICommonMemberQueryService
    {
        Task<ZoneIdAndCustomerNumber> Get(string billId);
        Task<IEnumerable<ZoneIdAndCustomerNumberAndBillId>> GetFromClient(ZoneIdsAndReadingNumber input, bool hasException);
        Task<IEnumerable<ZoneIdAndCustomerNumberAndBillId>> Get(IEnumerable<string> billId, IDbConnection connection, IDbTransaction transction);
        Task<MemberInfoGetDto> Get(ZoneIdAndCustomerNumber input);
        Task<IEnumerable<MemberInfoGetDto>> Get(IEnumerable<ZoneIdAndCustomerNumber> input, IDbConnection connection);
        Task<CustomerInfoGetDto> GetMembersBedBesTavizInfo(int zoneId, int customerNumber);
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
        public async Task<IEnumerable<ZoneIdAndCustomerNumberAndBillId>> GetFromClient(ZoneIdsAndReadingNumber input, bool hasException)
        {
            string query = GetZoneIdAndCustomerNumberByReadingNumberQuery();
            IEnumerable<ZoneIdAndCustomerNumberAndBillId> result = await _sqlReportConnection.QueryAsync<ZoneIdAndCustomerNumberAndBillId>(query, input);
            if (!result.Any() && hasException)
            {
                throw new InvalidBillIdException(ExceptionLiterals.InvalidReadingNumber);
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
        public async Task<IEnumerable<ZoneIdAndCustomerNumberAndBillId>> Get(IEnumerable<string> billId, IDbConnection connection, IDbTransaction transction)
        {
            DataTable table = new DataTable();
            table.Columns.Add("ZoneId", typeof(int));
            table.Columns.Add("CustomerNumbers", typeof(int));
            table.Columns.Add("BillId", typeof(string));
            foreach (var item in billId)
                table.Rows.Add(null, null, item);

            string tempTableCreateCommand = "Create Table #TempCustomerNumbers" +
                                                "(ZoneId int  Null," +
                                                "CustomerNumber int  Null," +
                                                "BillId NVARCHAR(14) Not NUll)";
            await connection.ExecuteAsync(tempTableCreateCommand, null, transction);
            using (var bulkCopy = new SqlBulkCopy((SqlConnection)connection, SqlBulkCopyOptions.Default, (SqlTransaction)transction))
            {
                bulkCopy.DestinationTableName = "#TempCustomerNumbers";
                bulkCopy.BatchSize = 10000;
                await bulkCopy.WriteToServerAsync(table);
            }
            await connection.ExecuteAsync(GetUpdateTemplateTableCommand(), null, transction);
            IEnumerable<ZoneIdAndCustomerNumberAndBillId> result = await connection.QueryAsync<ZoneIdAndCustomerNumberAndBillId>(GetTemplateQuery(), null, transction);
            return result;
        }
        public async Task<IEnumerable<MemberInfoGetDto>> Get(IEnumerable<ZoneIdAndCustomerNumber> input, IDbConnection connection)
        {
            DataTable table = new DataTable();
            table.Columns.Add("ZoneId", typeof(int));
            table.Columns.Add("CustomerNumber", typeof(int));
            foreach (var item in input)
                table.Rows.Add(item.ZoneId, item.CustomerNumber);

            string tempTableCreateCommand = "Create Table #TempCustomerNumbersToGetAll" +
                                                "(ZoneId int  Null," +
                                                "CustomerNumber int  Null);";
            await connection.ExecuteAsync(tempTableCreateCommand, null);
            using (var bulkCopy = new SqlBulkCopy((SqlConnection)connection))
            {
                bulkCopy.DestinationTableName = "#TempCustomerNumbersToGetAll";
                bulkCopy.BatchSize = 10000;
                await bulkCopy.WriteToServerAsync(table);
            }
            int zoneId = input?.FirstOrDefault()?.ZoneId ?? 0;
            IEnumerable<MemberInfoGetDto> result = await connection.QueryAsync<MemberInfoGetDto>(GetMemeberInfoByBulkQuery(GetDbName(zoneId)), null);
            return result;
        }
        public async Task<CustomerInfoGetDto> GetMembersBedBesTavizInfo(int zoneId, int customerNumber)
        {
            string dbName = GetDbName(zoneId);
            string memberQuery = GetMemeberInfoQuery(dbName);
            string bedBesQuery = GetBedBesQuery(dbName);
            string tavizQuery = GetTavisQuery(dbName);

            IEnumerable<int> validReturnCause = await GetLastMeterValid();
            MemberInfoGetDto membersInfo = await _sqlReportConnection.QueryFirstOrDefaultAsync<MemberInfoGetDto>(memberQuery, new { zoneId, customerNumber });
            LatesTavizInfo latestTavizInfo = await _sqlReportConnection.QueryFirstOrDefaultAsync<LatesTavizInfo>(tavizQuery, new { zoneId, customerNumber });
            LatestBedBesConsumptionInfo latestBedBesInfo = await _sqlReportConnection.QueryFirstOrDefaultAsync<LatestBedBesConsumptionInfo>(bedBesQuery, new { zoneId, customerNumber, validReturnCause });

            return new CustomerInfoGetDto(membersInfo, latestBedBesInfo, latestTavizInfo);
        }
        private async Task<IEnumerable<int>> GetLastMeterValid()
        {
            string query = GetLastMeterValidQuery();
            IEnumerable<int> result = await _sqlReportConnection.QueryAsync<int>(query, null);

            return result;
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
        private string GetZoneIdAndCustomerNumberByReadingNumberQuery()
        {
            return @"Select 
						ZoneId,
						ZoneTitle,
						CustomerNumber,
						BillId
                    From CustomerWarehouse.dbo.Clients	
                    Where 
                        ZoneId IN @ZoneIds AND
                    	ReadingNumber = @ReadingNumber AND
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
						bed_bes DebtAmount,
                        m.n_faz as SewageCalcState,
					    m.EJUCA as VirtualCategoryId
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
        private string GetMemeberInfoByBulkQuery(string dbName)
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
					Join #TempCustomerNumbersToGetAll temp
						On m.town=temp.ZoneId AND m.radif=temp.CustomerNumber
					Left Join [Db70].dbo.T51 t51
						ON m.town=t51.C0
					Left Join [Db70].dbo.T46 t46
						ON t51.C1=t46.C0
					Left Join [Db70].dbo.T41 t41
						ON m.cod_enshab=t41.C0
					Left Join [Db70].dbo.T5 t5
						ON m.enshab=t5.C0
					Left Join [Db70].dbo.T7 t7
						ON m.noe_va=t7.C0";
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
        private string GetUpdateTemplateTableCommand()
        {
            return $@"Update t
					Set
						ZoneId=c.ZoneId,
						CustomerNumber=c.CustomerNumber
					From CustomerWarehouse.dbo.Clients c
					Join #TempCustomerNumbers t
						 ON c.BillId collate Persian_100_CI_AI=t.BillId collate Persian_100_CI_AI
					Where  
						c.ToDayJalali IS NULL";
        }
        private string GetTemplateQuery()
        {
            return $@"Select * From #TempCustomerNumbers t";
        }
     
        private string GetLastMeterValidQuery()
        {
            return @"Select Id
                    From [Db70].dbo.BillReturnCause
                    Where 
                        RemoveDateTime IS NULL AND
                        IsLastMeterValid = 1";
        }
        private string GetBedBesQuery(string dbName)
        {
            return $@"With Cte As(
                        Select 
							b.id,
                            b.radif,
                    		b.date_bed,
                    		b.pri_date,
                    		b.today_date,
                    		b.pri_no,
                    		b.today_no,
                    		r.elat,
                    		b.del,
							b.cod_vas CounterStateCode,
							b.rate ConsumptionAverage,
							b.masraf Consumption,
                            b.baha,
                            Case 
                                When b.del = 0 And b.cod_vas In (4,7,8) Then NULL
                                When b.del = 0 And b.cod_vas Not In (4,7,8) Then b.today_no
                                When b.del = 1 And r.elat Not In @validReturnCause And b.cod_vas Not In (4,7,8) Then b.today_no
                                When b.del = 1 And r.elat Not In @validReturnCause And b.cod_vas In (4,7,8) Then NULL
                                Else NULL
                            End As PreviousNumber,
                    		Case 
                                When b.del = 0 And b.cod_vas In (4,7,8) Then NULL
                                When b.del = 0 And b.cod_vas Not In(4,7,8) Then b.today_date
                                When b.del = 1 And r.elat Not In @validReturnCause And b.cod_vas Not In (4,7,8) Then b.today_date
                                When b.del = 1 And r.elat Not In @validReturnCause And b.cod_vas In (4,7,8) Then NULL
                                Else NULL
                            End As PreviousDateJalali
                         From  [{dbName}].dbo.bed_bes b
                    	 Left Join [{dbName}].dbo.REPAIR r
                    		On b.town=r.town And b.radif=r.radif And b.pri_date>=r.pri_date And b.today_date<=r.today_date
                    	 Where b.radif=@CustomerNumber And b.town=@ZoneId
                    )
                    Select Top 1 
                            c.radif CustomerNumber,
                    		c.PreviousDateJalali LastMeterDateJalali,
                    		c.PreviousNumber LastMeterNumber,
							c.CounterStateCode LastCounterStateCode,
							cv.Title LastCounterStateTitle,
							c.ConsumptionAverage LastMonthlyConsumption,
							c.Consumption LastConsumption,
                            c.del IsReturned,
                            c.baha LastSumItems
                    From Cte c
					Join [Db70].dbo.CounterVaziat cv
						ON c.CounterStateCode=cv.MoshtarakinId
                    Where PreviousNumber Is Not Null 
                    Order By c.date_bed Desc ,c.Id Desc";
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

    }
}
