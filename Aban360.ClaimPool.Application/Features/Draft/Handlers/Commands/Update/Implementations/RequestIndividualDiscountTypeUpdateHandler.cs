using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Implementations
{
    internal sealed class RequestIndividualDiscountTypeUpdateHandler : IRequestIndividualDiscountTypeUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestIndividualDiscountTypeQueryService _requestIndividualDiscountTypeQueryService;
        public RequestIndividualDiscountTypeUpdateHandler(
            IMapper mapper,
            IRequestIndividualDiscountTypeQueryService requestIndividualDiscountTypeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestIndividualDiscountTypeQueryService = requestIndividualDiscountTypeQueryService;
            _requestIndividualDiscountTypeQueryService.NotNull(nameof(_requestIndividualDiscountTypeQueryService));
        }

        public async Task Handle(RequestIndividualDiscountTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var individualDiscountType = await _requestIndividualDiscountTypeQueryService.Get(updateDto.Id);
            individualDiscountType.Hash = "hash";
            individualDiscountType.ValidFrom = DateTime.Now;
            individualDiscountType.InsertLogInfo = "insertLogInfo";

            _mapper.Map(updateDto, individualDiscountType);
        }
    }
}
