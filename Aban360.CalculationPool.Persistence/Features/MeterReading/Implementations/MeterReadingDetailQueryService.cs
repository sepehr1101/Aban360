using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Implementations
{
    public sealed class MeterReadingDetailQueryService : AbstractBaseConnection, IMeterReadingDetailQueryService
    {
        public MeterReadingDetailQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<MeterReadingDetailDataOutputDto>> Get(int flowImportedId,bool? hasExcluded)
        {
            string query = GetWithFlowImportedIdQuery(hasExcluded);
            IEnumerable<MeterReadingDetailDataOutputDto> details = await _sqlReportConnection.QueryAsync<MeterReadingDetailDataOutputDto>(query, new { flowImportedId = flowImportedId });

            return details;
        }
        public async Task<MeterReadingDetailDataOutputDto> GetById(int id)
        {
            string query = GetSingleQuery();

            MeterReadingDetailDataOutputDto detail = await _sqlReportConnection.QueryFirstOrDefaultAsync<MeterReadingDetailDataOutputDto>(query, new { id = id });
            if (detail is null || detail.Id <= 0)
            {
                throw new ReadingException(ExceptionLiterals.InvalidId);
            }
            return detail;
        }
        public async Task<IEnumerable<MeterReadingDetailExcludedDataOutputDto>> Get(MeterReadingDetailExcludedInputDto inputDto)
        {
            string query = GetExcludedQuery();
            IEnumerable<MeterReadingDetailExcludedDataOutputDto> details = await _sqlReportConnection.QueryAsync<MeterReadingDetailExcludedDataOutputDto>(query, inputDto);

            return details;
        }
        public async Task<IEnumerable<MeterReadingDetailUpdatedDataOutputDto>> GetUpdated(MeterReadingDetailUpdatedInputDto inputDto)
        {
            string query = GetUpdatedReportQuery();
            IEnumerable<MeterReadingDetailUpdatedDataOutputDto> result = await _sqlReportConnection.QueryAsync<MeterReadingDetailUpdatedDataOutputDto>(query, inputDto);
            return result;
        }


        private string GetWithFlowImportedIdQuery(bool? hasExcluded)
        {
            string excludedConditionQuery = hasExcluded switch
            {
                null => string.Empty,
                true => " AND m.ExcludedByUserId IS NOT NULL ",
                _ => " AND m.ExcludedByUserId IS NULL ",
            };

            return $@"Select 
                        m.Id,
                        m.FlowImportedId,
                        m.ZoneId,
                        m.CustomerNumber,
                        m.ReadingNumber,
                        m.BillId,
                        m.AgentCode,
                        m.CurrentCounterStateCode,
                        m.PreviousDateJalali,
                        m.CurrentDateJalali,
                        m.PreviousNumber,
                        m.CurrentNumber,
                        m.ExcludedByUserId, 
                        m.ExcludedDateTime,
                        m.InsertByUserId,
                        m.InsertDateTime,
                        m.RemovedByUserId,
                        m.RemovedDateTime,
                        m.BranchTypeId,
                        t7.C1 as BranchTypeTitle,
                        m.UsageId,
                        t41.C1 as UsageTitle,
                        m.ConsumptionUsageId,
                        m.DomesticUnit,
                        m.CommercialUnit,
                        m.OtherUnit,
                        m.EmptyUnit,
                        m.WaterInstallationDateJalali,
                        m.SewageInstallationDateJalali,
                        m.WaterRegisterDate,
                        m.SewageRegisterDate,
                        m.WaterCount,
                        m.SewageCalcState,
                        m.ContractualCapacity,
                        m.HouseholdNumber,
                        m.HouseholdDate,
                        m.VillageId,
                        m.IsSpecial,
                        m.MeterDiameterId,
                        m.VirtualCategoryId,
                        m.BodySerial,
                        m.TavizDateJalali,
                        m.TavizCause,
                        m.TavizRegisterDateJalali,
                        m.TavizNumber,
                        m.LastMeterDateJalali,
                        m.LastMeterNumber,
                        m.LastMonthlyConsumption,
                        m.LastConsumption,
                        m.LastCounterStateCode,
                        m.LastSumItems,
                        m.SumItems,
                        m.SumItemsBeforeDiscount,
                        m.DiscountSum,
                        m.Consumption,
                        m.MonthlyConsumption,
                        --BedBesProps
                        m.barge,
                        m.pri_no as PriNo,
                        m.today_no as TodayNo,
                        m.pri_date as PriDate,
                        m.today_date as TodayDate,
                        m.abon_fas as AbonFas,
                        m.fas_baha as FasBaha,
                        m.ab_baha as AbBaha,
                        m.ztadil as Ztadil,
                        m.masraf as Masraf,
                        m.shahrdari as Shahrdari,
                        m.modat as Modat,
                        m.date_bed as DateBed,
                        m.jalase_no as JalaseNo,
                        m.mohlat as Mohlat,
                        m.baha as Baha,
                        m.abon_ab as AbonAb,
                        m.pard as Pard,
                        m.jam as Jam,
                        m.cod_vas as CodVas,
                        m.ghabs as Ghabs,
                        m.del as Del,
                        m.type as Type,
                        m.cod_enshab as CodEnshab,
                        m.enshab as Enshab,
                        m.elat as Elat,
                        m.serial as Serial,
                        m.ser as Ser,
                        m.zaribfasl as ZaribFasl,
                        m.ab_10 as Ab10,
                        m.ab_20 as Ab20,
                        m.tedad_vahd as TedadVahd,
                        m.ted_khane as TedKhane,
                        m.tedad_mas as TedadMas,
                        m.tedad_tej as TedadTej,
                        m.noe_va as NoeVa,
                        m.jarime as Jarime,
                        m.masjar as Masjar,
                        m.sabt as Sabt,
                        m.rate as Rate,
                        m.operator as Operator,
                        m.mamor as Mamor,
                        m.taviz_date as TavizDate,
                        m.zarib_cntr as ZaribCntr,
                        m.zabresani as Zabresani,
                        m.zarib_d as ZaribD,
                        m.tafavot as Tafavot,
                        m.kasr_ha as KasrHa,
                        m.fix_mas as FixMas,
                        m.sh_ghabs1 as ShGhabs1,
                        m.sh_pard1 as ShPard1,
                        m.TAB_ABN_A as TabAbnA,
                        m.TAB_ABN_F as TabAbnF,
                        m.TABS_FA as TabsFa,
                        m.NEWAB as NewAb,
                        m.NEWFA as NewFa,
                        m.BODJEH as Bodjeh,
                        m.group1 as Group1,
                        m.MAS_FAS as MasFas,
                        m.FAZ as Faz,
                        m.CHK_KARBARI as ChkKarbari,
                        m.C200 as C200,
                        m.Ab_sevom as AbSevom,
                        m.Ab_sevom1 as AbSevom1,
                        m.C70 as C70,
                        m.C80 as C80,
                        m.C90 as C90,
                        m.C101 as C101,
                        m.Khali_s as KhaliS,
                        m.edareh_k as EdarehK,
                        m.Avarez as Avarez
                    From Atlas.dbo.MeterReadingDetail m
                    Left Join [Db70].dbo.T7 t7 
                        On m.BranchTypeId = t7.C0
                    Left Join [Db70].dbo.T41 t41
                        On m.UsageId = t41.C0
                    Where 
                        m.FlowImportedId = @flowImportedId AND
                        m.RemovedByUserId IS NULL 
                        {excludedConditionQuery}";
        }
        private string GetSingleQuery()
        {
            return @"Select *
                     From Atlas.dbo.MeterReadingDetail
                     Where Id=@id";
        }
        private string GetExcludedQuery()
        {
            return $@"Select 
                        mr.Id,
                    	t46.C0 RegionId,
                    	t46.C2 RegionTitle,
                    	mr.ZoneId,
                    	t51.C2 ZoneTitle,
                    	c.FirstName,
                    	c.SureName Surname,
                    	c.FirstName + ' ' + c.SureName FullName,
                    	mr.CustomerNumber,
                    	mr.BillId,
                    	mr.FlowImportedId,
                    	mr.ReadingNumber,
                    	mr.CurrentCounterStateCode,
						cv.Title CurrentCounterStateTitle,
                    	mr.PreviousDateJalali,
                    	mr.CurrentDateJalali,
                    	mr.PreviousNumber,
                    	mr.CurrentNumber,
                    	mr.ExcludedDateTime,
                    	mr.ExcludedByUserId,
                        mr.ExcludedCauseId,
                        mr.ExcludedCauseTitle,
                    	mr.InsertByUserId,
                    	mr.InsertDateTime,
                    	mr.UsageId,
                    	t41_1.C1 UsageTitle,
                    	mr.BranchTypeId,
                    	t7.C1 BranchTypeTitle,
                    	mr.ConsumptionUsageId UsageConsumptionId ,
                    	t41_2.C1 UsageConsumptionTitle,
                    	mr.CommercialUnit,
                    	mr.DomesticUnit,
                    	mr.OtherUnit,
                    	mr.TavizDateJalali,
                    	mr.MeterDiameterId,
                    	t5.C2 MeterDiameterTitle
                    From Atlas.dbo.MeterReadingDetail mr
                    Left Join CustomerWarehouse.dbo.Clients c
                    	 ON mr.ZoneId=c.ZoneId AND mr.CustomerNumber=c.CustomerNumber
                    Join [Db70].dbo.T51 t51 
                    	ON mr.ZoneId=t51.C0
                    Join [Db70].dbo.T46 t46 
                    	ON t51.C1=t46.C0
                    Join [Db70].dbo.T41 t41_1 
                    	ON mr.UsageId=t41_1.C0
                    Join [Db70].dbo.T41 t41_2 
                    	ON mr.ConsumptionUsageId=t41_2.C0
                    Join [Db70].dbo.T7 t7
                    	ON mr.BranchTypeId=t7.C0
                    Join [Db70].dbo.T5 t5
                    	ON mr.MeterDiameterId=t5.C0
					Join [Db70].dbo.CounterVaziat cv
						ON mr.CurrentCounterStateCode=cv.MoshtarakinId
                    Where 
                    	c.ToDayJalali IS NULL AND
                    	mr.ExcludedByUserId IS NOT NUll AND
                    	mr.ZoneId = @ZoneId AND
                    	(FORMAT(CAST(mr.ExcludedDateTime as date),'yyyy/MM/dd','fa') BETWEEN @FromExcludeDateJalali AND @ToExcludeDateJalali) AND
                        (@FromReadingNumber IS NUll OR
                        @ToReadingNumber IS NULL OR
                        mr.ReadingNumber  BETWEEN @FromReadingNumber AND @ToReadingNumber) AND
                        mr.RemovedByUserId Is NUll";
        }
        private string GetUpdatedReportQuery()
        {
            return $@";With Cte As(
                        	Select 
                        		BillId,
                        		MAX(FlowImportedId) FlowImportedId
                        	From Atlas.dbo.MeterReadingDetail 
                        	Where 
                        		ZoneId=@ZoneId AND
                        		(FORMAT(CAST(InsertDateTime AS date),'yyyy/MM/dd','fa')  BETWEEN @FromDateJalali AND @ToDateJalali) AND
                        		RemovedDateTime IS NOT NULL AND
                        		(@FromReadingNumber IS NULL OR
                        		@ToReadingNumber IS NULL OR
                        		ReadingNumber BETWEEN @FromReadingNumber AND @ToReadingNumber)
                        	Group By BillId
                        )
                        Select 
                        	 mr.Id,
                        	t46.C0 RegionId,
                        	t46.C2 RegionTitle,
                        	mr.ZoneId,
                        	t51.C2 ZoneTitle,
                        	mr.CustomerNumber,
                        	mr.BillId,
                        	mr.FlowImportedId,
                        	mr.ReadingNumber,
                        	mr.CurrentCounterStateCode,
                        	cv.Title CurrentCounterStateTitle,
                        	mr.PreviousDateJalali,
                        	mr.CurrentDateJalali,
                        	mr.PreviousNumber,
                        	mr.CurrentNumber,
                        	mr.ExcludedDateTime,
                        	mr.ExcludedByUserId,
                            mr.ExcludedCauseId,
                            mr.ExcludedCauseTitle,
                        	mr.InsertByUserId,
                        	mr.InsertDateTime,
	                        mr.RemovedByUserId,
	                        mr.RemovedDateTime,
                        	mr.UsageId,
                        	t41_1.C1 UsageTitle,
                        	mr.BranchTypeId,
                        	t7.C1 BranchTypeTitle,
                        	mr.ConsumptionUsageId UsageConsumptionId ,
                        	t41_2.C1 UsageConsumptionTitle,
                        	mr.CommercialUnit,
                        	mr.DomesticUnit,
                        	mr.OtherUnit,
                        	mr.TavizDateJalali,
                        	mr.MeterDiameterId,
                        	t5.C2 MeterDiameterTitle
                        From Cte c
                        Join Atlas.dbo.MeterReadingDetail mr
                        	On c.BillId=mr.BillId AND c.FlowImportedId=mr.FlowImportedId
                        Join [Db70].dbo.T51 t51 
                        	ON mr.ZoneId=t51.C0
                        Join [Db70].dbo.T46 t46 
                        	ON t51.C1=t46.C0
                        Join [Db70].dbo.T41 t41_1 
                        	ON mr.UsageId=t41_1.C0
                        Join [Db70].dbo.T41 t41_2 
                        	ON mr.ConsumptionUsageId=t41_2.C0
                        Join [Db70].dbo.T7 t7
                        	ON mr.BranchTypeId=t7.C0
                        Join [Db70].dbo.T5 t5
                        	ON mr.MeterDiameterId=t5.C0
                        Join [Db70].dbo.CounterVaziat cv
                        	ON mr.CurrentCounterStateCode=cv.MoshtarakinId
                        Order By mr.BillId,mr.InsertDateTime Desc";
        }
    }
}