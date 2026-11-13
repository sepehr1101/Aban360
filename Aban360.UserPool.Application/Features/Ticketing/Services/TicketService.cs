using Aban360.Common.ApplicationUser;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Exceptions;
using Aban360.UserPool.Application.Features.Ticketing.Services.Contracts;
using Aban360.UserPool.Application.Features.Ticketing.Services.Implementations;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Aban360.UserPool.Domain.Features.Ticketing.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Aban360.UserPool.Persistence.Features.Ticketing.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.Ticketing.Queries.Contracts;
using AutoMapper;
using FluentValidation;

namespace Aban360.UserPool.Application.Features.Ticketing.Services;

public interface ITicketService
{
    Task<Ticket> CreateAsync(TicketCreateDto createDto, IAppUser currentUser, CancellationToken cancellationToken);
    Task UpdateAsync(TicketUpdateDto updateDto, IAppUser currentUser, CancellationToken cancellationToken);
    Task<TicketPage<TicketGetDto>> GetListAsync(TicketSearchInput input, IAppUser currentUser, CancellationToken cancellationToken);
    Task<TicketPage<TicketGetDto>> GetMyListAsync(TicketMySearchInput input, IAppUser currentUser, CancellationToken cancellationToken);
    Task<TicketGetDto> GetSingleAsync(long id, IAppUser currentUser, CancellationToken cancellationToken);
    Task<TicketReportOutput> GetReportAsync(TicketSearchInput input, IAppUser currentUser, CancellationToken cancellationToken);
    Task<TicketReportOutput> GetMyReportAsync(TicketMySearchInput input, IAppUser currentUser, CancellationToken cancellationToken);
}

internal sealed class TicketService : ITicketService
{
    private readonly IMapper _mapper;
    private readonly ITicketCommandService _ticketCommandService;
    private readonly IValidator<TicketCreateDto> _createValidator;
    private readonly ITicketAccessService _ticketAccessService;
    private readonly ITicketStatusQueryService _ticketStatusQueryService;
    private readonly IUserQueryService _userQueryService;
    private readonly IValidator<TicketUpdateDto> _updateValidator;
    private readonly ITicketQueryService _ticketQueryService;
    private readonly IValidator<TicketSearchInput> _searchValidator;
    private readonly ITicketReportQueryService _ticketReportQueryService;

    public TicketService(IMapper mapper, ITicketCommandService ticketCommandService, IValidator<TicketCreateDto> createValidator, ITicketAccessService ticketAccessService, ITicketStatusQueryService ticketStatusQueryService, IUserQueryService userQueryService, IValidator<TicketUpdateDto> updateValidator, ITicketQueryService ticketQueryService, IValidator<TicketSearchInput> searchValidator, ITicketReportQueryService ticketReportQueryService)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        ticketCommandService.NotNull(nameof(ticketCommandService));
        _ticketCommandService = ticketCommandService;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
        ticketAccessService.NotNull(nameof(ticketAccessService));
        _ticketAccessService = ticketAccessService;
        ticketStatusQueryService.NotNull(nameof(ticketStatusQueryService));
        _ticketStatusQueryService = ticketStatusQueryService;
        userQueryService.NotNull(nameof(userQueryService));
        _userQueryService = userQueryService;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
        ticketQueryService.NotNull(nameof(ticketQueryService));
        _ticketQueryService = ticketQueryService;
        searchValidator.NotNull(nameof(searchValidator));
        _searchValidator = searchValidator;
        ticketReportQueryService.NotNull(nameof(ticketReportQueryService));
        _ticketReportQueryService = ticketReportQueryService;
    }

    public async Task<Ticket> CreateAsync(TicketCreateDto createDto, IAppUser currentUser, CancellationToken cancellationToken)
    {
        await TicketHandlerValidation.Validate(_createValidator, createDto, cancellationToken);
        currentUser.NotNull(nameof(currentUser));
        if (currentUser.UserId == Guid.Empty)
            throw new CustomValidationException("????? ???? ????? ????.");
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
        return ticket;
    }

    public async Task UpdateAsync(TicketUpdateDto updateDto, IAppUser currentUser, CancellationToken cancellationToken)
    {
        await TicketHandlerValidation.Validate(_updateValidator, updateDto, cancellationToken);
        var ticket = await _ticketAccessService.Get(updateDto.Id, currentUser, cancellationToken);
        _ticketAccessService.EnsureStaff(ticket, currentUser);
        if (await _ticketStatusQueryService.Get(updateDto.StatusId, cancellationToken) == null)
            throw new InvalidIdException();
        if (ticket.AssignedToUserId != updateDto.AssignedToUserId)
        {
            if (!currentUser.IsAdmin)
                throw new CustomValidationException("????? ????? ???? ??? ???? ???? ???? ???.");
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

    public async Task<TicketPage<TicketGetDto>> GetListAsync(TicketSearchInput input, IAppUser currentUser, CancellationToken cancellationToken)
    {
        await TicketHandlerValidation.Validate(_searchValidator, input, cancellationToken);
        var result = await _ticketQueryService.Get(_ticketAccessService.Scope(input, currentUser), cancellationToken);
        return new TicketPage<TicketGetDto>
        {
            Items = _mapper.Map<ICollection<TicketGetDto>>(result.Items),
            TotalCount = result.TotalCount, PageNumber = result.PageNumber, PageSize = result.PageSize
        };
    }

    public Task<TicketPage<TicketGetDto>> GetMyListAsync(TicketMySearchInput input, IAppUser currentUser, CancellationToken cancellationToken)
    {
        input.NotNull(nameof(input));
        currentUser.NotNull(nameof(currentUser));
        // Always constrain ownership, including when the current user is an admin.
        // Reuse the existing search validation, query and DTO mapping.
        return GetListAsync(new TicketSearchInput
        {
            CreatedByUserId = currentUser.UserId,
            StatusId = input.StatusId,
            SearchText = input.SearchText,
            FromDate = input.FromDate,
            ToDateExclusive = input.ToDateExclusive,
            PageNumber = input.PageNumber,
            PageSize = input.PageSize
        }, currentUser, cancellationToken);
    }

    public async Task<TicketGetDto> GetSingleAsync(long id, IAppUser currentUser, CancellationToken cancellationToken)
    {
        var ticket = await _ticketAccessService.Get(id, currentUser, cancellationToken);
        return _mapper.Map<TicketGetDto>(ticket);
    }

    public async Task<TicketReportOutput> GetReportAsync(TicketSearchInput input, IAppUser currentUser, CancellationToken cancellationToken)
    {
        await TicketHandlerValidation.Validate(_searchValidator, input, cancellationToken);
        return await _ticketReportQueryService.Get(_ticketAccessService.Scope(input, currentUser), cancellationToken);
    }

    public Task<TicketReportOutput> GetMyReportAsync(TicketMySearchInput input, IAppUser currentUser, CancellationToken cancellationToken)
    {
        input.NotNull(nameof(input));
        currentUser.NotNull(nameof(currentUser));
        return GetReportAsync(new TicketSearchInput
        {
            CreatedByUserId = currentUser.UserId,
            StatusId = input.StatusId,
            SearchText = input.SearchText,
            FromDate = input.FromDate,
            ToDateExclusive = input.ToDateExclusive,
            PageNumber = input.PageNumber,
            PageSize = input.PageSize
        }, currentUser, cancellationToken);
    }

}
