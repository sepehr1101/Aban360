namespace Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries
{
    public record WaterMeterInstallationMethodGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
