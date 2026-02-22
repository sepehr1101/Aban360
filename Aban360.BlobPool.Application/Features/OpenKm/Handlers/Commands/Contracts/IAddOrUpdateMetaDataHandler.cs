using Aban360.BlobPool.Domain.Features.DmsServices.Dto.Commands;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts
{
    public interface IAddOrUpdateMetaDataHandler
    {
        Task Handle(AddOrUpdateMetaDataDto inputDto, string uuid,  CancellationToken cancellationToken);
    }
}
