using Aban360.Common.ApplicationUser;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Exceptions;
using Aban360.UserPool.Application.Features.Ticketing.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Application.Features.Ticketing.Services.Contracts;
using Aban360.UserPool.Application.Features.Ticketing.Services.Implementations;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands;
using Aban360.UserPool.Persistence.Features.Ticketing.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.Ticketing.Queries.Contracts;
using AutoMapper;
using FluentValidation;

namespace Aban360.UserPool.Application.Features.Ticketing.Handlers.Commands.Update.Implementations
{
    internal sealed class TicketAttachmentUpdateHandler : ITicketAttachmentUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ITicketAttachmentQueryService _ticketAttachmentQueryService;
        private readonly ITicketMessageQueryService _ticketMessageQueryService;
        private readonly ITicketAttachmentCommandService _ticketAttachmentCommandService;
        private readonly ITicketAccessService _ticketAccessService;
        private readonly IValidator<TicketAttachmentUpdateDto> _validator;
        public TicketAttachmentUpdateHandler(
            IMapper mapper,
            ITicketAttachmentQueryService ticketAttachmentQueryService,
            ITicketMessageQueryService ticketMessageQueryService,
            ITicketAttachmentCommandService ticketAttachmentCommandService,
            ITicketAccessService ticketAccessService,
            IValidator<TicketAttachmentUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));
            _ticketAttachmentQueryService = ticketAttachmentQueryService;
            _ticketAttachmentQueryService.NotNull(nameof(ticketAttachmentQueryService));
            _ticketMessageQueryService = ticketMessageQueryService;
            _ticketMessageQueryService.NotNull(nameof(ticketMessageQueryService));
            _ticketAttachmentCommandService = ticketAttachmentCommandService;
            _ticketAttachmentCommandService.NotNull(nameof(ticketAttachmentCommandService));
            _ticketAccessService = ticketAccessService;
            _ticketAccessService.NotNull(nameof(ticketAccessService));
            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(TicketAttachmentUpdateDto updateDto, IAppUser currentUser, CancellationToken cancellationToken)
        {
            await TicketHandlerValidation.Validate(_validator, updateDto, cancellationToken);
            var info = await _ticketAttachmentQueryService.GetInfo(updateDto.Id, cancellationToken) ?? throw new InvalidIdException();
            var message = await _ticketMessageQueryService.Get(info.TicketMessageId, cancellationToken) ?? throw new InvalidIdException();
            var ticket = await _ticketAccessService.Get(message.TicketId, currentUser, cancellationToken);
            _ticketAccessService.EnsureSender(message, currentUser);
            if (ticket.StatusId == 4)
                throw new CustomValidationException("تیکت بسته‌شده قابل ویرایش نیست.");
            var attachment = await _ticketAttachmentQueryService.GetForUpdate(updateDto.Id, cancellationToken) ?? throw new InvalidIdException();
            _mapper.Map(updateDto, attachment);
            _ticketAttachmentCommandService.Update(attachment);
            ticket.UpdatedAt = DateTime.UtcNow;
        }
    }
}
