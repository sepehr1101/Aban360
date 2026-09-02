namespace Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries
{
    public record BillReturnCauseGetDto
    {
        public short Id { get; set; }
        public int Code { get; set; }
        public string Title { get; set; } = default!;
        public bool IsInList { get; set; }
        public bool IsLastMeterValid { get; set; }
        public bool IsPartial { get; set; }
    }
}