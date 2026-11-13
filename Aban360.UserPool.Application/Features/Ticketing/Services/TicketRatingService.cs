using Aban360.Common.ApplicationUser;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Exceptions;
using Aban360.UserPool.Application.Features.Ticketing.Services.Contracts;
using Aban360.UserPool.Application.Features.Ticketing.Services.Implementations;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Aban360.UserPool.Domain.Features.Ticketing.Entities;
using Aban360.UserPool.Persistence.Features.Ticketing.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.Ticketing.Queries.Contracts;
using AutoMapper;
using FluentValidation;

namespace Aban360.UserPool.Application.Features.Ticketing.Services;

public interface ITicketRatingService
{
    Task CreateAsync(TicketRatingCreateDto createDto, IAppUser currentUser, CancellationToken cancellationToken);
    Task UpdateAsync(TicketRatingUpdateDto updateDto, IAppUser currentUser, CancellationToken cancellationToken);
    Task<TicketRatingGetDto?> GetByMessageAsync(long messageId, IAppUser currentUser, CancellationToken cancellationToken);
    Task<TicketRatingGetDto> GetSingleAsync(long id, IAppUser currentUser, CancellationToken cancellationToken);
}

internal sealed class TicketRatingService : ITicketRatingService
{
    private readonly IMapper _mapper;
    private readonly ITicketRatingCommandService _ticketRatingCommandService;
    private readonly ITicketRatingQueryService _ticketRatingQueryService;
    private readonly ITicketMessageQueryService _ticketMessageQueryService;
    private readonly ITicketAccessService _ticketAccessService;
    private readonly IValidator<TicketRatingCreateDto> _createValidator;
    private readonly IValidator<TicketRatingUpdateDto> _updateValidator;

    public TicketRatingService(IMapper mapper, ITicketRatingCommandService ticketRatingCommandService, ITicketRatingQueryService ticketRatingQueryService, ITicketMessageQueryService ticketMessageQueryService, ITicketAccessService ticketAccessService, IValidator<TicketRatingCreateDto> createValidator, IValidator<TicketRatingUpdateDto> updateValidator)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        ticketRatingCommandService.NotNull(nameof(ticketRatingCommandService));
        _ticketRatingCommandService = ticketRatingCommandService;
        ticketRatingQueryService.NotNull(nameof(ticketRatingQueryService));
        _ticketRatingQueryService = ticketRatingQueryService;
        ticketMessageQueryService.NotNull(nameof(ticketMessageQueryService));
        _ticketMessageQueryService = ticketMessageQueryService;
        ticketAccessService.NotNull(nameof(ticketAccessService));
        _ticketAccessService = ticketAccessService;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
    }

    public async Task CreateAsync(TicketRatingCreateDto createDto, IAppUser currentUser, CancellationToken cancellationToken)
    {
        await TicketHandlerValidation.Validate(_createValidator, createDto, cancellationToken);
        var message = await _ticketMessageQueryService.Get(createDto.TicketMessageId, cancellationToken) ?? throw new InvalidIdException();
        var ticket = await _ticketAccessService.Get(message.TicketId, currentUser, cancellationToken);
        if (ticket.CreatedByUserId != currentUser.UserId || message.SenderUserId == ticket.CreatedByUserId)
            throw new CustomValidationException("??? ????????? ???? ???????? ?? ???? ????? ?????? ???.");
        if (await _ticketRatingQueryService.GetByMessage(message.Id, cancellationToken) != null)
            throw new CustomValidationException("???? ??? ???? ????? ?????? ??? ??? ???.");
        TicketRating rating = _mapper.Map<TicketRating>(createDto);
        rating.RatedByUserId = currentUser.UserId;
        rating.CreatedAt = DateTime.UtcNow;
        await _ticketRatingCommandService.Add(rating, cancellationToken);
    }

    public async Task UpdateAsync(TicketRatingUpdateDto updateDto, IAppUser currentUser, CancellationToken cancellationToken)
    {
        await TicketHandlerValidation.Validate(_updateValidator, updateDto, cancellationToken);
        var rating = await _ticketRatingQueryService.Get(updateDto.Id, cancellationToken) ?? throw new InvalidIdException();
        var message = await _ticketMessageQueryService.Get(rating.TicketMessageId, cancellationToken) ?? throw new InvalidIdException();
        var ticket = await _ticketAccessService.Get(message.TicketId, currentUser, cancellationToken);
        if (rating.RatedByUserId != currentUser.UserId || ticket.CreatedByUserId != currentUser.UserId
            || message.SenderUserId == ticket.CreatedByUserId)
            throw new CustomValidationException("??? ????????? ?????? ???? ?? ?????? ?? ???.");
        _mapper.Map(updateDto, rating);
    }

    public async Task<TicketRatingGetDto?> GetByMessageAsync(long messageId, IAppUser currentUser, CancellationToken cancellationToken)
    {
        var message = await _ticketMessageQueryService.Get(messageId, cancellationToken) ?? throw new InvalidIdException();
        await _ticketAccessService.Get(message.TicketId, currentUser, cancellationToken);
        var rating = await _ticketRatingQueryService.GetByMessage(messageId, cancellationToken);
        return rating == null ? null : _mapper.Map<TicketRatingGetDto>(rating);
    }

    public async Task<TicketRatingGetDto> GetSingleAsync(long id, IAppUser currentUser, CancellationToken cancellationToken)
    {
        var rating = await _ticketRatingQueryService.Get(id, cancellationToken) ?? throw new InvalidIdException();
        var message = await _ticketMessageQueryService.Get(rating.TicketMessageId, cancellationToken) ?? throw new InvalidIdException();
        await _ticketAccessService.Get(message.TicketId, currentUser, cancellationToken);
        return _mapper.Map<TicketRatingGetDto>(rating);
    }

}
