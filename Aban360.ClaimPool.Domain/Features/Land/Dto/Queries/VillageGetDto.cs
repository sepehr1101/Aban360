namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record VillageGetDto
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Title { get; set; }
        public string StringCode { get; set; }
    }
}
