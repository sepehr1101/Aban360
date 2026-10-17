namespace Aban360.Common.BaseEntities
{
    public record SearchByIdInput
    {
        public int Id { get; set; } = default!;

    }
    public record ZoneIdWrapper
    {
        public int ZoneId { get; set; } = default!;

    }
}
