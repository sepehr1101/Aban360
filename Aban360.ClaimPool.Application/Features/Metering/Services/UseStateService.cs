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

public interface IUseStateService
{
    Task<UseStateGetDto> GetSingleAsync(UseStateEnum id, CancellationToken cancellationToken);
    Task<ICollection<UseStateGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(UseStateUpdateDto updateDto, CancellationToken cancellationToken);
    Task CreateAsync(UseStateCreateDto createDto, CancellationToken cancellationToken);
    Task DeleteAsync(UseStateDeleteDto deleteDto, CancellationToken cancellationToken);
}

internal sealed class UseStateService : IUseStateService
{
    private readonly IMapper _mapper;
    private readonly IUseStateQueryService _useStateQueryService;
    private readonly IValidator<UseStateUpdateDto> _updateValidator;
    private readonly IUseStateCommandService _useStateCommandService;
    private readonly IValidator<UseStateCreateDto> _createValidator;

    public UseStateService(IMapper mapper, IUseStateQueryService useStateQueryService, IValidator<UseStateUpdateDto> updateValidator, IUseStateCommandService useStateCommandService, IValidator<UseStateCreateDto> createValidator)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        useStateQueryService.NotNull(nameof(useStateQueryService));
        _useStateQueryService = useStateQueryService;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
        useStateCommandService.NotNull(nameof(useStateCommandService));
        _useStateCommandService = useStateCommandService;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
    }

    public async Task<UseStateGetDto> GetSingleAsync(UseStateEnum id, CancellationToken cancellationToken)
    {
        UseState useState = await _useStateQueryService.Get(id);
        if (useState == null)
        {
            throw new InvalidDataException();
        }
        return _mapper.Map<UseStateGetDto>(useState);
    }

    public async Task<ICollection<UseStateGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<UseState> useState = await _useStateQueryService.Get();
        if (useState == null)
        {
            throw new InvalidDataException();
        }
        return _mapper.Map<ICollection<UseStateGetDto>>(useState);
    }

    public async Task UpdateAsync(UseStateUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        UseState useState = await _useStateQueryService.Get(updateDto.Id);
        _mapper.Map(updateDto, useState);
    }

    public async Task CreateAsync(UseStateCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        UseState useState = _mapper.Map<UseState>(createDto);
        await _useStateCommandService.Add(useState);
    }

    public async Task DeleteAsync(UseStateDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        UseState useState = await _useStateQueryService.Get(deleteDto.Id);
        if (useState == null)
        {
            throw new InvalidDataException();
        }
        await _useStateCommandService.Remove(useState);
    }

}
