namespace Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands
{
    public record TariffDuplicateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }=null!;
    }
}
