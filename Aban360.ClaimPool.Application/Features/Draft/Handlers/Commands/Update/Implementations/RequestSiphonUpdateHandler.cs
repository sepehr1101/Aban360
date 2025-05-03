using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Implementations
{
    internal sealed class RequestSiphonUpdateHandler : IRequestSiphonUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestSiphonQueryService _requestSiphonQueryService;
        private readonly IValidator<SiphonRequestUpdateDto> _validator;

        public RequestSiphonUpdateHandler(
            IMapper mapper,
            IRequestSiphonQueryService requestSiphonQueryService,
            IValidator<SiphonRequestUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestSiphonQueryService = requestSiphonQueryService;
            _requestSiphonQueryService.NotNull(nameof(_requestSiphonQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(SiphonRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var requestSiphon = await _requestSiphonQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, requestSiphon);
        }
    }
}
