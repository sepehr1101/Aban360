using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Implementations
{
    internal sealed class RequestIndividualDiscountTypeUpdateHandler : IRequestIndividualDiscountTypeUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestIndividualDiscountTypeQueryService _requestIndividualDiscountTypeQueryService;
        private readonly IValidator<RequestIndividualDiscountTypeUpdateDto> _validator;

        public RequestIndividualDiscountTypeUpdateHandler(
            IMapper mapper,
            IRequestIndividualDiscountTypeQueryService requestIndividualDiscountTypeQueryService,
            IValidator<RequestIndividualDiscountTypeUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestIndividualDiscountTypeQueryService = requestIndividualDiscountTypeQueryService;
            _requestIndividualDiscountTypeQueryService.NotNull(nameof(_requestIndividualDiscountTypeQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(RequestIndividualDiscountTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            var individualDiscountType = await _requestIndividualDiscountTypeQueryService.Get(updateDto.Id);
            individualDiscountType.Hash = "hash";
            individualDiscountType.ValidFrom = DateTime.Now;
            individualDiscountType.InsertLogInfo = "insertLogInfo";

            _mapper.Map(updateDto, individualDiscountType);
        }
    }
}
