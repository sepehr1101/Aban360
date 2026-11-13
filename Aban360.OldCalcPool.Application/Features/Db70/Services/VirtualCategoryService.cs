using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Db70.Commands.Contracts;
using Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Db70.Services;

public interface IVirtualCategoryService
{
    Task<VirtualCategoryGetDto> GetSingleAsync(SearchShortInputDto input, CancellationToken cancellationToken);
    Task<IEnumerable<VirtualCategoryGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(VirtualCategoryUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(SearchShortInputDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(VirtualCategoryCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class VirtualCategoryService : IVirtualCategoryService
{
    private readonly IVirtualCategoryQueryService _virtualCategoryQueryService;
    private readonly IValidator<SearchShortInputDto> _validator;
    private readonly IVirtualCategoryCommandService _virtualCategoryCommandService;
    private readonly IValidator<VirtualCategoryUpdateDto> _updateValidator;
    private readonly IValidator<VirtualCategoryCreateDto> _createValidator;

    public VirtualCategoryService(IVirtualCategoryQueryService virtualCategoryQueryService, IValidator<SearchShortInputDto> validator, IVirtualCategoryCommandService virtualCategoryCommandService, IValidator<VirtualCategoryUpdateDto> updateValidator, IValidator<VirtualCategoryCreateDto> createValidator)
    {
        virtualCategoryQueryService.NotNull(nameof(virtualCategoryQueryService));
        _virtualCategoryQueryService = virtualCategoryQueryService;
        validator.NotNull(nameof(validator));
        _validator = validator;
        virtualCategoryCommandService.NotNull(nameof(virtualCategoryCommandService));
        _virtualCategoryCommandService = virtualCategoryCommandService;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
    }

    public async Task<VirtualCategoryGetDto> GetSingleAsync(SearchShortInputDto input, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(input, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        VirtualCategoryGetDto result = await _virtualCategoryQueryService.Get(input);
        return result;
    }

    public async Task<IEnumerable<VirtualCategoryGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<VirtualCategoryGetDto> result = await _virtualCategoryQueryService.Get();
        return result;
    }

    public async Task UpdateAsync(VirtualCategoryUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        await _virtualCategoryCommandService.Update(updateDto);
    }

    public async Task DeleteAsync(SearchShortInputDto deleteDto, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(deleteDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        await _virtualCategoryCommandService.Delete(deleteDto);
    }

    public async Task CreateAsync(VirtualCategoryCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        await _virtualCategoryCommandService.Create(createDto);
    }

}
