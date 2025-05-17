using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Implementations
{
    internal sealed class IndividualCreateHandler : IIndividualCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IIndividualCommandService _commandService;
        private readonly IEstateQueryService _estateQueryService;
        private readonly IIndividualEstateCommandService _individualEstateCommandService;
        private readonly IValidator<IndividualCreateDto> _individualValidator;
        public IndividualCreateHandler(
            IMapper mapper,
            IIndividualCommandService commandService,
            IEstateQueryService estateQueryService,
            IIndividualEstateCommandService individualEstateCommandService,
            IValidator<IndividualCreateDto> individualValidator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));

            _estateQueryService = estateQueryService;
            _estateQueryService.NotNull(nameof(estateQueryService));

            _individualEstateCommandService = individualEstateCommandService;
            _individualEstateCommandService.NotNull(nameof(individualEstateCommandService));

            _individualValidator = individualValidator;
            _individualValidator.NotNull(nameof(individualValidator));  
        }

        public async Task Handle(IndividualCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult=await _individualValidator .ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message=string.Join(", ", validationResult.Errors.Select(x=>x.ErrorMessage));
                throw new BaseException(message);
            }

            Individual individual = _mapper.Map<Individual>(createDto);
            individual.Hash = " "; //todo Hash in not null

            Estate estate = await _estateQueryService.Get(createDto.EstateId);
            IndividualEstate individualEstate = new IndividualEstate()
            {
                EstateId=estate.Id,
                Individual=individual,
                IndividualEstateRelationTypeId=createDto.IndividualEstateRelationTypeId,
            };
            //await _commandService.Add(individual);
            await _individualEstateCommandService.Add(individualEstate);
        }
    }
}
