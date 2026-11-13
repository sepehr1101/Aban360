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

public interface IRequestEstateService
{
    Task UpdateAsync(IAppUser currentUser, EstateRequestUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(EstateRequestDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(IAppUser currentUser,EstateRequestCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class RequestEstateService : IRequestEstateService
{
    private readonly IMapper _mapper;
    private readonly IRequestEstateQueryService _queries;
    private readonly IValidator<EstateRequestUpdateDto> _updateValidator;
    private readonly IRequestEstateCommandService _commands;
    private readonly IValidator<EstateRequestCreateDto> _createValidator;

    public RequestEstateService(IMapper mapper, IRequestEstateQueryService queries, IValidator<EstateRequestUpdateDto> updateValidator, IRequestEstateCommandService commands, IValidator<EstateRequestCreateDto> createValidator)
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

    public async Task UpdateAsync(IAppUser currentUser, EstateRequestUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var requestEstate = await _queries.Get(updateDto.Id);
        requestEstate.UserId=currentUser.UserId;

        _mapper.Map(updateDto, requestEstate);
    }

    public async Task DeleteAsync(EstateRequestDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var requestEstate = await _queries.Get(deleteDto.Id);
        _commands.Remove(requestEstate);
    }

    public async Task CreateAsync(IAppUser currentUser,EstateRequestCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var requestEstate = _mapper.Map<RequestEstate>(createDto);
        requestEstate.Hash = "-";
        requestEstate.InsertLogInfo = "-";
        requestEstate.ValidFrom = DateTime.Now;
        requestEstate.UserId=currentUser.UserId;

        await _commands.Add(requestEstate);
    }
}
