using Aban360.UserPool.Application.Features.Auth.Validations;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands;
using FluentValidation;

namespace Aban360.UserPool.Application.Features.Ticketing.Validations
{
    public class TicketCreateValidator : BaseValidator<TicketCreateDto>
    {
        public TicketCreateValidator()
        {
            RuleFor(x => x.Subject).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Body).MaximumLength(10000);
            RuleFor(x => x.Attachments).NotNull().Must(x => x.Count <= TicketFileCreateValidator.MaxFileCount);
            RuleForEach(x => x.Attachments).NotNull().SetValidator(new TicketFileCreateValidator());
            RuleFor(x => x).Must(x => !string.IsNullOrWhiteSpace(x.Body) || x.Attachments.Count > 0)
                .WithMessage("متن یا حداقل یک پیوست الزامی است.");
        }
    }
}
