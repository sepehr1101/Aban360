namespace Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands
{
    public record ZaribCCreateDto
    {
        public string FromDateJalali { get; set; } = default!;
        public string ToDateJalali { get; set; } = default!;
        public int C { get; set; }
        public int ConditionGroup { get; set; }
        public bool IsDeleted { get; set; }
    }
}
