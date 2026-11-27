using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Services;

public interface IProvinceService
{
    Task CreateAsync(ProvinceCreateDto createDto, CancellationToken cancellationToken);
    Task DeleteAsync(ProvinceDeleteDto deleteDto, CancellationToken cancellationToken);
    Task UpdateAsync(ProvinceUpdateDto updateDto, CancellationToken cancellationToken);
    Task<ICollection<ProvinceGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<ProvinceGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
}

internal sealed class ProvinceService : IProvinceService
{
    private readonly IMapper _mapper;
    private readonly IProvinceCommandService _commands;
    private readonly IProvinceQueryService _queries;

    public ProvinceService(IMapper mapper, IProvinceCommandService commands, IProvinceQueryService queries)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        commands.NotNull(nameof(commands));
        _commands = commands;
        queries.NotNull(nameof(queries));
        _queries = queries;
    }

    public async Task CreateAsync(ProvinceCreateDto createDto, CancellationToken cancellationToken)
    {
        Province province = _mapper.Map<Province>(createDto);
        await _commands.Add(province);
    }

    public async Task DeleteAsync(ProvinceDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        Province province = await _queries.Get(deleteDto.Id);
        await _commands.Remove(province);
    }

    public async Task UpdateAsync(ProvinceUpdateDto updateDto, CancellationToken cancellationToken)
    {
        Province province = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, province);
    }

    public async Task<ICollection<ProvinceGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<Province> province = await _queries.Get();
        return _mapper.Map<ICollection<ProvinceGetDto>>(province);
    }

    public async Task<ProvinceGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        Province province = await _queries.Get(id);
        return _mapper.Map<ProvinceGetDto>(province);
    }
}
