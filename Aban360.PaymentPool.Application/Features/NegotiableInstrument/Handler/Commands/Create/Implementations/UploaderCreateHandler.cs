using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Create.Contracts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Commands.Contracts;
using AutoMapper;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Create.Implementations
{
    internal sealed class UploaderCreateHandler : IUploaderCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IUploaderCommandService _uploaderCommandService;
        public UploaderCreateHandler(
            IMapper mapper,
            IUploaderCommandService uploaderCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _uploaderCommandService = uploaderCommandService;
            _uploaderCommandService.NotNull(nameof(_uploaderCommandService));
        }

        public async Task Handle(UploaderCreateDto createDto, CancellationToken cancellationToken)
        {
            var uploader = _mapper.Map<Uploader>(createDto);
            await _uploaderCommandService.Add(uploader);
        }
    }
}
