using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using Aban360.UserPool.Persistence.Features.UiElement.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.AccessTree.Services;

public interface IAppService
{
    Task<AppGetDto> GetSingleAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<AppWithModuleGetDto>> GetChildrenAsync(int id, CancellationToken cancellationToken);
    Task<ICollection<AppGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(AppUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(AppDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(AppCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class AppService : IAppService
{
    private readonly IMapper _mapper;
    private readonly IAppQueryService _appQueryService;
    private readonly IAppCommandService _appCommandService;

    public AppService(IMapper mapper, IAppQueryService appQueryService, IAppCommandService appCommandService)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        appQueryService.NotNull(nameof(appQueryService));
        _appQueryService = appQueryService;
        appCommandService.NotNull(nameof(appCommandService));
        _appCommandService = appCommandService;
    }

    public async Task<AppGetDto> GetSingleAsync(int id, CancellationToken cancellationToken)
    {
        App app = await _appQueryService.Get(id);
        return _mapper.Map<AppGetDto>(app);
    }

    public async Task<IEnumerable<AppWithModuleGetDto>> GetChildrenAsync(int id, CancellationToken cancellationToken)
    {
        IEnumerable<App> app = await _appQueryService.GetChildrens(id);
        return _mapper.Map<ICollection<AppWithModuleGetDto>>(app);
    }

    public async Task<ICollection<AppGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<App> app = await _appQueryService.Get();
        return _mapper.Map<ICollection<AppGetDto>>(app);
    }

    public async Task UpdateAsync(AppUpdateDto updateDto, CancellationToken cancellationToken)
    {
        App app = await _appQueryService.Get(updateDto.Id);
        _mapper.Map(updateDto, app);
    }

    public async Task DeleteAsync(AppDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        App app = await _appQueryService.Get(deleteDto.Id);
        _appCommandService.Remove(app);
    }

    public async Task CreateAsync(AppCreateDto createDto, CancellationToken cancellationToken)
    {
        App app = _mapper.Map<App>(createDto);
        app.IsActive = true;
        await _appCommandService.Add(app);
    }

}
