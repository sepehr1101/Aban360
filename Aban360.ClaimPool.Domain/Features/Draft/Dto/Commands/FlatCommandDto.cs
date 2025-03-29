namespace Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands
{
    public record FlatCommandDto
    {
        public int Id { get; set; }
        public int EstateId { get; set; }
        public string? PostalCode { get; set; }
        public short Storey { get; set; }
        public string? Description { get; set; }
    }
}
