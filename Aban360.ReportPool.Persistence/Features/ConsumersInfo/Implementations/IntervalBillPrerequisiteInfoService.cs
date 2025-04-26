using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Implementations
{
    internal sealed class IntervalBillPrerequisiteInfoService : AbstractBaseConnection, IIntervalBillPrerequisiteInfoService
    {
        public IntervalBillPrerequisiteInfoService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<IntervalBillSubscriptionInfo> GetInfo(string billId)
        {
            string summaryQuery = GetConsumerSummaryDtoQueryByBillId();
            IntervalBillSubscriptionInfo? summaryInfo = await _sqlConnection.QuerySingleOrDefaultAsync<IntervalBillSubscriptionInfo>(summaryQuery, new { id = billId });

            //string tagQuery = GetWaterMeterTagsQuery();
            //IEnumerable<string> tags = await _sqlConnection.QueryAsync<string>(tagQuery, new { id = billId });                      
            return summaryInfo;
        }

        public async Task<IEnumerable<IntervalBillSubscriptionInfo>> GetInfo(int zoneId, string registerDate ,string fromReadingNumber, string toReadingNumber)
        {
            string summaryQuery = GetConsumerSummaryDtoQueryByReadingNumber();
            IEnumerable<IntervalBillSubscriptionInfo>? summaryInfo = await _sqlConnection.QueryAsync<IntervalBillSubscriptionInfo>(summaryQuery, new { zoneId = zoneId, registerDate=registerDate, fromReadingNumber=fromReadingNumber, toReadingNumber=toReadingNumber });

            //string tagQuery = GetWaterMeterTagsQuery();
            //IEnumerable<string> tags = await _sqlConnection.QueryAsync<string>(tagQuery, new { id = billId });                      
            return summaryInfo;
        }

        private string GetConsumerSummaryDtoQueryByBillId()
        {
            string query = @"
                 SELECT TOP 1
                     W.CustomerNumber,
                     W.BillId,
                     W.ReadingNumber,
                     W.InstallationDate,
                     W.ProductDate,
                     E.ContractualCapacity,
                     E.HouseholdNumber,
                     E.UnitDomesticWater,
                     E.UnitCommercialWater,
                     E.UnitOtherWater,
                     E.UnitDomesticSewage,
                     E.UnitCommercialSewage,
                     E.UnitOtherSewage,
                     E.EmptyUnit,
                     C.Id AS ConstructionTypeId,
                     U.Id AS UsageConsumptionId,
                     UU.Id AS UsageSellId,
                     S.InstallationDate AS SiphonInstallationDate,
                     CD.Id AS CordinalDirectionId,
                     H.Id AS HeadquartersId,
                     P.Id AS ProvinceId,
                     R.Id AS RegionId,
                     Z.Id AS ZoneId,
                     M.Id AS MunicipalityId,
	                 0 AS PreviousWaterMeterNumber,
	                 '' AS PreviousWaterMeterDateJalali
                 FROM [ClaimPool].WaterMeter W
                 JOIN [ClaimPool].Estate E ON W.EstateId = E.Id
                 LEFT JOIN [ClaimPool].IndividualEstate IE ON E.Id = IE.EstateId
                 LEFT JOIN [ClaimPool].Individual I ON IE.IndividualId = I.Id
                 LEFT JOIN [ClaimPool].ConstructionType C ON E.ConstructionTypeId = C.Id
                 LEFT JOIN [ClaimPool].Flat F ON E.Id = F.EstateId
                 LEFT JOIN [ClaimPool].Usage U ON E.UsageConsumtionId = U.Id
                 LEFT JOIN [ClaimPool].Usage UU ON E.UsageSellId = UU.Id
                 LEFT JOIN [ClaimPool].WaterMeterSiphon WS ON W.Id = WS.WaterMeterId
                 LEFT JOIN [ClaimPool].Siphon S ON WS.SiphonId = S.Id
                 LEFT JOIN [LocationPool].Municipality M ON E.MunipulityId = M.Id
                 LEFT JOIN [LocationPool].Zone Z ON M.ZoneId = Z.Id
                 LEFT JOIN [LocationPool].Region R ON Z.RegionId = R.Id
                 LEFT JOIN [LocationPool].Headquarters H ON R.HeadquartersId = H.Id
                 LEFT JOIN [LocationPool].Province P ON H.ProvinceId = P.Id
                 LEFT JOIN [LocationPool].CordinalDirection CD ON P.CordinalDirectionId = CD.Id
                 WHERE W.BillId = @id";
            return query;
        }
        private string GetConsumerSummaryDtoQueryByReadingNumber()
        {
            string query = @"
                 SELECT
                     W.CustomerNumber,
                     W.BillId,
                     W.ReadingNumber,
                     W.InstallationDate,
                     W.ProductDate,
                     E.ContractualCapacity,
                     E.HouseholdNumber,
                     E.UnitDomesticWater,
                     E.UnitCommercialWater,
                     E.UnitOtherWater,
                     E.UnitDomesticSewage,
                     E.UnitCommercialSewage,
                     E.UnitOtherSewage,
                     E.EmptyUnit,
                     C.Id AS ConstructionTypeId,
                     U.Id AS UsageConsumptionId,
                     UU.Id AS UsageSellId,
                     S.InstallationDate AS SiphonInstallationDate,
                     CD.Id AS CordinalDirectionId,
                     H.Id AS HeadquartersId,
                     P.Id AS ProvinceId,
                     R.Id AS RegionId,
                     Z.Id AS ZoneId,
                     M.Id AS MunicipalityId,
	                 0 AS PreviousWaterMeterNumber,
	                 '' AS PreviousWaterMeterDateJalali
                 FROM [ClaimPool].WaterMeter W
                 JOIN [ClaimPool].Estate E ON W.EstateId = E.Id
                 LEFT JOIN [ClaimPool].IndividualEstate IE ON E.Id = IE.EstateId
                 LEFT JOIN [ClaimPool].Individual I ON IE.IndividualId = I.Id
                 LEFT JOIN [ClaimPool].ConstructionType C ON E.ConstructionTypeId = C.Id
                 LEFT JOIN [ClaimPool].Flat F ON E.Id = F.EstateId
                 LEFT JOIN [ClaimPool].Usage U ON E.UsageConsumtionId = U.Id
                 LEFT JOIN [ClaimPool].Usage UU ON E.UsageSellId = UU.Id
                 LEFT JOIN [ClaimPool].WaterMeterSiphon WS ON W.Id = WS.WaterMeterId
                 LEFT JOIN [ClaimPool].Siphon S ON WS.SiphonId = S.Id
                 LEFT JOIN [LocationPool].Municipality M ON E.MunipulityId = M.Id
                 LEFT JOIN [LocationPool].Zone Z ON M.ZoneId = Z.Id
                 LEFT JOIN [LocationPool].Region R ON Z.RegionId = R.Id
                 LEFT JOIN [LocationPool].Headquarters H ON R.HeadquartersId = H.Id
                 LEFT JOIN [LocationPool].Province P ON H.ProvinceId = P.Id
                 LEFT JOIN [LocationPool].CordinalDirection CD ON P.CordinalDirectionId = CD.Id
                 WHERE 
                    M.ZoneId=@zoneId AND 
	                TRIM(W.ReadingNumber) BETWEEN @fromReadingNumber AND @toReadingNumber";
            return query;
        }

        private string GetWaterMeterTagsQuery()
        {
            string query = @"
                SELECT WTD.Title
                FROM [ClaimPool].WaterMeter W
                JOIN [ClaimPool].WaterMeterTag WT ON W.Id = WT.WaterMeterId
                JOIN [ClaimPool].WaterMeterTagDefinition WTD ON WT.WaterMeterTagDefinitionId = WTD.Id
                WHERE W.BillId = @id";
            return query;
        }
    }
}