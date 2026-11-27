namespace Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto
{
    public record MaaherBody
    {
        public string Sstid { get; set; }
        public string Sstt { get; set; }
        public string Mu { get; set; }
        public int Am { get; set; }
        public long Fee { get; set; }
        public long Dis { get; set; }
        public MaaherBody(string sstid, string sstt, string mu, int am, long fee, long dis)
        {
            Sstid = sstid;
            Sstt = sstt;
            Mu = mu;
            Am = am;
            Fee = fee;
            Dis = dis;
        }
    }
}