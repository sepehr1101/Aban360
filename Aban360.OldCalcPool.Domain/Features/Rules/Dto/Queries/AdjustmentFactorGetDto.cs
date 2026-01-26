namespace Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries
{
    public record AdjustmentFactorGetDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public float AdjustmentFactor { get; set; }
        public long Price { get; set; }
    }
}
