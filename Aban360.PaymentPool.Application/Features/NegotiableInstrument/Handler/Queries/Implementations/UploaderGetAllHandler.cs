using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Queries.Contracts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts;
using AutoMapper;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Queries.Implementations
{
    internal sealed class UploaderGetAllHandler : IUploaderGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IUploaderQueryService _uploaderQueryService;
        public UploaderGetAllHandler(
            IMapper mapper,
            IUploaderQueryService uploaderQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _uploaderQueryService = uploaderQueryService;
            _uploaderQueryService.NotNull(nameof(_uploaderQueryService));
        }

        public async Task<ICollection<UploaderGetDto>> Handle(CancellationToken cancellationToken)
        {
            var uploader = await _uploaderQueryService.Get();
            return _mapper.Map<ICollection<UploaderGetDto>>(uploader);
        }
    }
}
