using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Update.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Persistence.Features.Queries.Contracts;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Update.Implementations
{
    internal sealed class EquipmentBrokerUpdateHandler : IEquipmentBrokerUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IEquipmentBrokerQueryService _equipmentBrokerQueryService;
        private readonly IValidator<EquipmentBrokerUpdateDto> _validator;
        public EquipmentBrokerUpdateHandler(
            IMapper mapper,
            IEquipmentBrokerQueryService equipmentBrokerQueryService,
            IValidator<EquipmentBrokerUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _equipmentBrokerQueryService = equipmentBrokerQueryService;
            _equipmentBrokerQueryService.NotNull(nameof(_equipmentBrokerQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(EquipmentBrokerUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            var equipmentBroker = await _equipmentBrokerQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, equipmentBroker);
        }
    }
}
