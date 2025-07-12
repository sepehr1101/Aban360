using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class UseStateReportQueryService : AbstractBaseConnection, IUseStateReportQueryService
    {
        public UseStateReportQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<UseStateReportHeaderOutputDto, UseStateReportDataOutputDto>> GetInfo(UseStateReportInputDto input)
        {
            string useStateQueryString = GetUseStateDataQuery();
            IEnumerable<UseStateReportDataOutputDto> useStateData = await _sqlReportConnection.QueryAsync<UseStateReportDataOutputDto>(useStateQueryString, new { useStateId=input.UseStateId, fromDate=input.FromDateJalali, toDate=input.ToDateJalali , zoneIds = input.ZoneIds, fromReadingNumber=input.FromReadingNumber, toReadingNumber=input.ToReadingNumber });
            UseStateReportHeaderOutputDto useStateHeader = new UseStateReportHeaderOutputDto()
            { 
                TotalDebtAmount=useStateData.Sum(useState=>useState.DebtAmount),
                FromDateJalali=input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                RecordCount=useStateData.Count(),
            };

            string useStateQuery = GetUseStateTitle();
            string useStateTitle=await _sqlConnection.QueryFirstAsync<string>(useStateQuery,new {useStateId=input.UseStateId});
            var result = new ReportOutput<UseStateReportHeaderOutputDto, UseStateReportDataOutputDto>(ReportLiterals.Report+" "+useStateTitle , useStateHeader, useStateData);
            
            return result;
        }

        private string GetUseStateDataQuery()
        {
            return @";WITH CTE AS 
                     (SELECT 
                    	c.CustomerNumber,
                    	c.ReadingNumber,
                    	TRIM(c.FirstName) AS FirstName,
                    	TRIM(c.SureName) As Surname,
                    	c.UsageTitle,
                    	c.WaterDiameterTitle MeterDiameterTitle,
                    	c.RegisterDayJalali AS EventDateJalali,
                    	0 AS DebtAmount,
                    	TRIM(c.Address) AS Address,
                    	c.ZoneTitle,
                    	c.DeletionStateId,
                        c.DeletionStateTitle AS DeletionStateTitle,
                        c.DomesticCount DomesticUnit,
	                    c.CommercialCount CommercialUnit,
	                    c.OtherCount OtherUnit,
	                    TRIM(c.BillId) BillId,
	                    RN=ROW_NUMBER() OVER (PARTITION BY ZoneId, CustomerNumber ORDER BY RegisterDayJalali DESC)
                    FROM [CustomerWarehouse].dbo.Clients c
                    WHERE 
                       c.FromDayJalali>=@fromDate and
                       c.ToDayJalali<=@toDate and
                       c.DeletionStateId=@useStateId and
                       --c.ReadingNumber BETWEEN @fromReadingNumber and @toReadingNumber AND
                       c.ZoneId in @zoneIds)
                    SELECT * FROM CTE
                    WHERE RN=1 AND DeletionStateId=@useStateId ";
        }

        private string GetUseStateTitle()
        {
            return @"select Title
                     from ClaimPool.UseState 
                     where Id=@useStateId";
        }
    }
}
