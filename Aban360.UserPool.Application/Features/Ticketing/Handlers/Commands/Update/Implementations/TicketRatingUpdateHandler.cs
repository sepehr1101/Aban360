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
    internal sealed class TicketRatingUpdateHandler : ITicketRatingUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ITicketRatingQueryService _ticketRatingQueryService;
        private readonly ITicketMessageQueryService _ticketMessageQueryService;
        private readonly ITicketAccessService _ticketAccessService;
        private readonly IValidator<TicketRatingUpdateDto> _validator;
        public TicketRatingUpdateHandler(
            IMapper mapper,
            ITicketRatingQueryService ticketRatingQueryService,
            ITicketMessageQueryService ticketMessageQueryService,
            ITicketAccessService ticketAccessService,
            IValidator<TicketRatingUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));
            _ticketRatingQueryService = ticketRatingQueryService;
            _ticketRatingQueryService.NotNull(nameof(ticketRatingQueryService));
            _ticketMessageQueryService = ticketMessageQueryService;
            _ticketMessageQueryService.NotNull(nameof(ticketMessageQueryService));
            _ticketAccessService = ticketAccessService;
            _ticketAccessService.NotNull(nameof(ticketAccessService));
            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(TicketRatingUpdateDto updateDto, IAppUser currentUser, CancellationToken cancellationToken)
        {
            await TicketHandlerValidation.Validate(_validator, updateDto, cancellationToken);
            var rating = await _ticketRatingQueryService.Get(updateDto.Id, cancellationToken) ?? throw new InvalidIdException();
            var message = await _ticketMessageQueryService.Get(rating.TicketMessageId, cancellationToken) ?? throw new InvalidIdException();
            var ticket = await _ticketAccessService.Get(message.TicketId, currentUser, cancellationToken);
            if (rating.RatedByUserId != currentUser.UserId || ticket.CreatedByUserId != currentUser.UserId
                || message.SenderUserId == ticket.CreatedByUserId)
                throw new CustomValidationException("فقط ثبت‌کننده امتیاز مجاز به ویرایش آن است.");
            _mapper.Map(updateDto, rating);
        }
    }
}
