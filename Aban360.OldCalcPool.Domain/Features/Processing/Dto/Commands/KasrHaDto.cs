namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands
{
    public record KasrHaDto
    {
        public long Id { get; set; }
        public decimal Town { get; set; }
        public decimal IdBedbes { get; set; }
        public decimal Radif { get; set; }
        public decimal CodEnshab { get; set; }
        public decimal Barge { get; set; }
        public string PriDate { get; set; }
        public string TodayDate { get; set; }
        public decimal PriNo { get; set; }
        public decimal TodayNo { get; set; }
        public decimal Masraf { get; set; }
        public decimal AbBaha { get; set; }
        public decimal FasBaha { get; set; }
        public decimal AbonAb { get; set; }
        public decimal AbonFas { get; set; }
        public decimal TabAbnA { get; set; }
        public decimal TabAbnF { get; set; }
        public decimal Ab10 { get; set; }
        public decimal Shahrdari { get; set; }
        public decimal Rate { get; set; }
        public decimal Baha { get; set; }
        public string ShGhabs { get; set; }
        public string ShPard { get; set; }
        public string DateBed { get; set; }
        public string TmpDateBed { get; set; }
        public string TmpTodayDate { get; set; }
        public int TedVahd { get; set; }
        public int TedadTej { get; set; }
        public int TedKhane { get; set; }
        public int TedadMas { get; set; }
        public decimal ZaribFasl { get; set; }
        public decimal NoeVa { get; set; }
        public decimal Bodjeh { get; set; }
       // public long TrackNumber { get; set; }
    }

}
