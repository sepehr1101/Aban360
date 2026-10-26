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
    internal sealed class TicketAttachmentContentGetHandler : ITicketAttachmentContentGetHandler
    {
        private readonly IMapper _mapper;
        private readonly ITicketAttachmentQueryService _ticketAttachmentQueryService;
        private readonly ITicketAttachmentGetSingleHandler _ticketAttachmentGetSingleHandler;
        public TicketAttachmentContentGetHandler(
            IMapper mapper,
            ITicketAttachmentQueryService ticketAttachmentQueryService,
            ITicketAttachmentGetSingleHandler ticketAttachmentGetSingleHandler)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));
            _ticketAttachmentQueryService = ticketAttachmentQueryService;
            _ticketAttachmentQueryService.NotNull(nameof(ticketAttachmentQueryService));
            _ticketAttachmentGetSingleHandler = ticketAttachmentGetSingleHandler;
            _ticketAttachmentGetSingleHandler.NotNull(nameof(ticketAttachmentGetSingleHandler));
        }

        public async Task<TicketAttachmentContentGetDto> Handle(long id, IAppUser currentUser, CancellationToken cancellationToken)
        {
            // Authorize from metadata before reading the potentially large file.
            await _ticketAttachmentGetSingleHandler.Handle(id, currentUser, cancellationToken);
            var attachment = await _ticketAttachmentQueryService.GetContent(id, cancellationToken) ?? throw new InvalidIdException();
            return _mapper.Map<TicketAttachmentContentGetDto>(attachment);
        }
    }
}
