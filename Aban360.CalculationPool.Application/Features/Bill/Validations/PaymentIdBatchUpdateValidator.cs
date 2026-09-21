using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Bill.Validations
{
    public class PaymentIdBatchUpdateValidator : AbstractValidator<PaymentIdBatchUpdateInputDto>
    {
        public PaymentIdBatchUpdateValidator()
        {
            RuleFor(x => x.ZoneId)
                .GreaterThan(0);

            RuleFor(x => x.Ids)
                .NotEmpty();

            RuleForEach(x => x.Ids)
                .GreaterThan(0);
        }
    }
}
