using Aban360.Common.ApplicationUser;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Exceptions;
using Aban360.UserPool.Application.Features.Ticketing.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Application.Features.Ticketing.Services.Contracts;
using Aban360.UserPool.Application.Features.Ticketing.Services.Implementations;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Aban360.UserPool.Persistence.Features.Ticketing.Queries.Contracts;
using AutoMapper;
using FluentValidation;

namespace Aban360.UserPool.Application.Features.Ticketing.Handlers.Commands.Update.Implementations
{
    internal sealed class TicketUpdateHandler : ITicketUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ITicketAccessService _ticketAccessService;
        private readonly ITicketStatusQueryService _ticketStatusQueryService;
        private readonly IUserQueryService _userQueryService;
        private readonly IValidator<TicketUpdateDto> _validator;
        public TicketUpdateHandler(
            IMapper mapper,
            ITicketAccessService ticketAccessService,
            ITicketStatusQueryService ticketStatusQueryService,
            IUserQueryService userQueryService,
            IValidator<TicketUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));
            _ticketAccessService = ticketAccessService;
            _ticketAccessService.NotNull(nameof(ticketAccessService));
            _ticketStatusQueryService = ticketStatusQueryService;
            _ticketStatusQueryService.NotNull(nameof(ticketStatusQueryService));
            _userQueryService = userQueryService;
            _userQueryService.NotNull(nameof(userQueryService));
            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(TicketUpdateDto updateDto, IAppUser currentUser, CancellationToken cancellationToken)
        {
            await TicketHandlerValidation.Validate(_validator, updateDto, cancellationToken);
            var ticket = await _ticketAccessService.Get(updateDto.Id, currentUser, cancellationToken);
            _ticketAccessService.EnsureStaff(ticket, currentUser);
            if (await _ticketStatusQueryService.Get(updateDto.StatusId, cancellationToken) == null)
                throw new InvalidIdException();
            if (ticket.AssignedToUserId != updateDto.AssignedToUserId)
            {
                if (!currentUser.IsAdmin)
                    throw new CustomValidationException("تغییر مسئول تیکت فقط توسط مدیر مجاز است.");
                if (updateDto.AssignedToUserId.HasValue)
                {
                    var assignee = await _userQueryService.Get(updateDto.AssignedToUserId.Value);
                    if (assignee == null || assignee.ValidTo != null)
                        throw new InvalidIdException();
                }
            }
            _mapper.Map(updateDto, ticket);
            ticket.UpdatedAt = DateTime.UtcNow;
            ticket.ClosedAt = ticket.StatusId == 4 ? ticket.ClosedAt ?? ticket.UpdatedAt : null;
        }
    }
}
