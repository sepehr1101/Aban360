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
    internal sealed class TicketMessageGetListHandler : ITicketMessageGetListHandler
    {
        private readonly IMapper _mapper;
        private readonly ITicketMessageQueryService _ticketMessageQueryService;
        private readonly ITicketAccessService _ticketAccessService;
        private readonly IValidator<TicketMessageSearchInput> _validator;
        public TicketMessageGetListHandler(
            IMapper mapper,
            ITicketMessageQueryService ticketMessageQueryService,
            ITicketAccessService ticketAccessService,
            IValidator<TicketMessageSearchInput> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));
            _ticketMessageQueryService = ticketMessageQueryService;
            _ticketMessageQueryService.NotNull(nameof(ticketMessageQueryService));
            _ticketAccessService = ticketAccessService;
            _ticketAccessService.NotNull(nameof(ticketAccessService));
            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<TicketPage<TicketMessageGetDto>> Handle(TicketMessageSearchInput input, IAppUser currentUser, CancellationToken cancellationToken)
        {
            await TicketHandlerValidation.Validate(_validator, input, cancellationToken);
            await _ticketAccessService.Get(input.TicketId, currentUser, cancellationToken);
            var result = await _ticketMessageQueryService.GetByTicket(input.TicketId, input.PageNumber, input.PageSize, cancellationToken);
            return new TicketPage<TicketMessageGetDto>
            {
                Items = _mapper.Map<ICollection<TicketMessageGetDto>>(result.Items),
                TotalCount = result.TotalCount, PageNumber = result.PageNumber, PageSize = result.PageSize
            };
        }
    }
}
