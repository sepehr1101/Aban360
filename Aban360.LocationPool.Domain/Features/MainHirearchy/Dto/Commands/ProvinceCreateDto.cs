namespace Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands
{
    public record ProvinceCreateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public short CordinalDirectionId { get; set; }
        public short CountryId { get; set; }

    }
}
