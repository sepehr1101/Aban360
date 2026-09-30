using Aban360.BlobPool.Domain.Features.DmsServices.Dto.Commands;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.Common.ApplicationUser;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts
{
    public interface IAddFileHandler
    {
        Task<AddFileDto> Handle(AddFormFileInput input, IAppUser appUser, CancellationToken cancellationToken);
        Task<AddFileDto> Handle(AddBase64FileInput input, IAppUser appUser, CancellationToken cancellationToken);
        Task<AddFileDto> Handle(AddDiscountFileInput input, IAppUser appUser, CancellationToken cancellationToken);
    }
}
