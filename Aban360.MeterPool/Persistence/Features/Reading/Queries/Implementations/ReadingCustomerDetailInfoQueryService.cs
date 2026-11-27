using Aban360.Common.Db.Dapper;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;
using Aban360.MeterPool.Persistence.Features.Reading.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.MeterPool.Persistence.Features.Reading.Queries.Implementations
{
    internal sealed class ReadingCustomerDetailInfoQueryService : AbstractBaseConnection, IReadingCustomerDetailInfoQueryService
    {
        public ReadingCustomerDetailInfoQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        //ToDo: SingleQuery Betweens: IBedBesQuery(OldCalc),ICommonMembers(Common)
        public async Task<IEnumerable<CustomerInfoByBillsAndChangeGetDto>> Get(int zoneId, string fromReadingNumber, string toReadingNumber)
        {
            IEnumerable<int> validReturnCause = await GetLastMeterValid();
            string dbName = GetDbName(zoneId);
            string query = GetCustomerInfoQuery(dbName);

            IEnumerable<CustomerInfoByBillsAndChangeGetDto> latestBedBesInfo = await _sqlReportConnection.QueryAsync<CustomerInfoByBillsAndChangeGetDto>(query, new { zoneId, fromReadingNumber, toReadingNumber, validReturnCause });
            return latestBedBesInfo;
        }
        private async Task<IEnumerable<int>> GetLastMeterValid()
        {
            string query = GetLastMeterValidQuery();
            IEnumerable<int> result = await _sqlReportConnection.QueryAsync<int>(query, null);

            return result;
        }

        private string GetCustomerInfoQuery(string dbName)
        {
            return $@";With CustomersInfo As(
						Select m.*
						From [{dbName}].dbo.members m 
						Where 
							m.town = @ZoneId AND
							m.eshtrak BETWEEN @FromReadingNumber AND @ToReadingNumber AND
							m.hasf IN (0,5)
					),
					AllBillsInfo as(
					Select 
					    b.id BillRecordId,
						b.town BillZoneId,
						b.radif BillCustomerNumber,
						b.date_bed BillRegisterDateJalali,
						r.elat BillReturnCauseId,
						b.del IsBillReturned,
						b.cod_vas BillCounterStateCode,
						b.rate BillConsumptionAverage,
						b.masraf BillConsumption,
						Case 
					        When b.del = 0 And b.cod_vas In (4,7,8) Then NULL
					        When b.del = 0 And b.cod_vas Not In (4,7,8) Then b.today_no
					        When b.del = 1 And r.elat Not In @validReturnCause And b.cod_vas Not In (4,7,8) Then b.today_no
					        When b.del = 1 And r.elat Not In @validReturnCause And b.cod_vas In (4,7,8) Then NULL
					        Else NULL
					    End As BillPreviousNumber,
						Case 
					        When b.del = 0 And b.cod_vas In (4,7,8) Then NULL
					        When b.del = 0 And b.cod_vas Not In(4,7,8) Then b.today_date
					        When b.del = 1 And r.elat Not In @validReturnCause And b.cod_vas Not In (4,7,8) Then b.today_date
					        When b.del = 1 And r.elat Not In @validReturnCause And b.cod_vas In (4,7,8) Then NULL
					        Else NULL
					    End As BillPreviousDateJalali
					From CustomersInfo m 
					LEFT Join  [{dbName}].dbo.bed_bes b
						ON m.town=b.town AND m.radif=b.radif 
					Left Join [{dbName}].dbo.REPAIR r
						On b.town=r.town And b.radif=r.radif And b.pri_date >= r.pri_date And b.today_date <= r.today_date
					),
					AllNotNUllBillInfo As (
					    Select 
							c.*,
							cv.Title BillCounterStateTitle,
					        ROW_NUMBER() OVER (PARTITION BY c.BillZoneId, c.BillCustomerNumber ORDER BY c.BillRegisterDateJalali Desc, c.BillRecordId Desc) As rn
					    From AllBillsInfo c
					    Join [Db70].dbo.CounterVaziat cv
					        ON c.BillCounterStateCode = cv.MoshtarakinId
					    Where c.BillPreviousNumber Is Not Null
					),
					LastInfo As(
						Select *
						From AllNotNUllBillInfo al
						Where al.rn=1
					)
					Select 
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
						m.noe_va AS BranchTypeId,
						t7.C1 BranchTypeTitle,
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
						m.bed_bes DebtAmount,
					    m.n_faz as SewageCalcState,
					    m.EJUCA as VirtualCategoryId,
						
						--Bill Info
						 ISNULL(lf.BillPreviousDateJalali,m.inst_ab) BillDateJalali,
						 ISNULL(lf.BillPreviousNumber,0) BillNumber,
						 ISNULL(lf.BillCounterStateCode,0) BillCounterStateCode,
						 ISNULL(lf.BillCounterStateTitle, N'عادی') BillCounterStateTitle,
						 ISNULL(lf.BillConsumptionAverage,0) BillConsumptionAverage,
						 ISNULL(lf.BillConsumption,0) BillConsumption,
						 ISNULL(lf.BillRegisterDateJalali,m.inst_ab) BillRegisterDateJalali,
					
						--Taviz Info
						tv.taviz_date as ChangeDateJalali,
						tv.elat as ChangeCause,
						tv.date_sabt as ChangeRegisterDateJalali,
						tv.taviz_no as ChangeNumber
					From CustomersInfo m
					Left Join LastInfo lf
						ON m.town=lf.BillZoneId AND m.radif=lf.BillCustomerNumber
					Left Join [{dbName}].dbo.taviz tv
						ON tv.town=m.town AND tv.radif=m.radif
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
					Order By tv.taviz_date Desc;";
        }
        private string GetLastMeterValidQuery()
        {
            return @"Select Id
                    From [Db70].dbo.BillReturnCause
                    Where 
                        RemoveDateTime IS NULL AND
                        IsLastMeterValid = 1";
        }
    }
}
