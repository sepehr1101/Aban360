namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record BaseOldTariffEngineOutputDto
    {
        public double  AbBahaAmount { get; set; }
        public double  FazelabAmount { get; set; }
        public double  HotSeasonAmount { get; set; }
        public double  BoodjePart1 { get; set; }
        public double  BoodjePart2 { get; set; }

        public BaseOldTariffEngineOutputDto(double _AbBahaAmount,double _FazelabAmoun,double _HotSeasonAmount,double _BoodjePar1,double _BoodjePar2)
        {
            AbBahaAmount = _AbBahaAmount;
            FazelabAmount = _FazelabAmoun;
            HotSeasonAmount = _HotSeasonAmount;
            BoodjePart1 = _BoodjePar1;
            BoodjePart2 = _BoodjePar2;
        }
    }
}
