namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record MoshtrakCompanyService
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsSelected { get; set; }
        public MoshtrakCompanyService(int id, string title, bool isSelected)
        {
            Id = id;
            Title = title;
            IsSelected = isSelected;
        }
        public MoshtrakCompanyService()
        {
        }
    }
}
