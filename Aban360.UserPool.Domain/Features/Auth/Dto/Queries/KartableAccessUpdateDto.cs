namespace Aban360.UserPool.Domain.Features.Auth.Dto.Queries
{
    public record KartableAccessUpdateDto
    {
        public Guid UserId { get; set; }
        public IEnumerable<int> KartableIds { get; set; }
    }
}