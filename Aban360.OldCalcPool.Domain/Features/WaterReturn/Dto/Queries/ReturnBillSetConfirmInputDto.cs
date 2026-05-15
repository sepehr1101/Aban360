namespace Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries
{
    public record ReturnBillSetConfirmInputDto
    {
        public int ConfirmedNumber { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
