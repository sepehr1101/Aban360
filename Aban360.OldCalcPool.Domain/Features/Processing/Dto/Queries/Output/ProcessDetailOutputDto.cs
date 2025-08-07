using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;

namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record ProcessDetailOutputDto
    {
        public double SumAbBahaAmount { get; set; }
        public double AbBahaAmount { get; set; }
        public double HotSeasonAmount { get; set; }
        public double AbonmanAbAmount { get; set; }

        public double FazelabAmount { get; set; }
        public double BoodjePart1Amount { get; set; }
        public double BoodjePart2Amount { get; set; }
        public double SumBoodje { get; set; }

        public double AbBahaDiscount { get; set; }
        public double HotSeasonDiscount { get; set; }
        public double FazelabDiscount { get; set; }
        public double AvarezAmount { get; set; }
        public double JavaniAmount { get; set; }
        public double MaliatAmount { get; set; }
        public double AbonmanFazelabAmount { get; set; }

        public double MonthlyConsumption { get; set; }
        public double DailyConsumption { get; set; }
        public int Duration { get; set; }

        public IEnumerable<NerkhGetDto> Nerkh { get; set; }
        public IEnumerable<AbAzadGetDto> AbAzad { get; set; }
        public IEnumerable<ZaribGetDto> Zarib { get; set; }
        public CustomerInfoOutputDto Customer { get; set; }
        public MeterInfoOutputDto MeterInfo { get; set; }


        public ProcessDetailOutputDto(double _sumAbBahaAmount,double _abBahaAmount, double _fazelabAmount, double _boodjePart1Amount, double _boodjePar2Amount, double _sumBoodje,double _hotSeasonAmount, double _abBahaDiscount, double _hotSeasonDiscount, double _fazelabDiscount, double _abonmanAbAmount, double _avarezAmount, double _javaniAmount, double _maliatAmount, double _abonmanFazelabAmount, IEnumerable<NerkhGetDto> _nerkh, IEnumerable<AbAzadGetDto> _abAzad, IEnumerable<ZaribGetDto> _zarib)
        {
            SumAbBahaAmount = _sumAbBahaAmount;
            AbBahaAmount = _abBahaAmount;
            FazelabAmount = _fazelabAmount;
            BoodjePart1Amount = _boodjePart1Amount;
            BoodjePart2Amount = _boodjePar2Amount;
            SumBoodje=_sumBoodje;
            HotSeasonAmount = _hotSeasonAmount;
            AbBahaDiscount = _abBahaDiscount;
            HotSeasonDiscount = _hotSeasonDiscount;
            FazelabDiscount = _fazelabDiscount;
            AbonmanAbAmount = _abonmanAbAmount;
            AvarezAmount = _avarezAmount;
            JavaniAmount = _javaniAmount;
            MaliatAmount = _maliatAmount;
            AbonmanFazelabAmount = _abonmanFazelabAmount;
            Nerkh = _nerkh;
            AbAzad = _abAzad;
            Zarib = _zarib;
        }
    }
}
