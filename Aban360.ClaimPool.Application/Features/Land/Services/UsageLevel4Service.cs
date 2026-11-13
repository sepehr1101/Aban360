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

public interface IUsageLevel4Service
{
    Task<UsageLevel4GetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<UsageLevel4GetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(UsageLevel4UpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(UsageLevel4DeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(UsageLevel4CreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class UsageLevel4Service : IUsageLevel4Service
{
    private readonly IMapper _mapper;
    private readonly IUsageLevel4QueryService _queries;
    private readonly IValidator<UsageLevel4UpdateDto> _updateValidator;
    private readonly IUsageLevel4CommandService _commands;
    private readonly IValidator<UsageLevel4CreateDto> _createValidator;

    public UsageLevel4Service(IMapper mapper, IUsageLevel4QueryService queries, IValidator<UsageLevel4UpdateDto> updateValidator, IUsageLevel4CommandService commands, IValidator<UsageLevel4CreateDto> createValidator)
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

    public async Task<UsageLevel4GetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        var usageLevel4 = await _queries.Get(id);
        return _mapper.Map<UsageLevel4GetDto>(usageLevel4);
    }

    public async Task<ICollection<UsageLevel4GetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var usageLevel4 = await _queries.Get();
        return _mapper.Map<ICollection<UsageLevel4GetDto>>(usageLevel4);
    }

    public async Task UpdateAsync(UsageLevel4UpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var usageLevel4 = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, usageLevel4);
    }

    public async Task DeleteAsync(UsageLevel4DeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var usageLevel4 = await _queries.Get(deleteDto.Id);
        await _commands.Remove(usageLevel4);
    }

    public async Task CreateAsync(UsageLevel4CreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var usageLevel4 = _mapper.Map<UsageLevel4>(createDto);
        await _commands.Add(usageLevel4);
    }
}
