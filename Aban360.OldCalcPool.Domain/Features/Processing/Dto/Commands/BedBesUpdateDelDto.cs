namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands
{
    public record BedBesUpdateDelDto
    {
        public int ZoneId { get; set; }
        public long Id { get; set; }
        public bool Del { get; set; }
        public BedBesUpdateDelDto(int zoneId, long id, bool del)
        {
            ZoneId = zoneId;
            Id = id;
            Del = del;
        }
    }
}
