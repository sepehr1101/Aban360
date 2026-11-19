using Aban360.Common.Categories.UseragentLog;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Factories;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries.ValueKeyItems;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Threading;
using Endpoint = Aban360.UserPool.Domain.Features.AceessTree.Entites.Endpoint;

namespace Aban360.UserPool.Application.Features.Auth.Services;

public interface IRoleService
{
    Task CreateAsync(RoleCreateDto createDto, CancellationToken cancellationToken);
    Task UpdateAsync(RoleUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(RoleDeleteDto deleteDto, CancellationToken cancellationToken);
    Task<RoleDisplayDto> GetSingleAsync(int id, CancellationToken cancellationToken);
    Task<ICollection<RoleGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<RoleParamsOfCreateDto> GetCreateParamsAsync(CancellationToken cancellationToken);
}

internal sealed class RoleService : IRoleService
{
    private readonly IMapper _mapper;
    private readonly IRoleCommandService _roleCommandService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IValidator<RoleCreateDto> _createValidator;
    private readonly IRoleQueryService _roleQueryService;
    private readonly IEndpointQueryService _endpointQueryService;
    private readonly IValidator<RoleUpdateDto> _updateValidator;
    public RoleService(
        IMapper mapper,
        IRoleCommandService roleCommandService,
        IHttpContextAccessor httpContextAccessor,
        IValidator<RoleCreateDto> roleValidator,
        IRoleQueryService roleQueryService,
        IEndpointQueryService endpointQueryService,
        IValidator<RoleUpdateDto> updateRoleValidator)
    {
        _mapper = mapper;
        _mapper.NotNull(nameof(mapper));
        _roleCommandService = roleCommandService;
        _roleCommandService.NotNull(nameof(roleCommandService));
        _httpContextAccessor = httpContextAccessor;
        _httpContextAccessor.NotNull(nameof(_httpContextAccessor));
        _createValidator = roleValidator;
        _createValidator.NotNull(nameof(roleValidator));
        _roleQueryService = roleQueryService;
        _roleQueryService.NotNull(nameof(roleQueryService));
        _endpointQueryService = endpointQueryService;
        _endpointQueryService.NotNull(nameof(endpointQueryService));
        _updateValidator = updateRoleValidator;
        _updateValidator.NotNull(nameof(updateRoleValidator));
        _httpContextAccessor.NotNull(nameof(httpContextAccessor));
    }

    public async Task CreateAsync(RoleCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        } //

        Role role = _mapper.Map<Role>(createDto);
        if (createDto.SelectedEndpointIds is not null && createDto.SelectedEndpointIds.Any())
        {
            role.DefaultClaims = JsonOperation.Marshal(createDto.SelectedEndpointIds);
        }

        LogInfo logInfo = DeviceDetection.GetLogInfo(_httpContextAccessor.HttpContext.Request);
        role.InsertLogInfo = JsonOperation.Marshal(logInfo);
        await _roleCommandService.Add(role);
    }

    public async Task UpdateAsync(RoleUpdateDto updateDto, CancellationToken cancellationToken)
    {
        //var validationResult = await _roleValidator.ValidateAsync(updateDto, cancellationToken);
        //if (!validationResult.IsValid)
        //{
        //    var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
        //    throw new CustomValidationException(message);
        //}//
        Role role = await _roleQueryService.Get(updateDto.Id);
        List<string> endpointValue = await _endpointQueryService.GetAuthValue(updateDto.SelectedEndpointIds);
        _mapper.Map(updateDto, role);
        if (updateDto.SelectedEndpointIds is not null && endpointValue.Any() && updateDto.SelectedEndpointIds.Any())
        {
            role.DefaultClaims = JsonOperation.Marshal(updateDto.SelectedEndpointIds.Distinct());
        }
    }

    public async Task DeleteAsync(RoleDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        Role role = await _roleQueryService.Get(deleteDto.Id);
        if (role.IsRemovable)
        {
            throw new BaseException("این نقش غیرقابل حذف است"); //todo magic string
        }

        string removeLogInfo = LogInfoJson.Get(_httpContextAccessor.HttpContext.Request, true);
        _roleCommandService.Remove(role, removeLogInfo);
    }

    public async Task<RoleDisplayDto> GetSingleAsync(int id, CancellationToken cancellationToken)
    {
        Role role = await _roleQueryService.Get(id);
        RoleGetDto dto = _mapper.Map<RoleGetDto>(role);
        RoleDisplayDto roleDisplayDto = new();
        roleDisplayDto.RoleInfo = dto;
        if (role.DefaultClaims is not null && role.DefaultClaims.Any())
        {
            var endpointIds = JsonOperation.Unmarshal<int[]>(role.DefaultClaims);
            roleDisplayDto.AccessTree = await GetSingleCreateAccessTree(endpointIds);
        }
        else
        {
            roleDisplayDto.AccessTree = await GetSingleCreateAccessTree(new List<int>());
        }

        return roleDisplayDto;
    }

    private async Task<AccessTreeValueKeyDto> GetSingleCreateAccessTree(ICollection<int> endpointIds)
    {
        ICollection<Endpoint> endpoints = await _endpointQueryService.GetIncludeAll();
        AccessTreeValueKeyDto accessTree = endpoints.CreateAccessTree(endpointIds);
        return accessTree;
    }

    public async Task<ICollection<RoleGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<Role> role = await _roleQueryService.Get();
        return _mapper.Map<ICollection<RoleGetDto>>(role);
    }

    public async Task<RoleParamsOfCreateDto> GetCreateParamsAsync(CancellationToken cancellationToken)
    {
        AccessTreeValueKeyDto accessTree = await GetCreateParamsCreateAccessTree();
        RoleParamsOfCreateDto roleParamsOfCreateDto = new(accessTree);
        return roleParamsOfCreateDto;
    }

    private async Task<AccessTreeValueKeyDto> GetCreateParamsCreateAccessTree()
    {
        ICollection<Endpoint> endpoints = await _endpointQueryService.GetIncludeAll();
        AccessTreeValueKeyDto accessTree = endpoints.CreateAccessTree();
        return accessTree;
    }
}
