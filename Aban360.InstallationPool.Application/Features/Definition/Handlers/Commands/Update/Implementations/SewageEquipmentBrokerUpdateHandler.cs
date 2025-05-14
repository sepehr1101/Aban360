using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Update.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Persistence.Features.Queries.Contracts;
using AutoMapper;
using FluentValidation;

namespace Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Update.Implementations
{
    internal sealed class SewageEquipmentBrokerUpdateHandler : ISewageEquipmentBrokerUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ISewageEquipmentBrokerQueryService _sewageEquipmentBrokerQueryService;
        private readonly IValidator<SewageEquipmentBrokerUpdateDto> _validator;
        public SewageEquipmentBrokerUpdateHandler(
            IMapper mapper,
            ISewageEquipmentBrokerQueryService sewageEquipmentBrokerQueryService,
            IValidator<SewageEquipmentBrokerUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _sewageEquipmentBrokerQueryService = sewageEquipmentBrokerQueryService;
            _sewageEquipmentBrokerQueryService.NotNull(nameof(_sewageEquipmentBrokerQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(SewageEquipmentBrokerUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var sewageEquipmentBroker = await _sewageEquipmentBrokerQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, sewageEquipmentBroker);
        }
    }
}
