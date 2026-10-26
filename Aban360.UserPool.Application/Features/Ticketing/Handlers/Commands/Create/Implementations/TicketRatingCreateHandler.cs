using Aban360.Common.ApplicationUser;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Exceptions;
using Aban360.UserPool.Application.Features.Ticketing.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Application.Features.Ticketing.Services.Contracts;
using Aban360.UserPool.Application.Features.Ticketing.Services.Implementations;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands;
using Aban360.UserPool.Domain.Features.Ticketing.Entities;
using Aban360.UserPool.Persistence.Features.Ticketing.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.Ticketing.Queries.Contracts;
using AutoMapper;
using FluentValidation;

namespace Aban360.UserPool.Application.Features.Ticketing.Handlers.Commands.Create.Implementations
{
    internal sealed class TicketRatingCreateHandler : ITicketRatingCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ITicketRatingCommandService _ticketRatingCommandService;
        private readonly ITicketRatingQueryService _ticketRatingQueryService;
        private readonly ITicketMessageQueryService _ticketMessageQueryService;
        private readonly ITicketAccessService _ticketAccessService;
        private readonly IValidator<TicketRatingCreateDto> _validator;
        public TicketRatingCreateHandler(
            IMapper mapper,
            ITicketRatingCommandService ticketRatingCommandService,
            ITicketRatingQueryService ticketRatingQueryService,
            ITicketMessageQueryService ticketMessageQueryService,
            ITicketAccessService ticketAccessService,
            IValidator<TicketRatingCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));
            _ticketRatingCommandService = ticketRatingCommandService;
            _ticketRatingCommandService.NotNull(nameof(ticketRatingCommandService));
            _ticketRatingQueryService = ticketRatingQueryService;
            _ticketRatingQueryService.NotNull(nameof(ticketRatingQueryService));
            _ticketMessageQueryService = ticketMessageQueryService;
            _ticketMessageQueryService.NotNull(nameof(ticketMessageQueryService));
            _ticketAccessService = ticketAccessService;
            _ticketAccessService.NotNull(nameof(ticketAccessService));
            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(TicketRatingCreateDto createDto, IAppUser currentUser, CancellationToken cancellationToken)
        {
            await TicketHandlerValidation.Validate(_validator, createDto, cancellationToken);
            var message = await _ticketMessageQueryService.Get(createDto.TicketMessageId, cancellationToken) ?? throw new InvalidIdException();
            var ticket = await _ticketAccessService.Get(message.TicketId, currentUser, cancellationToken);
            if (ticket.CreatedByUserId != currentUser.UserId || message.SenderUserId == ticket.CreatedByUserId)
                throw new CustomValidationException("فقط ثبت‌کننده تیکت می‌تواند به پاسخ مسئول امتیاز دهد.");
            if (await _ticketRatingQueryService.GetByMessage(message.Id, cancellationToken) != null)
                throw new CustomValidationException("برای این پاسخ قبلاً امتیاز ثبت شده است.");
            TicketRating rating = _mapper.Map<TicketRating>(createDto);
            rating.RatedByUserId = currentUser.UserId;
            rating.CreatedAt = DateTime.UtcNow;
            await _ticketRatingCommandService.Add(rating, cancellationToken);
        }
    }
}
