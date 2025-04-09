namespace Aban360.WorkflowPool.Domain.Features.Design.Dto.Commands
{
    public record WorkflowJsonDefinitionUpdateDto
    {
        public int Id { get; set; }
        public string JsonDefinition { get; set; }
    }
}
