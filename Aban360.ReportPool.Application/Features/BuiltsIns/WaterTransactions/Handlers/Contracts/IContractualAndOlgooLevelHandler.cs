using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts
{
    public interface IContractualAndOlgooLevelHandler
    {
        Task<ReportOutput<ContractualAndOlgooLevelHeaderOutputDto, ContractualAndOlgooLevelDataOutputDto>> Handle(ContractualAndOlgooLevelInputDto input, CancellationToken cancellationToken);
    }
}
