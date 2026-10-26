using Aban360.UserPool.Application.Features.Auth.Validations;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands;
using FluentValidation;

namespace Aban360.UserPool.Application.Features.Ticketing.Validations
{
    public class TicketAttachmentCreateValidator : BaseValidator<TicketAttachmentCreateDto>
    {
        public TicketAttachmentCreateValidator()
        {
            RuleFor(x => x.TicketMessageId).GreaterThan(0);
            RuleFor(x => (TicketFileCreateDto)x).SetValidator(new TicketFileCreateValidator());
        }
    }
}
