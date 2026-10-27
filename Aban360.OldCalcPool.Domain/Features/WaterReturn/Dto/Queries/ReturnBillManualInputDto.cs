using Aban360.OldCalcPool.Domain.Constants;

namespace Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries
{
    public record ReturnBillManualInputDto
    {
        public string BillId { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public string MinutesNumber { get; set; }
        public int ReturnCauseId { get; set; }
        public ReturnBillManualTypeEnum ReturnType { get; set; }


        public IEnumerable<ReturnBillManualItemInputDto> Items { get; set; }//whitch?
        public int Item1 { get; set; }
        public int Item2 { get; set; }
        public int Item3 { get; set; }
        public int Item4 { get; set; }
        public int Item5 { get; set; }
        public int Item6 { get; set; }
        public int Item10 { get; set; }
        public int Item11 { get; set; }
        public int Item13 { get; set; }
        public int Item14 { get; set; }
        public int Item16 { get; set; }
    }
}
