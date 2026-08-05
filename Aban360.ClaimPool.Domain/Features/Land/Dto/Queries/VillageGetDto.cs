namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record VillageGetDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public string? ZoneTitle { get; set; }
        public int VillageId { get; set; }
        public string VillageName { get; set; }
        public string StringCode { get; set; }
    }
}
