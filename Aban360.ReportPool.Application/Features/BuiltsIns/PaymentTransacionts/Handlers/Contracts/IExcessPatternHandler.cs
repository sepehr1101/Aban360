using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts
{
    public interface IExcessPatternHandler
    {
        Task<ReportOutput<ExcessPatternHeaderOutputDto, ExcessPatternDataOutputDto>> Handle(ExcessPatternInputDto input, CancellationToken cancellationToken);
    }
}
