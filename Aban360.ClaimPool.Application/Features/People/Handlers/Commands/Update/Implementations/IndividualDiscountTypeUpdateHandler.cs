using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Implementations
{
    internal sealed class IndividualDiscountTypeUpdateHandler : IIndividualDiscountTypeUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IIndividualDiscountTypeQueryService _individualDiscountTypeQueryService;
        private readonly IValidator<IndividualDiscountTypeUpdateDto> _validator;
        public IndividualDiscountTypeUpdateHandler(
            IMapper mapper,
            IIndividualDiscountTypeQueryService individualDiscountTypeQueryService,
            IValidator<IndividualDiscountTypeUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _individualDiscountTypeQueryService = individualDiscountTypeQueryService;
            _individualDiscountTypeQueryService.NotNull(nameof(_individualDiscountTypeQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(IndividualDiscountTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var individualDiscountType = await _individualDiscountTypeQueryService.Get(updateDto.Id);
            individualDiscountType.Hash = "hash";
            individualDiscountType.ValidFrom = DateTime.Now;
            individualDiscountType.InsertLogInfo = "insertLogInfo";

            _mapper.Map(updateDto, individualDiscountType);
        }
    }
}
