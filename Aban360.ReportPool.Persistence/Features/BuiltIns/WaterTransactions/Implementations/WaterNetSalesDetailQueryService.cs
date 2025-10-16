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

    internal sealed class WaterNetSalesDetailQueryService : WaterNetRawSalesBase, IWaterNetSalesDetailQueryService
    {
        public WaterNetSalesDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<WaterSalesHeaderOutputDto, WaterNetRawSalesDetailDataOutputDto>> GetInfo(WaterSalesInputDto input)
        {
            string query = GetDetailQuery(true);
          
            IEnumerable<WaterNetRawSalesDetailDataOutputDto> waterNetSalesData = await _sqlReportConnection.QueryAsync<WaterNetRawSalesDetailDataOutputDto>(query, input);
            WaterSalesHeaderOutputDto waterNetSalesHeader = new WaterSalesHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                CustomerCount = (waterNetSalesData is not null && waterNetSalesData.Any() ? waterNetSalesData.Count() : 0),
                RecordCount = (waterNetSalesData is not null && waterNetSalesData.Any() ? waterNetSalesData.Count() : 0),
                SumPayable = (waterNetSalesData is not null && waterNetSalesData.Any() ? waterNetSalesData.Sum(x => x.Payable) : 0),
                Title= ReportLiterals.WaterNetSalesDetail,
            };

            var result = new ReportOutput<WaterSalesHeaderOutputDto, WaterNetRawSalesDetailDataOutputDto>(ReportLiterals.WaterNetSalesDetail, waterNetSalesHeader, waterNetSalesData);
            return result;
        }
    }
}
