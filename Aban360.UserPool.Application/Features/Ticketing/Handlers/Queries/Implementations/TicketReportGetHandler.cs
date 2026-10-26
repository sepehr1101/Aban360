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
    internal sealed class TicketReportGetHandler : ITicketReportGetHandler
    {
        private readonly ITicketReportQueryService _ticketReportQueryService;
        private readonly ITicketAccessService _ticketAccessService;
        private readonly IValidator<TicketSearchInput> _validator;
        public TicketReportGetHandler(
            ITicketReportQueryService ticketReportQueryService,
            ITicketAccessService ticketAccessService,
            IValidator<TicketSearchInput> validator)
        {
            _ticketReportQueryService = ticketReportQueryService;
            _ticketReportQueryService.NotNull(nameof(ticketReportQueryService));
            _ticketAccessService = ticketAccessService;
            _ticketAccessService.NotNull(nameof(ticketAccessService));
            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<TicketReportOutput> Handle(TicketSearchInput input, IAppUser currentUser, CancellationToken cancellationToken)
        {
            await TicketHandlerValidation.Validate(_validator, input, cancellationToken);
            return await _ticketReportQueryService.Get(_ticketAccessService.Scope(input, currentUser), cancellationToken);
        }
    }
}
