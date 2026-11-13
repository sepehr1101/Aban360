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

public interface ICompanyServiceOfferingService
{
    Task<CompanyServiceOfferingGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<CompanyServiceOfferingGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(CompanyServiceOfferingUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(CompanyServiceOfferingDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(CompanyServiceOfferingCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class CompanyServiceOfferingService : ICompanyServiceOfferingService
{
    private readonly IMapper _mapper;
    private readonly ICompanyServiceOfferingQueryService _companyServiceOfferingQueryService;
    private readonly IValidator<CompanyServiceOfferingUpdateDto> _updateValidator;
    private readonly ICompanyServiceOfferingCommandService _companyServiceOfferingCommandService;
    private readonly IValidator<CompanyServiceOfferingCreateDto> _createValidator;

    public CompanyServiceOfferingService(IMapper mapper, ICompanyServiceOfferingQueryService companyServiceOfferingQueryService, IValidator<CompanyServiceOfferingUpdateDto> updateValidator, ICompanyServiceOfferingCommandService companyServiceOfferingCommandService, IValidator<CompanyServiceOfferingCreateDto> createValidator)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        companyServiceOfferingQueryService.NotNull(nameof(companyServiceOfferingQueryService));
        _companyServiceOfferingQueryService = companyServiceOfferingQueryService;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
        companyServiceOfferingCommandService.NotNull(nameof(companyServiceOfferingCommandService));
        _companyServiceOfferingCommandService = companyServiceOfferingCommandService;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
    }

    public async Task<CompanyServiceOfferingGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        CompanyServiceOffering companyServiceOffering = await _companyServiceOfferingQueryService.Get(id);
        return _mapper.Map<CompanyServiceOfferingGetDto>(companyServiceOffering);
    }

    public async Task<ICollection<CompanyServiceOfferingGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<CompanyServiceOffering> companyServiceOffering = await _companyServiceOfferingQueryService.Get();
        return _mapper.Map<ICollection<CompanyServiceOfferingGetDto>>(companyServiceOffering);
    }

    public async Task UpdateAsync(CompanyServiceOfferingUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        CompanyServiceOffering companyServiceOffering = await _companyServiceOfferingQueryService.Get(updateDto.Id);
        _mapper.Map(updateDto, companyServiceOffering);
    }

    public async Task DeleteAsync(CompanyServiceOfferingDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        CompanyServiceOffering companyServiceOffering = await _companyServiceOfferingQueryService.Get(deleteDto.Id);
        await _companyServiceOfferingCommandService.Remove(companyServiceOffering);
    }

    public async Task CreateAsync(CompanyServiceOfferingCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        CompanyServiceOffering companyServiceOffering = _mapper.Map<CompanyServiceOffering>(createDto);
        await _companyServiceOfferingCommandService.Add(companyServiceOffering);
    }

}
