using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;

namespace Aban360.ReportPool.GatewayAdhoc.Features.ConsumersInfo.Contracts
{
    public interface ILatestWaterMeterInfoAddhoc
    {
        Task<LatestWaterMeterInfoDto> Handle(string billId, CancellationToken cancellationToken);
    }
}
