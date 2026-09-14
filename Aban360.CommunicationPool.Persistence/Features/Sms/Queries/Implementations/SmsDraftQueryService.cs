using Aban360.Common.Db.Dapper;
using Aban360.CommunicationPool.Domain.Features.Sms.Queries;
using Aban360.CommunicationPool.Persistence.Features.Sms.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CommunicationPool.Persistence.Features.Sms.Queries.Implementations
{
    internal sealed class SmsDraftQueryService : AbstractBaseConnection, ISmsDraftQueryService
    {
        public SmsDraftQueryService(IConfiguration configuration)
                   : base(configuration)
        {
        }

        public async Task<IEnumerable<SmsDraftGetDto>> Get(string groupId,int smsFlowId, bool hasFetchDate, bool hasSendDate)
        {
            string query = GetByGroupId(hasFetchDate, hasSendDate);
            IEnumerable<SmsDraftGetDto> result = await _sqlReportConnection.QueryAsync<SmsDraftGetDto>(query, new { groupId , smsFlowId });
            return result;
        }

        private string GetByGroupId(bool hasFetchDate, bool hasSendDate)
        {
            string fetchCondition = hasFetchDate ? "  FetchDateTime IS NOT NULL " : "  FetchDateTime IS NULL ";
            string sendCondition = hasSendDate ? "  SendDateTime IS NOT NULL " : "  SendDateTime IS NULL ";

            return @$"Select Top 4000 
                        Id,
	                    TrackNumber,
	                    BillId,
                        GroupId,
	                    ReferenceId,
	                    SmsFlowId,
	                    Message,
	                    MobileNumber,
	                    InsertDateTime,
	                    FetchDateTime,
	                    SendDateTime
                    From [Atlas].dbo.SmsDraft
                    Where 
                        GroupId = @groupId AND
                        SmsFlowId = @smsFlowId AND
                        {fetchCondition} AND
                        {sendCondition}";
        }

    }
}
