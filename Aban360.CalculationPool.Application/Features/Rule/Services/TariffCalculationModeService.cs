using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.CalculationPool.Domain.Features.Rule.Entities;
using Aban360.CalculationPool.Persistence.Features.Rule.Commands.Contracts;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Rule.Services;

public interface ITariffCalculationModeService
{
    Task<TariffCalculationModeGetDto> GetSingleAsync(TariffCalculationModeEnum id, CancellationToken cancellationToken);
    Task<ICollection<TariffCalculationModeGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(TariffCalculationModeUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(TariffCalculationModeDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(TariffCalculationModeCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class TariffCalculationModeService : ITariffCalculationModeService
{
    private readonly IMapper _mapper;
    private readonly ITariffCalculationModeQueryService _queries;
    private readonly IValidator<TariffCalculationModeUpdateDto> _updateValidator;
    private readonly ITariffCalculationModeCommandService _commands;
    private readonly IValidator<TariffCalculationModeCreateDto> _createValidator;

    public TariffCalculationModeService(IMapper mapper, ITariffCalculationModeQueryService queries, IValidator<TariffCalculationModeUpdateDto> updateValidator, ITariffCalculationModeCommandService commands, IValidator<TariffCalculationModeCreateDto> createValidator)
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

    public async Task<TariffCalculationModeGetDto> GetSingleAsync(TariffCalculationModeEnum id, CancellationToken cancellationToken)
    {
        TariffCalculationMode tariffCalculationMode = await _queries.Get(id);
        return _mapper.Map<TariffCalculationModeGetDto>(tariffCalculationMode);
    }

    public async Task<ICollection<TariffCalculationModeGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<TariffCalculationMode> tariffCalculationMode = await _queries.Get();
        return _mapper.Map<ICollection<TariffCalculationModeGetDto>>(tariffCalculationMode);
    }

    public async Task UpdateAsync(TariffCalculationModeUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        TariffCalculationMode tariffCalculationMode = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, tariffCalculationMode);
    }

    public async Task DeleteAsync(TariffCalculationModeDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        TariffCalculationMode tariffCalculationMode = await _queries.Get(deleteDto.Id);
        await _commands.Remove(tariffCalculationMode);
    }

    public async Task CreateAsync(TariffCalculationModeCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        TariffCalculationMode tariffCalculationMode = _mapper.Map<TariffCalculationMode>(createDto);
        await _commands.Add(tariffCalculationMode);
    }
}
