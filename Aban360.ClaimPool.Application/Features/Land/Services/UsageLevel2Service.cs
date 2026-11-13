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

public interface IUsageLevel2Service
{
    Task<UsageLevel2GetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<UsageLevel2GetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(UsageLevel2UpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(UsageLevel2DeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(UsageLevel2CreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class UsageLevel2Service : IUsageLevel2Service
{
    private readonly IMapper _mapper;
    private readonly IUsageLevel2QueryService _queries;
    private readonly IValidator<UsageLevel2UpdateDto> _updateValidator;
    private readonly IUsageLevel2CommandService _commands;
    private readonly IValidator<UsageLevel2CreateDto> _createValidator;

    public UsageLevel2Service(IMapper mapper, IUsageLevel2QueryService queries, IValidator<UsageLevel2UpdateDto> updateValidator, IUsageLevel2CommandService commands, IValidator<UsageLevel2CreateDto> createValidator)
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

    public async Task<UsageLevel2GetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        var usageLevel2 = await _queries.Get(id);
        return _mapper.Map<UsageLevel2GetDto>(usageLevel2);
    }

    public async Task<ICollection<UsageLevel2GetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var usageLevel2 = await _queries.Get();
        return _mapper.Map<ICollection<UsageLevel2GetDto>>(usageLevel2);
    }

    public async Task UpdateAsync(UsageLevel2UpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var usageLevel2 = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, usageLevel2);
    }

    public async Task DeleteAsync(UsageLevel2DeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var usageLevel2 = await _queries.Get(deleteDto.Id);
        await _commands.Remove(usageLevel2);
    }

    public async Task CreateAsync(UsageLevel2CreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var usageLevel2 = _mapper.Map<UsageLevel2>(createDto);
        await _commands.Add(usageLevel2);
    }
}
