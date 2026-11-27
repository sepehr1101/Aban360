using Aban360.BlobPool.Application.Features.Base;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Validation
{
    public class ExecutableMimetypeUpdateValidator : BaseValidator<ExecutableMimetypeUpdateDto>
    {
        public ExecutableMimetypeUpdateValidator()
        {
            RuleFor(t => t.Id)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(t => t.Title)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

            RuleFor(t => t.StreamingOption)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);
            
            RuleFor(t => t.FrontendExecutor)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(t => t.IconName)
               .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);
        }
    }
}