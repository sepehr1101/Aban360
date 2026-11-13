using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using Aban360.UserPool.Persistence.Features.UiElement.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.AccessTree.Services;

public interface IModuleService
{
    Task<ModuleGetDto> GetSingleAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<ModuleWithSubModuleGetDto>> GetChildrenAsync(int id, CancellationToken cancellationToken);
    Task<ICollection<ModuleGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(ModuleUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(ModuleDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(ModuleCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class ModuleService : IModuleService
{
    private readonly IMapper _mapper;
    private readonly IModuleQueryService _moduleQueryService;
    private readonly IModuleCommandService _moduleCommandService;

    public ModuleService(IMapper mapper, IModuleQueryService moduleQueryService, IModuleCommandService moduleCommandService)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        moduleQueryService.NotNull(nameof(moduleQueryService));
        _moduleQueryService = moduleQueryService;
        moduleCommandService.NotNull(nameof(moduleCommandService));
        _moduleCommandService = moduleCommandService;
    }

    public async Task<ModuleGetDto> GetSingleAsync(int id, CancellationToken cancellationToken)
    {
        Module module = await _moduleQueryService.GetInclude(id);
        return _mapper.Map<ModuleGetDto>(module);
    }

    public async Task<IEnumerable<ModuleWithSubModuleGetDto>> GetChildrenAsync(int id, CancellationToken cancellationToken)
    {
        IEnumerable<Module> Module = await _moduleQueryService.GetChildrens(id);
        return _mapper.Map<ICollection<ModuleWithSubModuleGetDto>>(Module);
    }

    public async Task<ICollection<ModuleGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<Module> module = await _moduleQueryService.GetInclude();
        ICollection<ModuleGetDto> list = _mapper.Map<ICollection<ModuleGetDto>>(module);
        foreach (var item in list)
        {
            string description = string.IsNullOrWhiteSpace(item.Description) ?string.Empty:$"-{item.Description}";
            item.Title = $"{item.Title}{description}";
        }
    
        return list;
    }

    public async Task UpdateAsync(ModuleUpdateDto updateDto, CancellationToken cancellationToken)
    {
        Module module = await _moduleQueryService.Get(updateDto.Id);
        _mapper.Map(updateDto, module);
    }

    public async Task DeleteAsync(ModuleDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        Module module = await _moduleQueryService.Get(deleteDto.Id);
        _moduleCommandService.Remove(module);
    }

    public async Task CreateAsync(ModuleCreateDto createDto, CancellationToken cancellationToken)
    {
        Module module = _mapper.Map<Module>(createDto);
        module.IsActive = true;
        await _moduleCommandService.Add(module);
    }

}
