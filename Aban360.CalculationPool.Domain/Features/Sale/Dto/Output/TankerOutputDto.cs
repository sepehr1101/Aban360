namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Output
{
    public record TankerOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int RegionId { get; set; }
        public string RegionTitle { get; set; }
        public int CustomerNumber { get; set; }
        public string RegisterDateJalali { get; set; }
        public string FirstName { get; set; }
        public string? Surname { get; set; }
        public string? Address { get; set; }
        public int Consumption { get; set; }
        public long Amount { get; set; }
        public bool IsDeleted { get; set; }
        public int DeletedBy { get; set; }
        public string? DeletedDateJalali { get; set; }
    }
}
