using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Constants;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Base
{
    internal abstract class WaterIncomeAndConsumptionBase : AbstractBaseConnection
    {
        private static int[] _netItems = { 1, 3, 4, 5 };
        private static int[] _rawItems = { 1 };
        private static int[] _returnedItems = { 3, 4, 5 };
        public WaterIncomeAndConsumptionBase(IConfiguration configuration)
            : base(configuration)
        {
        }
        internal int[] GetTypeCodes(WaterIncomeAndConsumptionTypeEnum input)
        {
            return input switch
            {
                WaterIncomeAndConsumptionTypeEnum.Net => _netItems,
                WaterIncomeAndConsumptionTypeEnum.Raw => _rawItems,
                WaterIncomeAndConsumptionTypeEnum.Returned => _returnedItems,
                _ => _netItems
            };
        }
        internal string GetDetailQuery(bool hasZone, bool hasUsage, bool hasBranchType)
        {
            string zoneQuery = hasZone ? "AND b.ZoneId IN @zoneIds" : string.Empty;
            string usageQuery = hasUsage ? "AND b.UsageId IN @usageIds" : string.Empty;
            string branchTypeQuery = hasBranchType ? "AND b.BranchTypeId IN @branchTypeIds" : string.Empty;

            //todo: rename "RegisterDay" to "PhysicalSewageInstallDateJalali"
            return @$"use CustomerWarehouse
					Select
        				t46.C2 RegionTitle,
						b.ZoneTitle,
						TRIM(b.BillId) as BillId,
						b.UsageTitle,
						b.ReadingNumber,
						Case When b.UsageId IN (1,3) AND 
								  b.BranchTypeId NOT IN (4) AND 
								  b.RegisterDay>'1330/01/01' 
							 Then b.Consumption 
							 When b.UsageId NOT IN (1,3) AND 
								  b.BranchTypeId NOT IN (4) AND 
								  b.RegisterDay>'1330/01/01' 
							 Then b.Consumption 
						     Else 0
						End SewageConsumption,  --/PhysicalSewageInstallDateJalali	
						b.Consumption,
						b.ConsumptionAverage,
						b.WaterDiameterTitle as MeterDiameterTitle,
						b.BranchType AS BranchType,	
						b.Duration,
						--b.SumItems,
                        (b.Item1+b.Item2+b.Item3+b.Item4+b.Item5+b.Item6+b.Item7+b.Item8+b.Item9+b.Item10+b.Item11+b.Item12+b.Item13+b.Item14+b.Item15+b.Item16+b.Item17+b.Item18) SumItems,
                        (b.Item1 + b.Item9 + b.Item11 + b.Item12 ) as SumWater,
						b.Item1,
						b.Item2,
						b.Item3,
						b.Item4,
						b.Item5,
						b.Item6,
						b.Item7,
						b.Item8,
						b.Item9,
						b.Item10,
						b.Item11,
						b.Item12,
						b.Item13,
						b.Item14,
						b.Item15,
						b.Item16,
						b.Item17,
						b.Item18,
                        IIF((OtherCount+CommercialCount+DomesticCount)=0,1,OtherCount+CommercialCount+DomesticCount) - EmptyCount BillUnit,
                        IIF((OtherCount+CommercialCount+DomesticCount)=0,1,OtherCount+CommercialCount+DomesticCount) TotalUnit
					From [CustomerWarehouse].dbo.Bills b
                    Join [Db70].dbo.T51 t51
                    	On b.ZoneId=t51.C0
                    Join [Db70].dbo.T46 t46
                    	On t51.C1=t46.C0
					Where 
						(b.RegisterDay BETWEEN @fromDate AND @toDate) AND
						(@fromConsumption IS NULL OR
						@toConsumption IS NULL OR
						b.Consumption BETWEEn @fromConsumption AND @toConsumption) AND
						(@fromAmount IS NULL OR
						@toAmount IS NULL OR
						b.SumItems BETWEEN @fromAmount AND @toAmount) AND
                        (@fromReadingNumber IS NULL OR
                        @toReadingNumber IS NULL OR
                        b.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
						b.TypeCode IN @typeCodes
						{usageQuery}
						{zoneQuery}
                        {branchTypeQuery}";
        }
        internal string GetSummaryQuery(bool isUsageGroup, bool hasZone, bool hasUsage, bool hasBranchType, WaterIncomeAndConsumptionSummaryEnum enumState)
        {
            string usageGroupJoinQuery = isUsageGroup ? @"	Join [Db70].dbo.UsageGroup2 u2
				                                    	 	ON u2.Group1Id = @UsageGroupId 
				                                    	Join [Db70].dbo.UsageGroup3 u3 
				                                    		ON u2.Id=u3.Group2Id AND b.UsageId=u3.UsageId	" : string.Empty;
            string usageGroup2TitleSelect = isUsageGroup ? " u2.Title UsageGroup2Title, " : string.Empty;
            string zoneQuery = hasZone ? "AND b.ZoneId IN @zoneIds" : string.Empty;
            string usageQuery = hasUsage ? "AND b.UsageId IN @usageIds" : string.Empty;
            string branchTypeQuery = hasBranchType ? "AND b.BranchTypeId IN @branchTypeIds" : string.Empty;

            var (groupKey, SelectKey) = GetEnumQuery(enumState, isUsageGroup);

            //todo: rename "RegisterDay" to "PhysicalSewageInstallDateJalali"
            return @$";With cte as(
                    	Select
							t46.C2 RegionTitle,
							t46.C0 RegionId,
                    		b.ZoneTitle,
                    		TRIM(b.BillId) as BillId,
                    		t41.C1 as UsageTitle, 
                            {usageGroup2TitleSelect}
                    		b.ReadingNumber,
                    		(b.CommercialCount+b.DomesticCount+b.OtherCount) as BillUnitCounts,
                            Case When b.UsageId IN (1,3) AND 
							    	  b.BranchTypeId NOT IN (4) AND 
							    	  b.RegisterDay>'1330/01/01' 
							     Then b.Consumption 
							     When b.UsageId NOT IN (1,3) AND 
							    	  b.BranchTypeId NOT IN (4) AND 
							    	  b.RegisterDay>'1330/01/01' 
							     Then b.Consumption 
						         Else 0
						    End SewageConsumption,  --/PhysicalSewageInstallDateJalali	
                    		b.Consumption,
                    		b.ConsumptionAverage,
                    		b.WaterDiameterTitle as MeterDiameterTitle,
                    		b.BranchType AS BranchType,	
                            b.RegisterDay,
                    		b.Duration,
                    		--b.SumItems,
                            (b.Item1+b.Item2+b.Item3+b.Item4+b.Item5+b.Item6+b.Item7+b.Item8+b.Item9+b.Item10+b.Item11+b.Item12+b.Item13+b.Item14+b.Item15+b.Item16+b.Item17+b.Item18) SumItems,
                            (b.Item1 + b.Item9 + b.Item11 + b.Item12 ) as SumWater,                    		
                            b.Item1 ,
                    		b.Item2,
                    		b.Item3,
                    		b.Item4,
                    		b.Item5,
                    		b.Item6,
                    		b.Item7,
                    		b.Item8,
                    		b.Item9,
                    		b.Item10,
                    		b.Item11,
                    		b.Item12,
                    		b.Item13,
                    		b.Item14,
                    		b.Item15,
                    		b.Item16,
                    		b.Item17,
                    		b.Item18,
                            IIF((OtherCount+CommercialCount+DomesticCount)=0,1,OtherCount+CommercialCount+DomesticCount) - EmptyCount BillUnit,
                            IIF((OtherCount+CommercialCount+DomesticCount)=0,1,OtherCount+CommercialCount+DomesticCount) TotalUnit
                    From [CustomerWarehouse].dbo.Bills b
                    Join [Db70].dbo.T41 t41
                    	ON b.UsageId=t41.C0
                    Join [Db70].dbo.T51 t51
                    	ON b.ZoneId=t51.C0
                    Join [Db70].dbo.T46 t46
                    	ON t51.C1=t46.C0
                    {usageGroupJoinQuery}
                    Where 
                    		(b.RegisterDay BETWEEN @fromDate AND @toDate) AND
                    		(@fromConsumption IS NULL OR
                    		@toConsumption IS NULL OR
                    		b.Consumption BETWEEn @fromConsumption AND @toConsumption) AND
                    		(@fromAmount IS NULL OR
                    		@toAmount IS NULL OR
                    		b.SumItems BETWEEN @fromAmount AND @toAmount) AND
                    		b.TypeCode IN @typeCodes
                    		{usageQuery}
                    		{zoneQuery}
                    		{branchTypeQuery}
                    )
                    Select
						MAX(RegionId) RegionId,
						MAX(RegionTitle) RegionTitle,
                    	{SelectKey} as GroupKey,
                    	Count(1) as BillCount,
                    	SUM(SewageConsumption) as SewageConsumption,
                    	SUM(Consumption) as Consumption,
                    	AVG(ConsumptionAverage) as ConsumptionAverage,
                    	SUM(Duration) as Duration,
                    	SUM(SumItems) as SumItems,
                    	SUM(BillUnitCounts) as BillUnitCounts,
                    	SUM(SumWater) as SumWater,
                    	SUM(Item1) as Item1,
                    	SUM(Item2) as Item2,
                    	SUM(Item3) as Item3,
                    	SUM(Item4) as Item4,
                    	SUM(Item5) as Item5,
                    	SUM(Item6) as Item6,
                    	SUM(Item7) as Item7,
                    	SUM(Item8) as Item8,
                    	SUM(Item9) as Item9,
                    	SUM(Item10) as Item10,
                    	SUM(Item11) as Item11,
                    	SUM(Item12) as Item12,
                    	SUM(Item13) as Item13,
                    	SUM(Item14) as Item14,
                    	SUM(Item15) as Item15,
                    	SUM(Item16) as Item16,
                    	SUM(Item17) as Item17,
                    	SUM(Item18) as Item18,
                        SUM(BillUnit) as BillUnit,
                        SUM(TotalUnit) as TotalUnit
                    From cte
                    Group By {groupKey}
                    Order By {groupKey}";
        }
        internal (string, string) GetEnumQuery(WaterIncomeAndConsumptionSummaryEnum enumState, bool isUsageGroup)
        {
            if (enumState == WaterIncomeAndConsumptionSummaryEnum.AverageConsumption)
                return ("Ceiling(ConsumptionAverage)", "Ceiling(ConsumptionAverage)");
            if (enumState == WaterIncomeAndConsumptionSummaryEnum.RegisterDay)
                return ("RegisterDay", "RegisterDay");
            if (enumState == WaterIncomeAndConsumptionSummaryEnum.Zone)
                return ("ZoneTitle", "ZoneTitle");
            if (enumState == WaterIncomeAndConsumptionSummaryEnum.Usage)
                return isUsageGroup ? ("UsageGroup2Title", "UsageGroup2Title") : ("UsageTitle", "UsageTitle");
            if (enumState == WaterIncomeAndConsumptionSummaryEnum.Region)
                return ("RegionTitle", "RegionTitle");
            if (enumState == WaterIncomeAndConsumptionSummaryEnum.UsageAndZone)
                return isUsageGroup ?
                    ("ZoneTitle, UsageGroup2Title", "(ZoneTitle CollaTe SQL_Latin1_General_CP1_CI_AS+' - '+ UsageGroup2Title)") :
                    ("ZoneTitle, UsageTitle", "(ZoneTitle CollaTe SQL_Latin1_General_CP1_CI_AS+' - '+UsageTitle)");

            return ("ZoneTitle", "ZoneTitle");
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
    }
}
