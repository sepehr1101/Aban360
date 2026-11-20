using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.Extensions;
using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Implementations;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Persistence.Features.Sale.Commands.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Exceptions;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Sale.Services;
public interface ITankerWaterDistanceTariffService
{
    Task<IEnumerable<TankerWaterDistanceTariffOutputDto>> TankerWaterDistanceTariffGetAllAsync(CancellationToken cancellationToken);
    Task<TankerWaterDistanceTariffOutputDto> TankerWaterDistanceTariffGetAsync(SearchById input, CancellationToken cancellationToken);
    Task TankerWaterDistanceTariffCreateAsync(TankerWaterDistanceTariffInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task TankerWaterDistanceTariffUpdateAsync(TankerWaterDistanceTariffInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task TankerWaterDistanceTariffDeleteAsync(SearchById input, IAppUser appUser, CancellationToken cancellationToken);
}

internal sealed class TankerWaterDistanceTariffService : ITankerWaterDistanceTariffService
{
    private readonly ITankerWaterDistanceTariffQueryService _queryService;
    private readonly ITankerWaterDistanceTariffCommandService _commandService;
    private readonly IValidator<TankerWaterDistanceTariffInputDto> _validator;
    public TankerWaterDistanceTariffService(ITankerWaterDistanceTariffQueryService queryService, ITankerWaterDistanceTariffCommandService commandService, IValidator<TankerWaterDistanceTariffInputDto> validator)
    {
        _queryService = queryService;
        _queryService.NotNull(nameof(queryService));
        _commandService = commandService;
        _commandService.NotNull(nameof(commandService));
        _validator = validator;
        _validator.NotNull(nameof(validator));
    }

    public async Task<IEnumerable<TankerWaterDistanceTariffOutputDto>> TankerWaterDistanceTariffGetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<TankerWaterDistanceTariffOutputDto> result = await _queryService.Get();
        return result;
    }

    public async Task<TankerWaterDistanceTariffOutputDto> TankerWaterDistanceTariffGetAsync(SearchById input, CancellationToken cancellationToken)
    {
        TankerWaterDistanceTariffOutputDto result = await _queryService.Get(input.Id);
        return result;
    }

    public async Task TankerWaterDistanceTariffCreateAsync(TankerWaterDistanceTariffInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        TankerWaterDistanceTariffCreateDto tankerCreate = TankerWaterDistanceTariffCreateGetTankerWaterDistanceTariff(inputDto, appUser.UserId);
        await _commandService.Create(tankerCreate);
    }

    private TankerWaterDistanceTariffCreateDto TankerWaterDistanceTariffCreateGetTankerWaterDistanceTariff(TankerWaterDistanceTariffInputDto input, Guid userId)
    {
        return new TankerWaterDistanceTariffCreateDto()
        {
            FromDistance = input.FromDistance,
            ToDistance = input.ToDistance,
            Amount = input.Amount,
            FromDateJalali = input.FromDateJalali,
            ToDateJalali = input.ToDateJalali,
            RegisterDateTime = DateTime.Now,
            RegisterByUserId = userId
        };
    }

    public async Task TankerWaterDistanceTariffUpdateAsync(TankerWaterDistanceTariffInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        await _commandService.Update(inputDto);
    }

    public async Task TankerWaterDistanceTariffDeleteAsync(SearchById input, IAppUser appUser, CancellationToken cancellationToken)
    {
        DeleteDto deleteDto = new(input.Id, DateTime.Now, appUser.UserId);
        await _commandService.Delete(deleteDto);
    }
}
