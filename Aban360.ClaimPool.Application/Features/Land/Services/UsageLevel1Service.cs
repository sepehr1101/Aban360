using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Services;

public interface IUsageLevel1Service
{
    Task<UsageLevel1GetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<UsageLevel1GetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(UsageLevel1UpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(UsageLevel1DeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(UsageLevel1CreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class UsageLevel1Service : IUsageLevel1Service
{
    private readonly IMapper _mapper;
    private readonly IUsageLevel1QueryService _queries;
    private readonly IValidator<UsageLevel1UpdateDto> _updateValidator;
    private readonly IUsageLevel1CommandService _commands;
    private readonly IValidator<UsageLevel1CreateDto> _createValidator;

    public UsageLevel1Service(IMapper mapper, IUsageLevel1QueryService queries, IValidator<UsageLevel1UpdateDto> updateValidator, IUsageLevel1CommandService commands, IValidator<UsageLevel1CreateDto> createValidator)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        queries.NotNull(nameof(queries));
        _queries = queries;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
        commands.NotNull(nameof(commands));
        _commands = commands;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
    }

    public async Task<UsageLevel1GetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        var usageLevel1 = await _queries.Get(id);
        return _mapper.Map<UsageLevel1GetDto>(usageLevel1);
    }

    public async Task<ICollection<UsageLevel1GetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var usageLevel1 = await _queries.Get();
        return _mapper.Map<ICollection<UsageLevel1GetDto>>(usageLevel1);
    }

    public async Task UpdateAsync(UsageLevel1UpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var usageLevel1 = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, usageLevel1);
    }

    public async Task DeleteAsync(UsageLevel1DeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var usageLevel1 = await _queries.Get(deleteDto.Id);
        await _commands.Remove(usageLevel1);
    }

    public async Task CreateAsync(UsageLevel1CreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var usageLevel1 = _mapper.Map<UsageLevel1>(createDto);
        await _commands.Add(usageLevel1);
    }
}
