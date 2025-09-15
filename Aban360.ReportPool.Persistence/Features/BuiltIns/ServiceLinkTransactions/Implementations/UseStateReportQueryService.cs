using Aban360.Common.BaseEntities;
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
            var @params = new
            {
                useStateId = input.UseStateId,
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                zoneIds = input.ZoneIds,
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber
            };

            IEnumerable<UseStateReportDataOutputDto> useStateData = await _sqlReportConnection.QueryAsync<UseStateReportDataOutputDto>(useStateQueryString, @params);
            UseStateReportHeaderOutputDto useStateHeader = new UseStateReportHeaderOutputDto()
            {
                TotalDebtAmount = useStateData.Sum(useState => useState.DebtAmount),
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                RecordCount = (useStateData is not null && useStateData.Any()) ? useStateData.Count() : 0,
           
                SumCommercialUnit = useStateData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = useStateData.Sum(i => i.DomesticUnit),
                SumOtherUnit = useStateData.Sum(i => i.OtherUnit),
                TotalUnit = useStateData.Sum(i => i.TotalUnit)
            };

            string useStateQuery = GetUseStateTitle();
            string useStateTitle = await _sqlConnection.QueryFirstAsync<string>(useStateQuery, new { useStateId = input.UseStateId });
            var result = new ReportOutput<UseStateReportHeaderOutputDto, UseStateReportDataOutputDto>(ReportLiterals.Report + " " + useStateTitle, useStateHeader, useStateData);

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
                        c.MainSiphonTitle AS SiphonDiameterTitle,
                    	c.RegisterDayJalali AS EventDateJalali,
                    	0 AS DebtAmount,
                    	TRIM(c.Address) AS Address,
                    	c.ZoneTitle,
                    	c.DeletionStateId,
                        c.DeletionStateTitle AS DeletionStateTitle,
                        c.DomesticCount DomesticUnit,
	                    c.CommercialCount CommercialUnit,
                        (c.CommercialCount+c.DomesticCount+c.DomesticCount) as TotalUnit,
	                    c.OtherCount OtherUnit,
                    	c.ContractCapacity AS ContractualCapacity,
	                    TRIM(c.BillId) BillId,
						c.MeterSerialBody AS MeterSerial,
						c.WaterRegisterDateJalali AS MeterInstallationDateJalali,
						c.WaterRequestDate AS MeterRequestDateJalali,
                        TRIM(c.PhoneNo) AS PhoneNumber,
						TRIM(c.MobileNo) AS MobileNumber,
						TRIM(c.NationalId) AS NationalCode,
						TRIM(c.PostalCode) AS PostalCode,
	                    RN=ROW_NUMBER() OVER (PARTITION BY ZoneId, CustomerNumber ORDER BY RegisterDayJalali DESC)
                    FROM [CustomerWarehouse].dbo.Clients c
                    WHERE 
                       c.FromDayJalali>=@fromDate and
                       c.ToDayJalali<=@toDate and
                       c.DeletionStateId=@useStateId and
                        (@fromReadingNumber IS NULL OR
						@toReadingNumber IS NULL OR
						c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) and  
                        c.ZoneId in @zoneIds)
                    SELECT * FROM CTE
                    WHERE RN=1 AND DeletionStateId=@useStateId ";
        }

        private string GetUseStateTitle()
        {
            return @"select Title
                     from [Aban360].ClaimPool.UseState 
                     where Id=@useStateId";
        }
    }
}
