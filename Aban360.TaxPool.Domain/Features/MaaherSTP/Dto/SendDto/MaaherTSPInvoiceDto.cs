namespace Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto
{
    public record MaaherTSPInvoiceDto
    {
        public ICollection<MaaherTSPBodyInputDto> Body { get; set; }
        public string Creation_Type { get; set; }
        public MaaherTSPHeaderInputDto Header { get; set; }
        public bool Is_Draft { get; set; }
        //  public List<MaaherTSPPaymentInputDto> Payment { get; set; }
        public Guid Uid { get; set; }
    }
}
