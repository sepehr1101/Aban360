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
    internal sealed class RuinedMeterIncomeDetailQueryService : RuinedMeterIncomeBase, IRuinedMeterIncomeDetailQueryService
    {
        public RuinedMeterIncomeDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<RuinedMeterIncomeHeaderOutputDto, RuinedMeterIncomeDetailDataOutputDto>> GetInfo(RuinedMeterIncomeInputDto input)
        {
            string query = GetDetailQuery();
            
            IEnumerable<RuinedMeterIncomeDetailDataOutputDto> ruinedMeterIncomeData = await _sqlReportConnection.QueryAsync<RuinedMeterIncomeDetailDataOutputDto>(query, input);
            RuinedMeterIncomeHeaderOutputDto ruinedMeterIncomeHeader = new RuinedMeterIncomeHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                CustomerCount = ruinedMeterIncomeData is not null && ruinedMeterIncomeData.Any() ? ruinedMeterIncomeData.Count() : 0,
                RecordCount = ruinedMeterIncomeData is not null && ruinedMeterIncomeData.Any() ? ruinedMeterIncomeData.Count() : 0,
                Title= ReportLiterals.RuinedMeterIncomeDetail,
            };
            if (ruinedMeterIncomeData is not null && ruinedMeterIncomeData.Any())
            {
                ruinedMeterIncomeHeader.TotalPayable = ruinedMeterIncomeData.Sum(x => x.Payable);
                ruinedMeterIncomeHeader.TotalSumItems = ruinedMeterIncomeData.Sum(x => x.SumItems);
            }

            var result = new ReportOutput<RuinedMeterIncomeHeaderOutputDto, RuinedMeterIncomeDetailDataOutputDto>(ReportLiterals.RuinedMeterIncomeDetail, ruinedMeterIncomeHeader, ruinedMeterIncomeData);

            return result;
        }
    }
}
