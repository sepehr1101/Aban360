namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record ServiceLinkDisconnectResultDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsValid { get; set; }
        public ServiceLinkDisconnectResultDto(int id, string title, bool isValid)
        {
            Id = id;
            Title = title;
            IsValid = isValid;
        }
        public ServiceLinkDisconnectResultDto()
        {
        }
    }
}
