using Aban360.WorkflowPool.Domain.Constants;

namespace Aban360.WorkflowPool.Domain.Features.Design.Dto.Queries
{
    public record WorkflowGetMasterDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Name { get; set; } = null!;
        public short Version { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public WorkflowStatusEnum WorkflowStatusId { get; set; }
    }
}
