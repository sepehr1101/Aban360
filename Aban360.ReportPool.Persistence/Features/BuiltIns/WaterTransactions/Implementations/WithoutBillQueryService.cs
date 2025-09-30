using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class WithoutBillQueryService : AbstractBaseConnection, IWithoutBillQueryService
    {
        public WithoutBillQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<WithoutBillHeaderOutputDto, WithoutBillDataOutputDto>> GetInfo(WithoutBillInputDto input)
        {
            string withoutBill = GetWithoutBillQuery(input.ZoneIds?.Any()==true,input.UsageIds?.Any() == true);
            var @params = new
            { 
                FromDate=input.FromDateJalali,
                ToDate=input.ToDateJalali,
                FromReadingNumber=input.FromReadingNumber,
                ToReadingNumber=input.ToReadingNumber,
                ZoneIds=input.ZoneIds,
                usageIds=input.UsageIds,
            };

            IEnumerable<WithoutBillDataOutputDto> withoutBillData = await _sqlReportConnection.QueryAsync<WithoutBillDataOutputDto>(withoutBill,@params);
            WithoutBillHeaderOutputDto withoutBillHeader = new WithoutBillHeaderOutputDto()
            {
                FromDateJalali=input.FromDateJalali,
                ToDateJalali=input.ToDateJalali,
                FromReadingNumber=input.FromReadingNumber,
                ToReadingNumber=input.ToReadingNumber,
                RecordCount= (withoutBillData is not null && withoutBillData.Any()) ? withoutBillData.Count() : 0,
                CustomerCount = (withoutBillData is not null && withoutBillData.Any()) ? withoutBillData.Count() : 0,
                ReportDateJalali =DateTime.Now.ToShortPersianDateString()
            };

            var result = new ReportOutput<WithoutBillHeaderOutputDto, WithoutBillDataOutputDto>(ReportLiterals.WithoutBill, withoutBillHeader, withoutBillData);
            return result;
        }

        private string GetWithoutBillQuery(bool hasZone,bool hasUsage)
        {
            string zoneQuery = hasZone ? "AND c.ZoneId IN @ZoneIds" : string.Empty;
            string usageQuery = hasUsage ? "AND c.UsageId IN @usageIds" : string.Empty;

            
            return $@"	Select
						c.BillId AS BillId,
						c.ZoneId,
						bb.CounterStateTitle AS CounterStateTitle,
						c.WaterRequestDate AS MeterRequestDateJalali,
						c.WaterRegisterDateJalali AS MeterInstallationDateJalali,
						c.MobileNo as MobileNumber,
						c.PhoneNo as PhoneNumber,
						c.ContractCapacity as ContractualCapacity,
						c.CommercialCount as CommercialUnit,
						c.DomesticCount as DomesticUnit,
						c.OtherCount as OtherUnit,
						(c.ContractCapacity + c.DomesticCount + c.OtherCount) as TotalUnit,
						c.MainSiphonTitle as  SiphonDiameterTitle,
						c.UsageTitle as UsageTitle,
						TRIM(c.NationalId) as NationalCode,
						c.EmptyCount as EmptyUnit,
                    	c.CustomerNumber as CustomerNumber,
                        c.ReadingNumber,
                    	TRIM(c.FirstName) +' '+TRIM(c.SureName) as FullName,
                    	c.WaterDiameterTitle as MeterDiameterTitle,
                    	c.UsageTitle2 as UsageSellTitle,
                    	TRIM(c.Address) as Address,
                    	c.ZoneTitle as ZoneTitle,
						bb.RegisterDay as LatestBillDateJalali,
						bb.NextDay as LatestReadingDateJalali
					From [CustomerWarehouse].dbo.Clients c
					Join [CustomerWarehouse].dbo.Bills bb
						On c.ZoneId=bb.ZoneId AND c.CustomerNumber=bb.CustomerNumber
					Where NOT EXISTS(
						Select 1
						From [CustomerWarehouse].dbo.Bills b
						Where 
							c.ZoneId=b.ZoneId AND
							c.CustomerNumber=b.CustomerNumber AND
							(@FromDate IS NULL or
                    	   	  @ToDate IS NULL or 
                    	   	  b.RegisterDay BETWEEN @FromDate and @ToDate)AND 
							 (@FromReadingNumber IS NULL or
                    		  @ToReadingNumber IS NULL or 
                    		  c.ReadingNumber BETWEEN @FromReadingNumber and @ToReadingNumber) AND
                    		c.DeletionStateId IN (0,2)  AND
							b.TypeCode = 1 AND
							c.ToDayJalali IS NULL 
                            {zoneQuery}
                            {usageQuery}
						)AND
						 (@FromReadingNumber IS NULL or
                    		  @ToReadingNumber IS NULL or 
                    		  c.ReadingNumber BETWEEN @FromReadingNumber and @ToReadingNumber) AND
                    		c.DeletionStateId IN (0,2)  AND
							c.ToDayJalali IS NULL AND
                            bb.TypeCode = 1
							{zoneQuery}
                            {usageQuery}
					Order By bb.RegisterDay";
        }
    }
}
#region last query
/*return @$"Select 
                        c.BillId AS BillId,
						b.CounterStateTitle AS CounterStateTitle,
						c.WaterRequestDate AS WaterRequestDateJalali,
						c.WaterRegisterDateJalali AS WaterInstallationDateJalali,
						c.MobileNo as MobileNumber,
						c.PhoneNo as PhoneNumber,
						c.ContractCapacity as ContractualCapacity,
						c.CommercialCount as CommercialUnit,
						c.DomesticCount as DomesticUnit,
						c.OtherCount as OtherUnit,
						(c.ContractCapacity + c.DomesticCount + c.OtherCount) as TotalUnit,
						c.HasCommonSiphon as  SiphonDiameterTitle,
						c.UsageTitle as UsageTitle,
						TRIM(c.NationalId) as NationalCode,
						c.EmptyCount as EmptyUnit,
                    	c.CustomerNumber as CustomerNumber,
                        c.ReadingNumber,
                    	TRIM(c.FirstName) +' '+TRIM(c.SureName) as FullName,
                    	c.WaterDiameterTitle as MeterDiameterTitle,
                    	c.UsageTitle2 as UsageSellTitle,
                    	TRIM(c.Address) as Address,
                    	c.ZoneTitle as ZoneTitle,
						b.NextDay as LatestBillDateJalali
                    	--RN=ROW_NUMBER() OVER (Partition By c.CustomerNumber , c.ZoneId Order By c.CustomerNumber)
                    From [CustomerWarehouse].dbo.Clients c
                    LEFt JOIN [CustomerWarehouse].dbo.Bills b
                    on c.ZoneId=b.ZoneId AND c.CustomerNumber=b.CustomerNumber
                    where 
                    	 b.Id IS NULL AND
						 (@FromDate IS NULL or
                    	   	  @ToDate IS NULL or 
                    	   	  c.WaterInstallDate BETWEEN @FromDate and @ToDate)AND 
						 (@FromReadingNumber IS NULL or
                    	     @ToReadingNumber IS NULL or 
                    		 c.ReadingNumber BETWEEN @FromReadingNumber and @ToReadingNumber) AND
                    	c.DeletionStateId IN (0,2)  AND
						c.ToDayJalali IS NULL 
                       {zoneQuery}
                       {usageQuery}
                    Order By b.NextDay DESC";
/////
 * ;with cte as (
                   Select 
                       c.CustomerNumber as CustomerNumber,
                       c.ReadingNumber,
                       TRIM(c.FirstName) +' '+TRIM(c.SureName) as FullName,
                       c.WaterDiameterTitle as MeterDiameterTitle,
                       c.UsageTitle2 as UsageSellTitle,
                       TRIM(c.Address) as Address,
                       c.ZoneTitle as ZoneTitle,
                       RN=ROW_NUMBER() OVER (Partition By c.CustomerNumber , c.ZoneId Order By c.CustomerNumber)
                   From [CustomerWarehouse].dbo.Clients c
                   LEFt JOIN [CustomerWarehouse].dbo.Bills b
                   on c.ZoneId=b.ZoneId AND c.CustomerNumber=b.CustomerNumber
                   where 
                        b.Id IS NULL
                        AND (@FromDate IS NULL or
                             @ToDate IS NULL or 
                             c.WaterInstallDate BETWEEN @FromDate and @ToDate)
                        AND (@FromReadingNumber IS NULL or
                            @ToReadingNumber IS NULL or 
                            c.ReadingNumber BETWEEN @FromReadingNumber and @ToReadingNumber)AND
                       c.DeletionStateId IN (0,2)
                       {zoneQuery}
                   )
                   select * 
                   From cte 
                   where RN=1;with cte as (
                   Select 
                       c.CustomerNumber as CustomerNumber,
                       c.ReadingNumber,
                       TRIM(c.FirstName) +' '+TRIM(c.SureName) as FullName,
                       c.WaterDiameterTitle as MeterDiameterTitle,
                       c.UsageTitle2 as UsageSellTitle,
                       TRIM(c.Address) as Address,
                       c.ZoneTitle as ZoneTitle,
                       RN=ROW_NUMBER() OVER (Partition By c.CustomerNumber , c.ZoneId Order By c.CustomerNumber)
                   From [CustomerWarehouse].dbo.Clients c
                   LEFt JOIN [CustomerWarehouse].dbo.Bills b
                   on c.ZoneId=b.ZoneId AND c.CustomerNumber=b.CustomerNumber
                   where 
                        b.Id IS NULL
                        AND (@FromDate IS NULL or
                             @ToDate IS NULL or 
                             c.WaterInstallDate BETWEEN @FromDate and @ToDate)
                        AND (@FromReadingNumber IS NULL or
                            @ToReadingNumber IS NULL or 
                            c.ReadingNumber BETWEEN @FromReadingNumber and @ToReadingNumber)AND
                       c.DeletionStateId IN (0,2)
                       {zoneQuery}
                   )
                   select * 
                   From cte 
                   where RN=1
*/

#endregion