namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record FlatUpdateDto
    {
        public int Id { get; set; }
        public int EstateId { get; set; }
        public string? PostalCode { get; set; }
        public short Storey { get; set; }
        public string? Description { get; set; }
    }
}
