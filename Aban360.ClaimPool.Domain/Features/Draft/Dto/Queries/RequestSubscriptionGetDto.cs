namespace Aban360.ClaimPool.Domain.Features.Draft.Dto.Queries
{
    public record RequestSubscriptionGetDto
    {
        public RequestEstateGetDto Estate { get; set; } = default!;
        public RequestWaterMeterGetDto WaterMeter { get; set; } = default!;
        public ICollection<RequestFlatGetDto>? Flats { get; set; }
        public ICollection<RequestIndividualGetDto> Individuals { get; set; } = default!;
        public ICollection<RequestSiphonGetDto>? Siphons { get; set; }
    }
}