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
    internal sealed class WaterNetSalesSummaryQueryService : WaterNetRawSalesBase, IWaterNetSalesSummaryQueryService
    {
        public WaterNetSalesSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<WaterSalesHeaderOutputDto, WaterNetSalesSummaryDataOutputDto>> GetInfo(WaterSalesInputDto input)
        {
            string query = GetGroupedQuery(true);

            IEnumerable<WaterNetSalesSummaryDataOutputDto> waterNetSalesData = await _sqlReportConnection.QueryAsync<WaterNetSalesSummaryDataOutputDto>(query, input);
            WaterSalesHeaderOutputDto waterNetSalesHeader = new WaterSalesHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (waterNetSalesData is not null && waterNetSalesData.Any() ? waterNetSalesData.Count() : 0),
                SumPayable = (waterNetSalesData is not null && waterNetSalesData.Any() ? waterNetSalesData.Sum(x => x.Payable) : 0),
                CustomerCount = (waterNetSalesData is not null && waterNetSalesData.Any() ? waterNetSalesData.Sum(x => x.Count) : 0),
                Title= ReportLiterals.WaterNetSalesSummary,
            };

            var result = new ReportOutput<WaterSalesHeaderOutputDto, WaterNetSalesSummaryDataOutputDto>(ReportLiterals.WaterNetSalesSummary, waterNetSalesHeader, waterNetSalesData);
            return result;
        }
    }
}
