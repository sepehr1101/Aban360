using Aban360.ReportPool.Domain.Features.Usp.Input;
using Aban360.ReportPool.Domain.Features.Usp.Output;

namespace Aban360.ReportPool.Persistence.Features.Usp.Contracts
{
    public interface IUspFinancial2QueryService
    {
        Task<IEnumerable<UspFinancial2Output>> Get(UspFinancial2Input input);
    }
}