namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input
{
    public record RemoveBillInputDto
    {
        public int Id { get; set; }
        public string BillId { get; set; }
    }
}
