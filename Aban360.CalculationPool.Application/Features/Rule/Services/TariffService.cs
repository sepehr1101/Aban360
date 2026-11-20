using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.CalculationPool.Domain.Features.Rule.Entties;
using Aban360.CalculationPool.Persistence.Features.Rule.Commands.Contracts;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentMigrator.Runner.Generators;
using FluentValidation;
using System.Formats.Tar;

namespace Aban360.CalculationPool.Application.Features.Rule.Services;
public interface ITariffService
{
    Task TariffCreateAsync(TariffCreateDto createDto, CancellationToken cancellationToken);
    Task TariffDuplicateAsync(TariffDuplicateDto createDto, CancellationToken cancellationToken);
    Task TariffDeleteAsync(TariffDeleteDto deleteDto, CancellationToken cancellationToken);
    Task TariffUpdateAsync(TariffUpdateDto updateDto, CancellationToken cancellationToken);
    Task<ICollection<TariffGetDto>> TariffGetAllAsync(CancellationToken cancellationToken);
    Task<ICollection<TariffGetDto>> TariffGetByLineItemTypeIdAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<TariffGetDto>> TariffGetByOfferingIdAsync(short id, CancellationToken cancellationToken);
    Task<TariffGetDto> TariffGetSingleAsync(int id, CancellationToken cancellationToken);
}

internal sealed class TariffService : ITariffService
{
    private readonly IMapper _mapper;
    private readonly ITariffCommandService _tariffCommandService;
    private readonly IValidator<TariffCreateDto> _validator;
    private readonly ITariffQueryService _tariffQueryService;
    private readonly IValidator<TariffDuplicateDto> _tariffDuplicatevalidator;
    private readonly IValidator<TariffUpdateDto> _tariffUpdatevalidator;
    public TariffService(IMapper mapper, ITariffCommandService tariffCommandService, IValidator<TariffCreateDto> validator, ITariffQueryService tariffQueryService, IValidator<TariffDuplicateDto> tariffDuplicatevalidator, IValidator<TariffUpdateDto> tariffUpdatevalidator)
    {
        _mapper = mapper;
        _mapper.NotNull(nameof(mapper));
        _tariffCommandService = tariffCommandService;
        _tariffCommandService.NotNull(nameof(tariffCommandService));
        _validator = validator;
        _validator.NotNull(nameof(validator));
        _tariffQueryService = tariffQueryService;
        _tariffQueryService.NotNull(nameof(tariffQueryService));
        _tariffDuplicatevalidator = tariffDuplicatevalidator;
        _tariffDuplicatevalidator.NotNull(nameof(tariffDuplicatevalidator));
        _tariffUpdatevalidator = tariffUpdatevalidator;
        _tariffUpdatevalidator.NotNull(nameof(tariffUpdatevalidator));
    }

    public async Task TariffCreateAsync(TariffCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        Tariff tariff = _mapper.Map<Tariff>(createDto);
        await _tariffCommandService.Add(tariff);
    }

    public async Task TariffDuplicateAsync(TariffDuplicateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _tariffDuplicatevalidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        Tariff oldtTariff = await _tariffQueryService.Get(createDto.Id);
        Tariff newTariff = new Tariff()
        {
            Title = createDto.Title,
            LineItemTypeId = oldtTariff.LineItemTypeId,
            OfferingId = oldtTariff.OfferingId,
            Duration = oldtTariff.Duration,
            Condition = oldtTariff.Condition,
            Consumption = oldtTariff.Consumption,
            Description = oldtTariff.Description,
            Formula = oldtTariff.Formula,
            FromDateJalali = oldtTariff.FromDateJalali,
            ToDateJalali = oldtTariff.ToDateJalali
        };
        await _tariffCommandService.Add(newTariff);
    }

    public async Task TariffDeleteAsync(TariffDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        Tariff tariff = await _tariffQueryService.Get(deleteDto.Id);
        await _tariffCommandService.Remove(tariff);
    }

    public async Task TariffUpdateAsync(TariffUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _tariffUpdatevalidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        Tariff tariff = await _tariffQueryService.Get(updateDto.Id);
        _mapper.Map(updateDto, tariff);
    }

    public async Task<ICollection<TariffGetDto>> TariffGetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<Tariff> tariff = await _tariffQueryService.Get();
        return _mapper.Map<ICollection<TariffGetDto>>(tariff);
    }

    public async Task<ICollection<TariffGetDto>> TariffGetByLineItemTypeIdAsync(short id, CancellationToken cancellationToken)
    {
        ICollection<Tariff> tariff = await _tariffQueryService.GetByLineItemType(id);
        return _mapper.Map<ICollection<TariffGetDto>>(tariff);
    }

    public async Task<ICollection<TariffGetDto>> TariffGetByOfferingIdAsync(short id, CancellationToken cancellationToken)
    {
        ICollection<Tariff> tariff = await _tariffQueryService.GetByOfferingId(id);
        return _mapper.Map<ICollection<TariffGetDto>>(tariff);
    }

    public async Task<TariffGetDto> TariffGetSingleAsync(int id, CancellationToken cancellationToken)
    {
        Tariff tariff = await _tariffQueryService.Get(id);
        return _mapper.Map<TariffGetDto>(tariff);
    }
}
