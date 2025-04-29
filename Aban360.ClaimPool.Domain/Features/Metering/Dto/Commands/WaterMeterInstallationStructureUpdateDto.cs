using Aban360.ClaimPool.Domain.Constants;

namespace Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands
{
    public record WaterMeterInstallationStructureUpdateDto
    {
        public WaterMeterInstallationStructureEnum Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
