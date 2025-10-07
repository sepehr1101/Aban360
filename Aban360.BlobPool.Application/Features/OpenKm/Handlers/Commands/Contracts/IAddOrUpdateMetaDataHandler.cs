using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Implementations;
using DNTPersianUtils.Core.IranCities;
using static Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Implementations.AddOrUpdateMetaDataHandler;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts
{
    public interface IAddOrUpdateMetaDataHandler
    {
        Task Handle(AddOrUpdateMetaDataDto inputDto, string uuid,  CancellationToken cancellationToken);
    }
}
