﻿using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Implementations
{
    public class LineItemTypeGroupQueryService : ILineItemTypeGroupQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<LineItemTypeGroup> _lineItemTypeGroup;
        public LineItemTypeGroupQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _lineItemTypeGroup = _uow.Set<LineItemTypeGroup>();
            _lineItemTypeGroup.NotNull(nameof(_lineItemTypeGroup));
        }

        public async Task<LineItemTypeGroup> Get(short id)
        {
            return await _uow.FindOrThrowAsync<LineItemTypeGroup>(id);
        }

        public async Task<ICollection<LineItemTypeGroup>> Get()
        {
            return await _lineItemTypeGroup.ToListAsync();
        }
    }
}
