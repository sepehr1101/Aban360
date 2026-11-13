using Aban360.Common.ApplicationUser;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Exceptions;
using Aban360.UserPool.Application.Features.Ticketing.Services.Contracts;
using Aban360.UserPool.Application.Features.Ticketing.Services.Implementations;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Aban360.UserPool.Domain.Features.Ticketing.Entities;
using Aban360.UserPool.Persistence.Features.Ticketing.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.Ticketing.Queries.Contracts;
using AutoMapper;
using FluentValidation;

namespace Aban360.UserPool.Application.Features.Ticketing.Services;

public interface ITicketMessageService
{
    Task CreateAsync(TicketMessageCreateDto createDto, IAppUser currentUser, CancellationToken cancellationToken);
    Task UpdateAsync(TicketMessageUpdateDto updateDto, IAppUser currentUser, CancellationToken cancellationToken);
    Task<TicketPage<TicketMessageGetDto>> GetListAsync(TicketMessageSearchInput input, IAppUser currentUser, CancellationToken cancellationToken);
    Task<TicketMessageGetDto> GetSingleAsync(long id, IAppUser currentUser, CancellationToken cancellationToken);
}

internal sealed class TicketMessageService : ITicketMessageService
{
    private readonly IMapper _mapper;
    private readonly ITicketMessageCommandService _ticketMessageCommandService;
    private readonly ITicketAccessService _ticketAccessService;
    private readonly IValidator<TicketMessageCreateDto> _createValidator;
    private readonly ITicketMessageQueryService _ticketMessageQueryService;
    private readonly ITicketAttachmentQueryService _ticketAttachmentQueryService;
    private readonly IValidator<TicketMessageUpdateDto> _updateValidator;
    private readonly IValidator<TicketMessageSearchInput> _searchValidator;

    public TicketMessageService(IMapper mapper, ITicketMessageCommandService ticketMessageCommandService, ITicketAccessService ticketAccessService, IValidator<TicketMessageCreateDto> createValidator, ITicketMessageQueryService ticketMessageQueryService, ITicketAttachmentQueryService ticketAttachmentQueryService, IValidator<TicketMessageUpdateDto> updateValidator, IValidator<TicketMessageSearchInput> searchValidator)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        ticketMessageCommandService.NotNull(nameof(ticketMessageCommandService));
        _ticketMessageCommandService = ticketMessageCommandService;
        ticketAccessService.NotNull(nameof(ticketAccessService));
        _ticketAccessService = ticketAccessService;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
        ticketMessageQueryService.NotNull(nameof(ticketMessageQueryService));
        _ticketMessageQueryService = ticketMessageQueryService;
        ticketAttachmentQueryService.NotNull(nameof(ticketAttachmentQueryService));
        _ticketAttachmentQueryService = ticketAttachmentQueryService;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
        searchValidator.NotNull(nameof(searchValidator));
        _searchValidator = searchValidator;
    }

    public async Task CreateAsync(TicketMessageCreateDto createDto, IAppUser currentUser, CancellationToken cancellationToken)
    {
        await TicketHandlerValidation.Validate(_createValidator, createDto, cancellationToken);
        var ticket = await _ticketAccessService.Get(createDto.TicketId, currentUser, cancellationToken);
        if (ticket.StatusId == 4)
            throw new CustomValidationException("????? ???? ???? ???? ???????? ???? ????.");
        TicketMessage message = _mapper.Map<TicketMessage>(createDto);
        message.SenderUserId = currentUser.UserId;
        message.CreatedAt = DateTime.UtcNow;
        ticket.UpdatedAt = message.CreatedAt;
        ticket.StatusId = currentUser.UserId == ticket.CreatedByUserId ? (short)2 : (short)3;
        await _ticketMessageCommandService.Add(message, cancellationToken);
    }

    public async Task UpdateAsync(TicketMessageUpdateDto updateDto, IAppUser currentUser, CancellationToken cancellationToken)
    {
        await TicketHandlerValidation.Validate(_updateValidator, updateDto, cancellationToken);
        var message = await _ticketMessageQueryService.Get(updateDto.Id, cancellationToken) ?? throw new InvalidIdException();
        var ticket = await _ticketAccessService.Get(message.TicketId, currentUser, cancellationToken);
        _ticketAccessService.EnsureSender(message, currentUser);
        if (ticket.StatusId == 4)
            throw new CustomValidationException("???? ???????? ???? ?????? ????.");
        if (string.IsNullOrWhiteSpace(updateDto.Body)
            && (await _ticketAttachmentQueryService.GetByMessage(message.Id, cancellationToken)).Count == 0)
            throw new CustomValidationException("??? ?? ????? ?? ????? ?????? ???.");
        _mapper.Map(updateDto, message);
        ticket.UpdatedAt = DateTime.UtcNow;
    }

    public async Task<TicketPage<TicketMessageGetDto>> GetListAsync(TicketMessageSearchInput input, IAppUser currentUser, CancellationToken cancellationToken)
    {
        await TicketHandlerValidation.Validate(_searchValidator, input, cancellationToken);
        await _ticketAccessService.Get(input.TicketId, currentUser, cancellationToken);
        var result = await _ticketMessageQueryService.GetByTicket(input.TicketId, input.PageNumber, input.PageSize, cancellationToken);
        return new TicketPage<TicketMessageGetDto>
        {
            Items = _mapper.Map<ICollection<TicketMessageGetDto>>(result.Items),
            TotalCount = result.TotalCount, PageNumber = result.PageNumber, PageSize = result.PageSize
        };
    }

    public async Task<TicketMessageGetDto> GetSingleAsync(long id, IAppUser currentUser, CancellationToken cancellationToken)
    {
        var message = await _ticketMessageQueryService.Get(id, cancellationToken) ?? throw new InvalidIdException();
        await _ticketAccessService.Get(message.TicketId, currentUser, cancellationToken);
        return _mapper.Map<TicketMessageGetDto>(message);
    }

}
