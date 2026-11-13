using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using IT9QueryService = Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts.IT9QueryService;
using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.CalculationPool.Application.Features.Bill.Services;

public interface ICompanyServiceService
{
    Task<CompanyServiceGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<IEnumerable<NumericDictionary>> GetByTypeIdAsync(int typeId, CancellationToken cancellationToken);
    Task<ICollection<CompanyServiceGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(CompanyServiceUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(CompanyServiceDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(CompanyServiceCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class CompanyServiceService : ICompanyServiceService
{
    private readonly IMapper _mapper;
    private readonly ICompanyServiceQueryService _queries;
    private readonly IT9QueryService _t9QueryService;
    private readonly IValidator<CompanyServiceUpdateDto> _updateValidator;
    private readonly ICompanyServiceCommandService _commands;
    private readonly IValidator<CompanyServiceCreateDto> _createValidator;

    public CompanyServiceService(IMapper mapper, ICompanyServiceQueryService queries, IT9QueryService t9QueryService, IValidator<CompanyServiceUpdateDto> updateValidator, ICompanyServiceCommandService commands, IValidator<CompanyServiceCreateDto> createValidator)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        queries.NotNull(nameof(queries));
        _queries = queries;
        t9QueryService.NotNull(nameof(t9QueryService));
        _t9QueryService = t9QueryService;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
        commands.NotNull(nameof(commands));
        _commands = commands;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
    }

    public async Task<CompanyServiceGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        CompanyService companyService = await _queries.Get(id);
        return _mapper.Map<CompanyServiceGetDto>(companyService);
    }

    public async Task<IEnumerable<NumericDictionary>> GetByTypeIdAsync(int typeId, CancellationToken cancellationToken)
    {
        return await _t9QueryService.GetByTypeId(typeId);
    }

    public async Task<ICollection<CompanyServiceGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<CompanyService> companyService = await _queries.Get();
        return _mapper.Map<ICollection<CompanyServiceGetDto>>(companyService);
    }

    public async Task UpdateAsync(CompanyServiceUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        CompanyService companyService = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, companyService);
    }

    public async Task DeleteAsync(CompanyServiceDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        CompanyService companyService = await _queries.Get(deleteDto.Id);
        await _commands.Remove(companyService);
    }

    public async Task CreateAsync(CompanyServiceCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        CompanyService companyService = _mapper.Map<CompanyService>(createDto);
        await _commands.Add(companyService);
    }
}
