﻿using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Entities;

namespace Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts
{
    public interface ICaptchaCommandService
    {
        Task Create(Captcha captcha);
        void Delete(Captcha captcha);
        Task SetIsSelected(int id);
        Task Update(CapthcaUpdateDto captcha);
    }
}