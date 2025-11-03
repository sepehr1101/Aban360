namespace Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries
{
    public record VirtualCategoryGetDto
    {
        public short Id { get; set; }
        public short Code { get; set; }
        public string Title { get; set; }
        public float Multiplier { get; set; }
    }
}
