namespace Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto
{
    public record SentInvoiceInputDto
    {
        public ICollection<MaaherRecordInputDto> Records { get; set; }
    }
}
