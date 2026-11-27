using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Domain.Features.Design.Dto.Commands;
using Aban360.WorkflowPool.Domain.Features.Design.Dto.Queries;
using Aban360.WorkflowPool.Persistence.Features.Design.Commands.Contracts;
using Aban360.WorkflowPool.Persistence.Features.Design.Queries.Contracts;
using AutoMapper;

namespace Aban360.WorkflowPool.Application.Features.Design.Services;

public interface IStateService
{
    Task<StateGetDto> GetSingleAsync(int id, CancellationToken cancellationToken);
    Task<ICollection<StateGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(StateUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(StateDeleteDto deleteDto, CancellationToken cancellationToken);
}

internal sealed class StateService : IStateService
{
    private readonly IMapper _mapper;
    private readonly IStateQueryService _queries;
    private readonly IStateCommandService _commands;

    public StateService(IMapper mapper, IStateQueryService queries, IStateCommandService commands)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        queries.NotNull(nameof(queries));
        _queries = queries;
        commands.NotNull(nameof(commands));
        _commands = commands;
    }

    public async Task<StateGetDto> GetSingleAsync(int id, CancellationToken cancellationToken)
    {
        var state = await _queries.Get(id);
        return _mapper.Map<StateGetDto>(state);
    }

    public async Task<ICollection<StateGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var state = await _queries.Get();
        return _mapper.Map<ICollection<StateGetDto>>(state);
    }

    public async Task UpdateAsync(StateUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var state = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, state);
        state.ValidFrom = DateTime.Now;
        state.Hash = "hash";
        state.InsertLogInfo = "insertLogInfo";

    }

    public async Task DeleteAsync(StateDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var state = await _queries.Get(deleteDto.Id);
        await _commands.Remove(state);
    }
}
