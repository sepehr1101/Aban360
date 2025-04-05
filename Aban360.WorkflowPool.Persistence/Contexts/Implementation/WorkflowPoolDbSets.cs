using Aban360.WorkflowPool.Domain.Features.Design.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aban360.WorkflowPool.Persistence.Contexts.Implementation
{
    public partial class WorkflowPoolContext
    {

        public virtual DbSet<Workflow> Workflows { get; set; }

        public virtual DbSet<WorkflowStatus> WorkflowStatuses { get; set; }

    }
}
