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
    internal sealed class ServiceLinkPaymentReceivableQueryDetailService : PaymentReceivableBase, IServiceLinkPaymentReceivableQueryDetailService
    {
        public ServiceLinkPaymentReceivableQueryDetailService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableDataOutputDto>> GetInfo(ServiceLinkPaymentReceivableInputDto input)
        {
            string query = GetDetailQuery(false, input.ZoneIds?.Any() == true, input.UsageIds?.Any() == true);
            //string query = GetWaterPaymentReceivableQuery(input.ZoneIds?.Any() == true, input.UsageIds?.Any() == true);

            var @params = new
            {
                FromDate = input.FromDateJalali,
                ToDate = input.ToDateJalali,
                zoneIds = input.ZoneIds,
                usageIds = input.UsageIds,
            };
            IEnumerable<WaterPaymentReceivableDataOutputDto> waterPaymentReceivableData = await _sqlReportConnection.QueryAsync<WaterPaymentReceivableDataOutputDto>(query, @params);
            WaterPaymentReceivableHeaderOutputDto waterPaymentReceivableHeader = new WaterPaymentReceivableHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = waterPaymentReceivableData is not null && waterPaymentReceivableData.Any() ? waterPaymentReceivableData.Count() : 0,
                CustomerCount = waterPaymentReceivableData is not null && waterPaymentReceivableData.Any() ? waterPaymentReceivableData.Count() : 0,
                BillCount = waterPaymentReceivableData is not null && waterPaymentReceivableData.Any() ? waterPaymentReceivableData.Count() : 0,
                Title= ReportLiterals.ServiceLinkPaymentReceivableDetail,

            };
            var result = new ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableDataOutputDto>(ReportLiterals.ServiceLinkPaymentReceivableDetail, waterPaymentReceivableHeader, waterPaymentReceivableData);
            return result;
        }

        private string GetWaterPaymentReceivableQuery(bool hasZone, bool hasUsage)
        {
            string zoneQuery = hasZone ? "AND ZoneId IN @ZoneIds" : string.Empty;
            string usageQuery = hasUsage ? "AND UsageId IN @UsageIds" : string.Empty;

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
                    Where 
                        (@fromDate IS NULL OR
                        @toDate IS NULL OR
                        EventDate BETWEEN @fromDate and @toDate) 
                        {zoneQuery}
                        {usageQuery}
                )
                SELECT 
                    FullName, 
                	CustomerNumber,
                	ZoneTitle,
                	UsageTitle,
                    EventDate as EventDateJalali,
                    Amount,
                	Case When SumAmount>=0 Then N'{ReportLiterals.Due}' Else N'{ReportLiterals.Overdue}' End as AmountState
                FROM OrderedData
                WHERE IsPayed = 1
                ORDER BY 
                	ZoneId,
                	CustomerNumber, 
                	EventDate";
        }
    }
}
