using Aban360.Common.Db.Dapper;
using Aban360.Common.Excel;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class ServiceLinkRawItemsDetailQueryService : AbstractBaseConnection, IServiceLinkRawItemsDetailQueryService
    {
        public ServiceLinkRawItemsDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<ServiceLinkRawItemsHeaderOutputDto, ServiceLinkRawItemsDetailDataOutputDto>> Get(ServiceLinkRawItemsInputDto input)
        {
            string serviceLinkRawItemsDetailQuery = GetServiceLinkRawItemsDetailQuery(); ;

            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                zoneIds = input.ZoneIds,
            };
            IEnumerable<ServiceLinkRawItemsDetailDataOutputDto> data = await _sqlReportConnection.QueryAsync<ServiceLinkRawItemsDetailDataOutputDto>(serviceLinkRawItemsDetailQuery, @params);
            ServiceLinkRawItemsHeaderOutputDto collectionBranchHeader = new ServiceLinkRawItemsHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (data is not null && data.Any()) ? data.Count() : 0,

                SumAmount = data.Sum(x => x.Amount),
                SumOffAmount = data.Sum(x => x.OffAmount),
                SumFinalAmount = data.Sum(x => x.FinalAmount),
            };
            var result = new ReportOutput<ServiceLinkRawItemsHeaderOutputDto, ServiceLinkRawItemsDetailDataOutputDto>
                (ReportLiterals.ServiceLinkRawItemsDetail, collectionBranchHeader, data);

            return result;
        }
        private string GetServiceLinkRawItemsDetailQuery()
        {
            return @"Select
                    	r.TrackNumber,
                    	r.ZoneTitle,
                    	r.CustomerNumber,
                    	r.ItemTitle,
                    	r.Amount,
                    	r.OffAmount,
                    	r.FinalAmount
                    From [CustomerWarehouse].dbo.RequestBillDetails r
                    Where	
                    	r.RegisterDate BETWEEN @fromDate AND @toDate AND
                    	r.ZoneId IN @zoneIds AND
                    	r.TypeCode IN (1,2)";
        }
    }
}