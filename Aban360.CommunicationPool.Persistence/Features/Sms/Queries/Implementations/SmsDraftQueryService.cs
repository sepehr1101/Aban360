using Aban360.Common.Db.Dapper;
using Aban360.CommunicationPool.Domain.Features.Sms.Queries;
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

        public async Task<IEnumerable<SmsDraftGetDto>> GetByReferenceId(string referenceId, bool hasFetchDate, bool hasSendDate)
        {
            string query = GetByReferenceId(hasFetchDate, hasSendDate);
            IEnumerable<SmsDraftGetDto> result = await _sqlReportConnection.QueryAsync<SmsDraftGetDto>(query, new { referenceId });
            return result;
        }

        private string GetByReferenceId(bool hasFetchDate, bool hasSendDate)
        {
            string fetchCondition = hasFetchDate ? "  FetchDateTime IS NOT NULL " : "  FetchDateTime IS NULL ";
            string sendCondition = hasSendDate ? "  SendDateTime IS NOT NULL " : "  SendDateTime IS NULL ";

            return @$"Select 
                        Id,
	                    TrackNumber,
	                    BillId,
	                    ReferenceId,
	                    TypeId,
	                    TypeTitle,
	                    Message,
	                    MobileNumber,
	                    InsertDateTime,
	                    FetchDateTime,
	                    SendDateTime
                    From CustomerWarehouse.dbo.SmsDraft
                    Where 
                        ReferenceId=1158 AND
                        {fetchCondition} AND
                        {sendCondition}";
        }

    }
}
