namespace Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands
{
    public record OfferingToCreateRepairDto
    {
        public string BillId { get; set; }
        public decimal AbonFas { get; set; }
        public decimal FasBaha { get; set; }
        public decimal AbBaha { get; set; }
        public decimal Ztadil { get; set; }
        public decimal Shahrdari { get; set; }
        public decimal JalaseNo { get; set; }
        public decimal AbonAb { get; set; }
        public decimal Elat { get; set; }
        public decimal ZaribFasl { get; set; }
        public decimal Ab10 { get; set; }
        public decimal Ab20 { get; set; }
        public decimal Zabresani { get; set; }
        public decimal ZaribD { get; set; }
        public decimal TabAbnA { get; set; }
        public decimal TabAbnF { get; set; }
        public decimal TabsFa { get; set; }
        public decimal Bodjeh { get; set; }
        public decimal Group1 { get; set; }
        public bool Faz { get; set; }
        public decimal Avarez { get; set; }
    }
}
