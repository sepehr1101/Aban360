using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class WaterRawSalesSummaryQueryService : WaterNetRawSalesBase, IWaterRawSalesSummaryQueryService
    {
        public WaterRawSalesSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<WaterSalesHeaderOutputDto, WaterRawSalesSummaryDataOutputDto>> GetInfo(WaterSalesInputDto input)
        {
            string query = GetGroupedQuery(false);

            IEnumerable<WaterRawSalesSummaryDataOutputDto> waterRawSalesData = await _sqlReportConnection.QueryAsync<WaterRawSalesSummaryDataOutputDto>(query, input);
            WaterSalesHeaderOutputDto waterRawSalesHeader = new WaterSalesHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount =( waterRawSalesData is not null && waterRawSalesData.Any() ? waterRawSalesData.Count() : 0),
                SumPayable = (waterRawSalesData is not null && waterRawSalesData.Any() ? waterRawSalesData.Sum(x=>x.Payable): 0),
                CustomerCount = (waterRawSalesData is not null && waterRawSalesData.Any() ? waterRawSalesData.Sum(x=>x.Count) : 0),
                Title= ReportLiterals.WaterRawSalesSummary,
            };

            var result = new ReportOutput<WaterSalesHeaderOutputDto, WaterRawSalesSummaryDataOutputDto>(ReportLiterals.WaterRawSalesSummary, waterRawSalesHeader, waterRawSalesData);
            return result;
        }
    }
}
