namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record TariffItemResult
    {
        public int TmpDuration { get; }
        public double Summation { get;}
        public double Allowed { get; }
        public double Disallowed { get; }

        public TariffItemResult(double allowed)
        {
            Allowed= allowed;
            Disallowed = 0;
            Summation = allowed;
        }
        public TariffItemResult(double allowed, double disallowed, int tmpDuration=0)
        {
            Allowed=allowed;
            Disallowed=disallowed;
            Summation = allowed + disallowed;
            TmpDuration=tmpDuration;
        }
        public TariffItemResult()
        {
            Summation = 0;
            Allowed = 0;
            Disallowed = 0;
        }
    }
}
