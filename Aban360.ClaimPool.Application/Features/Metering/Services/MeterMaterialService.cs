using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.Metering.Services;

public interface IMeterMaterialService
{
    Task<ICollection<MeterMaterialGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<MeterMaterialGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task UpdateAsync(MeterMaterialUpdateDto updateDto, CancellationToken cancellationToken);
    Task CreateAsync(MeterMaterialCreateDto createDto, CancellationToken cancellationToken);
    Task DeleteAsync(MeterMaterialDeleteDto deleteDto, CancellationToken cancellationToken);
}

internal sealed class MeterMaterialService : IMeterMaterialService
{
    private readonly IMapper _mapper;
    private readonly IMeterMaterialQueryService _meterMaterialQueryService;
    private readonly IValidator<MeterMaterialUpdateDto> _updateValidator;
    private readonly IMeterMaterialCommandService _meterMaterialCommandService;
    private readonly IValidator<MeterMaterialCreateDto> _createValidator;

    public MeterMaterialService(IMapper mapper, IMeterMaterialQueryService meterMaterialQueryService, IValidator<MeterMaterialUpdateDto> updateValidator, IMeterMaterialCommandService meterMaterialCommandService, IValidator<MeterMaterialCreateDto> createValidator)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        meterMaterialQueryService.NotNull(nameof(meterMaterialQueryService));
        _meterMaterialQueryService = meterMaterialQueryService;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
        meterMaterialCommandService.NotNull(nameof(meterMaterialCommandService));
        _meterMaterialCommandService = meterMaterialCommandService;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
    }

    public async Task<ICollection<MeterMaterialGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<MeterMaterial> meterMaterial = await _meterMaterialQueryService.Get();
        if (meterMaterial == null)
        {
            throw new InvalidDataException();
        }
        return _mapper.Map<ICollection<MeterMaterialGetDto>>(meterMaterial);
    }

    public async Task<MeterMaterialGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        MeterMaterial meterMaterial = await _meterMaterialQueryService.Get(id);
        if (meterMaterial == null)
        {
            throw new InvalidDataException();
        }
        return _mapper.Map<MeterMaterialGetDto>(meterMaterial);
    }

    public async Task UpdateAsync(MeterMaterialUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        MeterMaterial meterMaterial = await _meterMaterialQueryService.Get(updateDto.Id);
        _mapper.Map(updateDto, meterMaterial);
    }

    public async Task CreateAsync(MeterMaterialCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        MeterMaterial meterMaterial = _mapper.Map<MeterMaterial>(createDto);
        await _meterMaterialCommandService.Add(meterMaterial);
    }

    public async Task DeleteAsync(MeterMaterialDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        MeterMaterial meterMaterial = await _meterMaterialQueryService.Get(deleteDto.Id);
        if (meterMaterial == null)
        {
            throw new InvalidDataException();
        }
        await _meterMaterialCommandService.Remove(meterMaterial);
    }

}
