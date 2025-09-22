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
            string usageDetailQuery = GetUsageDetailQuery();
            var @params = new
            {
                input.FromReadingNumber,
                input.ToReadingNumber,

                UsageIds = input.UsageSellIds,
                input.ZoneIds
            };

            IEnumerable<UsageDetailDataOutputDto> usageDetailData = await _sqlReportConnection.QueryAsync<UsageDetailDataOutputDto>(usageDetailQuery, @params);
            UsageDetailHeaderOutputDto usageDetailHeader = new UsageDetailHeaderOutputDto()
            {                
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                RecordCount = (usageDetailData is not null && usageDetailData.Any()) ? usageDetailData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString()
            };

            var result = new ReportOutput<UsageDetailHeaderOutputDto, UsageDetailDataOutputDto>(ReportLiterals.UsageDetail, usageDetailHeader, usageDetailData);

            return result;
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
            			c.UsageId in @UsageIds AND
							(@fromReadingNumber IS NULL OR
							@toReadingNumber IS NULL OR
							c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
                        c.ZoneId in @ZoneIds";
        }
    }
}
