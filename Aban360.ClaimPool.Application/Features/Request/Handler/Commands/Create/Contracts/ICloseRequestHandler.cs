using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts
{
    public interface ICloseRequestHandler
    {
        Task<RequestCloseOuputDto> Handle(int tracknumber, int userName, CancellationToken cancellationToken);
    }
}
