namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Input
{
    public record DeleteDto
    {
        public short Id { get; set; }
        public DateTime RemoveDateTime { get; set; }
        public Guid RemoveByUserId { get; set; }
        public DeleteDto(short id, DateTime removeDateTime, Guid removeByUserId)
        {
            Id = id;
            RemoveDateTime = removeDateTime;
            RemoveByUserId = removeByUserId;
        }
    }
}
