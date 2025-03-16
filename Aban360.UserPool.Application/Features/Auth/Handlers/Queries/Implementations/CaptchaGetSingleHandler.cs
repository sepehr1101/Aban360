using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Exceptions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
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
        public async Task<CaptchaActiveDto> Handle(CancellationToken cancellationToken)
        {
            Captcha captcha = await _queryService.Get();
            return _mapper.Map<CaptchaActiveDto>(captcha);
        }

        public async Task<CaptchaQueryDto> Handle(int id, CancellationToken cancellationToken)
        {
            Captcha captcha = await _queryService.Get(id);
            if (captcha == null)
            {
                throw new InvalidIdException();
            }
           return _mapper.Map<CaptchaQueryDto>(captcha);
        }
    }
}