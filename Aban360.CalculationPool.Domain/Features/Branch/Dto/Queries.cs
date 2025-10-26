namespace Aban360.CalculationPool.Domain.Features.Branch.Dto
{
    public record InstallInputDto
    {
        public int ZoneId { get; set; }
        public bool IsDomestic { get; set; }
        public int MyProperty { get; set; }
    }
    public record InstallOutputDto
    {

    }
}
