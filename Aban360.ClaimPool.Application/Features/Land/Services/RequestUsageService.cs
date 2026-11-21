using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.Land.Services;
public interface IRequestUsageService
{
    Task UsageCreateAsync(UsageCreateDto createDto, CancellationToken cancellationToken);
    Task UsageDeleteAsync(UsageDeleteDto deleteDto, CancellationToken cancellationToken);
    Task UsageUpdateAsync(UsageUpdateDto updateDto, CancellationToken cancellationToken);
    Task<ICollection<UsageGetDto>> UsageGetAllAsync(CancellationToken cancellationToken);
    Task<UsageGetDto> UsageGetSingleAsync(short id, CancellationToken cancellationToken);
}

internal sealed class RequestUsageService : IRequestUsageService
{
    private readonly IMapper _mapper;
    private readonly IUsageCommandSevice _usageCommandService;
    private readonly IValidator<UsageCreateDto> _validator;
    private readonly IUsageQuerySevice _usageQueryService;
    private readonly IValidator<UsageUpdateDto> _usageUpdatevalidator;
    public RequestUsageService(IMapper mapper, IUsageCommandSevice usageCommandService, IValidator<UsageCreateDto> validator, IUsageQuerySevice usageQueryService, IValidator<UsageUpdateDto> usageUpdatevalidator)
    {
        _mapper = mapper;
        _mapper.NotNull(nameof(mapper));
        _usageCommandService = usageCommandService;
        _usageCommandService.NotNull(nameof(usageCommandService));
        _validator = validator;
        _validator.NotNull(nameof(validator));
        _usageQueryService = usageQueryService;
        _usageQueryService.NotNull(nameof(usageQueryService));
        _usageUpdatevalidator = usageUpdatevalidator;
        _usageUpdatevalidator.NotNull(nameof(usageUpdatevalidator));
    }

    public async Task UsageCreateAsync(UsageCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        Usage usage = _mapper.Map<Usage>(createDto);
        await _usageCommandService.Add(usage);
    }

    public async Task UsageDeleteAsync(UsageDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        Usage usage = await _usageQueryService.Get(deleteDto.Id);
        await _usageCommandService.Remove(usage);
    }

    public async Task UsageUpdateAsync(UsageUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _usageUpdatevalidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        Usage usage = await _usageQueryService.Get(updateDto.Id);
        _mapper.Map(updateDto, usage);
    }

    public async Task<ICollection<UsageGetDto>> UsageGetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<Usage> usage = await _usageQueryService.Get();
        if (usage == null)
        {
            throw new InvalidDataException();
        }

        return _mapper.Map<ICollection<UsageGetDto>>(usage);
    }

    public async Task<UsageGetDto> UsageGetSingleAsync(short id, CancellationToken cancellationToken)
    {
        Usage usage = await _usageQueryService.Get(id);
        if (usage == null)
        {
            throw new InvalidDataException();
        }

        return _mapper.Map<UsageGetDto>(usage);
    }
}
