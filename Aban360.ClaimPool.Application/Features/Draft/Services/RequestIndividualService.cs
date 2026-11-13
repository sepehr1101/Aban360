using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Services;

public interface IRequestIndividualService
{
    Task UpdateAsync(IAppUser currentUser, IndividualRequestUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(IndividualRequestDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(IAppUser currentUser, IndividualRequestCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class RequestIndividualService : IRequestIndividualService
{
    private readonly IMapper _mapper;
    private readonly IRequestIndividualQueryService _requestIndividualQueryService;
    private readonly IRequestIndividualEstateCommandService _requestIndividualEstateCommandService;
    private readonly IRequestIndividualTagCommandService _requestIndividualTagCommandService;
    private readonly IValidator<IndividualRequestUpdateDto> _updateValidator;
    private readonly IRequestIndividualCommandService _requestIndividualCommandService;
    private readonly IRequestIndividualEstateQueryService _requestIndividualEstateQueryService;
    private readonly IRequestIndividualTagQueryService _requestIndividualTagQueryService;
    private readonly IValidator<IndividualRequestCreateDto> _createValidator;

    public RequestIndividualService(IMapper mapper, IRequestIndividualQueryService requestIndividualQueryService, IRequestIndividualEstateCommandService requestIndividualEstateCommandService, IRequestIndividualTagCommandService requestIndividualTagCommandService, IValidator<IndividualRequestUpdateDto> updateValidator, IRequestIndividualCommandService requestIndividualCommandService, IRequestIndividualEstateQueryService requestIndividualEstateQueryService, IRequestIndividualTagQueryService requestIndividualTagQueryService, IValidator<IndividualRequestCreateDto> createValidator)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        requestIndividualQueryService.NotNull(nameof(requestIndividualQueryService));
        _requestIndividualQueryService = requestIndividualQueryService;
        requestIndividualEstateCommandService.NotNull(nameof(requestIndividualEstateCommandService));
        _requestIndividualEstateCommandService = requestIndividualEstateCommandService;
        requestIndividualTagCommandService.NotNull(nameof(requestIndividualTagCommandService));
        _requestIndividualTagCommandService = requestIndividualTagCommandService;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
        requestIndividualCommandService.NotNull(nameof(requestIndividualCommandService));
        _requestIndividualCommandService = requestIndividualCommandService;
        requestIndividualEstateQueryService.NotNull(nameof(requestIndividualEstateQueryService));
        _requestIndividualEstateQueryService = requestIndividualEstateQueryService;
        requestIndividualTagQueryService.NotNull(nameof(requestIndividualTagQueryService));
        _requestIndividualTagQueryService = requestIndividualTagQueryService;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
    }

    public async Task UpdateAsync(IAppUser currentUser, IndividualRequestUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        var requestIndividual = await _requestIndividualQueryService.Get(updateDto.Id);
        requestIndividual.Hash = "-";
        requestIndividual.InsertLogInfo = "-";
        requestIndividual.ValidFrom = DateTime.Now;
        requestIndividual.UserId = currentUser.UserId;
    
        _mapper.Map(updateDto, requestIndividual);
    }

    public async Task DeleteAsync(IndividualRequestDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var requestIndividual = await _requestIndividualQueryService.Get(deleteDto.Id);
        _requestIndividualCommandService.Remove(requestIndividual);
    }

    public async Task CreateAsync(IAppUser currentUser, IndividualRequestCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        var requestIndividual = _mapper.Map<RequestIndividual>(createDto);
        requestIndividual.Hash = "-";
        requestIndividual.InsertLogInfo = "-";
        requestIndividual.ValidFrom = DateTime.Now;
        requestIndividual.UserId=currentUser.UserId;
    
        RequestIndividualEstate requestIndividualEstate = new RequestIndividualEstate()
        {
            RequestIndividual = requestIndividual,
            EstateId = createDto.EstateId,
            IndividualEstateRelationTypeId = createDto.IndividualEstateRelationTypeId,
        };
    
    
        createDto.TagIds.ForEach(tags =>
        {
            RequestIndividualTag requestIndividualTag = new RequestIndividualTag()
            {
                RequestIndividual = requestIndividual,
                IndividualTagDefinitionId = tags,
                Hash = "-",
                InsertLogInfo = "-",
                ValidFrom = DateTime.Now,
            };
            requestIndividual.IndividualTags.Add(requestIndividualTag);
        });
    
    
        await _requestIndividualCommandService.Add(requestIndividual);
        await _requestIndividualEstateCommandService.Add(requestIndividualEstate);
    }

}
