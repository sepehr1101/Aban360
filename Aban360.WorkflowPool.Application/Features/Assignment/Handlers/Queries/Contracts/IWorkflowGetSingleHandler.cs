using Aban360.WorkflowPool.Domain.Features.Assignment.Dto.Queries;

namespace Aban360.WorkflowPool.Application.Features.Assignment.Handlers.Queries.Contracts
{
    public interface IWorkflowGetSingleHandler
    {
        Task<WorkflowGetDto> Handle(int id, CancellationToken cancellationToken);
    }
}
