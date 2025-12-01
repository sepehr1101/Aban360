namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record BaseOldTariffEngineOutputDto
    {
        public TariffItemResult AbBahaValues { get; set; }
        public double FazelabAmount { get; set; }
        public double HotSeasonAbBahaAmount { get; set; }
        public double HotSeasonFazelabAmount { get; set; }
        public double BoodjePart1 { get; set; }
        public double BoodjePart2 { get; set; }
        public double AbonmanAbAmount { get; set; }
        public double AvarezAmount { get; set; }
        public double JavaniAmount { get; set; }

        public double AbBahaDiscount { get; set; }
        public double HotSeasonDiscount { get; set; }
        public double HotSeasonFazelabDiscount { get; set; }  
        public double FazelabDiscount { get; set; }
        public double AbonmanAbDiscount { get; set; }
        public double AbonmanFazelabDiscount { get; set; }
        public double AvarezDiscount { get; set; }
        public double JavaniDiscount { get; set; }
        public double BoodjeDiscount { get; set; }
        public double Multiplier { get;}


        //public double MaliatAmount { get; set; }

        public BaseOldTariffEngineOutputDto(
            TariffItemResult abBahaValues,
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
            double javaniAmount,
            double abonmanAbDiscount,
            double abonamenFazelabDiscount,
            double avarezDiscount,
            double javaniDiscount,
            double boodjeDiscount,
            double hotSeasonFazelabDiscount,
            double multiplier)
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
            AbonmanAbAmount= abonmanAbAmount;
            abonamenFazelabDiscount = abonmanAbDiscount;
            AvarezDiscount = avarezDiscount;
            JavaniDiscount = javaniDiscount;
            BoodjeDiscount = boodjeDiscount;
            HotSeasonFazelabDiscount = hotSeasonFazelabDiscount;
            Multiplier = multiplier;
        }
    }
}
