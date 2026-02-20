using Aban360.Common.BaseEntities;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Queries.Contracts
{
    public interface IBillInstallmentGetHandler
    {
        Task<ReportOutput<BillInstallmentHeaderOutputDto, BillInstallmentOutputDto>> Handle(string input, CancellationToken cancellationToken);
    }
}
