using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
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
    internal sealed class WaterDailyBankGroupedQueryService : DailyBankGroupedBase, IWaterDailyBankGroupedQueryService
    {
        public WaterDailyBankGroupedQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<DailyBankGroupedHeaderOutputDto, DailyBankGroupedDataOutputDto>> GetInfo(DailyBankGroupedInputDto input)
        {
            string query = GetDetailQuery(input.ZoneIds.HasValue(),GroupingFields.Payments);
            
            IEnumerable<DailyBankGroupedDataOutputDto> dailyBankGroupedData = await _sqlReportConnection.QueryAsync<DailyBankGroupedDataOutputDto>(query, input);
            DailyBankGroupedHeaderOutputDto dailyBankGroupedHeader = new DailyBankGroupedHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromAmount = input.FromAmount,
                ToAmount = input.ToAmount,
                FromBankId = input.FromBankId,
                ToBankId = input.ToBankId,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (dailyBankGroupedData is not null && dailyBankGroupedData.Any()) ? dailyBankGroupedData.Count() : 0,
                CustomerCount = (dailyBankGroupedData is not null && dailyBankGroupedData.Any()) ? dailyBankGroupedData.Count() : 0,
                Title= ReportLiterals.WaterDailyBankGrouped,

                TotalCount = dailyBankGroupedData?.Sum(r => r.TotalCount) ?? 0,
                TotalAmount = dailyBankGroupedData?.Sum(r => r.TotalAmount) ?? 0,
            };

            var result = new ReportOutput<DailyBankGroupedHeaderOutputDto, DailyBankGroupedDataOutputDto>(ReportLiterals.WaterDailyBankGrouped, dailyBankGroupedHeader, dailyBankGroupedData);
            return result;
        }
    }
}
