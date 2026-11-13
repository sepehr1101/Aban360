using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.Metering.Services;

public interface IMeterDiameterService
{
    Task<MeterDiameterGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<MeterDiameterGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(MeterDiameterUpdateDto updateDto, CancellationToken cancellationToken);
    Task CreateAsync(MeterDiameterCreateDto createDto, CancellationToken cancellationToken);
    Task DeleteAsync(MeterDiameterDeleteDto deleteDto, CancellationToken cancellationToken);
}

internal sealed class MeterDiameterService : IMeterDiameterService
{
    private readonly IMapper _mapper;
    private readonly IMeterDiameterQueryService _meterDiameterQueryService;
    private readonly IValidator<MeterDiameterUpdateDto> _updateValidator;
    private readonly IMeterDiameterCommandService _meterDiameterCommandService;
    private readonly IValidator<MeterDiameterCreateDto> _createValidator;

    public MeterDiameterService(IMapper mapper, IMeterDiameterQueryService meterDiameterQueryService, IValidator<MeterDiameterUpdateDto> updateValidator, IMeterDiameterCommandService meterDiameterCommandService, IValidator<MeterDiameterCreateDto> createValidator)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        meterDiameterQueryService.NotNull(nameof(meterDiameterQueryService));
        _meterDiameterQueryService = meterDiameterQueryService;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
        meterDiameterCommandService.NotNull(nameof(meterDiameterCommandService));
        _meterDiameterCommandService = meterDiameterCommandService;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
    }

    public async Task<MeterDiameterGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        MeterDiameter meterDiameter = await _meterDiameterQueryService.Get(id);
        if (meterDiameter == null)
        {
            throw new InvalidDataException();
        }
        return _mapper.Map<MeterDiameterGetDto>(meterDiameter);
    }

    public async Task<ICollection<MeterDiameterGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<MeterDiameter> meterDiameter = await _meterDiameterQueryService.Get();
        if (meterDiameter == null)
        {
            throw new InvalidDataException();
        }
        return _mapper.Map<ICollection<MeterDiameterGetDto>>(meterDiameter);
    }

    public async Task UpdateAsync(MeterDiameterUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        MeterDiameter meterDiameter = await _meterDiameterQueryService.Get(updateDto.Id);
        _mapper.Map(updateDto, meterDiameter);
    }

    public async Task CreateAsync(MeterDiameterCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        MeterDiameter meterDiameter = _mapper.Map<MeterDiameter>(createDto);
        await _meterDiameterCommandService.Add(meterDiameter);
    }

    public async Task DeleteAsync(MeterDiameterDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        MeterDiameter meterDiameter = await _meterDiameterQueryService.Get(deleteDto.Id);
        if (meterDiameter == null)
        {
            throw new InvalidDataException();
        }
        await _meterDiameterCommandService.Remove(meterDiameter);
    
    }

}
