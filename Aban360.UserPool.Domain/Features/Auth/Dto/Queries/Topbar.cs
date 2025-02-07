namespace Aban360.UserPool.Domain.Features.Auth.Dto.Queries
{
    public record Topbar
    {
        public ICollection<TopbarLevel1>? Level1s { get; set; }
    }
}
