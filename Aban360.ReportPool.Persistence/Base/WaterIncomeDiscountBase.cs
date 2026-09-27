using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Constants;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Base
{
    internal abstract class WaterIncomeDiscountBase : AbstractBaseConnection
    {
        private static int[] _netItems = { 1, 3, 4, 5 };
        private static int[] _rawItems = { 1 };
        private static int[] _returnedItems = { 3, 4, 5 };
        private static int[] _positiveModifications = [3];
        private static int[] _negativeModifications = [4];
        private static int[] _pureReturn = [5];
        public WaterIncomeDiscountBase(IConfiguration configuration)
                : base(configuration)
        {
        }
        internal string GetDetailQuery(bool hasZone, WaterIncomeDiscountCauseEnum discountCauseId)
        {
            string discountCauseCondition = GetDiscountCondition(discountCauseId);
            string zoneQuery = hasZone ? "AND b.ZoneId IN @zoneIds" : string.Empty;

            //todo: rename "RegisterDay" to "PhysicalSewageInstallDateJalali"
            return @$"use CustomerWarehouse
					Select
        				t46.C2 RegionTitle,
						b.ZoneTitle,
						TRIM(b.BillId) as BillId,
                        b.ZoneId,
                        b.UsageId,
						b.UsageTitle,
						b.ReadingNumber,
						/*Case When b.UsageId IN (1,3) AND 
								  b.BranchTypeId NOT IN (4) AND 
								  c.PhysicalSewageInstallDateJalali>'1330/01/01' 
							 Then b.Consumption 
							 When b.UsageId NOT IN (1,3) AND 
								  b.BranchTypeId NOT IN (4) AND 
								  c.PhysicalSewageInstallDateJalali>'1330/01/01' 
							 Then b.Consumption 
						     Else 0
						End SewageConsumption, */ 	
                        0 SewageConsumption,
						b.Consumption,
						b.ConsumptionAverage,
						b.WaterDiameterTitle as MeterDiameterTitle,
						b.BranchType AS BranchType,	
						b.Duration,
						--b.SumItems,
                        (b.ItemOff1+b.ItemOff2+b.ItemOff3+b.ItemOff4+b.ItemOff5+b.ItemOff6+b.ItemOff7+b.ItemOff8+b.ItemOff9+b.ItemOff10+b.ItemOff11+b.ItemOff12+b.ItemOff13+b.ItemOff14+b.ItemOff15+b.ItemOff16+b.ItemOff17+b.ItemOff18) SumItemOffs,
                        (b.ItemOff1 + b.ItemOff9 + b.ItemOff11 + b.ItemOff12 ) as SumOffWater,
						b.ItemOff1,
						b.ItemOff2,
						b.ItemOff3,
						b.ItemOff4,
						b.ItemOff5,
						b.ItemOff6,
						b.ItemOff7,
						b.ItemOff8,
						b.ItemOff9,
						b.ItemOff10,
						b.ItemOff11,
						b.ItemOff12,
						b.ItemOff13,
						b.ItemOff14,
						b.ItemOff15,
						b.ItemOff16,
						b.ItemOff17,
						b.ItemOff18,
                        IIF((b.OtherCount+b.CommercialCount+b.DomesticCount)=0,1,b.OtherCount+b.CommercialCount+b.DomesticCount) - b.EmptyCount BillUnit,
                        IIF((b.OtherCount+b.CommercialCount+b.DomesticCount)=0,1,b.OtherCount+b.CommercialCount+b.DomesticCount) TotalUnit
					From [CustomerWarehouse].dbo.Bills b
					--LEFT OUTER Join [CustomerWarehouse].dbo.Clients c
						--ON b.ZoneId=c.ZoneId and b.CustomerNumber=c.CustomerNumber
                    Join [Db70].dbo.T51 t51
                    	On b.ZoneId=t51.C0
                    Join [Db70].dbo.T46 t46
                    	On t51.C1=t46.C0
					Where 
						--c.ToDayJalali is null AND
						(b.RegisterDay BETWEEN @fromDate AND @toDate) AND
						(@fromConsumption IS NULL OR
						@toConsumption IS NULL OR
						b.Consumption BETWEEN @fromConsumption AND @toConsumption) AND
						(@fromAmount IS NULL OR
						@toAmount IS NULL OR
						b.SumItems BETWEEN @fromAmount AND @toAmount) AND
                        (@fromReadingNumber IS NULL OR
                        @toReadingNumber IS NULL OR
                        b.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
                        (b.ItemOff1+b.ItemOff2+b.ItemOff3+b.ItemOff4+b.ItemOff5+b.ItemOff6+b.ItemOff7+b.ItemOff8+b.ItemOff9+b.ItemOff10+b.ItemOff11+b.ItemOff12+b.ItemOff13+b.ItemOff14+b.ItemOff15+b.ItemOff16+b.ItemOff17+b.ItemOff18) > 0 AND
						b.TypeCode IN @typeCodes AND
                        {discountCauseCondition}
						{zoneQuery}";
        }
        internal string GetSummaryQuery(bool isUsageGroup, bool hasZone, WaterIncomeDiscountCauseEnum discountCauseId, WaterIncomeDiscountSummaryEnum summaryEnum)
        {
            string usageGroupJoinQuery = isUsageGroup ? @" Join [Db70].dbo.UsageGroup2 u2
				                                    	 	ON u2.Group1Id = @UsageGroupId 
				                                    	Join [Db70].dbo.UsageGroup3 u3 
				                                    		ON u2.Id=u3.Group2Id AND b.UsageId=u3.UsageId " : string.Empty;
            string usageGroup2TitleSelect = isUsageGroup ? " u2.Title UsageGroup2Title, " : string.Empty;
            string zoneQuery = hasZone ? "AND b.ZoneId IN @zoneIds" : string.Empty;
            string discountCauseCondition = GetDiscountCondition(discountCauseId);

            var (groupKey, SelectKey, orderKey) = GetEnumQuery(summaryEnum, isUsageGroup);

            //todo: rename "RegisterDay" to "PhysicalSewageInstallDateJalali"
            return @$";With cte as(
                    	Select
							t46.C2 RegionTitle,
							t46.C0 RegionId,
                    		b.ZoneTitle,
                            b.ZoneId,
                            b.UsageId,
                    		TRIM(b.BillId) as BillId,
                    		t41.C1 as UsageTitle, 
                            {usageGroup2TitleSelect}
                    		b.ReadingNumber,
                    		(b.CommercialCount+b.DomesticCount+b.OtherCount) as BillUnitCounts,
                            /*Case When b.UsageId IN (1,3) AND 
							    	  b.BranchTypeId NOT IN (4) AND 
							    	  c.PhysicalSewageInstallDateJalali>'1330/01/01' 
							     Then b.Consumption 
							     When b.UsageId NOT IN (1,3) AND 
							    	  b.BranchTypeId NOT IN (4) AND 
							    	  b.RegisterDay>'1330/01/01' 
							     Then b.Consumption 
						         Else 0
						    End SewageConsumption,*/
                            0 SewageConsumption,
                    		b.Consumption,
                    		b.ConsumptionAverage,
                    		b.WaterDiameterTitle as MeterDiameterTitle,
                    		b.BranchType AS BranchType,	
                            b.RegisterDay,
                    		b.Duration,
                    		--b.SumItems,
                            (b.ItemOff1+b.ItemOff2+b.ItemOff3+b.ItemOff4+b.ItemOff5+b.ItemOff6+b.ItemOff7+b.ItemOff8+b.ItemOff9+b.ItemOff10+b.ItemOff11+b.ItemOff12+b.ItemOff13+b.ItemOff14+b.ItemOff15+b.ItemOff16+b.ItemOff17+b.ItemOff18) SumItemOffs,
                            (b.ItemOff1 + b.ItemOff9 + b.ItemOff11 + b.ItemOff12 ) as SumWater,                    		
                            b.ItemOff1 ,
                    		b.ItemOff2,
                    		b.ItemOff3,
                    		b.ItemOff4,
                    		b.ItemOff5,
                    		b.ItemOff6,
                    		b.ItemOff7,
                    		b.ItemOff8,
                    		b.ItemOff9,
                    		b.ItemOff10,
                    		b.ItemOff11,
                    		b.ItemOff12,
                    		b.ItemOff13,
                    		b.ItemOff14,
                    		b.ItemOff15,
                    		b.ItemOff16,
                    		b.ItemOff17,
                    		b.ItemOff18,
                            IIF((b.OtherCount+b.CommercialCount+b.DomesticCount)=0,1,b.OtherCount+b.CommercialCount+b.DomesticCount) - b.EmptyCount BillUnit,
                            IIF((b.OtherCount+b.CommercialCount+b.DomesticCount)=0,1,b.OtherCount+b.CommercialCount+b.DomesticCount) TotalUnit,
                            Case
							    When b.TypeCode IN (1) THEN 1
							    When b.TypeCode NOT IN (1) Then 0
							    Else 0 
						    END BillC,
						    Case
							    When b.TypeCode IN (1) AND (b.OtherCount+b.CommercialCount+b.DomesticCount)<=0 THEN 1
							    When b.TypeCode IN (1) AND (b.OtherCount+b.CommercialCount+b.DomesticCount)>0 THEN (b.OtherCount+b.CommercialCount+b.DomesticCount)
							    Else 0 
						    END UnitC                            
                    From [CustomerWarehouse].dbo.Bills b
					--Join [CustomerWarehouse].dbo.Clients c
						--ON b.ZoneId=c.ZoneId and b.CustomerNumber=c.CustomerNumber
                    Join [Db70].dbo.T41 t41
                    	ON b.UsageId=t41.C0
                    Join [Db70].dbo.T51 t51
                    	ON b.ZoneId=t51.C0
                    Join [Db70].dbo.T46 t46
                    	ON t51.C1=t46.C0
                    {usageGroupJoinQuery}
                    Where 
                            --c.ToDayJalali is null AND
                    		(b.RegisterDay BETWEEN @fromDate AND @toDate) AND
                    		(@fromConsumption IS NULL OR
                    		@toConsumption IS NULL OR
                    		b.Consumption BETWEEn @fromConsumption AND @toConsumption) AND
                    		(@fromAmount IS NULL OR
                    		@toAmount IS NULL OR
                    		b.SumItems BETWEEN @fromAmount AND @toAmount) AND
                            (b.ItemOff1+b.ItemOff2+b.ItemOff3+b.ItemOff4+b.ItemOff5+b.ItemOff6+b.ItemOff7+b.ItemOff8+b.ItemOff9+b.ItemOff10+b.ItemOff11+b.ItemOff12+b.ItemOff13+b.ItemOff14+b.ItemOff15+b.ItemOff16+b.ItemOff17+b.ItemOff18) > 0 AND
                    		b.TypeCode IN @typeCodes AND
                            {discountCauseCondition}
                    		{zoneQuery}
                    )
                    Select
						MAX(RegionId) RegionId,
						MAX(RegionTitle) RegionTitle,
                    	{SelectKey} as GroupKey,
                        {orderKey} as OrderKey,
                        SUM(BillC) as BillCount,
                        COUNT(1) as TransactionCount,
                    	SUM(SewageConsumption) as SewageConsumption,
                    	SUM(Consumption) as Consumption,
                    	AVG(ConsumptionAverage) as ConsumptionAverage,
                    	SUM(Duration) as Duration,
                    	SUM(SumItemOffs) as SumItemOffs,
                    	SUM(BillUnitCounts) as BillUnitCounts,
                    	SUM(SumWater) as SumWater,
                    	SUM(ItemOff1) as ItemOff1,
                    	SUM(ItemOff2) as ItemOff2,
                    	SUM(ItemOff3) as ItemOff3,
                    	SUM(ItemOff4) as ItemOff4,
                    	SUM(ItemOff5) as ItemOff5,
                    	SUM(ItemOff6) as ItemOff6,
                    	SUM(ItemOff7) as ItemOff7,
                    	SUM(ItemOff8) as ItemOff8,
                    	SUM(ItemOff9) as ItemOff9,
                    	SUM(ItemOff10) as ItemOff10,
                    	SUM(ItemOff11) as ItemOff11,
                    	SUM(ItemOff12) as ItemOff12,
                    	SUM(ItemOff13) as ItemOff13,
                    	SUM(ItemOff14) as ItemOff14,
                    	SUM(ItemOff15) as ItemOff15,
                    	SUM(ItemOff16) as ItemOff16,
                    	SUM(ItemOff17) as ItemOff17,
                    	SUM(ItemOff18) as ItemOff18,
                        SUM(UnitC) as BillUnit,
                        SUM(TotalUnit) as TotalUnit
                    From cte
                    Group By {groupKey}, {orderKey}
                    Order By {orderKey}";
        }
        internal string GetIsZoneOrVillageTitle(IEnumerable<int> zoneIds)
        {
            int villageId = 140000;

            bool allVillages = zoneIds.All(z => z > villageId);
            bool anyVillage = zoneIds.Any(z => z > villageId);

            if (allVillages)
                return ReportLiterals.WithVillage;

            if (!anyVillage)
                return ReportLiterals.WithZone;

            return string.Empty;
        }
        internal (string, string, string) GetEnumQuery(WaterIncomeDiscountSummaryEnum enumState, bool isUsageGroup)
        {
            return enumState switch
            {
                WaterIncomeDiscountSummaryEnum.Zone => ("ZoneTitle", "ZoneTitle", "ZoneId"),
                WaterIncomeDiscountSummaryEnum.Region => ("RegionTitle", "RegionTitle", "RegionTitle"),
                _ => ("ZoneTitle", "ZoneTitle", "ZoneTitle")
            };
        }
        internal int[] GetTypeCodes(WaterIncomeAndConsumptionTypeEnum input)
        {
            return input switch
            {
                WaterIncomeAndConsumptionTypeEnum.Net => _netItems,
                WaterIncomeAndConsumptionTypeEnum.Raw => _rawItems,
                WaterIncomeAndConsumptionTypeEnum.Returned => _returnedItems,
                WaterIncomeAndConsumptionTypeEnum.PositiveModification => _positiveModifications,
                WaterIncomeAndConsumptionTypeEnum.NegativeModification => _negativeModifications,
                WaterIncomeAndConsumptionTypeEnum.PureReturn => _pureReturn,
                _ => _netItems
            };
        }
        internal string GetDiscountCondition(WaterIncomeDiscountCauseEnum discountCauseId)
        {
            return discountCauseId switch //ToDo: Compelete Conditions 
            {
                WaterIncomeDiscountCauseEnum.Madrese => " b.UsageId = 7 ",//u
                WaterIncomeDiscountCauseEnum.KomiteEmdad => " b.BranchTypeId = 7 ",//b
                WaterIncomeDiscountCauseEnum.Behzisti => " b.BranchTypeId = 6 ",//b
                WaterIncomeDiscountCauseEnum.GolzarShohada => " b.UsageId = 29 ",//u
                WaterIncomeDiscountCauseEnum.KhaneAlem => " b.BranchTypeId = 3 ",//b
                WaterIncomeDiscountCauseEnum.Masjed => " b.UsageId = 12 ",//u
                WaterIncomeDiscountCauseEnum.Hoseiniye=> " b.UsageId = 13 ",//u
                WaterIncomeDiscountCauseEnum.DarolGhoran => " b.UsageId = 10 ",//u
                WaterIncomeDiscountCauseEnum.MadaresOlomDini=> " b.UsageId = 32 ",//u
                WaterIncomeDiscountCauseEnum.AmakenMazhabiVaEmamZadeh=> " b.UsageId = 30 ",//u
                WaterIncomeDiscountCauseEnum.TajmiTakhfif=> " b.BranchTypeId IN (3,6,7) OR b.UsageId IN (7,29,12,13,10,32,30) ",
                _ => ""
            };
        }
    }
}
