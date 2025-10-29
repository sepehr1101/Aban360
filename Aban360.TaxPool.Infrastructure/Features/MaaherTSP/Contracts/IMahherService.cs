using Aban360.Common.Extensions;

namespace Aban360.TaxPool.Infrastructure.Features.MaaherTSP.Contracts
{
    public interface IMahherService
    {
        Task SendInvoice();
    }
    internal sealed class MahherService : IMahherService
    {
        private readonly HttpClient _httpClient;
        public MahherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.NotNull(nameof(httpClient));
        }

        public async Task SendInvoice()
        {

        }
    }
}
