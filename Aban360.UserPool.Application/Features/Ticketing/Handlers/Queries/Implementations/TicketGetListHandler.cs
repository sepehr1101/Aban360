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
    internal sealed class TicketGetListHandler : ITicketGetListHandler
    {
        private readonly IMapper _mapper;
        private readonly ITicketQueryService _ticketQueryService;
        private readonly ITicketAccessService _ticketAccessService;
        private readonly IValidator<TicketSearchInput> _validator;
        public TicketGetListHandler(
            IMapper mapper,
            ITicketQueryService ticketQueryService,
            ITicketAccessService ticketAccessService,
            IValidator<TicketSearchInput> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));
            _ticketQueryService = ticketQueryService;
            _ticketQueryService.NotNull(nameof(ticketQueryService));
            _ticketAccessService = ticketAccessService;
            _ticketAccessService.NotNull(nameof(ticketAccessService));
            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<TicketPage<TicketGetDto>> Handle(TicketSearchInput input, IAppUser currentUser, CancellationToken cancellationToken)
        {
            await TicketHandlerValidation.Validate(_validator, input, cancellationToken);
            var result = await _ticketQueryService.Get(_ticketAccessService.Scope(input, currentUser), cancellationToken);
            return new TicketPage<TicketGetDto>
            {
                Items = _mapper.Map<ICollection<TicketGetDto>>(result.Items),
                TotalCount = result.TotalCount, PageNumber = result.PageNumber, PageSize = result.PageSize
            };
        }
    }
}
