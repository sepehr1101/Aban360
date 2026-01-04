namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record BaseOldTariffEngineOutputDto
    {
        public TariffItemResult AbBahaValues { get; set; }

        public TariffItemResult Fazelab { get; }
        public double FazelabAmount { get; set; }

        public TariffItemResult HotSeasonAb { get; set; }
        public double HotSeasonAbBahaAmount { get; set; }

        public TariffItemResult HotSeasonFazelab { get; set; }
        public double HotSeasonFazelabAmount { get; set; }

        public TariffItemResult Boodje { get; set; }
        public double BoodjePart1 { get; set; }
        public double BoodjePart2 { get; set; }

        public TariffItemResult AbonmanAb { get; set; }
        public double AbonmanAbAmount { get; set; }

        public TariffItemResult Avarez { get; set; }
        public double AvarezAmount { get; set; }

        public TariffItemResult Javani { get; set; }
        public double JavaniAmount { get; set; }

        public TariffItemResult AbBahaDis { get; set; }
        public double AbBahaDiscount { get; set; }

        public TariffItemResult HotSeasonAbDis { get; set; }
        public double HotSeasonDiscount { get; set; }

        public TariffItemResult HotSeasonFazelabDis { get; set; }
        public double HotSeasonFazelabDiscount { get; set; }

        public TariffItemResult FazelabDis { get; set; }
        public double FazelabDiscount { get; set; }

        public TariffItemResult AbonmanAbDis { get; set; }
        public double AbonmanAbDiscount { get; set; }

        public TariffItemResult AbonmanFazelabDis { get; set; }
        public double AbonmanFazelabDiscount { get; set; }

        public TariffItemResult AvarezDis { get; set; }
        public double AvarezDiscount { get; set; }

        public TariffItemResult JavaninDis { get; set; }
        public double JavaniDiscount { get; set; }

        public TariffItemResult BoodjeDis { get; set; }
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
            double multiplier,
            TariffItemResult fazelab,
            TariffItemResult hotSeasonAb,
            TariffItemResult hotSeasonFazelab,
            TariffItemResult boodje
            )          
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

            Fazelab = fazelab;
            HotSeasonAb = hotSeasonAb;
            HotSeasonFazelab = hotSeasonFazelab;
            Boodje = boodje;
        }
    }
}
