using Aban360.CommunicationPool.Domain.Features.Sms.Queries;

namespace Aban360.CommunicationPool.Persistence.Features.Sms.Queries.Implementations
{
    public interface ISmsDraftQueryService
    {
        Task<IEnumerable<SmsDraftGetDto>> GetByReferenceId(string referenceId, bool hasFetchDate, bool hasSendDate);
    }
}
