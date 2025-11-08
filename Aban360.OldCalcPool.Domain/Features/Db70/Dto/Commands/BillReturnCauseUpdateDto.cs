namespace Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands
{
    public record BillReturnCauseUpdateDto
    {
        public short Id { get; set; }
        public int Code { get; set; }
        public string Title { get; set; } = default!;
    }
}
