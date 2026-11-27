using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Domain.Constants;
using Aban360.WorkflowPool.Domain.Features.Design.Dto.Commands;
using Aban360.WorkflowPool.Domain.Features.Design.Dto.Queries;
using Aban360.WorkflowPool.Domain.Features.Design.Entities;
using Aban360.WorkflowPool.Persistence.Features.Design.Commands.Contracts;
using Aban360.WorkflowPool.Persistence.Features.Design.Queries.Contracts;
using AutoMapper;

namespace Aban360.WorkflowPool.Application.Features.Design.Services;

public interface IWorkflowService
{
    Task<WorkflowGetDto> GetSingleAsync(int id, CancellationToken cancellationToken);
    Task<ICollection<WorkflowGetMasterDto>> GetMasterAsync(CancellationToken cancellationToken);
    Task<ICollection<WorkflowGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(WorkflowUpdateDto updateDto, CancellationToken cancellationToken);
    Task UpdateDefinitionAsync(WorkflowJsonDefinitionUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(WorkflowDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(WorkflowCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class WorkflowService : IWorkflowService
{
    private readonly IMapper _mapper;
    private readonly IWorkflowQueryService _workflowQueryService;
    private readonly IWorkflowCommandService _workflowCommandService;

    public WorkflowService(IMapper mapper, IWorkflowQueryService workflowQueryService, IWorkflowCommandService workflowCommandService)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        workflowQueryService.NotNull(nameof(workflowQueryService));
        _workflowQueryService = workflowQueryService;
        workflowCommandService.NotNull(nameof(workflowCommandService));
        _workflowCommandService = workflowCommandService;
    }

    public async Task<WorkflowGetDto> GetSingleAsync(int id, CancellationToken cancellationToken)
    {
        var workflow = await _workflowQueryService.Get(id);
        return _mapper.Map<WorkflowGetDto>(workflow);
    }

    public async Task<ICollection<WorkflowGetMasterDto>> GetMasterAsync(CancellationToken cancellationToken)
    {
        var workflow = await _workflowQueryService.GetMaster();
        return _mapper.Map<ICollection<WorkflowGetMasterDto>>(workflow);
    }

    public async Task<ICollection<WorkflowGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var workflow = await _workflowQueryService.Get();
        return _mapper.Map<ICollection<WorkflowGetDto>>(workflow);
    }

    public async Task UpdateAsync(WorkflowUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var workflow = await _workflowQueryService.Get(updateDto.Id);
        workflow.ValidFrom = DateTime.Now;
    
        _mapper.Map(updateDto, workflow);
    }

    public async Task UpdateDefinitionAsync(WorkflowJsonDefinitionUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var workflow = await _workflowQueryService.Get(updateDto.Id);
        workflow.JsonDefinition = updateDto.JsonDefinition;
    
        _mapper.Map(updateDto, workflow);
    }

    public async Task DeleteAsync(WorkflowDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var workflow = await _workflowQueryService.Get(deleteDto.Id);
        _workflowCommandService.Remove(workflow);
    }

    public async Task CreateAsync(WorkflowCreateDto createDto, CancellationToken cancellationToken)
    {
        Workflow workflow = _mapper.Map<Workflow>(createDto);
        workflow.ValidFrom = DateTime.Now;
        workflow.Name = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
        workflow.WorkflowStatusId = WorkflowStatusEnum.Draft;
    
        workflow.States.ForEach(s =>
        {
            s.ValidFrom = DateTime.Now;
            s.Hash = "hash";
            s.InsertLogInfo = "insertLogInfo";
        });
       await _workflowCommandService.Add(workflow);
    }

}
