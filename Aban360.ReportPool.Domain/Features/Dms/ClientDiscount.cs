namespace Aban360.ReportPool.Domain.Features.Dms
{
    public record ClientDiscount
    {
        public string FileName { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string FatherName { get; set; }
        public string CodeMeli { get; set; }
        public int DarsadJanbazi { get; set; }
        public int Radif { get; set; }
        public int PreRadif { get; set; }
        public int LifeType { get; set; }
    }
}
