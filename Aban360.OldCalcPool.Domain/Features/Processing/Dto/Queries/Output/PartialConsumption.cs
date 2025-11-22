namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record PartialConsumption
    {
        public double Allowed { get; set; }
        public double Disallowed { get; set; }
        public double Summation { get; set; }

        public double OlgooOrCapacity { get; set; }
        public double AllowedOlgooOrCapacity { get; set; }
        public double DisallowedOlgooOrCapacity { get; set; }
    }
}
