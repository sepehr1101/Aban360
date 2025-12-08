using Aban360.TaxPool.Domain.Constants;

namespace Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto
{
    public record MaaherTSPHeaderInputDto
    {
        public long Indatim { get; set; }
        public string Inty { get; set; }
        public long? Inno { get; set; }
        public string Inp { get; set; }
        public string? Ins { get; set; }
        public string Tob { get; set; }
        public string? Billid { get; set; }
        public long? Bid { get; set; }
        public string? Bpc { get; set; }
    }
}
