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
    internal sealed class TicketMessageGetSingleHandler : ITicketMessageGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly ITicketMessageQueryService _ticketMessageQueryService;
        private readonly ITicketAccessService _ticketAccessService;
        public TicketMessageGetSingleHandler(
            IMapper mapper,
            ITicketMessageQueryService ticketMessageQueryService,
            ITicketAccessService ticketAccessService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));
            _ticketMessageQueryService = ticketMessageQueryService;
            _ticketMessageQueryService.NotNull(nameof(ticketMessageQueryService));
            _ticketAccessService = ticketAccessService;
            _ticketAccessService.NotNull(nameof(ticketAccessService));
        }

        public async Task<TicketMessageGetDto> Handle(long id, IAppUser currentUser, CancellationToken cancellationToken)
        {
            var message = await _ticketMessageQueryService.Get(id, cancellationToken) ?? throw new InvalidIdException();
            await _ticketAccessService.Get(message.TicketId, currentUser, cancellationToken);
            return _mapper.Map<TicketMessageGetDto>(message);
        }
    }
}
