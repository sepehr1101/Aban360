using Aban360.BlobPool.Application.Features.Base;
using Aban360.BlobPool.Domain.Features.DMS.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.BlobPool.Application.Features.DMS.Validation
{
    public class DocumentEntityCreateValidator : BaseValidator<DocumentEntityCreateDto>
    {
        public DocumentEntityCreateValidator()
        {
            RuleFor(t => t.DocumentId)
                          .NotNull().WithMessage(ExceptionLiterals.NotNull)
                          .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(t => t.TableId)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(t => t.RelationEntityId)
               .IsInEnum().WithMessage(ExceptionLiterals.MustEnum);

        }
    }
}
