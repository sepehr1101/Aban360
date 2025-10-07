using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Constants;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Implementations
{
    internal sealed class ServiceLinkPaymentReceivableSummaryService : PaymentReceivableBase, IServiceLinkPaymentReceivableSummaryService
    {
        public ServiceLinkPaymentReceivableSummaryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableSummaryDataOutputDto>> GetInfo(ServiceLinkPaymentReceivableInputDto input)
        {
            string groupingField = input.IsZone ? GroupingFields.ZoneTitle : GroupingFields.UsageTitle;
            string query = GetGroupedQuery(false, input.ZoneIds?.Any() == true, groupingField, input.UsageIds?.Any() == true);
            // string query = GetWaterPaymentReceivableQuery(input.ZoneIds?.Any() == true, input.UsageIds?.Any() == true, input.IsZone);

            var @params = new
            {
                FromDate = input.FromDateJalali,
                ToDate = input.ToDateJalali,
                zoneIds = input.ZoneIds,
                usageIds = input.UsageIds
            };
            IEnumerable<WaterPaymentReceivableSummaryDataOutputDto> waterPaymentReceivableData = await _sqlReportConnection.QueryAsync<WaterPaymentReceivableSummaryDataOutputDto>(query, @params);
            WaterPaymentReceivableHeaderOutputDto waterPaymentReceivableHeader = new WaterPaymentReceivableHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = waterPaymentReceivableData is not null && waterPaymentReceivableData.Any() ? waterPaymentReceivableData.Count() : 0

            };

            var result = new ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableSummaryDataOutputDto>(ReportLiterals.ServiceLinkPaymentReceivableSummary + ReportLiterals.ByUsage, waterPaymentReceivableHeader, waterPaymentReceivableData);
            return result;
        }

        private string GetWaterPaymentReceivableQuery(bool hasZone, bool hasUsage, bool isZone)
        {
            string zoneQuery = hasZone ? "AND ZoneId IN @ZoneIds" : string.Empty;
            string usageQuery = hasUsage ? "AND UsageId IN @UsageIds" : string.Empty;
            string param = isZone ? ReportLiterals.ZoneTitle : ReportLiterals.UsageTitle;

            return @$"WITH OrderedData AS (
                    SELECT 
                        FullName, 
                		CustomerNumber,
                		ZoneId,
                		ZoneTitle,
                		UsageId,
                		UsageTitle,
                        EventDate,
                        IsPayed,
                        Amount,
                        SUM(Amount) OVER (
                            PARTITION BY ZoneId,CustomerNumber
                            ORDER BY EventDate
                            ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW
                        ) AS SumAmount
                	from [CustomerWarehouse].dbo.PaymentDue 
                	where 
                		 (@fromDate IS NULL OR
                        @toDate IS NULL OR
                        EventDate BETWEEN @fromDate and @toDate) 
                        {zoneQuery}
                        {usageQuery}
                )
                SELECT 
                	{param} as ItemTitle,
                	SUM(Case When SumAmount>=0 Then SumAmount else 0 end) as OverDueAmount,
                	SUM(Case When SumAmount<0 Then SumAmount else 0 end) as DueAmount,
                	Count(Case When SumAmount>=0 Then 1 else null end) as OverDueCount,
                	Count(Case When SumAmount<0 Then 1 else null end) as DueCount
                FROM OrderedData
                WHERE  IsPayed=1
                Group By {param}";
        }
    }
}
