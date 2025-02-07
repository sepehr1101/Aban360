using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Create.Implementations
{
    public sealed class CaptchaCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ICaptchaQueryService _captchaQueryService;
        private readonly ICaptchaCommandService _captchaCommandService;
        public CaptchaCreateHandler(
            IMapper mapper,
            ICaptchaQueryService captchaQueryService,
            ICaptchaCommandService captchaCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _captchaQueryService = captchaQueryService;
            _captchaQueryService.NotNull(nameof(captchaCommandService));

            _captchaCommandService=captchaCommandService;
            _captchaCommandService.NotNull(nameof(captchaCommandService));
        }
        public async Task Handle(CaptchaCreateDto captchaCreateDto, CancellationToken cancellationToken)
        {
            //todo: find if title exists, then validate, then insert
        }
    }
}
