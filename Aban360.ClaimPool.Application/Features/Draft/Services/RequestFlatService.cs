using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Services;

public interface IRequestFlatService
{
    Task UpdateAsync(FlatRequestUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(FlatRequestDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(FlatRequestCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class RequestFlatService : IRequestFlatService
{
    private readonly IMapper _mapper;
    private readonly IRequestFlatQueryService _queries;
    private readonly IValidator<FlatRequestUpdateDto> _updateValidator;
    private readonly IRequestFlatCommandService _commands;
    private readonly IValidator<FlatRequestCreateDto> _createValidator;

    public RequestFlatService(IMapper mapper, IRequestFlatQueryService queries, IValidator<FlatRequestUpdateDto> updateValidator, IRequestFlatCommandService commands, IValidator<FlatRequestCreateDto> createValidator)
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

    public async Task UpdateAsync(FlatRequestUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var requestFlat = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, requestFlat);
    }

    public async Task DeleteAsync(FlatRequestDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var requestFlat = await _queries.Get(deleteDto.Id);
        await _commands.Remove(requestFlat);
    }

    public async Task CreateAsync(FlatRequestCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var requestFlat = _mapper.Map<RequestFlat>(createDto);
        await _commands.Add(requestFlat);
    }
}
