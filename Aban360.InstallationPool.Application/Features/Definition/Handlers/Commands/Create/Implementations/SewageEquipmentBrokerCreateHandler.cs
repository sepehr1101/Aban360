using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Create.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Domain.Features.Definition.Entities;
using Aban360.InstallationPool.Persistence.Features.Commands.Contracts;
using AutoMapper;
using FluentValidation;

namespace Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Create.Implementations
{
    internal sealed class SewageEquipmentBrokerCreateHandler : ISewageEquipmentBrokerCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ISewageEquipmentBrokerCommandService _sewageEquipmentBrokerCommandService;
        private readonly IValidator<SewageEquipmentBrokerCreateDto> _validator;

        public SewageEquipmentBrokerCreateHandler(
            IMapper mapper,
            ISewageEquipmentBrokerCommandService sewageEquipmentBrokerCommandService,
            IValidator<SewageEquipmentBrokerCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _sewageEquipmentBrokerCommandService = sewageEquipmentBrokerCommandService;
            _sewageEquipmentBrokerCommandService.NotNull(nameof(_sewageEquipmentBrokerCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(SewageEquipmentBrokerCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            var sewageEquipmentBroker = _mapper.Map<SewageEquipmentBroker>(createDto);
            await _sewageEquipmentBrokerCommandService.Add(sewageEquipmentBroker);
        }
    }
}
