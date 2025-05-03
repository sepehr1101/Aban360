using Aban360.BlobPool.Application.Features.Base;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Validation
{
    public class DocumentUpdateValidator:BaseValidator<DocumentUpdateDto>
    {
        public DocumentUpdateValidator()
        {
            RuleFor(t => t.Id)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .Must(m => m != Guid.Empty);

            RuleFor(t => t.document)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(t => t.DocumentTypeId)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(t => t.ParrentId)
                .Must(m => m != Guid.Empty);

            RuleFor(t => t.Description)
                .MaximumLength(1023).WithMessage(ExceptionLiterals.NotMoreThan1023);

            RuleFor(t => t.IsThumbnail)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

        }
    }
}