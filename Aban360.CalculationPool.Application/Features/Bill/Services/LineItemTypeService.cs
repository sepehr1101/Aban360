using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace Aban360.CalculationPool.Application.Features.Bill.Services;

public interface ILineItemTypeService
{
    Task<LineItemTypeGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<LineItemTypeGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(LineItemTypeUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(LineItemTypeDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(LineItemTypeCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class LineItemTypeService : ILineItemTypeService
{
    private readonly IMapper _mapper;
    private readonly ILineItemTypeQueryService _queries;
    private readonly IValidator<LineItemTypeUpdateDto> _updateValidator;
    private readonly ILineItemTypeCommandService _commands;
    private readonly IValidator<LineItemTypeCreateDto> _createValidator;

    public LineItemTypeService(IMapper mapper, ILineItemTypeQueryService queries, IValidator<LineItemTypeUpdateDto> updateValidator, ILineItemTypeCommandService commands, IValidator<LineItemTypeCreateDto> createValidator)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        queries.NotNull(nameof(queries));
        _queries = queries;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
        commands.NotNull(nameof(commands));
        _commands = commands;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
    }

    public async Task<LineItemTypeGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        LineItemType lineItemType = await _queries.Get(id);
        return _mapper.Map<LineItemTypeGetDto>(lineItemType);
    }

    public async Task<ICollection<LineItemTypeGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<LineItemType> lineItemType = await _queries.Get();
        return _mapper.Map<ICollection<LineItemTypeGetDto>>(lineItemType);
    }

    public async Task UpdateAsync(LineItemTypeUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        LineItemType lineItemType = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, lineItemType);
    }

    public async Task DeleteAsync(LineItemTypeDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        LineItemType lineItemType = await _queries.Get(deleteDto.Id);
        await _commands.Remove(lineItemType);
    }

    public async Task CreateAsync(LineItemTypeCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        LineItemType lineItemType = _mapper.Map<LineItemType>(createDto);
        await _commands.Add(lineItemType);
    }
}
