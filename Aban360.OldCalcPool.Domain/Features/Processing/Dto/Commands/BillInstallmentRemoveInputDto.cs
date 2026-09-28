namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands
{
    public record BillInstallmentRemoveInputDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
    }
}