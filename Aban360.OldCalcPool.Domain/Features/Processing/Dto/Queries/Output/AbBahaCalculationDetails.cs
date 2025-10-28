using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;

namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record AbBahaCalculationDetails
    {
        public double SumItems { get; set; }
        public double SumAbBahaAmount { get; set; }
        public double AbBahaAmount { get; set; }
        public double HotSeasonAbBahaAmount { get; set; }
        public double HotSeasonFazelabAmount { get; set; }
        public double AbonmanAbAmount { get; set; }

        public double FazelabAmount { get; set; }
        public double BoodjePart1Amount { get; set; }
        public double BoodjePart2Amount { get; set; }
        public double SumBoodje { get; set; }
        public double JavaniAmount { get; set; }
        public double MaliatAmount { get; set; }
        public double AbonmanFazelabAmount { get; set; }
        public double AvarezAmount { get; set; }

        public double AbBahaDiscount { get; set; }
        public double HotSeasonDiscount { get; set; }
        public double FazelabDiscount { get; set; }
        public double AbonmanAbDiscount { get; set; }
        public double AbonmanFazelabDiscount { get; set; }
        public double AvarezDiscount { get; set; }
        public double JavaniDiscount { get; set; }
        public double BoodjeDiscount { get; set; }
        public double MaliatDiscount { get; set; }
        public double DiscountSum { get; set; }


        public double Consumption { get; set; }
        public double MonthlyConsumption { get; set; }
        public double DailyConsumption { get; set; }
        public int Duration { get; set; }

        public IEnumerable<NerkhGetDto> Nerkh { get; set; }
        public IEnumerable<AbAzadFormulaDto> AbAzad { get; set; }
        public IEnumerable<ZaribGetDto> Zarib { get; set; }
        public CustomerInfoOutputDto Customer { get; set; }
        public MeterInfoOutputDto MeterInfo { get; set; }

        public long StopWatch { get; set; }

        public AbBahaCalculationDetails(
            double _sumAbBahaAmount,
            double _abBahaAmount,
            double _fazelabAmount,
            double _boodjePart1Amount,
            double _boodjePar2Amount,
            double _sumBoodje,
            double _hotSeasonAbBahaAmount,
            double _hotSeasonFazelabAmount,
            double _abBahaDiscount,
            double _hotSeasonDiscount,
            double _fazelabDiscount,
            double _abonmanAbAmount, 
            double _avarezAmount, 
            double _javaniAmount,
            double _maliatAmount,
            double _abonmanFazelabAmount, 
            double abonmanAbDiscount,
            double abonmaneFazelabDiscount,
            double avarezDiscount,
            double javaniDiscount,
            double maliatDiscount,
            double boodjeDiscount,
            IEnumerable<NerkhGetDto> _nerkh,
            IEnumerable<AbAzadFormulaDto> _abAzad,
            IEnumerable<ZaribGetDto> _zarib,
            long _stopWatch)
        {
            SumItems = _abBahaAmount + _fazelabAmount + _sumBoodje + _hotSeasonAbBahaAmount + _hotSeasonFazelabAmount +
                       _avarezAmount + _javaniAmount + _maliatAmount + _abonmanAbAmount + _abonmanFazelabAmount;
            SumAbBahaAmount = _sumAbBahaAmount;
            AbBahaAmount = _abBahaAmount;
            FazelabAmount = _fazelabAmount;
            BoodjePart1Amount = _boodjePart1Amount;
            BoodjePart2Amount = _boodjePar2Amount;
            SumBoodje=_sumBoodje;
            HotSeasonAbBahaAmount = _hotSeasonAbBahaAmount;
            HotSeasonFazelabAmount = _hotSeasonFazelabAmount;           
            AbonmanAbAmount = _abonmanAbAmount;
            AvarezAmount = _avarezAmount;
            JavaniAmount = _javaniAmount;
            MaliatAmount = _maliatAmount;
            AbonmanFazelabAmount = _abonmanFazelabAmount;

            AbBahaDiscount = _abBahaDiscount;
            FazelabDiscount = _fazelabDiscount;
            HotSeasonDiscount = _hotSeasonDiscount;
            AbonmanAbDiscount = abonmanAbDiscount;
            AbonmanFazelabDiscount = abonmaneFazelabDiscount;
            AvarezDiscount = avarezDiscount;
            JavaniDiscount = javaniDiscount;
            MaliatDiscount = maliatDiscount;
            BoodjeDiscount = boodjeDiscount;

            DiscountSum = _abBahaDiscount + _hotSeasonDiscount + _fazelabDiscount + abonmanAbDiscount + 
                          abonmaneFazelabDiscount + avarezDiscount + javaniDiscount+ maliatDiscount+ boodjeDiscount;

            AbBahaAmount = TrimAmount(AbBahaAmount, AbBahaDiscount);
            FazelabAmount = TrimAmount(FazelabAmount, FazelabDiscount);
            HotSeasonAbBahaAmount = TrimAmount(HotSeasonAbBahaAmount, HotSeasonDiscount);
            AbonmanAbAmount = TrimAmount(AbonmanAbAmount, AbonmanAbDiscount);
            AbonmanFazelabAmount = TrimAmount(AbonmanFazelabAmount, AbonmanFazelabDiscount);
            AvarezAmount = TrimAmount(AvarezAmount, AvarezDiscount);
            JavaniAmount = TrimAmount(JavaniAmount, JavaniDiscount);
            MaliatAmount = TrimAmount(MaliatAmount, MaliatDiscount);
            SumItems = TrimAmount(SumItems, DiscountSum);            

            Nerkh = _nerkh;
            AbAzad = _abAzad;
            Zarib = _zarib;
            StopWatch = _stopWatch;
        }
        private double TrimAmount(double mainAmount, double discountAmount)
        {
            double remained = mainAmount - discountAmount;
            return remained < 1 ? 0 : mainAmount;
        }
    }
}
