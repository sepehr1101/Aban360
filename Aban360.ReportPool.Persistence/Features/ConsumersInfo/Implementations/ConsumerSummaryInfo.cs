﻿using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
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
                    CounterState = N'عادی',--todo
               	    CounterStatus=N'متصل به شبکه',--todo
               	    LastDept = N'53000',--todo
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