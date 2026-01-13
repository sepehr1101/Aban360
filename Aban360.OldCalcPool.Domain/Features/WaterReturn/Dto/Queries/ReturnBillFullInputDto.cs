namespace Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries
{
    public record ReturnBillFullInputDto
    {
        public string BillId { get; set; }
        public int ReturnCauseId { get; set; }
        public int Minutes { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public bool IsConfirm { get; set; }
    
    }
}
