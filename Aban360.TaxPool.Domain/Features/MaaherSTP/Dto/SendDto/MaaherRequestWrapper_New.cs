namespace Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto
{
    public record MaaherRequestWrapper_New
    {
        public ICollection<MaaherBody> Body { get; set; }
        public string Creation_Type { get; set; }
        public MaaherHeader  Header { get; set; }
        public bool Is_Draft { get; set; }
        public ICollection<MaaherPayment> Payment { get; set; }
        public string Uid { get; set; }
        public MaaherRequestWrapper_New(MaaherHeader h, ICollection<MaaherBody> b)
        {
            Header = h;
            Body = b;
            Is_Draft = false;
            Uid = Guid.NewGuid().ToString();
            Payment = new List<MaaherPayment>();
        }
        public MaaherRequestWrapper_New()
        {

        }
    }
}
