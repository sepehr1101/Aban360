﻿using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts
{
    public interface IHeadquarterCommandService
    {
        Task Add(Headquarters headquarter);
        Task Remove(Headquarters headquarter);
    }
}
