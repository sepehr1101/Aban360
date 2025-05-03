using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Validations
{
    public partial class RequestFlatCreateValidator
    {
        public class RequestIndividualEstateUpdateValidator : BaseValidator<IndividualEstateRequestUpdateDto>
        {
            public RequestIndividualEstateUpdateValidator()
            {
                RuleFor(f => f.Id)
                    .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                    .NotNull().WithMessage(ExceptionLiterals.NotNUll);

                RuleFor(f => f.EstateId)
                    .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                    .NotNull().WithMessage(ExceptionLiterals.NotNUll);

                RuleFor(f => f.IndividualId)
                    .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                    .NotNull().WithMessage(ExceptionLiterals.NotNUll);

                RuleFor(f => f.IndividualEstateRelationTypeId)
                    .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                    .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                    .IsInEnum().WithMessage(ExceptionLiterals.MustEnum);

            }
        }
    }
}