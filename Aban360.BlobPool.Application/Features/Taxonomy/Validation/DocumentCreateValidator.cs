using Aban360.BlobPool.Application.Features.Base;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Validation
{
    public class DocumentCreateValidator:BaseValidator<DocumentCreateDto>
    {
        public DocumentCreateValidator()
        {
            RuleFor(t => t.DocumentFile)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(t => t.DocumentTypeId)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(t => t.ParrentId)
                .Must(m => m != Guid.Empty);

            RuleFor(t => t.Description)
                .MaximumLength(1023).WithMessage(ExceptionLiterals.NotMoreThan1023);

        }
    }
}