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
    internal sealed class UsageDetailQueryService : AbstractBaseConnection, IUsageDetailQueryService
    {
        public UsageDetailQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<ReportOutput<UsageDetailHeaderOutputDto, UsageDetailDataOutputDto>> GetInfo(UsageDetailInputDto input)
        {
            string query = GetQuery();
            //string query = GetUsageDetailQuery();

            IEnumerable<UsageDetailDataOutputDto> usageDetailData = await _sqlReportConnection.QueryAsync<UsageDetailDataOutputDto>(query, input);
            UsageDetailHeaderOutputDto usageDetailHeader = new UsageDetailHeaderOutputDto()
            {                
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                CustomerCount = (usageDetailData is not null && usageDetailData.Any()) ? usageDetailData.Count() : 0,
                RecordCount = (usageDetailData is not null && usageDetailData.Any()) ? usageDetailData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title= ReportLiterals.UsageDetail
            };

            var result = new ReportOutput<UsageDetailHeaderOutputDto, UsageDetailDataOutputDto>(ReportLiterals.UsageDetail, usageDetailHeader, usageDetailData);

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
                    	        c.ZoneId IN @ZoneIds AND
                    	        c.UsageId IN @UsageSellIds AND
                    	        (
                    	        @fromReadingNumber IS NULL OR 
                    	        @toReadingNumber IS NULL OR
                    	        c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber
                    	        ) AND
                    	        c.CustomerNumber<>0 AND
                    	        c.RegisterDayJalali <= @ToDateJalali
                      )
                      Select	
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
                    		c.DeletionStateTitle AS UseStateTitle,
                    		c.DomesticCount DomesticUnit,
                    		c.CommercialCount CommercialUnit,
                    		c.OtherCount OtherUnit,
                    		TRIM(c.BillId) BillId
                     FROM CTE c
                     JOIN [Db70].dbo.T51 t51
                         On t51.C0=c.ZoneId
                     JOIN [Db70].dbo.T46 t46
                         On t51.C1=t46.C0
                     WHERE	  
                         c.RN=1 AND
                         c.DeletionStateId NOT IN(1,2)";
        }
        private string GetUsageDetailQuery()
        {
            return @"SELECT 
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
                        c.DeletionStateTitle AS UseStateTitle,
                        c.DomesticCount DomesticUnit,
            	        c.CommercialCount CommercialUnit,
            	        c.OtherCount OtherUnit,
            	        TRIM(c.BillId) BillId
                    FROM [CustomerWarehouse].dbo.Clients c
                    WHERE 
            			c.ToDayJalali IS NULL AND
            			c.UsageId in @UsageSellIds AND
							(@fromReadingNumber IS NULL OR
							@toReadingNumber IS NULL OR
							c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
                        c.ZoneId in @ZoneIds";
        }
    }
}
