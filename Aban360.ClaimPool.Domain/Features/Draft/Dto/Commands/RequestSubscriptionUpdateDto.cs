namespace Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands
{
    public record RequestSubscriptionUpdateDto
    {
        public EstateRequestUpdateDto Estate { get; set; }
        public WaterMeterRequestUpdateDto WaterMeter { get; set; }
        public ICollection<FlatRequestUpdateDto>? Flats { get; set; }
        public ICollection<IndividualRequestCreateDto> Individuals { get; set; } = default!;
        public ICollection<SiphonRequestUpdateDto>? Siphons { get; set; }

    }
}
