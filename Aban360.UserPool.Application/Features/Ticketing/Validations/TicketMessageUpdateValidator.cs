using Aban360.UserPool.Application.Features.Auth.Validations;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands;
using FluentValidation;

namespace Aban360.UserPool.Application.Features.Ticketing.Validations
{
    public class TicketMessageUpdateValidator : BaseValidator<TicketMessageUpdateDto>
    {
        public TicketMessageUpdateValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Body).MaximumLength(10000);
        }
    }
}
