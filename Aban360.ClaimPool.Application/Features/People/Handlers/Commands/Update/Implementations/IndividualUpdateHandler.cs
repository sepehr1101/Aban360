using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Implementations
{
    internal sealed class IndividualUpdateHandler : IIndividualUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IIndividualQueryService _queryService;
        private readonly IValidator<IndividualUpdateDto> _individualValidator;
        public IndividualUpdateHandler(
            IMapper mapper,
            IIndividualQueryService queryService,
            IValidator<IndividualUpdateDto> individualValidator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));

            _individualValidator = individualValidator;
            _individualValidator.NotNull(nameof(individualValidator));
        }

        public async Task Handle(IndividualUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _individualValidator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message=string.Join(", ", validationResult.Errors.Select(x=>x.ErrorMessage) );
                throw new BaseException(message);
            }

            Individual individual = await _queryService.Get(updateDto.Id);
            _mapper.Map(updateDto, individual);
        }
    }
}
