namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record ServiceGroupWithCheckedOutputDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsChecked { get; set; }
        public ServiceGroupWithCheckedOutputDto(int id, string title, bool isChecked)
        {
            Id = id;
            Title = title;
            IsChecked = isChecked;
        }
        public ServiceGroupWithCheckedOutputDto()
        {
            
        }
    }
}