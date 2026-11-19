using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Contracts;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Rules.Services;

public interface ITable1Service
{
    Task CreateAsync(Table1CreateDto CreateDto, CancellationToken cancellationToken);
    Task UpdateAsync(Table1UpdateDto UpdateDto, CancellationToken cancellationToken);
    Task<Table1GetDto> GetAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Table1GetDto>> GetAllAsync(CancellationToken cancellationToken);
}

internal sealed class Table1Service : ITable1Service
{
    private readonly ITable1CreateService _table1CreateService;
    private readonly IValidator<Table1CreateDto> _createValidator;
    private readonly ITable1UpdateService _table1UpdateService;
    private readonly IValidator<Table1UpdateDto> _updateValidator;
    private readonly ITable1GetService _table1GetService;
    private readonly ITable1GetAllService _table1GetAllService;
    public Table1Service(
        ITable1CreateService table1CreateService,
        IValidator<Table1CreateDto> table1CreateValidator,
        ITable1UpdateService table1UpdateService,
        IValidator<Table1UpdateDto> table1UpdateValidator,
        ITable1GetService table1GetService,
        ITable1GetAllService table1GetAllService)
    {
        _table1CreateService = table1CreateService;
        _table1CreateService.NotNull(nameof(table1CreateService));
        _createValidator = table1CreateValidator;
        _createValidator.NotNull(nameof(_createValidator));
        _table1UpdateService = table1UpdateService;
        _table1UpdateService.NotNull(nameof(table1UpdateService));
        _updateValidator = table1UpdateValidator;
        _updateValidator.NotNull(nameof(_updateValidator));
        _table1GetService = table1GetService;
        _table1GetService.NotNull(nameof(table1GetService));
        _table1GetAllService = table1GetAllService;
        _table1GetAllService.NotNull(nameof(table1GetAllService));
    }

    public async Task CreateAsync(Table1CreateDto CreateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(CreateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        await _table1CreateService.Create(CreateDto);
    }

    public async Task UpdateAsync(Table1UpdateDto UpdateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(UpdateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        await _table1UpdateService.Update(UpdateDto);
    }

    public async Task<Table1GetDto> GetAsync(int id, CancellationToken cancellationToken)
    {
        Table1GetDto result = await _table1GetService.Get(id);
        return result;
    }

    public async Task<IEnumerable<Table1GetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<Table1GetDto> result = await _table1GetAllService.Get();
        return result;
    }
}
