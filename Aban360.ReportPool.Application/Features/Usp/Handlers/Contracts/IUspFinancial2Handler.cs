using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.Usp.Input;
using Aban360.ReportPool.Domain.Features.Usp.Output;

namespace Aban360.ReportPool.Application.Features.Usp.Handlers.Contracts
{
    public interface IUspFinancial2Handler
    {
        Task<ReportOutput<UspFinancialHeader, UspFinancial2Output>> Handle(UspFinancial2Input input, CancellationToken cancellationToken);
    }
}