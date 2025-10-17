using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Implementations
{
    internal sealed class RequestEstateUpdateHandler : IRequestEstateUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestEstateQueryService _requestEstateQueryService;
        private readonly IValidator<EstateRequestUpdateDto> _validator;

        public RequestEstateUpdateHandler(
            IMapper mapper,
            IRequestEstateQueryService requestEstateQueryService,
            IValidator<EstateRequestUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestEstateQueryService = requestEstateQueryService;
            _requestEstateQueryService.NotNull(nameof(_requestEstateQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(IAppUser currentUser, EstateRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            var requestEstate = await _requestEstateQueryService.Get(updateDto.Id);
            requestEstate.UserId=currentUser.UserId;

            _mapper.Map(updateDto, requestEstate);
        }
    }
}
