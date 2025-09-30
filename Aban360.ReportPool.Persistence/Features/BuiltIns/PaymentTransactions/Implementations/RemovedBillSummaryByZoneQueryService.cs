using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Implementations
{
    internal sealed class RemovedBillSummaryByZoneQueryService : AbstractBaseConnection, IRemovedBillSummaryByZoneQueryService
    {
        public RemovedBillSummaryByZoneQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<RemovedBillHeaderOutputDto, RemovedBillSummaryByZoneDataOutputDto>> GetInfo(RemovedBillInputDto input)
        {
            string RemovedBillQueryString = GetRemovedBillDataQuery(input.ZoneIds?.Any() == true);
            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,
                fromAmount = input.FromAmount,
                toAmount = input.ToAmount,
                zoneIds = input.ZoneIds,
            };
            IEnumerable<RemovedBillSummaryByZoneDataOutputDto> RemovedBillData = await _sqlReportConnection.QueryAsync<RemovedBillSummaryByZoneDataOutputDto>(RemovedBillQueryString, @params);
            RemovedBillHeaderOutputDto RemovedBillHeader = new RemovedBillHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                FromAmount = input.FromAmount,
                ToAmount = input.ToAmount,
                RecordCount = RemovedBillData is not null && RemovedBillData.Any() ? RemovedBillData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),

                SumAmount = RemovedBillData.Sum(x => x.Amount),
                CustomerCount = RemovedBillData.Sum(x => x.CustomerCount)
            };

            var result = new ReportOutput<RemovedBillHeaderOutputDto, RemovedBillSummaryByZoneDataOutputDto>(ReportLiterals.RemovedBillSummary + ReportLiterals.ByZone, RemovedBillHeader, RemovedBillData);

            return result;
        }

        private string GetRemovedBillDataQuery(bool hasZone)
        {
            string zoneQuery = hasZone ? "AND c.ZoneId IN @zoneIds" : string.Empty;
            return @$"Select
						MAX(t46.C2) AS RegionTitle,
                      	c.ZoneTitle,
                        Count(c.UsageTitle) As CustomerCount,
                      	AVG(rb.Consumption) AS AverageConsumption,
                      	SUM(rb.Consumption) AS SumConsumption,
                      	SUM(rb.SumItems) AS Amount
                      From [CustomerWarehouse].dbo.RemovedBills rb
                      Join [CustomerWarehouse].dbo.Clients c
                      	on c.CustomerNumber=rb.CustomerNumber AND c.ZoneId=rb.ZoneId
                      Join [Db70].dbo.T51 t51
				        	On t51.C0=c.ZoneId
				        Join [Db70].dbo.T46 t46
				        	On t51.C1=t46.C0
                      Where
                      	(@fromDate IS NULL OR
                      	@toDate IS NULL OR
                      	rb.RegisterDay BETWEEN @fromDate AND @toDate) AND
                      	(@fromAmount IS NULL OR
                      	@toAmount IS NULL OR
                      	rb.SumItems BETWEEN @fromAmount AND @toAmount) AND
                        (@fromReadingNumber IS NULL OR
                        @toReadingNumber IS NULL OR
                        c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
				       	c.ToDayJalali IS NULL
                        {zoneQuery}
                      Group By c.ZoneTitle";
        }
    }
}
