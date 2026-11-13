using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.CalculationPool.Domain.Features.Rule.Entities;
using Aban360.CalculationPool.Persistence.Features.Rule.Commands.Contracts;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Rule.Services;

public interface ITariffConstantService
{
    Task<TariffConstantGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<TariffConstantGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<ICollection<StringDictionary>> GetDictionaryAsync(CancellationToken cancellationToken);
    Task UpdateAsync(TariffConstantUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(TariffConstantDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(TariffConstantCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class TariffConstantService : ITariffConstantService
{
    private readonly IMapper _mapper;
    private readonly ITariffConstantQueryService _tariffConstantQueryService;
    private readonly IValidator<TariffConstantUpdateDto> _updateValidator;
    private readonly ITariffConstantCommandService _tariffConstantCommandService;
    private readonly IValidator<TariffConstantCreateDto> _createValidator;

    public TariffConstantService(IMapper mapper, ITariffConstantQueryService tariffConstantQueryService, IValidator<TariffConstantUpdateDto> updateValidator, ITariffConstantCommandService tariffConstantCommandService, IValidator<TariffConstantCreateDto> createValidator)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        tariffConstantQueryService.NotNull(nameof(tariffConstantQueryService));
        _tariffConstantQueryService = tariffConstantQueryService;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
        tariffConstantCommandService.NotNull(nameof(tariffConstantCommandService));
        _tariffConstantCommandService = tariffConstantCommandService;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
    }

    public async Task<TariffConstantGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        TariffConstant tariffConstant = await _tariffConstantQueryService.Get(id);
        return _mapper.Map<TariffConstantGetDto>(tariffConstant);
    }

    public async Task<ICollection<TariffConstantGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<TariffConstant> tariffConstant = await _tariffConstantQueryService.Get();
        return _mapper.Map<ICollection<TariffConstantGetDto>>(tariffConstant);
    }

    public async Task<ICollection<StringDictionary>> GetDictionaryAsync(CancellationToken cancellationToken)
    {
        ICollection<StringDictionary> tariffConstant = await _tariffConstantQueryService.GetDictionary();
        return tariffConstant;
    }

    public async Task UpdateAsync(TariffConstantUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        TariffConstant tariffConstant = await _tariffConstantQueryService.Get(updateDto.Id);
        _mapper.Map(updateDto, tariffConstant);
    }

    public async Task DeleteAsync(TariffConstantDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        TariffConstant tariffConstant = await _tariffConstantQueryService.Get(deleteDto.Id);
        await _tariffConstantCommandService.Remove(tariffConstant);
    }

    public async Task CreateAsync(TariffConstantCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        TariffConstant tariffConstant = _mapper.Map<TariffConstant>(createDto);
        await _tariffConstantCommandService.Add(tariffConstant);
    }

}
