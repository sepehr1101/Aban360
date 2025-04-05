using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Domain.Features.Design.Entities;
using Aban360.WorkflowPool.Persistence.Contexts.Contracts;
using Aban360.WorkflowPool.Persistence.Features.Design.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.WorkflowPool.Persistence.Features.Design.Queries.Implementations
{
    internal sealed class WorkflowStatusQueryService : IWorkflowStatusQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<WorkflowStatus> _workflowStatuses;
        public WorkflowStatusQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _workflowStatuses = _uow.Set<WorkflowStatus>();
            _workflowStatuses.NotNull(nameof(_workflowStatuses));
        }
        public bool AnySync()
        {
            return _workflowStatuses.Any();
        }
        public async Task<bool> Any()
        {
            return await _workflowStatuses.AnyAsync();
        }
        public async Task<ICollection<WorkflowStatus>> Get()
        {
            return await _workflowStatuses
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
