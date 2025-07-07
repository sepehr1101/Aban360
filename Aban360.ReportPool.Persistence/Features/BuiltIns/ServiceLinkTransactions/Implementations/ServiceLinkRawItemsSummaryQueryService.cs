using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class ServiceLinkRawItemsSummaryQueryService : AbstractBaseConnection, IServiceLinkRawItemsSummaryQueryService
    {
        public ServiceLinkRawItemsSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<ServiceLinkRawItemsHeaderOutputDto, ServiceLinkRawItemsSummaryDataOutputDto>> Get(ServiceLinkRawItemsInputDto input)
        {
            string serviceLinkRawItemsSummaryQuery = GetServiceLinkRawItemsSummaryQuery(); ;

            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                zoneIds = input.ZoneIds,
            };
            IEnumerable<ServiceLinkRawItemsSummaryDataOutputDto> data = await _sqlReportConnection.QueryAsync<ServiceLinkRawItemsSummaryDataOutputDto>(serviceLinkRawItemsSummaryQuery, @params);
            ServiceLinkRawItemsHeaderOutputDto header = new ServiceLinkRawItemsHeaderOutputDto()
            {
                FromDataJalali = input.FromDateJalali,
                ToDataJalali = input.ToDateJalali,
                ReportDate = DateTime.Now.ToShortPersianDateString(),
                RecordCount = data.Count(),

                SumAmount = data.Sum(x => x.Amount),
                SumOffAmount = data.Sum(x => x.OffAmount),
                SumFinalAmount = data.Sum(x => x.FinalAmount),
            };
            var result = new ReportOutput<ServiceLinkRawItemsHeaderOutputDto, ServiceLinkRawItemsSummaryDataOutputDto>
                (ReportLiterals.ServiceLinkRawItemsSummary, header, data);

            return result;
        }
        private string GetServiceLinkRawItemsSummaryQuery()
        {
            return @"Select
                    	r.ItemTitle ,
                    	SUM(r.Amount) AS Amount,
                    	SUM(r.OffAmount) AS OffAmount,
                    	SUM(r.FinalAmount) AS FinalAmount
                    From [CustomerWarehouse].dbo.RequestBillDetails r
                    Where	
                    	r.RegisterDate BETWEEN @fromDate AND @toDate AND
                    	r.ZoneId IN @zoneIds AND
                    	r.TypeCode=1 OR r.TypeCode=2
                    Group By r.ItemTitle";
        }
    }
}
