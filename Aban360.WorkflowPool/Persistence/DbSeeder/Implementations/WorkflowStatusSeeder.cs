using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Domain.Constants;
using Aban360.WorkflowPool.Domain.Features.Design.Entities;
using Aban360.WorkflowPool.Persistence.Contexts.Contracts;
using Aban360.WorkflowPool.Persistence.Features.Design.Commands.Contracts;
using Aban360.WorkflowPool.Persistence.Features.Design.Queries.Contracts;

namespace Aban360.WorkflowPool.Persistence.DbSeeder.Implementations
{
    public class WorkflowStatusSeeder : IDataSeeder
    {
        public int Order { get; set; } = 100;
        private readonly IUnitOfWork _uow;
        private readonly IWorkflowStatusCommandService _workflowStatusCommandService;
        private readonly IWorkflowStatusQueryService _workflowStatusQueryService;

        public WorkflowStatusSeeder(
            IUnitOfWork uow,
            IWorkflowStatusCommandService workflowStatusCommandService,
            IWorkflowStatusQueryService workflowStatusQueryService)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _workflowStatusCommandService = workflowStatusCommandService;
            _workflowStatusCommandService.NotNull(nameof(workflowStatusCommandService));

            _workflowStatusQueryService = workflowStatusQueryService;
            _workflowStatusQueryService.NotNull(nameof(workflowStatusQueryService));
        }

        public void SeedData()
        {
            if (_workflowStatusQueryService.AnySync())
            {
                return;
            }
            List<WorkflowStatus> list = new List<WorkflowStatus>()
            {
                new WorkflowStatus(){Id= WorkflowStatusEnum.Draft,Title="ایجاد اولیه"},
                new WorkflowStatus(){Id=WorkflowStatusEnum.Created, Title="ایجاد شده"},
                new WorkflowStatus(){Id=WorkflowStatusEnum.Published, Title="منتشر شده"}
            };
            _workflowStatusCommandService.AddSync(list);
            _uow.SaveChanges();
        }
    }
}
