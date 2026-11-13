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

public interface IHandoverService
{
    Task<HandoverGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<HandoverGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(HandoverUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(HandoverDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(HandoverCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class HandoverService : IHandoverService
{
    private readonly IMapper _mapper;
    private readonly IHandoverQueryService _queries;
    private readonly IValidator<HandoverUpdateDto> _updateValidator;
    private readonly IHandoverCommandService _commands;
    private readonly IValidator<HandoverCreateDto> _createValidator;

    public HandoverService(IMapper mapper, IHandoverQueryService queries, IValidator<HandoverUpdateDto> updateValidator, IHandoverCommandService commands, IValidator<HandoverCreateDto> createValidator)
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

    public async Task<HandoverGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        var handover = await _queries.Get(id);
        return _mapper.Map<HandoverGetDto>(handover);
    }

    public async Task<ICollection<HandoverGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var handover = await _queries.Get();
        return _mapper.Map<ICollection<HandoverGetDto>>(handover);
    }

    public async Task UpdateAsync(HandoverUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var handover = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, handover);
    }

    public async Task DeleteAsync(HandoverDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var handover = await _queries.Get(deleteDto.Id);
        await _commands.Remove(handover);
    }

    public async Task CreateAsync(HandoverCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var handover = _mapper.Map<Handover>(createDto);
        await _commands.Add(handover);
    }
}
