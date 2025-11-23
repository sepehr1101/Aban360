namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record CalculateAbBahaOutputDto
    {
     
        public double AbBahaAmount { get; set; }
        public double AbBaha1 { get; set; }
        public double AbBaha2 { get; set; }
        public CalculateAbBahaOutputDto()
        {
            AbBahaAmount = 0;
            AbBaha1 = 0;
            AbBaha2 = 0;
        }
        public CalculateAbBahaOutputDto(double abBahaAmount, double abBaha1, double abBaha2, double multiplier)
        {
            AbBahaAmount = abBahaAmount;
            AbBaha1 = abBaha1;
            AbBaha2 = abBaha2;
        }
    }
}
