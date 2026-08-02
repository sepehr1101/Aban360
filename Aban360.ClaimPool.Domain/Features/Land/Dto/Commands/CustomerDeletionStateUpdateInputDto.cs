using Aban360.ClaimPool.Domain.Constants;

namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record CustomerDeletionStateUpdateInputDto
    {
        public string BillId { get; set; }
        public UseStateEnum DeletionStateType { get; set; }
    }
}
