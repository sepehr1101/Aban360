namespace Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries
{
    public record ProvinceGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public short CordinalDirectionId { get; set; }
        public short CountryId { get; set; }

    }
}
