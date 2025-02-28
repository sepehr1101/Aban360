using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using Aban360.ReportPool.Persistence.Queries.Implementations;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Implementations
{
    internal class ConsumerSummaryQueryService : AbstractBaseConnection, IConsumerSummaryQueryService
    {
        public ConsumerSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<ConsumerSummaryDto> GetInfo(string billId)
        {
            string summaryQuery = GetConsumerSummaryDtoQuery();
            ConsumerSummaryDto? summaryInfo = await _sqlConnection.QuerySingleOrDefaultAsync<ConsumerSummaryDto>(summaryQuery, new { id = billId });

            string tagQuery = GetWaterMeterTagsQuery();
            IEnumerable<string> tags = await _sqlConnection.QueryAsync<string>(tagQuery, new { id = billId });

            if (summaryInfo is not null)
            {
                summaryInfo.WaterMeterTags = new[] { "سگ نگهبان", "گیاه اکالیپتوس" };//tags.ToList();
            }
            return summaryInfo;
        }

        private string GetConsumerSummaryDtoQuery()
        {
            string query = @"
                SELECT TOP 1
                    W.CustomerNumber,
                    W.BillId,
                    W.ReadingNumber,
                    W.InstallationDate,
                    W.ProductDate,
                    W.GuaranteeDate,
                    E.Address,
                    E.ContractualCapacity,
                    E.HouseholdNumber,
                    E.UnitDomesticWater,
                    E.UnitCommercialWater,
                    E.UnitOtherWater,
                    E.UnitDomesticSewage,
                    E.UnitCommercialSewage,
                    E.UnitOtherSewage,
                    E.EmptyUnit,
                    C.Title AS ConstructionType,
                    U.Title AS UsageConsumtion,
                    UU.Title AS UsageSell,
                    I.FullName,
                    S.InstallationDate AS SiphonInstallationDate,
                    CD.Title CordinalDirectionTitle,
                    H.Title AS HeadquartersTitle,
                    P.Title AS ProvinceTitle,
                    R.Title AS RegionTitle,
                    Z.Title AS ZoneTitle,
                    M.Title AS MunicipalityTitle
                FROM WaterMeter W
                JOIN Estate E ON W.EstateId = E.Id
                LEFT JOIN IndividualEstate IE ON E.Id = IE.EstateId
                LEFT JOIN Individual I ON IE.IndividualId = I.Id
                LEFT JOIN ConstructionType C ON E.ConstructionTypeId = C.Id
                LEFT JOIN Flat F ON E.Id = F.EstateId
                LEFT JOIN Usage U ON E.UsageConsumtionId = U.Id
                LEFT JOIN Usage UU ON E.UsageSellId = UU.Id
                LEFT JOIN WaterMeterSiphon WS ON W.Id = WS.WaterMeterId
                LEFT JOIN Siphon S ON WS.SiphonId = S.Id
                LEFT JOIN Municipality M ON E.MunipulityId = M.Id
                LEFT JOIN Zone Z ON M.ZoneId = Z.Id
                LEFT JOIN Region R ON Z.RegionId = R.Id
                LEFT JOIN Headquarters H ON R.HeadquartersId = H.Id
                LEFT JOIN Province P ON H.ProvinceId = P.Id
                LEFT JOIN CordinalDirection CD ON P.CordinalDirectionId = CD.Id
                WHERE W.BillId = @id";
            return query;
        }

        private string GetWaterMeterTagsQuery()
        {
            string query = @"
                SELECT WTD.Title
                FROM WaterMeter W
                JOIN WaterMeterTag WT ON W.Id = WT.WaterMeterId
                JOIN WaterMeterTagDefinition WTD ON WT.WaterMeterTagDefinitionId = WTD.Id
                WHERE W.BillId = @id";
            return query;
        }
    }
}