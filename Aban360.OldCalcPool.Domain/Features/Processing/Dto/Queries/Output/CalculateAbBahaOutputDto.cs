namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record CalculateAbBahaOutputDto
    {
        public float Multilpier { get; set; }
        public double AbBahaAmount { get; set; }
        public (double, double) AbBahaValues { get; set; }
        public CalculateAbBahaOutputDto(double abBahaAmount, (double, double) abBahaValues)
        {
            AbBahaAmount = abBahaAmount;
            AbBahaValues = abBahaValues;
        }
    }
}
