﻿using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        public async Task<ICollection<CaptchaQueryDto>> Handle(CancellationToken cancellationToken)
        {
            var captchas = await _queryService.GetAll();
            return _mapper.Map<ICollection<CaptchaQueryDto>>(captchas);
        }
    }
}