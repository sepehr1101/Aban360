using Aban360.BlobPool.Application.Features.Base;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Validations
{
    public class UseStateReportValidator:BaseValidator<UseStateReportInputDto>
    {
        public UseStateReportValidator()
        {
            RuleFor(useState=>useState.UseStateId)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(useState => useState.FromDate)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull)
             .Length(10).WithMessage(ExceptionLiterals.Equal10);

            RuleFor(useState => useState.ToDate)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull)
             .Length(10).WithMessage(ExceptionLiterals.Equal10);
        }
    }
}
