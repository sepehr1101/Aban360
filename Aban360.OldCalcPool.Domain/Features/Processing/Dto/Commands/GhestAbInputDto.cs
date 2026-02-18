namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands
{
    public record GhestAbInputDto
    {
        public string BillId { get; set; }
        public int InstallmentCount { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
