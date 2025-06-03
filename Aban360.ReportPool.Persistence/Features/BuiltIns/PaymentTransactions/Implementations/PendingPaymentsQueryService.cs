using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Implementations
{
    internal sealed class PendingPaymentsQueryService : AbstractBaseConnection, IPendingPaymentsQueryService
    {
        public PendingPaymentsQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<PendingPaymentsHeaderOutputDto, PendingPaymentsDataOutputDto>> GetInfo(PendingPaymentsInputDto input)
        {
            string pendingPaymentsQueryString = GetPendingPaymentsDataQuery();
            IEnumerable<PendingPaymentsDataOutputDto> pendingPaymentsData = await _sqlConnection.QueryAsync<PendingPaymentsDataOutputDto>(pendingPaymentsQueryString);//todo: parameters
            PendingPaymentsHeaderOutputDto pendingPaymentsHeader = new PendingPaymentsHeaderOutputDto()
            { 
                RecordCount=pendingPaymentsData.Count(),
                TotalBeginDebt=pendingPaymentsData.Sum(payment=>payment.BeginDebt),
                TotalDeptPeriodCount=pendingPaymentsData.Sum(payment=>payment.DeptPeriodCount),
                TotalEndingDebt=pendingPaymentsData.Sum(payment=>payment.EndingDebt),
                TotalPayedAmount=pendingPaymentsData.Sum(payment=>payment.PayedAmount),
            };

            var result = new ReportOutput<PendingPaymentsHeaderOutputDto, PendingPaymentsDataOutputDto>(ReportLiterals.PendingPayments, pendingPaymentsHeader, pendingPaymentsData);

            return result;
        }

        private string GetPendingPaymentsDataQuery()
        {
            return " ";
        }

    }
}
