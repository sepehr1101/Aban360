namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record ResultFlatDto
    {
        public int Id { get; set; }
        public string? PostalCode { get; set; }
        public short Storey { get; set; }
        public string? Description { get; set; }
    }
}
