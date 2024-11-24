using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations
{
    public sealed class CaptchaGetListHandler : ICaptchaGetListHandler
    {
        private readonly IMapper _mapper;
        private readonly ICaptchaQueryService _queryService;

        public CaptchaGetListHandler(IMapper mapper, ICaptchaQueryService captchaQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull();

            _queryService = captchaQueryService;
            _queryService.NotNull();
        }
        public async Task<ICollection<CaptchaListQueryDto>> Handle(CancellationToken cancellationToken)
        {
            var captchas = await _queryService.GetAll();
            return _mapper.Map<ICollection<CaptchaListQueryDto>>(captchas);
        }
    }
}
