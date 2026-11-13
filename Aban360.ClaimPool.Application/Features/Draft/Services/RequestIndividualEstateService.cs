using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Services;

public interface IRequestIndividualEstateService
{
    Task UpdateAsync(IndividualEstateRequestUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(IndividualEstateRequestDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(IndividualEstateRequestCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class RequestIndividualEstateService : IRequestIndividualEstateService
{
    private readonly IMapper _mapper;
    private readonly IRequestIndividualEstateQueryService _queries;
    private readonly IValidator<IndividualEstateRequestUpdateDto> _updateValidator;
    private readonly IRequestIndividualEstateCommandService _commands;
    private readonly IValidator<IndividualEstateRequestCreateDto> _createValidator;

    public RequestIndividualEstateService(IMapper mapper, IRequestIndividualEstateQueryService queries, IValidator<IndividualEstateRequestUpdateDto> updateValidator, IRequestIndividualEstateCommandService commands, IValidator<IndividualEstateRequestCreateDto> createValidator)
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

    public async Task UpdateAsync(IndividualEstateRequestUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var requestIndividualEstate = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, requestIndividualEstate);
    }

    public async Task DeleteAsync(IndividualEstateRequestDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var requestIndividualEstate = await _queries.Get(deleteDto.Id);
        _commands.Remove(requestIndividualEstate);
    }

    public async Task CreateAsync(IndividualEstateRequestCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var requestIndividualEstate = _mapper.Map<RequestIndividualEstate>(createDto);

        await _commands.Add(requestIndividualEstate);
    }
}
