using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts
{
    public interface IMeterLifeService
    {
        Task<IEnumerable<MeterLifeOutputDto>> Get();
        Task Create(IEnumerable<MeterLifeOutputDto> input);
    }
}
