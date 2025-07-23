namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input
{
    public record CustomerInfoInputDto
    {
        public int ZoneId { get; set; }
        public int Radif { get; set; }
        public CustomerInfoInputDto(int _zoneId, int _radif)
        {
            ZoneId = _zoneId;
            Radif = _radif;
        }
    }
}
