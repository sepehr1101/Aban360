namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record BaseOldTariffEngineOutputDto
    {
        public CalculateAbBahaOutputDto AbBahaValues { get; set; }
        public double FazelabAmount { get; set; }
        public double HotSeasonAmount { get; set; }
        public double BoodjePart1 { get; set; }
        public double BoodjePart2 { get; set; }
        public double AbBahaDiscount { get; set; }
        public double HotSeasonDiscount { get; set; }
        public double FazelabDiscount { get; set; }
        public double AbonmanAbAmount { get; set; }
        public double  AvarezAmount { get; set; }
        public double JavaniAmount { get; set; }

        public BaseOldTariffEngineOutputDto(
            CalculateAbBahaOutputDto abBahaValues,
            double fazelabAmount,
            double hotSeasonAmount,
            double boodjePart1,
            double boodjePart2,
            double abBahaDiscount,
            double hotSeasonDiscount,
            double fazelabDiscount,
            double abonmanAbAmount,
            double avarezAmount,
            double javaniAmount)
        {
            AbBahaValues = abBahaValues;
            FazelabAmount = fazelabAmount;
            HotSeasonAmount = hotSeasonAmount;
            BoodjePart1 = boodjePart1;
            BoodjePart2 = boodjePart2;
            AbBahaDiscount = abBahaDiscount;
            HotSeasonDiscount = hotSeasonDiscount;
            FazelabDiscount = fazelabDiscount;
            AbonmanAbAmount = abonmanAbAmount;
            AvarezAmount = avarezAmount;
            JavaniAmount= javaniAmount;
        }
    }
}
