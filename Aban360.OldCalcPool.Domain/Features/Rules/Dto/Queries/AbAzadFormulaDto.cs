namespace Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries
{
    public record AbAzadFormulaDto
    {
        public string? Formula { get; set; }
        public string? AllowedFormula { get; set; }
        public string? DisallowedFormula { get; set; }
    }
}
