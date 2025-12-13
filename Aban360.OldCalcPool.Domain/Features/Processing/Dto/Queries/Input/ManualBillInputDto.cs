namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input
{
    public record ManualBillInputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public ICollection<int> ZoneIds { get; set; }
    }
}
