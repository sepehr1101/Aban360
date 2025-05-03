using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Implementations
{
    internal sealed class RequestIndividualEstateUpdateHandler : IRequestIndividualEstateUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestIndividualEstateQueryService _requestIndividualEstateQueryService;
        private readonly IValidator<IndividualEstateRequestUpdateDto> _validator;

        public RequestIndividualEstateUpdateHandler(
            IMapper mapper,
            IRequestIndividualEstateQueryService requestIndividualEstateQueryService,
            IValidator<IndividualEstateRequestUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestIndividualEstateQueryService = requestIndividualEstateQueryService;
            _requestIndividualEstateQueryService.NotNull(nameof(_requestIndividualEstateQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(IndividualEstateRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var requestIndividualEstate = await _requestIndividualEstateQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, requestIndividualEstate);
        }
    }
}
