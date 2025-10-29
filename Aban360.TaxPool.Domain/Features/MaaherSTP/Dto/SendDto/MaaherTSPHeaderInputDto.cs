using Aban360.TaxPool.Domain.Constants;

namespace Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto
{
    public record MaaherTSPHeaderInputDto
    {
        public long Indatim { get; set; }
        public InvoiceTypeEnum Inty { get; set; }
        public long? Inno { get; set; }
        public InvoiceOlgooEnum Inp { get; set; }
        public InvoiceSubjectEnum? Ins { get; set; }
        public CustomerTypeEnum Tob { get; set; }
        public string? Billid { get; set; }
        public long? Bid { get; set; }
        public string? Bpc { get; set; }
    }
}
