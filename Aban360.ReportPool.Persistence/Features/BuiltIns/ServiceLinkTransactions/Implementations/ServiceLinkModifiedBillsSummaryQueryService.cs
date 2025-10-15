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
    internal sealed class ServiceLinkModifiedBillsSummaryQueryService : AbstractBaseConnection, IServiceLinkModifiedBillsSummaryQueryService
    {
        public ServiceLinkModifiedBillsSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<ServiceLinkModifiedBillsHeaderOutputDto, ServiceLinkModifiedBillsSummaryDataOutputDto>> GetInfo(ServiceLinkModifiedBillsInputDto input)
        {
            string modifiedBills = GetServiceLinkModifiedBillsQuery();
            
            IEnumerable<ServiceLinkModifiedBillsSummaryDataOutputDto> modifiedBillsData = await _sqlReportConnection.QueryAsync<ServiceLinkModifiedBillsSummaryDataOutputDto>(modifiedBills, input);
            ServiceLinkModifiedBillsHeaderOutputDto modifiedBillsHeader = new ServiceLinkModifiedBillsHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (modifiedBillsData is not null && modifiedBillsData.Any()) ? modifiedBillsData.Count() : 0,
                CustomerCount = (modifiedBillsData is not null && modifiedBillsData.Any()) ? modifiedBillsData.Count() : 0,
                Title= ReportLiterals.ServiceLinkModifiedBillsSummary,

                SumAmount = modifiedBillsData.Sum(x => x.Amount),
                SumOffAmount = modifiedBillsData.Sum(x => x.OffAmount),
                SumFinalAmount = modifiedBillsData.Sum(x => x.FinalAmount),
            };

            var result = new ReportOutput<ServiceLinkModifiedBillsHeaderOutputDto, ServiceLinkModifiedBillsSummaryDataOutputDto>(ReportLiterals.ServiceLinkModifiedBillsSummary, modifiedBillsHeader, modifiedBillsData);
            return result;
        }

        private string GetServiceLinkModifiedBillsQuery()
        {
            return @"Select
                    	r.ItemTitle AS ItemTitle,
                    	MAX(r.ZoneTitle) AS ZoneTitle,
                    	SUM(r.Amount) AS Amount,
                    	SUM(r.OffAmount) AS OffAmount,
                    	SUM(r.FinalAmount) AS FinalAmount
                    From [CustomerWarehouse].dbo.RequestBillDetails r
                    Where
                    	r.RegisterDate BETWEEN @FromDateJalali AND @ToDateJalali AND
                    	r.ZoneId IN @zoneIds AND
                    	r.TypeCode IN @TypeIds
                    Group By
                    	r.ItemTitle,
                    	r.ZoneTitle";
        }
    }
}
