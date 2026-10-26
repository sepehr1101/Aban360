using Aban360.UserPool.Application.Features.Auth.Validations;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands;
using FluentValidation;

namespace Aban360.UserPool.Application.Features.Ticketing.Validations
{
    public class TicketUpdateValidator : BaseValidator<TicketUpdateDto>
    {
        public TicketUpdateValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Subject).NotEmpty().MaximumLength(255);
            RuleFor(x => x.StatusId).InclusiveBetween((short)1, (short)4);
            RuleFor(x => x.AssignedToUserId).NotEqual(Guid.Empty).When(x => x.AssignedToUserId.HasValue);
        }
    }
}
