using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Implementations
{
    internal sealed class RequestIndividualTagUpdateHandler : IRequestIndividualTagUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestIndividualTagQueryService _requestIndividualTagQueryService;
        private readonly IValidator<IndividualTagRequestUpdateDto> _validator;

        public RequestIndividualTagUpdateHandler(
            IMapper mapper,
            IRequestIndividualTagQueryService requestIndividualTagQueryService,
            IValidator<IndividualTagRequestUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestIndividualTagQueryService = requestIndividualTagQueryService;
            _requestIndividualTagQueryService.NotNull(nameof(_requestIndividualTagQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(IndividualTagRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var requestIndividualTag = await _requestIndividualTagQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, requestIndividualTag);
        }
    }
}
