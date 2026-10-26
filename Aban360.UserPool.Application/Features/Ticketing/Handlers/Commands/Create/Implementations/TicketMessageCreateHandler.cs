using Aban360.Common.ApplicationUser;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Ticketing.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Application.Features.Ticketing.Services.Contracts;
using Aban360.UserPool.Application.Features.Ticketing.Services.Implementations;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands;
using Aban360.UserPool.Domain.Features.Ticketing.Entities;
using Aban360.UserPool.Persistence.Features.Ticketing.Commands.Contracts;
using AutoMapper;
using FluentValidation;

namespace Aban360.UserPool.Application.Features.Ticketing.Handlers.Commands.Create.Implementations
{
    internal sealed class TicketMessageCreateHandler : ITicketMessageCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ITicketMessageCommandService _ticketMessageCommandService;
        private readonly ITicketAccessService _ticketAccessService;
        private readonly IValidator<TicketMessageCreateDto> _validator;
        public TicketMessageCreateHandler(
            IMapper mapper,
            ITicketMessageCommandService ticketMessageCommandService,
            ITicketAccessService ticketAccessService,
            IValidator<TicketMessageCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));
            _ticketMessageCommandService = ticketMessageCommandService;
            _ticketMessageCommandService.NotNull(nameof(ticketMessageCommandService));
            _ticketAccessService = ticketAccessService;
            _ticketAccessService.NotNull(nameof(ticketAccessService));
            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(TicketMessageCreateDto createDto, IAppUser currentUser, CancellationToken cancellationToken)
        {
            await TicketHandlerValidation.Validate(_validator, createDto, cancellationToken);
            var ticket = await _ticketAccessService.Get(createDto.TicketId, currentUser, cancellationToken);
            if (ticket.StatusId == 4)
                throw new CustomValidationException("ارسال پیام برای تیکت بسته‌شده مجاز نیست.");
            TicketMessage message = _mapper.Map<TicketMessage>(createDto);
            message.SenderUserId = currentUser.UserId;
            message.CreatedAt = DateTime.UtcNow;
            ticket.UpdatedAt = message.CreatedAt;
            ticket.StatusId = currentUser.UserId == ticket.CreatedByUserId ? (short)2 : (short)3;
            await _ticketMessageCommandService.Add(message, cancellationToken);
        }
    }
}
