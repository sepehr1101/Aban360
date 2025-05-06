using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Contracts;
using AutoMapper;
using Aban360.Common.Extensions;
using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;
using FluentValidation;
using System.Threading;
using Aban360.Common.Exceptions;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Update.Implementations
{
    internal sealed class SiphonDiameterUpdateHandler : ISiphonDiameterUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ISiphonDiameterQueryService _queryService;
        private readonly IValidator<SiphonDiameterUpdateDto> _validator;
        public SiphonDiameterUpdateHandler(
            IMapper mapper,
            ISiphonDiameterQueryService queryService,
            IValidator<SiphonDiameterUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(SiphonDiameterUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            SiphonDiameter siphonDiameter = await _queryService.Get(updateDto.Id);
            _mapper.Map(updateDto, siphonDiameter);
        }
    }
}
