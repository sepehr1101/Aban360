using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Validations
{
    public partial class RequestFlatCreateValidator
    {
        public class RequestIndividualEstateCreateValidator : BaseValidator<IndividualEstateRequestCreateDto>
        {
            public RequestIndividualEstateCreateValidator()
            {
                RuleFor(f => f.EstateId)
                    .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                    .NotNull().WithMessage(ExceptionLiterals.NotNull);

                RuleFor(f => f.IndividualId)
                    .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                    .NotNull().WithMessage(ExceptionLiterals.NotNull);

                RuleFor(f => f.IndividualEstateRelationTypeId)
                    .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                    .NotNull().WithMessage(ExceptionLiterals.NotNull)
                    .IsInEnum().WithMessage(ExceptionLiterals.MustEnum);

            }
        }
    }
}