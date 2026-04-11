using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;

namespace Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts
{
    public interface IClientCommentCommandService
    {
        Task Insert(ClientCommentInsertDto inputDto);
    }
}
