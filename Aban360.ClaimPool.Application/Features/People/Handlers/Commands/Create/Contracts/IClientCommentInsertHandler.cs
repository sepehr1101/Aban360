using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.Common.ApplicationUser;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Contracts
{
    public interface IClientCommentInsertHandler
    {
        Task Handle(CustomerCommentInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}
