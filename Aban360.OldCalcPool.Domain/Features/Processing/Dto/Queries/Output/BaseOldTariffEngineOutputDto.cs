namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record BaseOldTariffEngineOutputDto
    {
        public CalculateAbBahaOutputDto AbBahaValues { get; set; }
        public double FazelabAmount { get; set; }
        public double HotSeasonAbBahaAmount { get; set; }
        public double HotSeasonFazelabAmount { get; set; }
        public double BoodjePart1 { get; set; }
        public double BoodjePart2 { get; set; }
        public double AbBahaDiscount { get; set; }
        public double HotSeasonDiscount { get; set; }
        public double FazelabDiscount { get; set; }
        public double AbonmanAbAmount { get; set; }
        public double AvarezAmount { get; set; }
        public double JavaniAmount { get; set; }
        //public double MaliatAmount { get; set; }

        public BaseOldTariffEngineOutputDto(
            CalculateAbBahaOutputDto abBahaValues,
            double fazelabAmount,
            double hotSeasonAbBahaAmount,
            double hotSeasonFazelabAmount,
            double boodjePart1,
            double boodjePart2,
            double abBahaDiscount,
            double hotSeasonDiscount,
            double fazelabDiscount,
            double abonmanAbAmount,
            double avarezAmount,
            double javaniAmount)
           //double maliatAmount)
        {
            AbBahaValues = abBahaValues;
            FazelabAmount = fazelabAmount;
            HotSeasonAbBahaAmount = hotSeasonAbBahaAmount;
            HotSeasonFazelabAmount = hotSeasonFazelabAmount;
            BoodjePart1 = boodjePart1;
            BoodjePart2 = boodjePart2;
            AbBahaDiscount = abBahaDiscount;
            HotSeasonDiscount = hotSeasonDiscount;
            FazelabDiscount = fazelabDiscount;
            AbonmanAbAmount = abonmanAbAmount;
            AvarezAmount = avarezAmount;
            JavaniAmount = javaniAmount;
            //MaliatAmount = maliatAmount;
        }
    }
}
