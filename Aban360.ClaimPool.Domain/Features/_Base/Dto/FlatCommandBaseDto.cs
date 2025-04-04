namespace Aban360.ClaimPool.Domain.Features._Base.Dto
{
    public record FlatCommandBaseDto
    { 
        public string? PostalCode { get; set; }
        public short Storey { get; set; }
        public string? Description { get; set; }
    }
}
