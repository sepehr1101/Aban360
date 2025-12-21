using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
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
            int zoneId=await GetZoneId(input.BillId);

            string reportTitle = ReportLiterals.PaymentInquiry;
            string dbName = GetDbName(zoneId);
            string PaymentInquiryDataInfoQuery = getQuery(dbName);

            var @params = new
            {
                zoneId=zoneId,
                input.BillId,
                input.PaymentId,
                input.DateJalali
            };

            IEnumerable<PaymentInquiryDataOutputDto> data = await _sqlReportConnection.QueryAsync<PaymentInquiryDataOutputDto>(PaymentInquiryDataInfoQuery, @params);
            PaymentInquiryHeaderOutputDto header = new PaymentInquiryHeaderOutputDto()
            {
                FromDateJalali = input.DateJalali,
                CustomerCount = data is not null && data.Any() ? data.Count() : 0,
                RecordCount = data is not null && data.Any() ? data.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title = reportTitle,
            };

            var result = new ReportOutput<PaymentInquiryHeaderOutputDto, PaymentInquiryDataOutputDto>(reportTitle, header, data);
            return result;
        }
        private async Task<int> GetZoneId(string billId)
        {
            string query = GetZoneId();
            int? zoneId = await _sqlReportConnection.QueryFirstOrDefaultAsync<int>(query, new { billId = billId });
            if (!zoneId.HasValue || zoneId.Value <= 0)
            {
                throw new InvalidBillIdException(ExceptionLiterals.InvalidBillId);
            }

            return zoneId.Value;
        }

        private string getQuery(string dbName)
        {
            return @$"Select
                    	v.date_bank as PayDateJalali,
                    	v.date_bes as RegisterDateJalali,
                    	v.type_pay as PaymentMethodId,
                    	t150.C2 as PaymentMethodTitle,
                    	cod_bank as BankCode,
                    	sh_ghabs as BillId,
                    	sh_pard as PaymentId
                    From [{dbName}].dbo.vosolEN  v
                    Left Join [Db70].dbo.T150 t150
                    	On v.type_pay COLLATE SQL_Latin1_General_CP1_CI_AS = t150.C1 COLLATE SQL_Latin1_General_CP1_CI_AS
                    Where 
                    	v.Town=@zoneId AND
                    	v.date_bank>=@dateJalali AND
                    	LTRIM(SUBSTRING(v.sh_ghabs, PATINDEX('%[^0]%', v.sh_ghabs), LEN(v.sh_ghabs)))=@billId AND
                    	LTRIM(SUBSTRING(v.sh_pard, PATINDEX('%[^0]%', v.sh_pard), LEN(v.sh_pard)))=@paymentId";
        }
        private string GetZoneId()
        {
            return @"Select ZoneId
                	From CustomerWarehouse.dbo.Clients
                	Where 
                		ToDayJalali IS NULL AND
                		billId=@billId";
        }
    }
}
