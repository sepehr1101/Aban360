using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class SewageWaterRequestNonInstalledDetailQueryService : AbstractBaseConnection, ISewageWaterRequestNonInstalledDetailQueryService
    {
        public SewageWaterRequestNonInstalledDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<SewageWaterRequestNonInstalledHeaderOutputDto, SewageWaterRequestNonInstalledDetailDataOutputDto>> Get(SewageWaterRequestNonInstalledInputDto input)
        {
            string requestNonInstalledDetailQuery;
            if (input.IsWater)
                requestNonInstalledDetailQuery = GetWaterRequestNonInstalledDetailQuery();
            else
                requestNonInstalledDetailQuery = GetSewageRequestNonInstalledDetailQuery();

            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                zoneIds = input.ZoneIds
            };
            IEnumerable<SewageWaterRequestNonInstalledDetailDataOutputDto> requestNonInstalledData = await _sqlReportConnection.QueryAsync<SewageWaterRequestNonInstalledDetailDataOutputDto>(requestNonInstalledDetailQuery, @params);
            SewageWaterRequestNonInstalledHeaderOutputDto requestNonInstalledHeader = new SewageWaterRequestNonInstalledHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (requestNonInstalledData is not null && requestNonInstalledData.Any()) ? requestNonInstalledData.Count() : 0
            };
            var result = new ReportOutput<SewageWaterRequestNonInstalledHeaderOutputDto, SewageWaterRequestNonInstalledDetailDataOutputDto>
                (input.IsWater ? ReportLiterals.WaterRequestNonInstalledDetail : ReportLiterals.SewageRequestNonInstalledDetail,
                requestNonInstalledHeader,
                requestNonInstalledData);

            return result;
        }
        private string GetWaterRequestNonInstalledDetailQuery()
        {
            return @"Select
                        c.CustomerNumber, 
                    	c.ReadingNumber,
                    	c.FirstName,
                    	c.SureName AS Surname,
                    	c.UsageTitle2 AS UsageTitle,
                    	c.WaterDiameterTitle AS MeterDiameterTitle,
                    	c.Address,
                    	c.ZoneTitle,
                    	c.ZoneId,
                    	c.DomesticCount	AS DomesticUnit,
                    	c.CommercialCount AS CommercialUnit,
                    	c.OtherCount AS OtherUnit,
                    	c.BillId,
                    	c.BranchType AS UseStateTitle,
                    	c.ContractCapacity,
                    	c.WaterRequestDate AS RequestDate
                    From [CustomerWarehouse].dbo.Clients c
                    Where	
                    	c.WaterRequestDate BETWEEN @fromDate AND @toDate AND
						(TRIM(c.WaterInstallDate)='' OR c.WaterInstallDate IS NULL) AND
                    	c.ZoneId IN @zoneIds";
        }
        private string GetSewageRequestNonInstalledDetailQuery()
        {
            return @"Select
                    	c.CustomerNumber, 
                    	c.ReadingNumber,
                    	c.FirstName,
                    	c.SureName AS Surname,
                    	c.UsageTitle2 AS UsageTitle,
                    	c.WaterDiameterTitle AS MeterDiameterTitle,
                    	c.Address,
                    	c.ZoneTitle,
                    	c.ZoneId,
                    	c.DomesticCount	AS DomesticUnit,
                    	c.CommercialCount AS CommercialUnit,
                    	c.OtherCount AS OtherUnit,
                    	c.BillId,
                    	c.BranchType AS UseStateTitle,
                    	c.ContractCapacity,
                    	c.WaterRequestDate AS RequestDate
                    From [CustomerWarehouse].dbo.Clients c
                    Where	
                    	c.SewageRequestDate BETWEEN @fromDate AND @toDate AND
						(TRIM(c.SewageInstallDate)='' OR c.SewageInstallDate IS NULL) AND
                    	c.ZoneId IN @zoneIds";
        }
    }
}
