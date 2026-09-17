using Aban360.ClaimPool.Domain.Constants;

namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record CustomerDeletionStateUpdateInputDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
        public UseStateEnum DeletionStateType { get; set; }
    }
}
