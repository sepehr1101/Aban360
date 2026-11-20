using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.Extensions;
using DNTPersianUtils.Core;
using FluentValidation;
using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Implementations;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.Common.Exceptions;
using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Persistence.Features.Sale.Commands.Contracts;
using Aban360.Common.ApplicationUser;

namespace Aban360.CalculationPool.Application.Features.Sale.Services;
public interface IInstallationAndEquipmentService
{
    Task<IEnumerable<InstallationAndEquipmentOutputDto>> InstallationAndEquipmentGetAllAsync(CancellationToken cancellationToken);
    Task<InstallationAndEquipmentOutputDto> InstallationAndEquipmentGetAsync(SearchById input, CancellationToken cancellationToken);
    Task InstallationAndEquipmentCreateAsync(InstallationAndEquipmentCreateDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task InstallationAndEquipmentUpdateAsync(InstallationAndEquipmentUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task InstallationAndEquipmentDeleteAsync(SearchById inputDto, IAppUser appUser, CancellationToken cancellationToken);
}

internal sealed class InstallationAndEquipmentService : IInstallationAndEquipmentService
{
    private readonly IInstallationAndEquipmentQueryService _queryService;
    private readonly IValidator<SearchById> _validator;
    private readonly IInstallationAndEquipmentCommandService _commandService;
    private readonly IValidator<InstallationAndEquipmentCreateDto> _installationAndEquipmentCreateValidator;
    private readonly IValidator<InstallationAndEquipmentUpdateDto> _installationAndEquipmentUpdateValidator;
    public InstallationAndEquipmentService(IInstallationAndEquipmentQueryService queryService, IValidator<SearchById> validator, IInstallationAndEquipmentCommandService commandService, IValidator<InstallationAndEquipmentCreateDto> installationAndEquipmentCreateValidator, IValidator<InstallationAndEquipmentUpdateDto> installationAndEquipmentUpdateValidator)
    {
        _queryService = queryService;
        _queryService.NotNull(nameof(queryService));
        _validator = validator;
        _validator.NotNull(nameof(validator));
        _commandService = commandService;
        _commandService.NotNull(nameof(commandService));
        _installationAndEquipmentCreateValidator = installationAndEquipmentCreateValidator;
        _installationAndEquipmentCreateValidator.NotNull(nameof(installationAndEquipmentCreateValidator));
        _installationAndEquipmentUpdateValidator = installationAndEquipmentUpdateValidator;
        _installationAndEquipmentUpdateValidator.NotNull(nameof(installationAndEquipmentUpdateValidator));
    }

    public async Task<IEnumerable<InstallationAndEquipmentOutputDto>> InstallationAndEquipmentGetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<InstallationAndEquipmentOutputDto> result = await _queryService.Get(DateTime.Now.ToShortPersianDateString());
        return result;
    }

    public async Task<InstallationAndEquipmentOutputDto> InstallationAndEquipmentGetAsync(SearchById input, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(input, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        InstallationAndEquipmentOutputDto result = await _queryService.Get(input.Id, DateTime.Now.ToShortPersianDateString());
        return result;
    }

    public async Task InstallationAndEquipmentCreateAsync(InstallationAndEquipmentCreateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        var validationResult = await _installationAndEquipmentCreateValidator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        InstallationAndEquipmentInputDto installationAndEquipment = InstallationAndEquipmentCreateGetInstallationAndEquipment(inputDto, appUser.UserId);
        await _commandService.Create(installationAndEquipment);
    }

    private InstallationAndEquipmentInputDto InstallationAndEquipmentCreateGetInstallationAndEquipment(InstallationAndEquipmentCreateDto input, Guid userId)
    {
        return new InstallationAndEquipmentInputDto()
        {
            IsWater = input.IsWater,
            DiameterId = input.DiameterId,
            InstallationAmount = input.InstallationAmount,
            EquipmentAmount = input.EquipmentAmount,
            FromDateJalali = input.FromDateJalali,
            ToDateJalali = input.ToDateJalali,
            RegisterDateTime = DateTime.Now,
            RegisterByUserId = userId
        };
    }

    public async Task InstallationAndEquipmentUpdateAsync(InstallationAndEquipmentUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        var validationResult = await _installationAndEquipmentUpdateValidator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        InstallationAndEquipmentInputDto installationAndEquipmentCreate = InstallationAndEquipmentUpdateGetInstallationAndEquipment(inputDto, appUser.UserId);
        DeleteDto installationAndEquipmentDelete = new(inputDto.Id, DateTime.Now, appUser.UserId);
        await _commandService.Update(installationAndEquipmentCreate, installationAndEquipmentDelete);
    }

    private InstallationAndEquipmentInputDto InstallationAndEquipmentUpdateGetInstallationAndEquipment(InstallationAndEquipmentUpdateDto input, Guid userId)
    {
        return new InstallationAndEquipmentInputDto()
        {
            IsWater = input.IsWater,
            DiameterId = input.DiameterId,
            InstallationAmount = input.InstallationAmount,
            EquipmentAmount = input.EquipmentAmount,
            FromDateJalali = input.FromDateJalali,
            ToDateJalali = input.ToDateJalali,
            RegisterDateTime = DateTime.Now,
            RegisterByUserId = userId
        };
    }

    public async Task InstallationAndEquipmentDeleteAsync(SearchById inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var deleteDto = new DeleteDto(inputDto.Id, DateTime.Now, appUser.UserId);
        await _commandService.Delete(deleteDto);
    }
}
