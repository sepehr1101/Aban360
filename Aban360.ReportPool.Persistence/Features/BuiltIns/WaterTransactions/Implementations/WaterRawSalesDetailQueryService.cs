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
    internal sealed class WaterRawSalesDetailQueryService : WaterNetRawSalesBase, IWaterRawSalesDetailQueryService
    {
        public WaterRawSalesDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<WaterSalesHeaderOutputDto, WaterNetRawSalesDetailDataOutputDto>> GetInfo(WaterSalesInputDto input)
        {
            string query = GetDetailQuery(false);

            IEnumerable<WaterNetRawSalesDetailDataOutputDto> waterRawSalesData = await _sqlReportConnection.QueryAsync<WaterNetRawSalesDetailDataOutputDto>(query, input);
            WaterSalesHeaderOutputDto waterRawSalesHeader = new WaterSalesHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (waterRawSalesData is not null && waterRawSalesData.Any() ? waterRawSalesData.Count() : 0),
                CustomerCount = (waterRawSalesData is not null && waterRawSalesData.Any() ? waterRawSalesData.Count() : 0),
                SumPayable = (waterRawSalesData is not null && waterRawSalesData.Any() ? waterRawSalesData.Sum(x => x.Payable) : 0),
                Title= ReportLiterals.WaterRawSalesDetail,
            };

            var result = new ReportOutput<WaterSalesHeaderOutputDto, WaterNetRawSalesDetailDataOutputDto>(ReportLiterals.WaterRawSalesDetail, waterRawSalesHeader, waterRawSalesData);
            return result;
        }
    }
}
