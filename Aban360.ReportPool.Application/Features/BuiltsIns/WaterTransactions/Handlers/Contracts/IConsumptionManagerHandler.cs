using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts
{
    public interface IConsumptionManagerHandler
    {
        Task<ReportOutput<ConsumptionManagerHeaderOutputDto, ConsumptionManagerDataOutputDto>> Handle(ConsumptionManagerInputDto input, CancellationToken cancellationToken);
    }
}
