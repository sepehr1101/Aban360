namespace Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Commands
{
    public record WaterReturedRemoveInputDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
    }
}
