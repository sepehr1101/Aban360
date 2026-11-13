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

public interface IUsageLevel3Service
{
    Task<UsageLevel3GetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<UsageLevel3GetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(UsageLevel3UpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(UsageLevel3DeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(UsageLevel3CreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class UsageLevel3Service : IUsageLevel3Service
{
    private readonly IMapper _mapper;
    private readonly IUsageLevel3QueryService _queries;
    private readonly IValidator<UsageLevel3UpdateDto> _updateValidator;
    private readonly IUsageLevel3CommandService _commands;
    private readonly IValidator<UsageLevel3CreateDto> _createValidator;

    public UsageLevel3Service(IMapper mapper, IUsageLevel3QueryService queries, IValidator<UsageLevel3UpdateDto> updateValidator, IUsageLevel3CommandService commands, IValidator<UsageLevel3CreateDto> createValidator)
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

    public async Task<UsageLevel3GetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        var usageLevel3 = await _queries.Get(id);
        return _mapper.Map<UsageLevel3GetDto>(usageLevel3);
    }

    public async Task<ICollection<UsageLevel3GetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var usageLevel3 = await _queries.Get();
        return _mapper.Map<ICollection<UsageLevel3GetDto>>(usageLevel3);
    }

    public async Task UpdateAsync(UsageLevel3UpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var usageLevel3 = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, usageLevel3);
    }

    public async Task DeleteAsync(UsageLevel3DeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var usageLevel3 = await _queries.Get(deleteDto.Id);
        await _commands.Remove(usageLevel3);
    }

    public async Task CreateAsync(UsageLevel3CreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var usageLevel3 = _mapper.Map<UsageLevel3>(createDto);
        await _commands.Add(usageLevel3);
    }
}
