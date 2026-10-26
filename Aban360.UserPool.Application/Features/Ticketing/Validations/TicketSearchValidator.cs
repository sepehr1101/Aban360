using Aban360.UserPool.Application.Features.Auth.Validations;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using FluentValidation;

namespace Aban360.UserPool.Application.Features.Ticketing.Validations
{
    public class TicketSearchValidator : BaseValidator<TicketSearchInput>
    {
        public TicketSearchValidator()
        {
            RuleFor(x => x.PageNumber).InclusiveBetween(1, int.MaxValue / 100);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
            RuleFor(x => x.SearchText).MaximumLength(255);
            RuleFor(x => x.StatusId).GreaterThan((short)0).When(x => x.StatusId.HasValue);
            RuleFor(x => x.CreatedByUserId).NotEqual(Guid.Empty).When(x => x.CreatedByUserId.HasValue);
            RuleFor(x => x.AssignedToUserId).NotEqual(Guid.Empty).When(x => x.AssignedToUserId.HasValue);
            RuleFor(x => x).Must(x => !x.FromDate.HasValue || !x.ToDateExclusive.HasValue || x.FromDate < x.ToDateExclusive)
                .WithMessage("بازه زمانی معتبر نیست.");
        }
    }
}
