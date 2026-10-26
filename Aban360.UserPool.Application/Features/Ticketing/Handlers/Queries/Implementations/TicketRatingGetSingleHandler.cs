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
    internal sealed class TicketRatingGetSingleHandler : ITicketRatingGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly ITicketRatingQueryService _ticketRatingQueryService;
        private readonly ITicketMessageQueryService _ticketMessageQueryService;
        private readonly ITicketAccessService _ticketAccessService;
        public TicketRatingGetSingleHandler(
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

        public async Task<TicketRatingGetDto> Handle(long id, IAppUser currentUser, CancellationToken cancellationToken)
        {
            var rating = await _ticketRatingQueryService.Get(id, cancellationToken) ?? throw new InvalidIdException();
            var message = await _ticketMessageQueryService.Get(rating.TicketMessageId, cancellationToken) ?? throw new InvalidIdException();
            await _ticketAccessService.Get(message.TicketId, currentUser, cancellationToken);
            return _mapper.Map<TicketRatingGetDto>(rating);
        }
    }
}
