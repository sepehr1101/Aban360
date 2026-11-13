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

public interface IMeterTypeService
{
    Task<MeterTypeGetDto> GetSingleAsync(short id,CancellationToken cancellationToken);
    Task<ICollection<MeterTypeGetDto>> GetAllAsync( CancellationToken cancellationToken);
    Task UpdateAsync(MeterTypeUpdateDto updateDto, CancellationToken cancellationToken);
    Task CreateAsync(MeterTypeCreateDto createDto, CancellationToken cancellationToken);
    Task DeleteAsync(MeterTypeDeleteDto deleteDto, CancellationToken cancellationToken);
}

internal sealed class MeterTypeService : IMeterTypeService
{
    private readonly IMapper _mapper;
    private readonly IMeterTypeQueryService _meterTypeQueryService;
    private readonly IValidator<MeterTypeUpdateDto> _updateValidator;
    private readonly IMeterTypeCommandService _meterTypeCommandService;
    private readonly IValidator<MeterTypeCreateDto> _createValidator;

    public MeterTypeService(IMapper mapper, IMeterTypeQueryService meterTypeQueryService, IValidator<MeterTypeUpdateDto> updateValidator, IMeterTypeCommandService meterTypeCommandService, IValidator<MeterTypeCreateDto> createValidator)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        meterTypeQueryService.NotNull(nameof(meterTypeQueryService));
        _meterTypeQueryService = meterTypeQueryService;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
        meterTypeCommandService.NotNull(nameof(meterTypeCommandService));
        _meterTypeCommandService = meterTypeCommandService;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
    }

    public async Task<MeterTypeGetDto> GetSingleAsync(short id,CancellationToken cancellationToken)
    {
        MeterType meterType = await _meterTypeQueryService.Get(id);
        if (meterType == null)
        {
            throw new InvalidDataException();
        }
        return _mapper.Map<MeterTypeGetDto>(meterType);
    }

    public async Task<ICollection<MeterTypeGetDto>> GetAllAsync( CancellationToken cancellationToken)
    {
        ICollection<MeterType> meterType = await _meterTypeQueryService.Get();
        if (meterType == null)
        {
            throw new InvalidDataException();
        }
        return _mapper.Map<ICollection<MeterTypeGetDto>>(meterType);
    }

    public async Task UpdateAsync(MeterTypeUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        MeterType meterType = await _meterTypeQueryService.Get(updateDto.Id);
        _mapper.Map(updateDto, meterType);
    }

    public async Task CreateAsync(MeterTypeCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        MeterType meterType = _mapper.Map<MeterType>(createDto);
        await _meterTypeCommandService.Add(meterType);
    }

    public async Task DeleteAsync(MeterTypeDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        MeterType meterType = await _meterTypeQueryService.Get(deleteDto.Id);
        if (meterType == null)
        {
            throw new InvalidDataException();
        }
        await _meterTypeCommandService.Remove(meterType);
    }

}
