using Aban260.BlobPool.Infrastructure.Features.DmsServices.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.BlobPool.Application.Features.DmsServices.Handlers.Queries.Contracts
{
    public interface ITokenGetHandler
    {
        Task<string> Handle();
    }
    internal sealed class TokenGetHandler : ITokenGetHandler
    {
        private readonly ITokenGetServices _getTokenSevice;
        public TokenGetHandler(ITokenGetServices getTokenSevice)
        {
            _getTokenSevice = getTokenSevice;
            _getTokenSevice.NotNull(nameof(getTokenSevice));    
        }

        public async Task<string> Handle()
        {
           return await _getTokenSevice.Service();
        }
    }
}
