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

public interface IInvoiceStatusService
{
    Task<InvoiceStatusGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<InvoiceStatusGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(InvoiceStatusUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(InvoiceStatusDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(InvoiceStatusCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class InvoiceStatusService : IInvoiceStatusService
{
    private readonly IMapper _mapper;
    private readonly IInvoiceStatusQueryService _queries;
    private readonly IValidator<InvoiceStatusUpdateDto> _updateValidator;
    private readonly IInvoiceStatusCommandService _commands;
    private readonly IValidator<InvoiceStatusCreateDto> _createValidator;

    public InvoiceStatusService(IMapper mapper, IInvoiceStatusQueryService queries, IValidator<InvoiceStatusUpdateDto> updateValidator, IInvoiceStatusCommandService commands, IValidator<InvoiceStatusCreateDto> createValidator)
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

    public async Task<InvoiceStatusGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        InvoiceStatus invoiceStatus = await _queries.Get(id);
        return _mapper.Map<InvoiceStatusGetDto>(invoiceStatus);
    }

    public async Task<ICollection<InvoiceStatusGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<InvoiceStatus> invoiceStatus = await _queries.Get();
        return _mapper.Map<ICollection<InvoiceStatusGetDto>>(invoiceStatus);
    }

    public async Task UpdateAsync(InvoiceStatusUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        InvoiceStatus invoiceStatus = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, invoiceStatus);
    }

    public async Task DeleteAsync(InvoiceStatusDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        InvoiceStatus invoiceStatus = await _queries.Get(deleteDto.Id);
        await _commands.Remove(invoiceStatus);
    }

    public async Task CreateAsync(InvoiceStatusCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        InvoiceStatus invoiceStatus = _mapper.Map<InvoiceStatus>(createDto);
        await _commands.Add(invoiceStatus);
    }
}
