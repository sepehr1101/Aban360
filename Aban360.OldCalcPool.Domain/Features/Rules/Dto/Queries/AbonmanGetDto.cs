namespace Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries
{
    public record AbonmanGetDto
    {
        public int Id { get; set; }
        public string Date1 { get; set; }
        public string Date2 { get; set; }
        public string Vaj { get; set; }
        public int Code { get; set; }
        public string? Desc { get; set; }
    }
}
