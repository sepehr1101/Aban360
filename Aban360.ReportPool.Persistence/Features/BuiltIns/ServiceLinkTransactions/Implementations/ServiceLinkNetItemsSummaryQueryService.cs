using Aban360.Common.BaseEntities;
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
    internal sealed class ServiceLinkNetItemsSummaryQueryService : AbstractBaseConnection, IServiceLinkNetItemsSummaryQueryService
    {
        public ServiceLinkNetItemsSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<ServiceLinkNetItemsHeaderOutputDto, ServiceLinkNetItemsSummaryDataOutputDto>> Get(ServiceLinkNetItemsInputDto input)
        {
            string serviceLinkNetItemsSummaryQuery = GetServiceLinkNetItemsSummaryQuery(); ;

            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                zoneIds = input.ZoneIds,
            };
            IEnumerable<ServiceLinkNetItemsSummaryDataOutputDto> data = await _sqlReportConnection.QueryAsync<ServiceLinkNetItemsSummaryDataOutputDto>(serviceLinkNetItemsSummaryQuery, @params);
            ServiceLinkNetItemsHeaderOutputDto header = new ServiceLinkNetItemsHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (data is not null && data.Any()) ? data.Count() : 0,

                SumAmount = data.Sum(x => x.Amount),
                SumOffAmount = data.Sum(x => x.OffAmount),
                SumFinalAmount = data.Sum(x => x.FinalAmount),
            };
            var result = new ReportOutput<ServiceLinkNetItemsHeaderOutputDto, ServiceLinkNetItemsSummaryDataOutputDto>
                (ReportLiterals.ServiceLinkNetItemsSummary, header, data);

            return result;
        }
        private string GetServiceLinkNetItemsSummaryQuery()
        {
            return @"Select
                    	r.ItemTitle ,
                    	SUM(r.Amount) AS Amount,
                    	SUM(r.OffAmount) AS OffAmount,
                    	SUM(r.FinalAmount) AS FinalAmount
                    From [CustomerWarehouse].dbo.RequestBillDetails r
                    Where	
                    	r.RegisterDate BETWEEN @fromDate AND @toDate AND
                    	r.ZoneId IN @zoneIds 
                    Group By r.ItemTitle";
        }
    }
}
