﻿using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Commands.Implementations
{
    public class OfferingUnitCommandService : IOfferingUnitCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<OfferingUnit> _offeringUnit;
        public OfferingUnitCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _offeringUnit = _uow.Set<OfferingUnit>();
            _offeringUnit.NotNull(nameof(OfferingUnit));
        }

        public async Task Add(OfferingUnit offeringUnit)
        {
            await _offeringUnit.AddAsync(offeringUnit);
        }

        public async Task Remove(OfferingUnit offeringUnit)
        {
            _offeringUnit.Remove(offeringUnit);
        }
    }
}
