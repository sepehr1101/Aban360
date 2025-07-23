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
    internal sealed class SewageWaterInstallationDetailQueryService : AbstractBaseConnection, ISewageWaterInstallationDetailQueryService
    {
        public SewageWaterInstallationDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<SewageWaterInstallationHeaderOutputDto, SewageWaterInstallationDetailDataOutputDto>> Get(SewageWaterInstallationInputDto input)
        {
            string reportTitle = input.IsWater ? ReportLiterals.WaterInstallationDetail : ReportLiterals.SewageInstallationDetail;
            string installationDetailQuery;
            if (input.IsWater)
                installationDetailQuery = GetWaterInstallationDetailQuery();
            else
                installationDetailQuery = GetSewageInstallationDetailQuery();

            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                fromReadingNumber=input.FromReadingNumber,
                toReadingNumber=input.ToReadingNumber,
                zoneIds = input.ZoneIds
            };
            IEnumerable<SewageWaterInstallationDetailDataOutputDto> installationData = await _sqlReportConnection.QueryAsync<SewageWaterInstallationDetailDataOutputDto>(installationDetailQuery, @params);
            SewageWaterInstallationHeaderOutputDto installationHeader = new SewageWaterInstallationHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (installationData is not null && installationData.Any()) ? installationData.Count() : 0
            };

            ReportOutput<SewageWaterInstallationHeaderOutputDto, SewageWaterInstallationDetailDataOutputDto> result = new(reportTitle, installationHeader,installationData);

            return result;
        }
        private string GetWaterInstallationDetailQuery()
        {
            return @"Select
                    	c.CustomerNumber, 
                    	c.ReadingNumber,
                    	TRIM(c.FirstName) AS FirstName,
                    	TRIM(c.SureName) AS Surname,
                    	TRIM(c.Address) AS Address,
                    	c.UsageTitle2 AS UsageTitle,
                    	c.WaterDiameterTitle AS MeterDiameterTitle,
                    	c.ZoneTitle,
                    	c.ZoneId,
                    	c.DomesticCount	AS DomesticUnit,
                    	c.CommercialCount AS CommercialUnit,
                    	c.OtherCount AS OtherUnit,
                    	c.BillId,
                    	c.BranchType AS UseStateTitle,
                    	c.ContractCapacity AS ContractualCapacity,
                    	c.WaterRequestDate AS RequestDate,
                    	c.WaterInstallDate AS InstallationDate
                    From [CustomerWarehouse].dbo.Clients c
                    Where	
                    	c.WaterInstallDate BETWEEN @fromDate AND @toDate AND
                    	c.ZoneId IN @zoneIds AND
                        (@fromReadingNumber IS NULL OR
					    @toReadingNumber IS NULL OR
					    c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber)";
        }
        private string GetSewageInstallationDetailQuery()
        {
            return @"Select
                    	c.CustomerNumber, 
                    	c.ReadingNumber,
                    	TRIM(c.FirstName) AS FirstName,
                    	TRIM(c.SureName) AS Surname,
                    	TRIM(c.Address) AS Address,
                    	c.UsageTitle2 AS UsageTitle,
                    	c.WaterDiameterTitle AS MeterDiameterTitle,
                    	c.ZoneTitle,
                    	c.ZoneId,
                    	c.DomesticCount	AS DomesticUnit,
                    	c.CommercialCount AS CommercialUnit,
                    	c.OtherCount AS OtherUnit,
                    	c.BillId,
                    	c.BranchType AS UseStateTitle,
                    	c.ContractCapacity AS ContractualCapacity,
                    	c.SewageRequestDate AS RequestDate,
                    	c.SewageInstallDate AS InstallationDate
                    From [CustomerWarehouse].dbo.Clients c
                    Where	
                    	c.SewageInstallDate BETWEEN @fromDate AND @toDate AND
                    	c.ZoneId IN @zoneIds AND
                        (@fromReadingNumber IS NULL OR
					    @toReadingNumber IS NULL OR
					    c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber)";
        }
    }
}