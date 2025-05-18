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
    internal sealed class RequestEstateCreateHandler : IRequestEstateCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestEstateCommandService _requestEstateCommandService;
        private readonly IValidator<EstateRequestCreateDto> _validator;

        public RequestEstateCreateHandler(
            IMapper mapper,
            IRequestEstateCommandService requestEstateCommandService,
            IValidator<EstateRequestCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestEstateCommandService = requestEstateCommandService;
            _requestEstateCommandService.NotNull(nameof(_requestEstateCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(EstateRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var requestEstate = _mapper.Map<RequestEstate>(createDto);
            requestEstate.Hash = "-";
            requestEstate.InsertLogInfo = "-";
            requestEstate.ValidFrom = DateTime.Now;

            await _requestEstateCommandService.Add(requestEstate);
        }
    }
}
