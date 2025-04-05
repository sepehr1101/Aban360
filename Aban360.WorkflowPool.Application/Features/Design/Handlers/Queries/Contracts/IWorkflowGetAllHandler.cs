using Aban360.WorkflowPool.Domain.Features.Design.Dto.Queries;

namespace Aban360.WorkflowPool.Application.Features.Assignment.Handlers.Queries.Contracts
{
    public interface IWorkflowGetAllHandler
    {
        Task<ICollection<WorkflowGetDto>> Handle(CancellationToken cancellationToken);
    }
}
