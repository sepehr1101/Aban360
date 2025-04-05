using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Domain.Features.Design.Entities;
using Aban360.WorkflowPool.Persistence.Contexts.Contracts;
using Aban360.WorkflowPool.Persistence.Features.Trigger.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.WorkflowPool.Persistence.Features.Trigger.Queries.Implementations
{
    internal sealed class WorkflowQueryService : IWorkflowQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Workflow> _workflow;
        public WorkflowQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _workflow = _uow.Set<Workflow>();
            _workflow.NotNull(nameof(_workflow));
        }

        public async Task<Workflow> Get(int id)
        {
            return await _uow.FindOrThrowAsync<Workflow>(id);
        }

        public async Task<ICollection<Workflow>> Get()
        {
            return await _workflow.ToListAsync();
        }
    }
}
