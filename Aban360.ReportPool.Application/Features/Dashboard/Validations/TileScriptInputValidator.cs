using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.Base.Validations;
using Aban360.ReportPool.Domain.Features.Dashboard.Dtos;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.Dashboard.Validations
{
    public class TileScriptInputValidator : BaseValidator<TileScriptInputDto>
    {
        public TileScriptInputValidator()
        {
            RuleFor(customer => customer.FromDateJalali)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);

        }
    }
}