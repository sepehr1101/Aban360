using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Domain.Features.Design.Entities;
using Aban360.WorkflowPool.Persistence.Contexts.Contracts;
using Aban360.WorkflowPool.Persistence.Features.Design.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.WorkflowPool.Persistence.Features.Design.Commands.Implementations
{
    internal sealed class WorkflowStatusCommandService : IWorkflowStatusCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<WorkflowStatus> _workflowStatuses;

        public WorkflowStatusCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _workflowStatuses = _uow.Set<WorkflowStatus>();
            _workflowStatuses.NotNull(nameof(_workflowStatuses));
        }

        public async Task Add(WorkflowStatus workflowStatus)
        {
            await _workflowStatuses.AddAsync(workflowStatus);
        }
        public void AddSync(ICollection<WorkflowStatus> workflowStatuses)
        {
            _workflowStatuses.AddRange(workflowStatuses);
        }
    }
}
