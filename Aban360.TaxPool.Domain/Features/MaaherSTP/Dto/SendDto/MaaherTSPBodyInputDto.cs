namespace Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto
{
    public record MaaherTSPBodyInputDto
    {//todo: validation
        public string Sstid { get; set; }
        public string Sstt { get; set; }
        public string Mu { get; set; }
        public int Am { get; set; }
        public long Fee { get; set; }
        public long Dis { get; set; }
    }
}
