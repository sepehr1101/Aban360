using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    internal sealed class WaterResourceUpdateHandler : IWaterResourceUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IWaterResourceQueryService _waterResourceQueryService;
        private readonly IHeadquartersAddhoc _headquartersAddhoc;
        private readonly IValidator<WaterResourceUpdateDto> _validator;

        public WaterResourceUpdateHandler(
            IMapper mapper,
            IWaterResourceQueryService WaterResourceQueryService,
            IHeadquartersAddhoc headquartersAddhoc,
            IValidator<WaterResourceUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _waterResourceQueryService = WaterResourceQueryService;
            _waterResourceQueryService.NotNull(nameof(_waterResourceQueryService));

            _headquartersAddhoc = headquartersAddhoc;
            _headquartersAddhoc.NotNull(nameof(_headquartersAddhoc));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(WaterResourceUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            WaterResource waterResource = await _waterResourceQueryService.Get(updateDto.Id);
         
            string headquartersTitle = await _headquartersAddhoc.Handle(updateDto.HeadquartersId, cancellationToken);
            waterResource.HeadquartersTitle = headquartersTitle;
            
            _mapper.Map(updateDto, waterResource);
        }
    }
}
