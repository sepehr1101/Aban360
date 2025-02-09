﻿using Aban360.Common.BaseEntities;

namespace Aban360.LocationPool.GatewayAdhoc.Features.MainHeirarchy.Contracts
{
    public interface IZoneQueryAddhoc
    {
        Task<ICollection<NumericDictionary>> Get(CancellationToken cancellationToken);
    }
}
