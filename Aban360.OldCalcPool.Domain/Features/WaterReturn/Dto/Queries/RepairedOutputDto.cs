namespace Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Queries
{
    public record RepairedOutputDto
    {
        public int CustomerNumber { get; set; }
        public string RegisterDateJalali { get; set; }
        public long Amount { get; set; }
    }
}