using Aban360.Common.Excel;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts
{
    public interface IWithoutBillHandler
    {
        Task<ReportOutput<WithoutBillHeaderOutputDto, WithoutBillDataOutputDto>> Handle(WithoutBillInputDto input, CancellationToken cancellationToken);
    }
}
