using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Implementations
{
    internal sealed class IndividualDiscountTypeCreateHandler : IIndividualDiscountTypeCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IIndividualDiscountTypeCommandService _individualDiscountTypeCommandService;
        public IndividualDiscountTypeCreateHandler(
            IMapper mapper,
            IIndividualDiscountTypeCommandService individualDiscountTypeCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _individualDiscountTypeCommandService = individualDiscountTypeCommandService;
            _individualDiscountTypeCommandService.NotNull(nameof(_individualDiscountTypeCommandService));
        }

        public async Task Handle(IndividualDiscountTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            var individualDiscountType = _mapper.Map<IndividualDiscountType>(createDto);
            individualDiscountType.Hash = "hash";
            individualDiscountType.ValidFrom = DateTime.Now;
            individualDiscountType.InsertLogInfo = "insertLogInfo";

            await _individualDiscountTypeCommandService.Add(individualDiscountType);
        }
    }
}
