namespace Aban360.Common.BaseEntities
{
    public record GuidDictionary
    {
        public int Id { get; set; }
        public Guid Title { get; set; } = default!;
        public GuidDictionary()
        {
        }
        public GuidDictionary(int id, Guid title)
        {
            Id = id;
            Title = title;
        }
    }
}
