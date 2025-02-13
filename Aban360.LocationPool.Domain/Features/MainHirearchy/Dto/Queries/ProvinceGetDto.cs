namespace Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Queries
{
    public record ProvinceGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public short CordinalDirectionId { get; set; }
        public string CordinalDirectionTitle { get; set; }
        public short CountryId { get; set; }
        public string CountryTitle { get; set; }

    }
}
