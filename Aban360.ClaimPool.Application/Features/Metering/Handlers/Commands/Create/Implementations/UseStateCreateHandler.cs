using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Implementations
{
    internal sealed class UseStateCreateHandler : IUseStateCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IUseStateCommandService _useStateCommandService;
        private readonly IValidator<UseStateCreateDto> _validator;

        public UseStateCreateHandler(
            IMapper mapper,
            IUseStateCommandService useStateCommandService,
            IValidator<UseStateCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _useStateCommandService = useStateCommandService;
            _useStateCommandService.NotNull(nameof(useStateCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(UseStateCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            UseState useState = _mapper.Map<UseState>(createDto);
            await _useStateCommandService.Add(useState);
        }
    }
}
