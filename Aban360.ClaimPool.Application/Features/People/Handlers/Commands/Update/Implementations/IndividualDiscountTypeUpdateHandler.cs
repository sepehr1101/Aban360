using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Implementations
{
    internal sealed class IndividualDiscountTypeUpdateHandler : IIndividualDiscountTypeUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IIndividualDiscountTypeQueryService _individualDiscountTypeQueryService;
        public IndividualDiscountTypeUpdateHandler(
            IMapper mapper,
            IIndividualDiscountTypeQueryService individualDiscountTypeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _individualDiscountTypeQueryService = individualDiscountTypeQueryService;
            _individualDiscountTypeQueryService.NotNull(nameof(_individualDiscountTypeQueryService));
        }

        public async Task Handle(IndividualDiscountTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var individualDiscountType = await _individualDiscountTypeQueryService.Get(updateDto.Id);
            individualDiscountType.Hash = "hash";
            individualDiscountType.ValidFrom = DateTime.Now;
            individualDiscountType.InsertLogInfo = "insertLogInfo";

            _mapper.Map(updateDto, individualDiscountType);
        }
    }
}
