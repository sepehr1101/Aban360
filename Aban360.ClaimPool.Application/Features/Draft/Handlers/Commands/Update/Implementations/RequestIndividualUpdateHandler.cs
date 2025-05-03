using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Implementations
{
    internal sealed class RequestIndividualUpdateHandler : IRequestIndividualUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestIndividualQueryService _requestIndividualQueryService;
        private readonly IValidator<IndividualRequestUpdateDto> _validator;

        public RequestIndividualUpdateHandler(
            IMapper mapper,
            IRequestIndividualQueryService requestIndividualQueryService,
            IValidator<IndividualRequestUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestIndividualQueryService = requestIndividualQueryService;
            _requestIndividualQueryService.NotNull(nameof(_requestIndividualQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(IndividualRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var requestIndividual = await _requestIndividualQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, requestIndividual);
        }
    }
}
