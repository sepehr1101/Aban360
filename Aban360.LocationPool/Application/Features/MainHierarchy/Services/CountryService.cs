using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Implementations;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Services;

public interface ICountryService
{
    Task CreateAsync(CountryCreateDto countryCreateDto, CancellationToken cancellationToken);
    Task DeleteAsync(CountryDeleteDto countryDeleteDto, CancellationToken cancellationToken);
    Task UpdateAsync(CountryUpdateDto countryUpdateDto, CancellationToken cancellationToken);
    Task<ICollection<CountryGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<CountryGetDto> GetSingleAsync(short id,CancellationToken cancellationToken);
}

internal sealed class CountryService : ICountryService
{
    private readonly IMapper _mapper;
    private readonly ICountryCommandService _commands;
    private readonly ICountryQueryService _queries;

    public CountryService(IMapper mapper, ICountryCommandService commands, ICountryQueryService queries)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        commands.NotNull(nameof(commands));
        _commands = commands;
        queries.NotNull(nameof(queries));
        _queries = queries;
    }

    public async Task CreateAsync(CountryCreateDto countryCreateDto, CancellationToken cancellationToken)
    {
        Country coutry = _mapper.Map<Country>(countryCreateDto);
        await _commands.Add(coutry);
    }

    public async Task DeleteAsync(CountryDeleteDto countryDeleteDto, CancellationToken cancellationToken)
    {
        Country country = await _queries.Get(countryDeleteDto.Id);
        await _commands.Remove(country);
    }

    public async Task UpdateAsync(CountryUpdateDto countryUpdateDto, CancellationToken cancellationToken)
    {
        Country country = await _queries.Get(countryUpdateDto.Id);
        _mapper.Map(countryUpdateDto, country);
    }

    public async Task<ICollection<CountryGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<Country> countries = await _queries.Get();
        return _mapper.Map<ICollection<CountryGetDto>>(countries);
    }

    public async Task<CountryGetDto> GetSingleAsync(short id,CancellationToken cancellationToken)
    {
        Country country = await _queries.Get(id);
        return _mapper.Map<CountryGetDto>(country);
    }
}
