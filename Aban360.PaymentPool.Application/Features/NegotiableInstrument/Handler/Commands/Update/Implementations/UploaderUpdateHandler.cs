using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Update.Contracts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts;
using AutoMapper;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Update.Implementations
{
    internal sealed class UploaderUpdateHandler : IUploaderUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IUploaderQueryService _uploaderQueryService;
        public UploaderUpdateHandler(
            IMapper mapper,
            IUploaderQueryService uploaderQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _uploaderQueryService = uploaderQueryService;
            _uploaderQueryService.NotNull(nameof(_uploaderQueryService));
        }

        public async Task Handle(UploaderUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var uploader = await _uploaderQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, uploader);
        }
    }
}
