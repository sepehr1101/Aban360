namespace Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.RecieveDto
{
    public record MaliatMaaherDetailGetDto
    {
        public int WrapperId { get; set; }
        public long Indatim { get; set; }
        public string Inty { get; set; }
        public long Inno { get; set; }
        public string Inp { get; set; }
        public string Ins { get; set; }
        public string Tob { get; set; }
        public string Bid { get; set; }
        public string Bpc { get; set; }
        public int Radif { get; set; }
        public string BillId { get; set; }
        public string Sstid { get; set; }
        public string Sstt { get; set; }
        public string Mu { get; set; }
        public int Am { get; set; }
        public long Fee { get; set; }
        public int Dis { get; set; }
        public int Town { get; set; }
        public string Date_Bed { get; set; }
        public long Item1 { get; set; }
        public long Item2 { get; set; }
        public long Item3 { get; set; }
        public long Item4 { get; set; }
        public long Item5 { get; set; }
        public string ItemUnit1 { get; set; }
        public string ItemUnit2 { get; set; }
        public string ItemUnit3 { get; set; }
        public string ItemUnit4 { get; set; }
        public string ItemUnit5 { get; set; }
        public DateTime? Fetch_DateTime { get; set; }
        public DateTime? SendDateTime { get; set; }
        public string? Uuid { get; set; }
        public int Flow_State { get; set; }
        public int? Error_Code { get; set; }
        public string? Final_State { get; set; }
        public string? Result { get; set; }
        public bool? IsDelete { get; set; }
        public string? TaxId { get; set; }
    }
}