namespace Aban360.ClaimPool.Domain.Features.Draft.Dto.Queries
{
    public record RequestFlatGetDto
    {
        public int Id { get; set; }
        public string? PostalCode { get; set; }
        public short Storey { get; set; }
        public string? Description { get; set; }
        public int EstateId { get; set; }
    }
}
