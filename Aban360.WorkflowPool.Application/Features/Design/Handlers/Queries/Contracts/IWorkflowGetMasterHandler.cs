using Aban360.WorkflowPool.Domain.Features.Design.Dto.Queries;

namespace Aban360.WorkflowPool.Application.Features.Assignment.Handlers.Queries.Contracts
{
    public interface IWorkflowGetMasterHandler
    {
        Task<ICollection<WorkflowGetMasterDto>> Handle(CancellationToken cancellationToken);
    }
}
