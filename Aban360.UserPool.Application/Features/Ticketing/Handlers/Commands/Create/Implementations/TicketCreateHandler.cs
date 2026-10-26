using Aban360.Common.ApplicationUser;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Ticketing.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Application.Features.Ticketing.Services.Implementations;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands;
using Aban360.UserPool.Domain.Features.Ticketing.Entities;
using Aban360.UserPool.Persistence.Features.Ticketing.Commands.Contracts;
using AutoMapper;
using FluentValidation;

namespace Aban360.UserPool.Application.Features.Ticketing.Handlers.Commands.Create.Implementations
{
    internal sealed class TicketCreateHandler : ITicketCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ITicketCommandService _ticketCommandService;
        private readonly IValidator<TicketCreateDto> _validator;
        public TicketCreateHandler(
            IMapper mapper,
            ITicketCommandService ticketCommandService,
            IValidator<TicketCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));
            _ticketCommandService = ticketCommandService;
            _ticketCommandService.NotNull(nameof(ticketCommandService));
            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<string> Handle(TicketCreateDto createDto, IAppUser currentUser, CancellationToken cancellationToken)
        {
            await TicketHandlerValidation.Validate(_validator, createDto, cancellationToken);
            currentUser.NotNull(nameof(currentUser));
            if (currentUser.UserId == Guid.Empty)
                throw new CustomValidationException("کاربر جاری معتبر نیست.");
            var now = DateTime.UtcNow;
            Ticket ticket = _mapper.Map<Ticket>(createDto);
            ticket.TicketNumber = Guid.NewGuid().ToString("N");
            ticket.CreatedByUserId = currentUser.UserId;
            ticket.StatusId = 1;
            ticket.CreatedAt = now;
            ticket.UpdatedAt = now;
            ticket.Messages.Add(new TicketMessage
            {
                SenderUserId = currentUser.UserId, Body = createDto.Body, CreatedAt = now,
                Attachments = _mapper.Map<ICollection<TicketAttachment>>(createDto.Attachments)
            });
            // One entity graph is committed atomically by the caller's unit of work.
            await _ticketCommandService.Add(ticket, cancellationToken);
            return ticket.TicketNumber;
        }
    }
}
