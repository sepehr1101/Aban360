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

public interface ICompanyServiceTypeService
{
    Task<CompanyServiceTypeGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<CompanyServiceTypeGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(CompanyServiceTypeUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(CompanyServiceTypeDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(CompanyServiceTypeCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class CompanyServiceTypeService : ICompanyServiceTypeService
{
    private readonly IMapper _mapper;
    private readonly ICompanyServiceTypeQueryService _queries;
    private readonly IValidator<CompanyServiceTypeUpdateDto> _updateValidator;
    private readonly ICompanyServiceTypeCommandService _commands;
    private readonly IValidator<CompanyServiceTypeCreateDto> _createValidator;

    public CompanyServiceTypeService(IMapper mapper, ICompanyServiceTypeQueryService queries, IValidator<CompanyServiceTypeUpdateDto> updateValidator, ICompanyServiceTypeCommandService commands, IValidator<CompanyServiceTypeCreateDto> createValidator)
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

    public async Task<CompanyServiceTypeGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        CompanyServiceType companyServiceType = await _queries.Get(id);
        return _mapper.Map<CompanyServiceTypeGetDto>(companyServiceType);
    }

    public async Task<ICollection<CompanyServiceTypeGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<CompanyServiceType> companyServiceType = await _queries.Get();
        return _mapper.Map<ICollection<CompanyServiceTypeGetDto>>(companyServiceType);
    }

    public async Task UpdateAsync(CompanyServiceTypeUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        CompanyServiceType companyServiceType = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, companyServiceType);
    }

    public async Task DeleteAsync(CompanyServiceTypeDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        CompanyServiceType companyServiceType = await _queries.Get(deleteDto.Id);
        await _commands.Remove(companyServiceType);
    }

    public async Task CreateAsync(CompanyServiceTypeCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        CompanyServiceType companyServiceType = _mapper.Map<CompanyServiceType>(createDto);
        await _commands.Add(companyServiceType);
    }
}
