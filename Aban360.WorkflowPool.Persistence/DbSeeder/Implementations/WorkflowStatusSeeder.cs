using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Domain.Constants;
using Aban360.WorkflowPool.Domain.Features.Design.Entities;
using Aban360.WorkflowPool.Persistence.Contexts.Contracts;
using Aban360.WorkflowPool.Persistence.Features.Design.Commands.Contracts;

namespace Aban360.WorkflowPool.Persistence.DbSeeder.Implementations
{
    public class WorkflowStatusSeeder : IDataSeeder
    {
        public int Order { get; set; } = 100;
        private readonly IUnitOfWork _uow;
        private readonly IWorkflowStatusCommandService _workflowStatusCommandService;
        public WorkflowStatusSeeder(
            IUnitOfWork uow,
            IWorkflowStatusCommandService workflowStatusCommandService)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _workflowStatusCommandService = workflowStatusCommandService;
            _workflowStatusCommandService.NotNull(nameof(workflowStatusCommandService));
        }

        public void SeedData()
        {
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
