using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations
{
    public sealed class CaptchaGetSingleHandler : ICaptchaGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly ICaptchaQueryService _queryService;
        public CaptchaGetSingleHandler(IMapper mapper, ICaptchaQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }
        public async Task<CaptchaDto> Handle()
         {
            var captcha = await _queryService.Get();
            return _mapper.Map<CaptchaDto>(captcha);
        }
    }
}