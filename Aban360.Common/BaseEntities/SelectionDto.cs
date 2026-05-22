namespace Aban360.Common.BaseEntities
{
    public record SelectionDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsSelected { get; set; }
        public SelectionDto(int id, string title, bool isSelected)
        {
            Id = id;
            Title = title;
            IsSelected = isSelected;
        }
        public SelectionDto()
        {
        }
    }
}
