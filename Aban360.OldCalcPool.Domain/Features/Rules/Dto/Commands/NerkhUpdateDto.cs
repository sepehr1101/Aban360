namespace Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands
{
    public record NerkhUpdateDto
    {
        public int Id { get; set; }
        public string? Date1 { get; set; }
        public string? Date2 { get; set; }
        public decimal Ebt { get; set; }
        public decimal Ent { get; set; }
        public string? Vaj { get; set; }
        public int Cod { get; set; }
        public int Olgo { get; set; }
        public string? Desc { get; set; }
        public string? OVaj { get; set; }
        public string? OVajFaz { get; set; }
    }
}
