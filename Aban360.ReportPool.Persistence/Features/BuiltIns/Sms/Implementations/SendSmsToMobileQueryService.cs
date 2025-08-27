using Aban360.Common.Db.Dapper;
using Aban360.Common.Excel;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.Sms.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.Sms.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.Sms.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.Sms.Implementations
{
    internal sealed class SendSmsToMobileQueryService : AbstractBaseConnection, ISendSmsToMobileQueryService
    {
        public SendSmsToMobileQueryService(IConfiguration configuration) :
            base(configuration)
        { }

        public async Task<ReportOutput<SendSmsToMobileHeaderOutputDto, SendSmsToMobileDataOutputDto>> Get(SendSmsToMobileInputDto input)
        {
            string sendSmsToMobileQuery = GetSendSmsToMobileQuery();
            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                mobile = input.Mobile,
            };
            IEnumerable<SendSmsToMobileDataOutputDto> smsData = await _sqlReportConnection.QueryAsync<SendSmsToMobileDataOutputDto>(sendSmsToMobileQuery, @params);
            SendSmsToMobileHeaderOutputDto smsHeader = new SendSmsToMobileHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                Receiver = input.Mobile,
                RecordCount = (smsData is not null && smsData.Any()) ? smsData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString()
            };

            var result = new ReportOutput<SendSmsToMobileHeaderOutputDto, SendSmsToMobileDataOutputDto>(ReportLiterals.SendSmsToMobile, smsHeader, smsData);

            return result;
        }

        private string GetSendSmsToMobileQuery()
        {
            return @"Select 
                        q.Text,
                        q.InsertTime AS SendTime,
                        q.InsertDateJalali AS SendDate,
                        m.Title AS FinalDeliveryStateTitle
                    From [Sms].dbo.Queue q
                    Join [Sms].dbo.MagfaDeliveryState m on q.FinalDeliveryState=m.Id
                    Where	
                    	q.InsertDateJalali BETWEEN @fromDate AND @toDate AND
                    	q.Receiver=@mobile";
        }
    }
}
