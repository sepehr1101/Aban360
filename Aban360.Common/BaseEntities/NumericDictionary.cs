namespace Aban360.Common.BaseEntities
{
    public record NumericDictionary
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public NumericDictionary()
        {
            
        }
        public NumericDictionary(int id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}
