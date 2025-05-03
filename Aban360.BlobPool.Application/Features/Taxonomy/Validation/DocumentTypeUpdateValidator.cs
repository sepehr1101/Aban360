using Aban360.BlobPool.Application.Features.Base;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Validation
{
    public class DocumentTypeUpdateValidator:BaseValidator<DocumentTypeUpdateDto>
    {
        public DocumentTypeUpdateValidator()
        {
            RuleFor(t => t.Id)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(t => t.DocumentCategoryId)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);
            
            RuleFor(t => t.Title)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
               .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

            RuleFor(t => t.Css)
               .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);
        }
    }
}