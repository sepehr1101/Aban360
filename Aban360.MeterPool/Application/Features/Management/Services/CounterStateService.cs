using Aban360.Common.Extensions;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using Aban360.MeterPool.Persistence.Features.Management.Commands.Contracts;
using Aban360.MeterPool.Persistence.Features.Management.Queries.Contracts;
using AutoMapper;

namespace Aban360.MeterPool.Application.Features.Management.Services;

public interface ICounterStateService
{
    Task<CounterStateGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<CounterStateGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(CounterStateUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(CounterStateDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(CounterStateCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class CounterStateService : ICounterStateService
{
    private readonly IMapper _mapper;
    private readonly ICounterStateQueryService _counterStateQueryService;
    private readonly IHeadquartersAddhoc _headquarterAddhoc;
    private readonly ICounterStateCommandService _counterStateCommandService;

    public CounterStateService(IMapper mapper, ICounterStateQueryService counterStateQueryService, IHeadquartersAddhoc headquarterAddhoc, ICounterStateCommandService counterStateCommandService)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        counterStateQueryService.NotNull(nameof(counterStateQueryService));
        _counterStateQueryService = counterStateQueryService;
        headquarterAddhoc.NotNull(nameof(headquarterAddhoc));
        _headquarterAddhoc = headquarterAddhoc;
        counterStateCommandService.NotNull(nameof(counterStateCommandService));
        _counterStateCommandService = counterStateCommandService;
    }

    public async Task<CounterStateGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        CounterState counterState = await _counterStateQueryService.Get(id);
        return _mapper.Map<CounterStateGetDto>(counterState);
    }

    public async Task<ICollection<CounterStateGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<CounterState> counterState = await _counterStateQueryService.Get();
        return _mapper.Map<ICollection<CounterStateGetDto>>(counterState);
    }

    public async Task UpdateAsync(CounterStateUpdateDto updateDto, CancellationToken cancellationToken)
    {
        CounterState counterState = await _counterStateQueryService.Get(updateDto.Id);
        _mapper.Map(updateDto, counterState);
    
        var headquarterTitle = await _headquarterAddhoc.Handle(updateDto.HeadquartersId, cancellationToken);
        counterState.HeadquartersTitle = headquarterTitle;
    }

    public async Task DeleteAsync(CounterStateDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        CounterState counterState = await _counterStateQueryService.Get(deleteDto.Id);
        await _counterStateCommandService.Remove(counterState);
    }

    public async Task CreateAsync(CounterStateCreateDto createDto, CancellationToken cancellationToken)
    {
        CounterState counterState = _mapper.Map<CounterState>(createDto);
        string headquarterTitle = await _headquarterAddhoc.Handle(createDto.HeadquartersId, cancellationToken);
        counterState.HeadquartersTitle = headquarterTitle;
    
        await _counterStateCommandService.Add(counterState);
    }

}
