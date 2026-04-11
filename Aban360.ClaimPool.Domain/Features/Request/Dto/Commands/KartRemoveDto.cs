namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record KartRemoveDto
    {
        public int Id { get; set; }
        public int Serial { get; set; }
        public KartRemoveDto(int id,int serial)
        {
            Id=id;
            Serial=serial;
        }
    }
}
