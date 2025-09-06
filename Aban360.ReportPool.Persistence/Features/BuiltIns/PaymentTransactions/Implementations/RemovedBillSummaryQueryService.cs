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
    internal sealed class RemovedBillSummaryQueryService : AbstractBaseConnection, IRemovedBillSummaryQueryService
    {
        public RemovedBillSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<RemovedBillHeaderOutputDto, RemovedBillSummaryDataOutputDto>> GetInfo(RemovedBillInputDto input)
        {
            string RemovedBillQueryString = GetRemovedBillDataQuery(input.ZoneIds.Any());
            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                fromAmount = input.FromAmount,
                toAmount = input.ToAmount,
                zoneIds = input.ZoneIds,
            };
            IEnumerable<RemovedBillSummaryDataOutputDto> RemovedBillData = await _sqlReportConnection.QueryAsync<RemovedBillSummaryDataOutputDto>(RemovedBillQueryString, @params);
            RemovedBillHeaderOutputDto RemovedBillHeader = new RemovedBillHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromAmount = input.FromAmount,
                ToAmount = input.ToAmount,
                RecordCount = RemovedBillData is not null && RemovedBillData.Any() ? RemovedBillData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),

                SumAmount = RemovedBillData.Sum(x => x.Amount)
            };

            var result = new ReportOutput<RemovedBillHeaderOutputDto, RemovedBillSummaryDataOutputDto>(ReportLiterals.RemovedBillSummary, RemovedBillHeader, RemovedBillData);

            return result;
        }

        private string GetRemovedBillDataQuery(bool hasZone)
        {
            string zoneQuery = hasZone ? "AND c.ZoneId IN @zoneIds" : string.Empty;
            return @$"Select
                	c.ZoneTitle,
                	AVG(rb.Consumption) AS AverageConsumption,
                	SUM(rb.Consumption) AS SumConsumption,
                	SUM(rb.SumItems) AS Amount
                From [CustomerWarehouse].dbo.RemovedBills rb
                Join [CustomerWarehouse].dbo.Clients c
                	on c.CustomerNumber=rb.CustomerNumber AND c.ZoneId=rb.ZoneId
                Where
                	(@fromDate IS NULL OR
                	@toDate IS NULL OR
                	rb.RegisterDay BETWEEN @fromDate AND @toDate) AND
                	(@fromAmount IS NULL OR
                	@toAmount IS NULL OR
                	rb.SumItems BETWEEN @fromAmount AND @toAmount)
                    {zoneQuery}
                Group By c.ZoneTitle";
        }

    }
}
