using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Create.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Domain.Features.Definition.Entities;
using Aban360.InstallationPool.Persistence.Features.Commands.Contracts;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Create.Implementations
{
    internal sealed class EquipmentBrokerCreateHandler : IEquipmentBrokerCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IEquipmentBrokerCommandService _equipmentBrokerCommandService;
        private readonly IValidator<EquipmentBrokerCreateDto> _validator;

        public EquipmentBrokerCreateHandler(
            IMapper mapper,
            IEquipmentBrokerCommandService equipmentBrokerCommandService,
            IValidator<EquipmentBrokerCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _equipmentBrokerCommandService = equipmentBrokerCommandService;
            _equipmentBrokerCommandService.NotNull(nameof(_equipmentBrokerCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(EquipmentBrokerCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var equipmentBroker = _mapper.Map<EquipmentBroker>(createDto);
            await _equipmentBrokerCommandService.Add(equipmentBroker);
        }
    }
}
