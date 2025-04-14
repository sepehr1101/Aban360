using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Implementations
{
    internal sealed class RequestIndividualDiscountTypeCreateHandler : IRequestIndividualDiscountTypeCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestIndividualDiscountTypeCommandService _requestIndividualDiscountTypeCommandService;
        public RequestIndividualDiscountTypeCreateHandler(
            IMapper mapper,
            IRequestIndividualDiscountTypeCommandService requestIndividualDiscountTypeCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestIndividualDiscountTypeCommandService = requestIndividualDiscountTypeCommandService;
            _requestIndividualDiscountTypeCommandService.NotNull(nameof(_requestIndividualDiscountTypeCommandService));
        }

        public async Task Handle(RequestIndividualDiscountTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            var individualDiscountType = _mapper.Map<RequestIndividualDiscountType>(createDto);
            individualDiscountType.Hash = "hash";
            individualDiscountType.ValidFrom = DateTime.Now;
            individualDiscountType.InsertLogInfo = "insertLogInfo";
            await _requestIndividualDiscountTypeCommandService.Add(individualDiscountType);
        }
    }
}
