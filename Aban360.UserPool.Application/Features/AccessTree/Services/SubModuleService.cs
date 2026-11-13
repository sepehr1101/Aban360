using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using Aban360.UserPool.Persistence.Features.UiElement.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.AccessTree.Services;

public interface ISubModuleService
{
    Task<SubModuleGetDto> GetSingleAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<SubModuleWithEndPointGetDto>> GetChildrenAsync(int id, CancellationToken cancellationToken);
    Task<ICollection<SubModuleGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(SubModuleUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(SubModuleDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(SubModuleCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class SubModuleService : ISubModuleService
{
    private readonly IMapper _mapper;
    private readonly ISubModuleQueryService _subModuleQueryService;
    private readonly ISubModuleCommandService _subModuleCommandService;

    public SubModuleService(IMapper mapper, ISubModuleQueryService subModuleQueryService, ISubModuleCommandService subModuleCommandService)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        subModuleQueryService.NotNull(nameof(subModuleQueryService));
        _subModuleQueryService = subModuleQueryService;
        subModuleCommandService.NotNull(nameof(subModuleCommandService));
        _subModuleCommandService = subModuleCommandService;
    }

    public async Task<SubModuleGetDto> GetSingleAsync(int id, CancellationToken cancellationToken)
    {
        SubModule subModule = await _subModuleQueryService.GetInclude(id);
        return _mapper.Map<SubModuleGetDto>(subModule);
    }

    public async Task<IEnumerable<SubModuleWithEndPointGetDto>> GetChildrenAsync(int id, CancellationToken cancellationToken)
    {
        IEnumerable<SubModule> SubModule = await _subModuleQueryService.GetChildrens(id);
        return _mapper.Map<ICollection<SubModuleWithEndPointGetDto>>(SubModule);
    }

    public async Task<ICollection<SubModuleGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<SubModule> subModule = await _subModuleQueryService.GetInclude();
        return _mapper.Map<ICollection<SubModuleGetDto>>(subModule);
    }

    public async Task UpdateAsync(SubModuleUpdateDto updateDto, CancellationToken cancellationToken)
    {
        SubModule subModule = await _subModuleQueryService.Get(updateDto.Id);
        _mapper.Map(updateDto, subModule);
    }

    public async Task DeleteAsync(SubModuleDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        SubModule subModule = await _subModuleQueryService.Get(deleteDto.Id);
        _subModuleCommandService.Remove(subModule);
    }

    public async Task CreateAsync(SubModuleCreateDto createDto, CancellationToken cancellationToken)
    {
        SubModule subModule = _mapper.Map<SubModule>(createDto);
        subModule.IsActive = true;
        await _subModuleCommandService.Add(subModule);
    }

}
