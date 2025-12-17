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
    internal sealed class PaymentInquiryQueryService : AbstractBaseConnection, IPaymentInquiryQueryService
    {
        public PaymentInquiryQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<PaymentInquiryHeaderOutputDto, PaymentInquiryDataOutputDto>> GetInfo(PaymentInquiryInputDto input)
        {
            string reportTitle = ReportLiterals.PaymentInquiry;
            string PaymentInquiryDataInfoQuery = getQuery();

            IEnumerable<PaymentInquiryDataOutputDto> data = await _sqlReportConnection.QueryAsync<PaymentInquiryDataOutputDto>(PaymentInquiryDataInfoQuery, input);
            PaymentInquiryHeaderOutputDto header = new PaymentInquiryHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                CustomerCount = data is not null && data.Any() ? data.Count() : 0,
                RecordCount = data is not null && data.Any() ? data.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title = reportTitle,
            };

            var result = new ReportOutput<PaymentInquiryHeaderOutputDto, PaymentInquiryDataOutputDto>(reportTitle, header, data);
            return result;
        }

        private string getQuery()
        {
            return @"";
        }
    }
}
