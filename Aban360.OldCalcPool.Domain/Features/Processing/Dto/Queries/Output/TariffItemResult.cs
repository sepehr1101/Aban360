namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record TariffItemResult
    {
        public double Summation { get; set; }
        public double Allowed { get; set; }
        public double Disallowed { get; set; }
        public TariffItemResult(double allowed, double disallowed, double summation)
        {
            Allowed=allowed;
            Disallowed=disallowed;
            Summation=summation;
        }
    }
}
