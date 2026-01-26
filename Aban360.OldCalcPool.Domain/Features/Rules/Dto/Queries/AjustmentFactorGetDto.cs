namespace Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries
{
    public record AjustmentFactorGetDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public int AjustmentFactor { get; set; }
        public long Price { get; set; }
    }
}
