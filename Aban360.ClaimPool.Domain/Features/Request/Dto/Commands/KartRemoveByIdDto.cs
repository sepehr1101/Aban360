namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record KartRemoveByIdDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public KartRemoveByIdDto(int id, int zoneId, int customerNumber)
        {
            Id = id;
            ZoneId = zoneId;
            CustomerNumber = customerNumber;
        }
        public KartRemoveByIdDto()
        {
        }
    }
}
