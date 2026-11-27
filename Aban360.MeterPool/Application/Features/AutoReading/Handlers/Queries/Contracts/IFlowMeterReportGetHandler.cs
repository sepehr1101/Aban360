using Aban360.MeterPool.Domain.Features.AutoReading.Dtos.Queries;

namespace Aban360.MeterPool.Application.Features.AutoReading.Handlers.Queries.Contracts
{
    public interface IFlowMeterReportGetHandler
    {
        Task<FlowMeterReportGetDto> Handle(FlowMeterReportInputDto inputDto, CancellationToken cancellationToken);
    }
}
