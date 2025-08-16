using Aban260.BlobPool.Infrastructure.Features.DmsServices.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.BlobPool.Application.Features.DmsServices.Handlers.Queries.Commands
{
    public interface IGetTokenHandler
    {
        Task<string> Handle();
    }
    internal sealed class GetokenHandler : IGetTokenHandler
    {
        private readonly IGetTokenServices _getTokenSevice;
        public GetokenHandler(IGetTokenServices getTokenSevice)
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
