namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record CalculateAbBahaOutputDto
    {
     
        public double AbBahaAmount { get; set; }
        public (double, double) AbBahaValues { get; set; }
        public double AbBaha1 { get; set; }
        public double AbBaha2 { get; set; }
        public double Multiplier { get;}
        public CalculateAbBahaOutputDto(double abBahaAmount, (double, double) abBahaValues, double abBaha1, double abBaha2, double multiplier)
        {
            AbBahaAmount = abBahaAmount;
            AbBahaValues = abBahaValues;
            AbBaha1 = abBaha1;
            AbBaha2 = abBaha2;
            Multiplier = multiplier;
        }
    }
}
