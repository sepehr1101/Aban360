using Aban360.Common.ApplicationUser;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Exceptions;
using Aban360.UserPool.Application.Features.Ticketing.Handlers.Queries.Contracts;
using Aban360.UserPool.Application.Features.Ticketing.Services.Contracts;
using Aban360.UserPool.Application.Features.Ticketing.Services.Implementations;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Aban360.UserPool.Persistence.Features.Ticketing.Queries.Contracts;
using AutoMapper;
using FluentValidation;

namespace Aban360.UserPool.Application.Features.Ticketing.Handlers.Queries.Implementations
{
    internal sealed class TicketRatingGetByMessageHandler : ITicketRatingGetByMessageHandler
    {
        private readonly IMapper _mapper;
        private readonly ITicketRatingQueryService _ticketRatingQueryService;
        private readonly ITicketMessageQueryService _ticketMessageQueryService;
        private readonly ITicketAccessService _ticketAccessService;
        public TicketRatingGetByMessageHandler(
            IMapper mapper,
            ITicketRatingQueryService ticketRatingQueryService,
            ITicketMessageQueryService ticketMessageQueryService,
            ITicketAccessService ticketAccessService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));
            _ticketRatingQueryService = ticketRatingQueryService;
            _ticketRatingQueryService.NotNull(nameof(ticketRatingQueryService));
            _ticketMessageQueryService = ticketMessageQueryService;
            _ticketMessageQueryService.NotNull(nameof(ticketMessageQueryService));
            _ticketAccessService = ticketAccessService;
            _ticketAccessService.NotNull(nameof(ticketAccessService));
        }

        public async Task<TicketRatingGetDto?> Handle(long messageId, IAppUser currentUser, CancellationToken cancellationToken)
        {
            var message = await _ticketMessageQueryService.Get(messageId, cancellationToken) ?? throw new InvalidIdException();
            await _ticketAccessService.Get(message.TicketId, currentUser, cancellationToken);
            var rating = await _ticketRatingQueryService.GetByMessage(messageId, cancellationToken);
            return rating == null ? null : _mapper.Map<TicketRatingGetDto>(rating);
        }
    }
}
