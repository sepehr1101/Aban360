using Aban360.UserPool.Application.Features.Auth.Validations;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands;
using FluentValidation;

namespace Aban360.UserPool.Application.Features.Ticketing.Validations
{
    public class TicketRatingUpdateValidator : BaseValidator<TicketRatingUpdateDto>
    {
        public TicketRatingUpdateValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Score).InclusiveBetween((byte)1, (byte)5);
            RuleFor(x => x.Comment).MaximumLength(1023);
        }
    }
}
