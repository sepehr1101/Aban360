using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.CalculationPool.Application.Features.Bill.Services;

public interface ILineItemTypeGroupService
{
    Task<LineItemTypeGroupGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<LineItemTypeGroupGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(LineItemTypeGroupUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(LineItemTypeGroupDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(LineItemTypeGroupCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class LineItemTypeGroupService : ILineItemTypeGroupService
{
    private readonly IMapper _mapper;
    private readonly ILineItemTypeGroupQueryService _queries;
    private readonly IValidator<LineItemTypeGroupUpdateDto> _updateValidator;
    private readonly ILineItemTypeGroupCommandService _commands;
    private readonly IValidator<LineItemTypeGroupCreateDto> _createValidator;

    public LineItemTypeGroupService(IMapper mapper, ILineItemTypeGroupQueryService queries, IValidator<LineItemTypeGroupUpdateDto> updateValidator, ILineItemTypeGroupCommandService commands, IValidator<LineItemTypeGroupCreateDto> createValidator)
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

    public async Task<LineItemTypeGroupGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        LineItemTypeGroup lineItemTypeGroup = await _queries.Get(id);
        return _mapper.Map<LineItemTypeGroupGetDto>(lineItemTypeGroup);
    }

    public async Task<ICollection<LineItemTypeGroupGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<LineItemTypeGroup> lineItemTypeGroup = await _queries.Get();
        return _mapper.Map<ICollection<LineItemTypeGroupGetDto>>(lineItemTypeGroup);
    }

    public async Task UpdateAsync(LineItemTypeGroupUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        LineItemTypeGroup lineItemTypeGroup = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, lineItemTypeGroup);
    }

    public async Task DeleteAsync(LineItemTypeGroupDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        LineItemTypeGroup lineItemTypeGroup = await _queries.Get(deleteDto.Id);
        await _commands.Remove(lineItemTypeGroup);
    }

    public async Task CreateAsync(LineItemTypeGroupCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        LineItemTypeGroup lineItemTypeGroup = _mapper.Map<LineItemTypeGroup>(createDto);
        await _commands.Add(lineItemTypeGroup);
    }
}
