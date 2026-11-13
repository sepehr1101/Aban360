using Aban360.CalculationPool.Domain.Constants;
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

public interface IInvoiceLineItemInsertModeService
{
    Task<InvoiceLineItemInsertModeGetDto> GetSingleAsync(InvoiceLineItemInsertModeEnum id, CancellationToken cancellationToken);
    Task<ICollection<InvoiceLineItemInsertModeGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(InvoiceLineItemInsertModeUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(InvoiceLineItemInsertModeDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(InvoiceLineItemInsertModeCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class InvoiceLineItemInsertModeService : IInvoiceLineItemInsertModeService
{
    private readonly IMapper _mapper;
    private readonly IInvoiceLineItemInsertModeQueryService _queries;
    private readonly IValidator<InvoiceLineItemInsertModeUpdateDto> _updateValidator;
    private readonly IInvoiceLineItemInsertModeCommandService _commands;
    private readonly IValidator<InvoiceLineItemInsertModeCreateDto> _createValidator;

    public InvoiceLineItemInsertModeService(IMapper mapper, IInvoiceLineItemInsertModeQueryService queries, IValidator<InvoiceLineItemInsertModeUpdateDto> updateValidator, IInvoiceLineItemInsertModeCommandService commands, IValidator<InvoiceLineItemInsertModeCreateDto> createValidator)
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

    public async Task<InvoiceLineItemInsertModeGetDto> GetSingleAsync(InvoiceLineItemInsertModeEnum id, CancellationToken cancellationToken)
    {
        InvoiceLineItemInsertMode invoiceLineItemInsertMode = await _queries.Get(id);
        return _mapper.Map<InvoiceLineItemInsertModeGetDto>(invoiceLineItemInsertMode);
    }

    public async Task<ICollection<InvoiceLineItemInsertModeGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<InvoiceLineItemInsertMode> invoiceLineItemInsertMode = await _queries.Get();
        return _mapper.Map<ICollection<InvoiceLineItemInsertModeGetDto>>(invoiceLineItemInsertMode);
    }

    public async Task UpdateAsync(InvoiceLineItemInsertModeUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        InvoiceLineItemInsertMode invoiceLineItemInsertMode = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, invoiceLineItemInsertMode);
    }

    public async Task DeleteAsync(InvoiceLineItemInsertModeDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        InvoiceLineItemInsertMode invoiceLineItemInsertMode = await _queries.Get(deleteDto.Id);
        await _commands.Remove(invoiceLineItemInsertMode);
    }

    public async Task CreateAsync(InvoiceLineItemInsertModeCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        InvoiceLineItemInsertMode invoiceLineItemInsertMode = _mapper.Map<InvoiceLineItemInsertMode>(createDto);
        await _commands.Add(invoiceLineItemInsertMode);
    }
}
