namespace Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands
{
    public record RequestSubscriptionCreateDto
    {
        public EstateRequestCreateDto Estate { get; set; } = default!;
        public WaterMeterRequestCreateDto WaterMeter { get; set; } = default!;
        public ICollection<FlatRequestCreateDto>? Flats { get; set; }
        public ICollection<IndividualRequestCreateDto> Individuals { get; set; } = default!;

        public ICollection<SiphonRequestCreateDto>? Siphons { get; set; }
    }
}
