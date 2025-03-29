namespace Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands
{
    public record WaterMeterSiphonCommandDto
    {
        public int Id { get; set; }
        public int WaterMeterId { get; set; }
        public int SiphonId { get; set; }
    }
}
