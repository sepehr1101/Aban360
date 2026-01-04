using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class HandoverDetailQueryService : AbstractBaseConnection, IHandoverDetailQueryService
    {
        public HandoverDetailQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<HandoverHeaderOutputDto, HandoverDetailDataOutputDto>> Get(HandoverInputDto input)
        {
            string query = GetQuery();

            IEnumerable<HandoverDetailDataOutputDto> data = await _sqlReportConnection.QueryAsync<HandoverDetailDataOutputDto>(query, input);
            HandoverHeaderOutputDto header = new HandoverHeaderOutputDto()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                CustomerCount = (data is not null && data.Any()) ? data.Count() : 0,
                RecordCount = (data is not null && data.Any()) ? data.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title = ReportLiterals.HandoverDetail,

                SumCommercialUnit = data?.Sum(r => r.CommercialUnit) ?? 0,
                SumDomesticUnit = data?.Sum(r => r.DomesticUnit) ?? 0,
                SumOtherUnit = data?.Sum(r => r.OtherUnit) ?? 0,
                TotalUnit = data?.Sum(r => r.TotalUnit) ?? 0,
            };

            var result = new ReportOutput<HandoverHeaderOutputDto, HandoverDetailDataOutputDto>(ReportLiterals.HandoverDetail, header, data);
            return result;
        }

        private string GetQuery()
        {
            return @";WITH CTE AS
                    (
                    	SELECT 
                    	    RN= ROW_NUMBER() OVER (PARTITION by ZoneId , CustomerNumber ORDER BY RegisterDayJalali DESC, LocalId DESC),
                    	    *
                    	From [CustomerWarehouse].dbo.Clients c
                    	Where				
                    	    c.RegisterDayJalali BETWEEN @FromDateJalali AND @ToDateJalali AND
                    	    c.ZoneId IN @zoneIds AND                    	    
                    	    (
                    	        @fromReadingNumber IS NULL OR 
                    	        @toReadingNumber IS NULL OR
                    	        c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber
                    	    ) AND
                    	    c.CustomerNumber<>0 
                    )
                    Select	
                        c.ZoneTitle,
                    	c.CustomerNumber,
                    	c.ReadingNumber,
                    	c.FirstName,
                    	c.SureName AS Surname,
                    	c.FirstName +' '+ c.SureName AS FullName,
                    	c.UsageTitle2 AS UsageTitle,
                    	c.WaterDiameterTitle AS MeterDiameterTitle,
                    	c.Address ,
		                TRIM(c.PhoneNo) PhoneNumber,
		                TRIM(c.MobileNo) MobileNumber,
		                TRIM(c.NationalId) NationalCode,
                    	c.ZoneTitle,
                    	c.DomesticCount AS DomesticUnit,
                    	c.CommercialCount AS CommercialUnit,
                    	c.OtherCount AS OtherUnit,
                        IIF((c.DomesticCount+c.CommercialCount +c.OtherCount=0) ,1, (c.DomesticCount+c.CommercialCount +c.OtherCount)) AS TotalUnit,
                    	c.BillId,
                    	c.ContractCapacity AS ContractualCapacity,
                    	c.WaterRequestDate AS MeterRequestDate,
                    	c.WaterInstallDate AS MeterInstallationDate,
                    	c.BranchType AS UseStateTitle
                     FROM CTE c
                     WHERE	  
                         c.RN=1 AND
                         c.DeletionStateId NOT IN(1,2) AND
                         c.UsageStateId IN @branchTypeIds
                    Order By   
                        c.ZoneTitle,
                    	c.CustomerNumber";
        }
    }
}
