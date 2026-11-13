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

public interface ISubscriptionTypeService
{
    Task<SubscriptionTypeGetDto> GetSingleAsync(SubscriptionTypeEnum id, CancellationToken cancellationToken);
    Task<ICollection<SubscriptionTypeGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(SubscriptionTypeUpdateDto updateDto, CancellationToken cancellationToken);
    Task CreateAsync(SubscriptionTypeCreateDto createDto, CancellationToken cancellationToken);
    Task DeleteAsync(SubscriptionTypeDeleteDto deleteDto, CancellationToken cancellationToken);
}

internal sealed class SubscriptionTypeService : ISubscriptionTypeService
{
    private readonly IMapper _mapper;
    private readonly ISubscriptionTypeQueryService _subscriptionTypeQueryService;
    private readonly IValidator<SubscriptionTypeUpdateDto> _updateValidator;
    private readonly ISubscriptionTypeCommandService _subscriptionTypeCommandService;
    private readonly IValidator<SubscriptionTypeCreateDto> _createValidator;

    public SubscriptionTypeService(IMapper mapper, ISubscriptionTypeQueryService subscriptionTypeQueryService, IValidator<SubscriptionTypeUpdateDto> updateValidator, ISubscriptionTypeCommandService subscriptionTypeCommandService, IValidator<SubscriptionTypeCreateDto> createValidator)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        subscriptionTypeQueryService.NotNull(nameof(subscriptionTypeQueryService));
        _subscriptionTypeQueryService = subscriptionTypeQueryService;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
        subscriptionTypeCommandService.NotNull(nameof(subscriptionTypeCommandService));
        _subscriptionTypeCommandService = subscriptionTypeCommandService;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
    }

    public async Task<SubscriptionTypeGetDto> GetSingleAsync(SubscriptionTypeEnum id, CancellationToken cancellationToken)
    {
        SubscriptionType subscriptionType = await _subscriptionTypeQueryService.Get(id);
        if (subscriptionType == null)
        {
            throw new InvalidDataException();
        }
        return _mapper.Map<SubscriptionTypeGetDto>(subscriptionType);
    }

    public async Task<ICollection<SubscriptionTypeGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<SubscriptionType> subscriptionType = await _subscriptionTypeQueryService.Get();
        if (subscriptionType == null)
        {
            throw new InvalidDataException();
        }
        return _mapper.Map<ICollection<SubscriptionTypeGetDto>>(subscriptionType);
    }

    public async Task UpdateAsync(SubscriptionTypeUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        SubscriptionType subscriptionType = await _subscriptionTypeQueryService.Get(updateDto.Id);
        _mapper.Map(updateDto, subscriptionType);
    }

    public async Task CreateAsync(SubscriptionTypeCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        SubscriptionType subscriptionType = _mapper.Map<SubscriptionType>(createDto);
        await _subscriptionTypeCommandService.Add(subscriptionType);
    }

    public async Task DeleteAsync(SubscriptionTypeDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        SubscriptionType subscriptionType = await _subscriptionTypeQueryService.Get(deleteDto.Id);
        await _subscriptionTypeCommandService.Remove(subscriptionType);
    }

}
