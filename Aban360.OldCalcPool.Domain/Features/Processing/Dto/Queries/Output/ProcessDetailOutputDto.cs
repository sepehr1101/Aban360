using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;

namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record ProcessDetailOutputDto
    {
        public double AbBahaAmount { get; set; }
        public double FazelabAmount { get; set; }
        public double BoodjePart1Amount { get; set; }
        public double BoodjePart2Amount { get; set; }
        public double HotSeasonAmount { get; set; }
        public IEnumerable<NerkhGetDto> Nerkh { get; set; }
        public IEnumerable<AbAzadGetDto> AbAzad { get; set; }
        public IEnumerable<ZaribGetDto> Zarib { get; set; }
        public CustomerInfoOutputDto Customer { get; set; }
        public MeterInfoOutputDto MeterInfo { get; set; }


        public ProcessDetailOutputDto(double _abBahaAmount, double _fazelabAmount, double _boodjePart1Amount, double _boodjePar2Amount, double _hotSeasonAmount, IEnumerable<NerkhGetDto> _nerkh, IEnumerable<AbAzadGetDto> _abAzad, IEnumerable<ZaribGetDto> _zarib)
        {
            AbBahaAmount = _abBahaAmount;
            FazelabAmount = _fazelabAmount;
            BoodjePart1Amount = _boodjePart1Amount;
            BoodjePart2Amount = _boodjePar2Amount;
            HotSeasonAmount = _hotSeasonAmount;
            Nerkh = _nerkh;
            AbAzad = _abAzad;
            Zarib = _zarib;
        }
    }
}
