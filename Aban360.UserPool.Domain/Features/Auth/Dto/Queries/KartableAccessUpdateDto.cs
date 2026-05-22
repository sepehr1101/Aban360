namespace Aban360.UserPool.Domain.Features.Auth.Dto.Queries
{
    public record KartableAccessUpdateDto
    {
        public IEnumerable<int> KartableIds { get; set; }
    }
}