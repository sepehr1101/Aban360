using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Services;

public interface IRequestIndividualTagService
{
    Task UpdateAsync(IndividualTagRequestUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(IndividualTagRequestDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(IndividualTagRequestCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class RequestIndividualTagService : IRequestIndividualTagService
{
    private readonly IMapper _mapper;
    private readonly IRequestIndividualTagQueryService _queries;
    private readonly IValidator<IndividualTagRequestUpdateDto> _updateValidator;
    private readonly IRequestIndividualTagCommandService _commands;
    private readonly IValidator<IndividualTagRequestCreateDto> _createValidator;

    public RequestIndividualTagService(IMapper mapper, IRequestIndividualTagQueryService queries, IValidator<IndividualTagRequestUpdateDto> updateValidator, IRequestIndividualTagCommandService commands, IValidator<IndividualTagRequestCreateDto> createValidator)
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

    public async Task UpdateAsync(IndividualTagRequestUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var requestIndividualTag = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, requestIndividualTag);
    }

    public async Task DeleteAsync(IndividualTagRequestDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var requestIndividualTag = await _queries.Get(deleteDto.Id);
        await _commands.Remove(requestIndividualTag);
    }

    public async Task CreateAsync(IndividualTagRequestCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var requestIndividualTag = _mapper.Map<RequestIndividualTag>(createDto);
        requestIndividualTag.Hash = "-";
        requestIndividualTag.InsertLogInfo = "-";
        requestIndividualTag.ValidFrom = DateTime.Now;

        await _commands.Add(requestIndividualTag);
    }
}
