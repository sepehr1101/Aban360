using Aban360.Common.ApplicationUser;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Exceptions;
using Aban360.UserPool.Application.Features.Ticketing.Services.Contracts;
using Aban360.UserPool.Application.Features.Ticketing.Services.Implementations;
using Aban360.UserPool.Application.Features.Ticketing.Validations;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Aban360.UserPool.Domain.Features.Ticketing.Entities;
using Aban360.UserPool.Persistence.Features.Ticketing.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.Ticketing.Queries.Contracts;
using AutoMapper;
using FluentValidation;

namespace Aban360.UserPool.Application.Features.Ticketing.Services;

public interface ITicketAttachmentService
{
    Task CreateAsync(TicketAttachmentCreateDto createDto, IAppUser currentUser, CancellationToken cancellationToken);
    Task UpdateAsync(TicketAttachmentUpdateDto updateDto, IAppUser currentUser, CancellationToken cancellationToken);
    Task<TicketAttachmentContentGetDto> GetContentAsync(long id, IAppUser currentUser, CancellationToken cancellationToken);
    Task<ICollection<TicketAttachmentInfo>> GetListAsync(long messageId, IAppUser currentUser, CancellationToken cancellationToken);
    Task<TicketAttachmentInfo> GetSingleAsync(long id, IAppUser currentUser, CancellationToken cancellationToken);
}

internal sealed class TicketAttachmentService : ITicketAttachmentService
{
    private readonly IMapper _mapper;
    private readonly ITicketAttachmentCommandService _ticketAttachmentCommandService;
    private readonly ITicketAttachmentQueryService _ticketAttachmentQueryService;
    private readonly ITicketMessageQueryService _ticketMessageQueryService;
    private readonly ITicketAccessService _ticketAccessService;
    private readonly IValidator<TicketAttachmentCreateDto> _createValidator;
    private readonly IValidator<TicketAttachmentUpdateDto> _updateValidator;

    public TicketAttachmentService(IMapper mapper, ITicketAttachmentCommandService ticketAttachmentCommandService, ITicketAttachmentQueryService ticketAttachmentQueryService, ITicketMessageQueryService ticketMessageQueryService, ITicketAccessService ticketAccessService, IValidator<TicketAttachmentCreateDto> createValidator, IValidator<TicketAttachmentUpdateDto> updateValidator)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        ticketAttachmentCommandService.NotNull(nameof(ticketAttachmentCommandService));
        _ticketAttachmentCommandService = ticketAttachmentCommandService;
        ticketAttachmentQueryService.NotNull(nameof(ticketAttachmentQueryService));
        _ticketAttachmentQueryService = ticketAttachmentQueryService;
        ticketMessageQueryService.NotNull(nameof(ticketMessageQueryService));
        _ticketMessageQueryService = ticketMessageQueryService;
        ticketAccessService.NotNull(nameof(ticketAccessService));
        _ticketAccessService = ticketAccessService;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
    }

    public async Task CreateAsync(TicketAttachmentCreateDto createDto, IAppUser currentUser, CancellationToken cancellationToken)
    {
        await TicketHandlerValidation.Validate(_createValidator, createDto, cancellationToken);
        var message = await _ticketMessageQueryService.Get(createDto.TicketMessageId, cancellationToken) ?? throw new InvalidIdException();
        var ticket = await _ticketAccessService.Get(message.TicketId, currentUser, cancellationToken);
        _ticketAccessService.EnsureSender(message, currentUser);
        if (ticket.StatusId == 4)
            throw new CustomValidationException("???? ???????? ???? ?????? ????.");
        var existingCount = await _ticketAttachmentQueryService.CountByMessage(message.Id, cancellationToken);
        if (existingCount >= TicketFileCreateValidator.MaxFileCount)
            throw new CustomValidationException("????? ????????? ???? ??? ?? ?? ???? ???.");
        TicketAttachment attachment = _mapper.Map<TicketAttachment>(createDto);
        ticket.UpdatedAt = DateTime.UtcNow;
        await _ticketAttachmentCommandService.Add(attachment, cancellationToken);
    }

    public async Task UpdateAsync(TicketAttachmentUpdateDto updateDto, IAppUser currentUser, CancellationToken cancellationToken)
    {
        await TicketHandlerValidation.Validate(_updateValidator, updateDto, cancellationToken);
        var info = await _ticketAttachmentQueryService.GetInfo(updateDto.Id, cancellationToken) ?? throw new InvalidIdException();
        var message = await _ticketMessageQueryService.Get(info.TicketMessageId, cancellationToken) ?? throw new InvalidIdException();
        var ticket = await _ticketAccessService.Get(message.TicketId, currentUser, cancellationToken);
        _ticketAccessService.EnsureSender(message, currentUser);
        if (ticket.StatusId == 4)
            throw new CustomValidationException("???? ???????? ???? ?????? ????.");
        var attachment = await _ticketAttachmentQueryService.GetForUpdate(updateDto.Id, cancellationToken) ?? throw new InvalidIdException();
        _mapper.Map(updateDto, attachment);
        _ticketAttachmentCommandService.Update(attachment);
        ticket.UpdatedAt = DateTime.UtcNow;
    }

    public async Task<TicketAttachmentContentGetDto> GetContentAsync(long id, IAppUser currentUser, CancellationToken cancellationToken)
    {
        // Authorize from metadata before reading the potentially large file.
        await GetSingleAsync(id, currentUser, cancellationToken);
        var attachment = await _ticketAttachmentQueryService.GetContent(id, cancellationToken) ?? throw new InvalidIdException();
        return _mapper.Map<TicketAttachmentContentGetDto>(attachment);
    }

    public async Task<ICollection<TicketAttachmentInfo>> GetListAsync(long messageId, IAppUser currentUser, CancellationToken cancellationToken)
    {
        var message = await _ticketMessageQueryService.Get(messageId, cancellationToken) ?? throw new InvalidIdException();
        await _ticketAccessService.Get(message.TicketId, currentUser, cancellationToken);
        return await _ticketAttachmentQueryService.GetByMessage(messageId, cancellationToken);
    }

    public async Task<TicketAttachmentInfo> GetSingleAsync(long id, IAppUser currentUser, CancellationToken cancellationToken)
    {
        var attachment = await _ticketAttachmentQueryService.GetInfo(id, cancellationToken) ?? throw new InvalidIdException();
        var message = await _ticketMessageQueryService.Get(attachment.TicketMessageId, cancellationToken) ?? throw new InvalidIdException();
        await _ticketAccessService.Get(message.TicketId, currentUser, cancellationToken);
        return attachment;
    }

}
