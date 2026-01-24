namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Input
{
    public record AfterSaleInputDto
    {
        public int ZoneId { get; set; }
        public string? Block { get; set; }
        public ICollection<int> CompanyServiceIds { get; set; } 
        public AfterSaleItemsInputDto PreviousData{ get; set; }
        public AfterSaleItemsInputDto CurrentData{ get; set; }
    }
}
