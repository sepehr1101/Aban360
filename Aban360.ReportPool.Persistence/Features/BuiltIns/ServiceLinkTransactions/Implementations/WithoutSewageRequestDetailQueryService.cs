using Aban360.Common.BaseEntities;
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
    internal sealed class WithoutSewageRequestDetailQueryService : AbstractBaseConnection, IWithoutSewageRequestDetailQueryService
    {
        public WithoutSewageRequestDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<WithoutSewageRequestHeaderOutputDto, WithoutSewageRequestDetailDataOutputDto>> Get(WithoutSewageRequestInputDto input)
        {
            string withoutSewageRequest = GetBranchWithoutSewageRequestQuery();

            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                zoneIds = input.ZoneIds
            };
            IEnumerable<WithoutSewageRequestDetailDataOutputDto> withoutSewageRequestData = await _sqlReportConnection.QueryAsync<WithoutSewageRequestDetailDataOutputDto>(withoutSewageRequest, @params);
            WithoutSewageRequestHeaderOutputDto withoutSewageRequestHeader = new WithoutSewageRequestHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (withoutSewageRequestData is not null && withoutSewageRequestData.Any()) ? withoutSewageRequestData.Count() : 0,
            };
            var result = new ReportOutput<WithoutSewageRequestHeaderOutputDto, WithoutSewageRequestDetailDataOutputDto>
                (ReportLiterals.WithoutSewageRequestDetail, withoutSewageRequestHeader, withoutSewageRequestData);

            return result;
        }
        private string GetBranchWithoutSewageRequestQuery()
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
                    	c.WaterRequestDate AS WaterRequestDate,
						c.WaterInstallDate AS WaterInstallationDate
                    From [CustomerWarehouse].dbo.Clients c
                    Where	
                    	c.WaterInstallDate BETWEEN @fromDate AND @toDate AND
						(TRIM(c.SewageRequestDate)='' OR c.SewageRequestDate IS NULL) AND
                    	c.ZoneId IN @zoneIds AND
						c.ToDayJalali IS NULL";
        }
    }
}
