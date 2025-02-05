using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations
{
    public sealed class CaptchaDictionaryHandler : ICaptchaDictionaryHandler
    {
        private readonly ICaptchaQueryService _captchaQueryService;
        public CaptchaDictionaryHandler(ICaptchaQueryService captchaQueryService)
        {
            _captchaQueryService = captchaQueryService;
            _captchaQueryService.NotNull(nameof(captchaQueryService));
        }

        public async Task<ICollection<NumericDictionary>> Handle(CancellationToken cancellationToken)
        {
            var dictionary = await _captchaQueryService.GetDictionary();
            return dictionary;
        }
    }
}
