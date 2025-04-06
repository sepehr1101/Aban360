using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    internal sealed class EstateUpdateHandler : IEstateUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IEstateQueryService _queryService;
        private readonly IValidator<EstateUpdateDto> _estateValidator;
        public EstateUpdateHandler(
            IMapper mapper,
            IEstateQueryService queryService,
            IValidator<EstateUpdateDto> estateValidator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));

            _estateValidator = estateValidator;
            _estateValidator.NotNull(nameof(estateValidator));
        }

        public async Task Handle(EstateUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _estateValidator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new BaseException(message);
            }

            Estate estate = await _queryService.Get(updateDto.Id);
            _mapper.Map(updateDto, estate);
        }
    }
}
