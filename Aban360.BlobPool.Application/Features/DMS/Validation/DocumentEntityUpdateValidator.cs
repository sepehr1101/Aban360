using Aban360.BlobPool.Application.Features.Base;
using Aban360.BlobPool.Domain.Features.DMS.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.BlobPool.Application.Features.DMS.Validation
{
    public class DocumentEntityUpdateValidator : BaseValidator<DocumentEntityUpdateDto>
    {
        public DocumentEntityUpdateValidator()
        {
            RuleFor(t => t.Id)
              .NotNull().WithMessage(ExceptionLiterals.NotNUll)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);
            
            RuleFor(t => t.DocumentId)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(t => t.TableId)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(t => t.RelationEntityId)
               .IsInEnum().WithMessage(ExceptionLiterals.MustEnum);

        }
    }
}
