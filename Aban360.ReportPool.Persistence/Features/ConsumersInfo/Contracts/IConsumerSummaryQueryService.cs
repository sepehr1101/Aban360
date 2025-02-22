using Aban360.ReportPool.Persistence.Queries.Implementations;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts
{
    public interface IConsumerSummaryQueryService
    {
        Task<ConsumerSummaryDto> GetInfo(string billId);
    }

}