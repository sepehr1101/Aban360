using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Implementations
{
    internal sealed class BedBesQueryService : AbstractBaseConnection, IBedBesQueryService
    {
        string _manualBillTitle = "قبوض دستی";
        public BedBesQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<BedBesConsumptionOutputDto> Get(string billId)
        {
            string zoneIdQueryString = GetZoneIdQuery();
            ZoneIdAndCustomerNumberOutputDto zoneIdAndCustomerNumber = await _sqlReportConnection.QueryFirstOrDefaultAsync<ZoneIdAndCustomerNumberOutputDto>(zoneIdQueryString, new { billId });
            if (zoneIdAndCustomerNumber == null)
            {
                throw new InvalidBillIdException(ExceptionLiterals.BillIdNotFound + billId);
            }
            string DataBaseName = GetDbName(zoneIdAndCustomerNumber.ZoneId);
            string customerInfoQueryString = GetBedBesConsumptionDataQuery(DataBaseName);
            BedBesConsumptionOutputDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<BedBesConsumptionOutputDto>(customerInfoQueryString, new { billId });

            return result;
        }
        public async Task<BedBesDataInfoOutptuDto> Get(int id)
        {
            string customerInfoQueryString = GetBedBesByIdQuery();
            BedBesDataInfoOutptuDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<BedBesDataInfoOutptuDto>(customerInfoQueryString, new { Id = id });

            return result;
        }
        public async Task<IEnumerable<BillsCanRemoveOutputDto>> GetToRemove(RemovedBillSearchDto input)
        {
            string dbName = GetDbName(input.ZoneId);
            //string dbName = "Atlas";
            string query = GetBedBesListToRemoveOrReturn(dbName, true);

            IEnumerable<BillsCanRemoveOutputDto> result = await _sqlReportConnection.QueryAsync<BillsCanRemoveOutputDto>(query, input);
            if (result is null || !result.Any())
            {
                throw new RemovedBillException(ExceptionLiterals.NotFoundBillsToRemoved);
            }
            return result;
        }
        public async Task<RemoveBillDataInputDto> GetToRemove(RemoveBillGetDto input)
        {
            string dbName = GetDbName(input.ZoneId);
            //string dbName = "Atlas";
            string query = GetBedBesToRemove(dbName);

            RemoveBillDataInputDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<RemoveBillDataInputDto>(query, input);
            if (result is null || result.Id <= 0)
            {
                throw new RemovedBillException(ExceptionLiterals.InvalidId);
            }
            return result;
        }
        public async Task<IEnumerable<BillsCanReturnOutputDto>> GetToReturned(ReturnBillSearchDto input)
        {
            string dbName = GetDbName(input.ZoneId);
            //string dbName = "Atlas";
            string query = GetBedBesListToRemoveOrReturn(dbName, false);

            IEnumerable<BillsCanReturnOutputDto> result = await _sqlReportConnection.QueryAsync<BillsCanReturnOutputDto>(query, input);
            if (result is null || !result.Any())
            {
                throw new ReturnedBillException(ExceptionLiterals.NotFoundBillsToReturned);
            }
            return result;
        }
        public async Task<IEnumerable<BillsCanRemoveOutputDto>> Get(BillToReturnInputDto input)
        {
            //string dbName = GetDbName(input.ZoneId);
            string dbName = "Atlas";
            string query = GetBillToReturnQuery(dbName);

            IEnumerable<BillsCanRemoveOutputDto> result = await _sqlReportConnection.QueryAsync<BillsCanRemoveOutputDto>(query, input);
            if (result is null || !result.Any())
            {
                throw new ReturnedBillException(ExceptionLiterals.NotFoundBillsToReturned);
            }
            return result;
        }
        public async Task<ReportOutput<ManualBillHeaderOutputDto, ManualBillDataOutputDto>> Get(ManualBillInputDto input)
        {
            string query = GetManualBedBesQuery();
            IEnumerable<ManualBillDataOutputDto> data = await _sqlReportConnection.QueryAsync<ManualBillDataOutputDto>(query, input);
            ManualBillHeaderOutputDto header = new()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                RecordCount = data.Count(),
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                ConumptionAverage = data.Count() > 0 ? data.Average(x => x.ConsumptionAverage) : 0,
            };

            ReportOutput<ManualBillHeaderOutputDto, ManualBillDataOutputDto> result = new(_manualBillTitle, header, data);
            return result;
        }
        public async Task<float> GetPreviousBill(int zoneId, int customerNumber, string dateJalali)
        {
            string dbName = GetDbName(zoneId);
            string query = GetPrviousBill(dbName);
            var @params = new
            {
                zoneId = zoneId,
                customerNumber = customerNumber,
                nexDay = dateJalali
            };
            float rate = await _sqlReportConnection.QueryFirstOrDefaultAsync<float>(query, @params);
            return rate;
        }
        public async Task<IEnumerable<BedBesCreateDto>> Get(ZoneCustomerFromToDateDto input)
        {
            string dbName = GetDbName(input.ZoneId);
            //string dbName = "Atlas";
            string query = GetListByFromToDate(dbName);
            IEnumerable<BedBesCreateDto> result = await _sqlReportConnection.QueryAsync<BedBesCreateDto>(query, input);
            return result;
        }
        public async Task<int> GetCountInDateBed(int zoneId, int customernumber, string date,bool isPreviousDate)
        {
            string dbName = GetDbName(zoneId);
            //string dbName = "Atlas";
            string query = GetCountInDateBedQuery(dbName);
            var @params = new
            {
                zoneId = zoneId,
                customerNumber = customernumber,
                isPreviousDate= isPreviousDate,
                date = date
            };
            int count = await _sqlReportConnection.QueryFirstOrDefaultAsync<int>(query, @params);
            return count;
        }
        public async Task<int?> GetLatestJalaseNumber(ZoneIdAndCustomerNumberOutputDto input)
        {
            //string dbName = GetDbName(input.ZoneId);
            string dbName = "Atlas";
            string query = GetLatestJalaseNumberQuery(dbName);
            var @params = new
            {
                zoneId = input.ZoneId,
                customerNumber = input.CustomerNumber,
                date = DateTime.Now.ToShortPersianDateString()
            };
            int? jalaseNumber = await _sqlReportConnection.QueryFirstOrDefaultAsync<int>(query, @params);
            return jalaseNumber;
        }

        private string GetBedBesConsumptionDataQuery(string dataBaseName)
        {
            return @$"Select Top 1 
                        rate as ConsumptionAverage
                    From [{dataBaseName}].dbo.bed_bes 
                    Where 
                    	TRIM(sh_ghabs1)=@billId AND
                    	cod_vas NOT IN (4,7,8)
                    Order By 
                    	today_date DESC,
                    	pri_date DESC";
        }
        private string GetBedBesByIdQuery()
        {
            return @$"Select 
                        sh_ghabs1 as BillId,
	                    pri_date as PreviousDateJalali,
	                    today_date as CurrentDateJalali,
	                    date_bed as DateBed,
	                    cod_vas as CounterStateCode,
	                    serial as BodySerial
                    From [Atlas].dbo.bed_bes 
                    Where 
                    	Id=@Id AND
                    	cod_vas NOT IN (4,7,8)";
        }
        private string GetZoneIdQuery()
        {
            return @"Select c.ZoneId,c.CustomerNumber
                    From [CustomerWarehouse].dbo.Clients c
                    Where 
                    	c.BillId=@billId AND
                    	c.ToDayJalali IS NULL";
        }
        private string GetBedBesListToRemoveOrReturn(string dbName, bool isRemove)
        {
            string condition = isRemove ?
                $"JOIN [{dbName}].dbo.variab v ON b.town=v.town AND b.date_bed collate Persian_100_CI_AI>=v.date_check" :
                string.Empty;
            return @$"SELECT	
                    	b.id,
                    	b.town as ZoneId,
						t51.C2 as ZoneTitle,
                    	b.radif as CustomerNumber,
                    	b.pri_no as PreviousNumber, 
                    	b.today_no as CurrentNumber,
                    	b.pri_date as PreviousDateJalali,
                    	b.today_date as CurrentDateJalali,
                    	b.date_bed as RegisterDateJalali,
                    	b.masraf as Consumption,
                    	b.rate as MonthlyConsumption,
                    	b.pard as Payable,
                    	b.baha as SumItems,
                    	b.kasr_ha as Discount,
                    	b.mamor as AgentCode,
                        TRIM(b.sh_pard1) as PaymentId,
                    	TRIM(b.sh_ghabs1) as BillId,
                    	t41.C1 as UsageTitle,
                    	t7.C1 as BranchTypeTitle,
                    	b.fix_mas as ContractualCapacity,
                    	b.Khali_s as EmptyUnit,
                    	b.tedad_mas as DomesticUnit,
                    	b.tedad_tej as CommercialUnit,
                    	b.tedad_vahd as OtherUnit,
                    	b.ted_khane as HouseholdNumber,
						b.del as IsReturned
                    FROM [{dbName}].dbo.bed_bes b
                    {condition}
                    JOIN [Db70].dbo.T41 t41 
                    	ON b.cod_enshab=t41.C0
                    JOIN [Db70].dbo.T7 t7 
                    	ON b.noe_va=t7.C0
					JOIN [Db70].dbo.T51 t51
						ON b.town=t51.c0
                    WHERE 
                    	b.date_bed>=@ComparisonDateJalali AND
                    	b.town=@ZoneId AND
                    	b.radif=@CustomerNumber";
        }
        private string GetBedBesToRemove(string dbName)
        {
            return @$"SELECT	
                    	b.id,
                    	b.town as ZoneId,
                    	b.radif as CustomerNumber,
                        b.barge as Barge,
                    	b.pri_no as PreviousNumber, 
                    	b.today_no as CurrentNumber,
                    	b.pri_date as PrviousDateJalali,
                    	b.today_date as CurrentDateJalali,
                    	b.date_bed as RegisterDateJalali,
                    	b.masraf as Consumption,
                        b.sh_pard1 as PaymentId,
						b.ab_baha as AbBahaAmount,
						b.fas_baha as FazelabAmount,
                    	b.baha as Baha,
                    	b.sh_ghabs1 as BillId
                    FROM [{dbName}].dbo.bed_bes b
                    WHERE 
                    	b.id=@id";
        }
        private string GetBillToReturnQuery(string dbName)
        {
            return @"";
        }
        private string GetManualBedBesQuery()
        {
            return @"Select 
                    	b.sh_ghabs1 BillId,
                    	b.rate ConsumptionAverage,
                    	b.masraf Consumption,
                    	b.baha Amount,
                    	b.pri_date PreviousDateJalali,
                    	b.today_date CurrentDateJalali,
                    	b.pri_no PreviousNumber,
                    	b.today_no CurrentNumber,
                    	b.cod_vas CounterStateCode,
                    	cv.Title CounterStateTitle,
                    	b.town ZoneId,
                    	t51.C2 ZoneTitle
                    From [Atlas].dbo.bed_bes b
                    Left Join [Db70].dbo.T51 t51
                    	On b.town=t51.C0	
                    Left Join [Db70].dbo.CounterVaziat cv
                    	On b.cod_vas=cv.MoshtarakinId	
                    Where 
                    	b.operator=5 AND
                    	b.date_bed BETWEEN @FromDateJalali AND @ToDateJalali";
        }
        private string GetPrviousBill(string dbName)
        {
            return @$"Select top 1 rate
                    From [{dbName}].dbo.bed_bes
                    Where
                    	town=@zoneId AND
                    	radif=@customerNumber AND
                    	today_date=@nexDay
                    Order By date_bed Desc";
        }
        private string GetListByFromToDate(string dbName)
        {
            return @$"SELECT
                        id AS id,
                        town AS Town,
                        radif AS Radif,
                        eshtrak AS Eshtrak,
                        barge AS Barge,
                        pri_no AS PriNo,
                        today_no AS TodayNo,
                        pri_date AS PriDate,
                        today_date AS TodayDate,
                        abon_fas AS AbonFas,
                        fas_baha AS FasBaha,
                        ab_baha AS AbBaha,
                        ztadil AS Ztadil,
                        masraf AS Masraf,
                        shahrdari AS Shahrdari,
                        modat AS Modat,
                        date_bed AS DateBed,
                        jalase_no AS JalaseNo,
                        mohlat AS Mohlat,
                        baha AS Baha,
                        abon_ab AS AbonAb,
                        pard AS Pard,
                        jam  AS Jam,
                        cod_vas AS CodVas,
                        ghabs AS Ghabs,
                        del AS Del,
                        [type] AS Type,
                        cod_enshab  AS CodEnshab,
                        enshab AS Enshab,
                        elat AS Elat,
                        serial AS Serial,
                        ser AS Ser,
                        zaribfasl AS ZaribFasl,
                        ab_10 AS Ab10,
                        ab_20 AS Ab20,
                        tedad_vahd AS TedadVahd,
                        ted_khane AS TedKhane,
                        tedad_mas AS TedadMas,
                        tedad_tej AS TedadTej,
                        noe_va AS NoeVa,
                        jarime AS Jarime,
                        masjar AS Masjar,
                        sabt AS Sabt,
                        rate AS Rate,
                        operator AS Operator,
                        mamor AS Mamor,
                        taviz_date AS TavizDate,
                        zarib_cntr AS ZaribCntr,
                        zabresani AS Zabresani,
                        zarib_d AS ZaribD,
                        tafavot AS Tafavot,
                        kasr_ha AS KasrHa,
                        fix_mas AS FixMas,
                        sh_ghabs1 AS ShGhabs1,
                        sh_pard1 AS ShPard1,
                        TAB_ABN_A AS TabAbnA,
                        TAB_ABN_F AS TabAbnF,
                        TABS_FA AS TabsFa,    
                        NEWAB AS NewAb,
                        NEWFA AS NewFa,
                        bodjeh AS Bodjeh,
                        group1 AS Group1,
                        MAS_FAS AS MasFas,
                        FAZ AS Faz,
                        CHK_KARBARI AS ChkKarbari,
                        C200 AS C200, 
                        Ab_sevom AS AbSevom,
                        Ab_sevom1 AS AbSevom1,
                        Khali_s AS KhaliS,
                        edareh_k AS EdarehK,
                        Avarez AS Avarez
                    FROM [{dbName}].dbo.Bed_Bes
                    WHERE
                    	town=@zoneId AND
                    	radif=@customerNumber AND
						pri_date>= @FromDate AND today_date <=@ToDate AND
                        cod_vas NOT IN (4,7,8) --AND
                        --del=0
                    Order by date_bed";
        }
        private string GetCountInDateBedQuery(string dbName)
        {
            //Comment for test
            return @$"Select COUNT(1)
                        From [{dbName}].dbo.bed_bes
                        Where 
                        	town=@zoneId AND
                        	radif=@customerNumber AND
	                        ((@isPreviousDate=1 AND pri_date=@date) OR
	                        (@isPreviousDate<>1 AND today_date=@date)) --AND
                            --del=0";
        }
        private string GetLatestJalaseNumberQuery(string dbName)
        {
                return $@"Select top 1 jalase_no
                        From atlas.dbo.autoback
                        Where 
                    	    town=@ZoneId AND
                    	    radif=@CustomerNumber AND
                    	    date_bed=@Date
                        Order By date_bed desc,id desc";
        }
    }
}
