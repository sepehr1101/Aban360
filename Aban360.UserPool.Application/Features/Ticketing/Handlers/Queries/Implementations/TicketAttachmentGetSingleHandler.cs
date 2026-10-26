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
    internal sealed class TicketAttachmentGetSingleHandler : ITicketAttachmentGetSingleHandler
    {
        private readonly ITicketAttachmentQueryService _ticketAttachmentQueryService;
        private readonly ITicketMessageQueryService _ticketMessageQueryService;
        private readonly ITicketAccessService _ticketAccessService;
        public TicketAttachmentGetSingleHandler(
            ITicketAttachmentQueryService ticketAttachmentQueryService,
            ITicketMessageQueryService ticketMessageQueryService,
            ITicketAccessService ticketAccessService)
        {
            _ticketAttachmentQueryService = ticketAttachmentQueryService;
            _ticketAttachmentQueryService.NotNull(nameof(ticketAttachmentQueryService));
            _ticketMessageQueryService = ticketMessageQueryService;
            _ticketMessageQueryService.NotNull(nameof(ticketMessageQueryService));
            _ticketAccessService = ticketAccessService;
            _ticketAccessService.NotNull(nameof(ticketAccessService));
        }

        public async Task<TicketAttachmentInfo> Handle(long id, IAppUser currentUser, CancellationToken cancellationToken)
        {
            var attachment = await _ticketAttachmentQueryService.GetInfo(id, cancellationToken) ?? throw new InvalidIdException();
            var message = await _ticketMessageQueryService.Get(attachment.TicketMessageId, cancellationToken) ?? throw new InvalidIdException();
            await _ticketAccessService.Get(message.TicketId, currentUser, cancellationToken);
            return attachment;
        }
    }
}
