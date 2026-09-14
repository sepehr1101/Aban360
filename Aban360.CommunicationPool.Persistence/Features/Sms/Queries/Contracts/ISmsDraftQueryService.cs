using Aban360.CommunicationPool.Domain.Features.Sms.Queries;

namespace Aban360.CommunicationPool.Persistence.Features.Sms.Queries.Contracts
{
    public interface ISmsDraftQueryService
    {
        Task<IEnumerable<SmsDraftGetDto>> Get(string groupId, int smsFlowId, bool hasFetchDate, bool hasSendDate);
    }
}
