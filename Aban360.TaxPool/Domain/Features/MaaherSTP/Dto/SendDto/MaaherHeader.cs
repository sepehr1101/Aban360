namespace Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto
{
    public record MaaherHeader
    {
        public long Indatim { get; set; }
        public string Inty { get; set; }
        public long Inno { get; set; }
        public string Inp { get; set; }
        public string Ins { get; set; }
        public string Tob { get; set; }
        //public string Bid { get; set; }
        //public string Bpc { get; set; }
        public string Billid { get; set; }
        public string? Irtaxid { get; set; }
        public string? Setm { get; set; } = "Cash";
        public string? Bid { get; set; } = string.Empty;
        public string Vop { get; set; } = "10";//remove
    }
}