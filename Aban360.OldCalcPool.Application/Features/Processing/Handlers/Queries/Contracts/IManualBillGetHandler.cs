using Aban360.Common.BaseEntities;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Queries.Contracts
{
    public interface IManualBillGetHandler
    {
        Task<ReportOutput<ManualBillHeaderOutputDto, ManualBillDataOutputDto>> Handle(ManualBillInputDto inputDto, CancellationToken cancellationToken);
    }
}