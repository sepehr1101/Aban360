using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.Request.Inputs;
using Aban360.ReportPool.Domain.Features.Request.Outputs;

namespace Aban360.ReportPool.Application.Features.Requests.Handlers.Contracts
{
    public interface IReceivedSmdByPaigingHandler
    {
        Task<ReportOutput<ReceivedSmsHeaderOutputDto, ReceivedSmsDataOutputDto>> Handle(ReceivedSmsInputDto inputDto, CancellationToken cancellationToken);
    }
}
