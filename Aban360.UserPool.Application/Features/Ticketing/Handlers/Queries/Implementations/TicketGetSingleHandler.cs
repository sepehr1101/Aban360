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
    internal sealed class TicketGetSingleHandler : ITicketGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly ITicketAccessService _ticketAccessService;
        public TicketGetSingleHandler(
            IMapper mapper,
            ITicketAccessService ticketAccessService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));
            _ticketAccessService = ticketAccessService;
            _ticketAccessService.NotNull(nameof(ticketAccessService));
        }

        public async Task<TicketGetDto> Handle(long id, IAppUser currentUser, CancellationToken cancellationToken)
        {
            var ticket = await _ticketAccessService.Get(id, currentUser, cancellationToken);
            return _mapper.Map<TicketGetDto>(ticket);
        }
    }
}
