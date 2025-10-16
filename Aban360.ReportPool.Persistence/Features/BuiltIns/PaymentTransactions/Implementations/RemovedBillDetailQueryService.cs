using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Implementations
{
    internal sealed class RemovedBillDetailQueryService : RemovedBillBase, IRemovedBillDetailQueryService
    {
        public RemovedBillDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<RemovedBillHeaderOutputDto, RemovedBillDetailDataOutputDto>> GetInfo(RemovedBillInputDto input)
        {
            string query = GetDetailQuery(input.ZoneIds.HasValue());

            IEnumerable<RemovedBillDetailDataOutputDto> RemovedBillData = await _sqlReportConnection.QueryAsync<RemovedBillDetailDataOutputDto>(query, input);
            RemovedBillHeaderOutputDto RemovedBillHeader = new RemovedBillHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber= input.FromReadingNumber,
                ToReadingNumber= input.ToReadingNumber,
                FromAmount = input.FromAmount,
                ToAmount = input.ToAmount,
                CustomerCount = RemovedBillData is not null && RemovedBillData.Any() ? RemovedBillData.Count() : 0,
                RecordCount = RemovedBillData is not null && RemovedBillData.Any() ? RemovedBillData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title= ReportLiterals.RemovedBillDetail,

                SumAmount = RemovedBillData.Sum(x => x.Amount)
            };

            var result = new ReportOutput<RemovedBillHeaderOutputDto, RemovedBillDetailDataOutputDto>(ReportLiterals.RemovedBillDetail, RemovedBillHeader, RemovedBillData);

            return result;
        }
    }
}
