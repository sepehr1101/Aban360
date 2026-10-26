using Aban360.Common.ApplicationUser;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Exceptions;
using Aban360.UserPool.Application.Features.Ticketing.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Application.Features.Ticketing.Services.Contracts;
using Aban360.UserPool.Application.Features.Ticketing.Services.Implementations;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands;
using Aban360.UserPool.Persistence.Features.Ticketing.Queries.Contracts;
using AutoMapper;
using FluentValidation;

namespace Aban360.UserPool.Application.Features.Ticketing.Handlers.Commands.Update.Implementations
{
    internal sealed class TicketMessageUpdateHandler : ITicketMessageUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ITicketMessageQueryService _ticketMessageQueryService;
        private readonly ITicketAttachmentQueryService _ticketAttachmentQueryService;
        private readonly ITicketAccessService _ticketAccessService;
        private readonly IValidator<TicketMessageUpdateDto> _validator;
        public TicketMessageUpdateHandler(
            IMapper mapper,
            ITicketMessageQueryService ticketMessageQueryService,
            ITicketAttachmentQueryService ticketAttachmentQueryService,
            ITicketAccessService ticketAccessService,
            IValidator<TicketMessageUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));
            _ticketMessageQueryService = ticketMessageQueryService;
            _ticketMessageQueryService.NotNull(nameof(ticketMessageQueryService));
            _ticketAttachmentQueryService = ticketAttachmentQueryService;
            _ticketAttachmentQueryService.NotNull(nameof(ticketAttachmentQueryService));
            _ticketAccessService = ticketAccessService;
            _ticketAccessService.NotNull(nameof(ticketAccessService));
            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(TicketMessageUpdateDto updateDto, IAppUser currentUser, CancellationToken cancellationToken)
        {
            await TicketHandlerValidation.Validate(_validator, updateDto, cancellationToken);
            var message = await _ticketMessageQueryService.Get(updateDto.Id, cancellationToken) ?? throw new InvalidIdException();
            var ticket = await _ticketAccessService.Get(message.TicketId, currentUser, cancellationToken);
            _ticketAccessService.EnsureSender(message, currentUser);
            if (ticket.StatusId == 4)
                throw new CustomValidationException("تیکت بسته‌شده قابل ویرایش نیست.");
            if (string.IsNullOrWhiteSpace(updateDto.Body)
                && (await _ticketAttachmentQueryService.GetByMessage(message.Id, cancellationToken)).Count == 0)
                throw new CustomValidationException("متن یا حداقل یک پیوست الزامی است.");
            _mapper.Map(updateDto, message);
            ticket.UpdatedAt = DateTime.UtcNow;
        }
    }
}
