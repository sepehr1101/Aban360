namespace Aban360.CalculationPool.Domain.Features.ServiceLink
{
    public record VosoEnInsertDto
    {
        public int Town { get; set; }
        public long Radif { get; set; }
        public string ParNo { get; set; }
        public string PayDate { get; set; }
        public string DateBes { get; set; }
        public string DateBank { get; set; }
        public string DateSabt { get; set; }
        public string CodBank { get; set; }
        public int Serial { get; set; }
        public short Ser { get; set; }
        public long Cod1 { get; set; }
        public long Cod2 { get; set; }
        public long Cod3 { get; set; }
        public long Pard { get; set; }
        public long Jam { get; set; }
        public int Elat { get; set; }
        public int Barge { get; set; }
        public decimal Enshab { get; set; }
        public int CodEnshab { get; set; }
        public short Operator { get; set; }
        public string TypePay { get; set; }
        public string ShPard { get; set; }
        public string ShGhabs { get; set; }
        public int Type { get; set; }
        public int NoeBed { get; set; }
        public string Mohlat { get; set; }
        public int TedadMas { get; set; }
        public int TedadTej { get; set; }
        public int TedadVahd { get; set; }
        public string CheckNo { get; set; }
        public string CodReport { get; set; }
        public int ChkKarbari { get; set; }
        public int PassCheck { get; set; }
        public long C120 { get; set; }
        public long C220 { get; set; }
        public string TmpDateBes { get; set; }
        public string TmpDateSabt { get; set; }
        public string TmpPayDate { get; set; }
        public string TmpDateBank { get; set; }
    }
}
