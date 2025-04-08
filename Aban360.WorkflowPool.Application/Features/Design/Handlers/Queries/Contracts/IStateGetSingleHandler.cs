using Aban360.WorkflowPool.Domain.Features.Design.Dto.Queries;

namespace Aban360.WorkflowPool.Application.Features.Design.Handlers.Queries.Contracts
{
    public interface IStateGetSingleHandler
    {
        Task<StateGetDto> Handle(int id, CancellationToken cancellationToken);
    }
}
