namespace Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands
{
    public record VirtualCategoryCreateDto
    {
        public short Code { get; set; }
        public string Title { get; set; }
        public float Multiplier { get; set; }
    }
}
