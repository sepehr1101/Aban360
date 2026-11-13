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

public interface IMeterProducerService
{
    Task<MeterProducerGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<MeterProducerGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(MeterProducerUpdateDto updateDto, CancellationToken cancellationToken);
    Task CreateAsync(MeterProducerCreateDto createDto, CancellationToken cancellationToken);
    Task DeleteAsync(MeterProducerDeleteDto deleteDto, CancellationToken cancellationToken);
}

internal sealed class MeterProducerService : IMeterProducerService
{
    private readonly IMapper _mapper;
    private readonly IMeterProducerQueryService _meterProducerQueryService;
    private readonly IValidator<MeterProducerUpdateDto> _updateValidator;
    private readonly IMeterProducerCommandService _meterProducerCommandService;
    private readonly IValidator<MeterProducerCreateDto> _createValidator;

    public MeterProducerService(IMapper mapper, IMeterProducerQueryService meterProducerQueryService, IValidator<MeterProducerUpdateDto> updateValidator, IMeterProducerCommandService meterProducerCommandService, IValidator<MeterProducerCreateDto> createValidator)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        meterProducerQueryService.NotNull(nameof(meterProducerQueryService));
        _meterProducerQueryService = meterProducerQueryService;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
        meterProducerCommandService.NotNull(nameof(meterProducerCommandService));
        _meterProducerCommandService = meterProducerCommandService;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
    }

    public async Task<MeterProducerGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        var meterProducer = await _meterProducerQueryService.Get(id);
        if (meterProducer == null)
        {
            throw new InvalidDataException();
        }
        return _mapper.Map<MeterProducerGetDto>(meterProducer);
    }

    public async Task<ICollection<MeterProducerGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<MeterProducer> meterProducer = await _meterProducerQueryService.Get();
        if (meterProducer == null)
        {
            throw new InvalidDataException();
        }
        return _mapper.Map<ICollection<MeterProducerGetDto>>(meterProducer);
    }

    public async Task UpdateAsync(MeterProducerUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        MeterProducer meterProducer = await _meterProducerQueryService.Get(updateDto.Id);
        _mapper.Map(updateDto, meterProducer);
    }

    public async Task CreateAsync(MeterProducerCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        MeterProducer meterProducer = _mapper.Map<MeterProducer>(createDto);
        await _meterProducerCommandService.Add(meterProducer);
    }

    public async Task DeleteAsync(MeterProducerDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        MeterProducer meterProducer = await _meterProducerQueryService.Get(deleteDto.Id);
        if (meterProducer == null)
        {
            throw new InvalidDataException();
        }
        await _meterProducerCommandService.Remove(meterProducer);
    }

}
