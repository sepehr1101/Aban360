namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record FlatGetDto
    {
        public int Id { get; set; }
        public int EstateId { get; set; }
        public string EstateTitle { get; set; }
        public string? PostalCode { get; set; }
        public short Storey { get; set; }
        public string? Description { get; set; }
    }
}
