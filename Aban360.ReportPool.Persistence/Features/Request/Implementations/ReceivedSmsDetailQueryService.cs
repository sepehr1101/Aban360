using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Features.Request.Inputs;
using Aban360.ReportPool.Domain.Features.Request.Outputs;
using Aban360.ReportPool.Persistence.Features.Request.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.Request.Implementations
{
    internal sealed class ReceivedSmsDetailQueryService : AbstractBaseConnection, IReceivedSmsDetailQueryService
    {
        public ReceivedSmsDetailQueryService(IConfiguration configuration)
            :base(configuration)
        { 
        }

        public async Task<IEnumerable<ReceivedSmsDataOutputDto>> Get(ReceivedSmsInputDto inputDto)
        {
            string query = GetQuery();
            IEnumerable<ReceivedSmsDataOutputDto> result = await _sqlReportConnection.QueryAsync<ReceivedSmsDataOutputDto>(query, inputDto);
            return result;
        }
        private string GetQuery()
        {
            return @"Select 
	                    Id,
	                    SenderNumber,
	                    RecipientNumber,
	                    TRIM(Body) Text,
	                    DateJalali,
	                    Time,
                        DateAndTime
                    From Sms.dbo.ReceiveDetail
                    Where 
	                    DateJalali BETWEEN @fromDateJalali AND @toDateJalali
                     Order By DateAndTime Desc";
        }
    }
}
