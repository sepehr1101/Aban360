namespace Aban360.Common.BaseEntities
{
    public record StringDictionary
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = default!;
        public StringDictionary()
        {
            
        }
        public StringDictionary(string id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}
