using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.Base.Validations;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Validations
{
    public class ContractualAndOlgooLevelValidator : BaseValidator<ContractualAndOlgooLevelInputDto>
    {
        public ContractualAndOlgooLevelValidator()
        {
            RuleFor(MeterByDuration => MeterByDuration.Values)
              .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
