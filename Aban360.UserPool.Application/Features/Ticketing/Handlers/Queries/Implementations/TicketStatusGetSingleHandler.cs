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
    internal sealed class TicketStatusGetSingleHandler : ITicketStatusGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly ITicketStatusQueryService _ticketStatusQueryService;
        public TicketStatusGetSingleHandler(
            IMapper mapper,
            ITicketStatusQueryService ticketStatusQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));
            _ticketStatusQueryService = ticketStatusQueryService;
            _ticketStatusQueryService.NotNull(nameof(ticketStatusQueryService));
        }

        public async Task<TicketStatusGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            var status = await _ticketStatusQueryService.Get(id, cancellationToken) ?? throw new InvalidIdException();
            return _mapper.Map<TicketStatusGetDto>(status);
        }
    }
}
