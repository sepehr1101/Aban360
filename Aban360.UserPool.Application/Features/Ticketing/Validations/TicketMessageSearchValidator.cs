using Aban360.UserPool.Application.Features.Auth.Validations;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using FluentValidation;

namespace Aban360.UserPool.Application.Features.Ticketing.Validations
{
    public class TicketMessageSearchValidator : BaseValidator<TicketMessageSearchInput>
    {
        public TicketMessageSearchValidator()
        {
            RuleFor(x => x.TicketId).GreaterThan(0);
            RuleFor(x => x.PageNumber).InclusiveBetween(1, int.MaxValue / 100);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        }
    }
}
