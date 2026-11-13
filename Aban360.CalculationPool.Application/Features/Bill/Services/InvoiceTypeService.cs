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

public interface IInvoiceTypeService
{
    Task<InvoiceTypeGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<InvoiceTypeGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(InvoiceTypeUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(InvoiceTypeDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(InvoiceTypeCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class InvoiceTypeService : IInvoiceTypeService
{
    private readonly IMapper _mapper;
    private readonly IInvoiceTypeQueryService _queries;
    private readonly IValidator<InvoiceTypeUpdateDto> _updateValidator;
    private readonly IInvoiceTypeCommandService _commands;
    private readonly IValidator<InvoiceTypeCreateDto> _createValidator;

    public InvoiceTypeService(IMapper mapper, IInvoiceTypeQueryService queries, IValidator<InvoiceTypeUpdateDto> updateValidator, IInvoiceTypeCommandService commands, IValidator<InvoiceTypeCreateDto> createValidator)
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

    public async Task<InvoiceTypeGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        InvoiceType invoiceType = await _queries.Get(id);
        return _mapper.Map<InvoiceTypeGetDto>(invoiceType);
    }

    public async Task<ICollection<InvoiceTypeGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<InvoiceType> invoiceType = await _queries.Get();
        return _mapper.Map<ICollection<InvoiceTypeGetDto>>(invoiceType);
    }

    public async Task UpdateAsync(InvoiceTypeUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        InvoiceType invoiceType = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, invoiceType);
    }

    public async Task DeleteAsync(InvoiceTypeDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        InvoiceType invoiceType = await _queries.Get(deleteDto.Id);
        await _commands.Remove(invoiceType);
    }

    public async Task CreateAsync(InvoiceTypeCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        InvoiceType invoiceType = _mapper.Map<InvoiceType>(createDto);
        await _commands.Add(invoiceType);
    }
}
