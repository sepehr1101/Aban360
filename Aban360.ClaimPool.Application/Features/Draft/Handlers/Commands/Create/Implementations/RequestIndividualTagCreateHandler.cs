using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Implementations
{
    internal sealed class RequestIndividualTagCreateHandler : IRequestIndividualTagCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestIndividualTagCommandService _requestIndividualTagCommandService;
        private readonly IValidator<IndividualTagRequestCreateDto> _validator;
   
        public RequestIndividualTagCreateHandler(
            IMapper mapper,
            IRequestIndividualTagCommandService requestIndividualTagCommandService,
            IValidator<IndividualTagRequestCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestIndividualTagCommandService = requestIndividualTagCommandService;
            _requestIndividualTagCommandService.NotNull(nameof(_requestIndividualTagCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(IndividualTagRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            var requestIndividualTag = _mapper.Map<RequestIndividualTag>(createDto);
            requestIndividualTag.Hash = "-";
            requestIndividualTag.InsertLogInfo = "-";
            requestIndividualTag.ValidFrom = DateTime.Now;

            await _requestIndividualTagCommandService.Add(requestIndividualTag);
        }
    }
}
