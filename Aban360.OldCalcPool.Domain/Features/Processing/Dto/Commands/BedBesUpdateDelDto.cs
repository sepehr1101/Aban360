namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands
{
    public record BedBesUpdateDelDto
    {
        public int ZoneId { get; set; }
        public int Id { get; set; }
        public bool Del { get; set; }
        public BedBesUpdateDelDto(int zoneId, int id, bool del)
        {
            ZoneId = zoneId;
            Id = id;
            Del = del;
        }
    }
}
