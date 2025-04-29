using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Delete.Contracts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Commands.Contracts;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Delete.Implementations
{
    internal sealed class UploaderDeleteHandler : IUploaderDeleteHandler
    {
        private readonly IUploaderCommandService _uploaderCommandService;
        private readonly IUploaderQueryService _uploaderQueryService;
        public UploaderDeleteHandler(
            IUploaderCommandService uploaderCommandService,
            IUploaderQueryService uploaderQueryService)
        {
            _uploaderCommandService = uploaderCommandService;
            _uploaderCommandService.NotNull(nameof(_uploaderCommandService));

            _uploaderQueryService = uploaderQueryService;
            _uploaderQueryService.NotNull(nameof(_uploaderQueryService));
        }

        public async Task Handle(UploaderDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var uploader = await _uploaderQueryService.Get(deleteDto.Id);
            await _uploaderCommandService.Remove(uploader);
        }
    }
}
