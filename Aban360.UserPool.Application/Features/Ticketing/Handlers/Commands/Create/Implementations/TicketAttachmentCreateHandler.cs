using Aban360.Common.ApplicationUser;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Exceptions;
using Aban360.UserPool.Application.Features.Ticketing.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Application.Features.Ticketing.Services.Contracts;
using Aban360.UserPool.Application.Features.Ticketing.Services.Implementations;
using Aban360.UserPool.Application.Features.Ticketing.Validations;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands;
using Aban360.UserPool.Domain.Features.Ticketing.Entities;
using Aban360.UserPool.Persistence.Features.Ticketing.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.Ticketing.Queries.Contracts;
using AutoMapper;
using FluentValidation;

namespace Aban360.UserPool.Application.Features.Ticketing.Handlers.Commands.Create.Implementations
{
    internal sealed class TicketAttachmentCreateHandler : ITicketAttachmentCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ITicketAttachmentCommandService _ticketAttachmentCommandService;
        private readonly ITicketAttachmentQueryService _ticketAttachmentQueryService;
        private readonly ITicketMessageQueryService _ticketMessageQueryService;
        private readonly ITicketAccessService _ticketAccessService;
        private readonly IValidator<TicketAttachmentCreateDto> _validator;
        public TicketAttachmentCreateHandler(
            IMapper mapper,
            ITicketAttachmentCommandService ticketAttachmentCommandService,
            ITicketAttachmentQueryService ticketAttachmentQueryService,
            ITicketMessageQueryService ticketMessageQueryService,
            ITicketAccessService ticketAccessService,
            IValidator<TicketAttachmentCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));
            _ticketAttachmentCommandService = ticketAttachmentCommandService;
            _ticketAttachmentCommandService.NotNull(nameof(ticketAttachmentCommandService));
            _ticketAttachmentQueryService = ticketAttachmentQueryService;
            _ticketAttachmentQueryService.NotNull(nameof(ticketAttachmentQueryService));
            _ticketMessageQueryService = ticketMessageQueryService;
            _ticketMessageQueryService.NotNull(nameof(ticketMessageQueryService));
            _ticketAccessService = ticketAccessService;
            _ticketAccessService.NotNull(nameof(ticketAccessService));
            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(TicketAttachmentCreateDto createDto, IAppUser currentUser, CancellationToken cancellationToken)
        {
            await TicketHandlerValidation.Validate(_validator, createDto, cancellationToken);
            var message = await _ticketMessageQueryService.Get(createDto.TicketMessageId, cancellationToken) ?? throw new InvalidIdException();
            var ticket = await _ticketAccessService.Get(message.TicketId, currentUser, cancellationToken);
            _ticketAccessService.EnsureSender(message, currentUser);
            if (ticket.StatusId == 4)
                throw new CustomValidationException("تیکت بسته‌شده قابل ویرایش نیست.");
            var existingCount = await _ticketAttachmentQueryService.CountByMessage(message.Id, cancellationToken);
            if (existingCount >= TicketFileCreateValidator.MaxFileCount)
                throw new CustomValidationException("تعداد پیوست‌های پیام بیش از حد مجاز است.");
            TicketAttachment attachment = _mapper.Map<TicketAttachment>(createDto);
            ticket.UpdatedAt = DateTime.UtcNow;
            await _ticketAttachmentCommandService.Add(attachment, cancellationToken);
        }
    }
}
