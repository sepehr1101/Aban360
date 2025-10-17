using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Commands.Contracts;
using AutoMapper;
using Aban360.Common.Extensions;
using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;
using FluentValidation;
using Aban360.Common.Exceptions;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Create.Implementations
{
    internal sealed class SiphonDiameterCreateHandler : ISiphonDiameterCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ISiphonDiameterCommandService _commandService;
        private readonly IValidator<SiphonDiameterCreateDto> _validator;

        public SiphonDiameterCreateHandler(
            IMapper mapper,
            ISiphonDiameterCommandService commandService,
            IValidator<SiphonDiameterCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(SiphonDiameterCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            SiphonDiameter SiphonDiameter = _mapper.Map<SiphonDiameter>(createDto);
            await _commandService.Add(SiphonDiameter);
        }
    }
}
