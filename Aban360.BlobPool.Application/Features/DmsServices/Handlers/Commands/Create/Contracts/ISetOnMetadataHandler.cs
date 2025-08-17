using Aban260.BlobPool.Infrastructure.Features.DmsServices.Contracts;
using Aban360.BlobPool.Domain.Features.DmsServices.Dto.Queries;
using Aban360.Common.Extensions;

namespace Aban360.BlobPool.Application.Features.DmsServices.Handlers.Commands.Create.Contracts
{
    public interface ISetOnMetadataHandler
    {
        Task Handle(SetOnMetadataInputDto input);
    }
    internal sealed class SetOnMetadataHandler : ISetOnMetadataHandler
    {
        private readonly ISetOnMetadataServices _setOnMetadataServices;
        public SetOnMetadataHandler(ISetOnMetadataServices setOnMetadataServices)
        {
            _setOnMetadataServices = setOnMetadataServices;
            _setOnMetadataServices.NotNull(nameof(setOnMetadataServices));
        }

        public async Task Handle(SetOnMetadataInputDto input)
        {
            await _setOnMetadataServices.Service(input.Body, input.NodeId, input.GroupName);
        }
    }
}
