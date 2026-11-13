using Aban360.ClaimPool.Domain.Constants;
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

public interface IMeterUseTypeService
{
    Task<MeterUseTypeGetDto> GetSingleAsync(MeterUseTypeEnum id, CancellationToken cancellationToken);
    Task<ICollection<MeterUseTypeGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(MeterUseTypeUpdateDto updateDto, CancellationToken cancellationToken);
    Task CreateAsync(MeterUseTypeCreateDto createDto, CancellationToken cancellationToken);
    Task DeleteAsync(MeterUseTypeDeleteDto deleteDto, CancellationToken cancellationToken);
}

internal sealed class MeterUseTypeService : IMeterUseTypeService
{
    private readonly IMapper _mapper;
    private readonly IMeterUseTypeQueryService _meterUseTypeQueryService;
    private readonly IValidator<MeterUseTypeUpdateDto> _updateValidator;
    private readonly IMeterUseTypeCommandService _meterUseTypeCommandService;
    private readonly IValidator<MeterUseTypeCreateDto> _createValidator;

    public MeterUseTypeService(IMapper mapper, IMeterUseTypeQueryService meterUseTypeQueryService, IValidator<MeterUseTypeUpdateDto> updateValidator, IMeterUseTypeCommandService meterUseTypeCommandService, IValidator<MeterUseTypeCreateDto> createValidator)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        meterUseTypeQueryService.NotNull(nameof(meterUseTypeQueryService));
        _meterUseTypeQueryService = meterUseTypeQueryService;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
        meterUseTypeCommandService.NotNull(nameof(meterUseTypeCommandService));
        _meterUseTypeCommandService = meterUseTypeCommandService;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
    }

    public async Task<MeterUseTypeGetDto> GetSingleAsync(MeterUseTypeEnum id, CancellationToken cancellationToken)
    {
        MeterUseType meterUseType = await _meterUseTypeQueryService.Get(id);
        if (meterUseType == null)
        {
            throw new InvalidDataException();
        }
        return _mapper.Map<MeterUseTypeGetDto>(meterUseType);
    }

    public async Task<ICollection<MeterUseTypeGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<MeterUseType> meterUseTypes = await _meterUseTypeQueryService.Get();
        if (meterUseTypes == null)
        {
            throw new InvalidDataException();
        }
        return _mapper.Map<ICollection<MeterUseTypeGetDto>>(meterUseTypes);
    }

    public async Task UpdateAsync(MeterUseTypeUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        MeterUseType meterUseType = await _meterUseTypeQueryService.Get(updateDto.Id);
        _mapper.Map(updateDto, meterUseType);
    }

    public async Task CreateAsync(MeterUseTypeCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        MeterUseType meterUseType = _mapper.Map<MeterUseType>(createDto);
        await _meterUseTypeCommandService.Add(meterUseType);
    }

    public async Task DeleteAsync(MeterUseTypeDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        MeterUseType meterUseType = await _meterUseTypeQueryService.Get(deleteDto.Id);
        if (meterUseType == null)
        {
            throw new InvalidDataException();
        }
        await _meterUseTypeCommandService.Remove(meterUseType);
    }

}
