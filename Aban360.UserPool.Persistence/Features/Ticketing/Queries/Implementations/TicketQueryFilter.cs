using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Aban360.UserPool.Domain.Features.Ticketing.Entities;

namespace Aban360.UserPool.Persistence.Features.Ticketing.Queries.Implementations
{
    internal static class TicketQueryFilter
    {
        internal static IQueryable<Ticket> Apply(IQueryable<Ticket> query, TicketSearchInput input)
        {
            input.NotNull(nameof(input));
            if (input.FromDate.HasValue && input.ToDateExclusive.HasValue && input.FromDate >= input.ToDateExclusive)
                throw new ArgumentException("FromDate must be earlier than ToDateExclusive.", nameof(input));

            if (input.CreatedByUserId.HasValue)
                query = query.Where(x => x.CreatedByUserId == input.CreatedByUserId.Value);
            if (input.AssignedToUserId.HasValue)
                query = query.Where(x => x.AssignedToUserId == input.AssignedToUserId.Value);
            if (input.StatusId.HasValue)
                query = query.Where(x => x.StatusId == input.StatusId.Value);
            if (input.FromDate.HasValue)
                query = query.Where(x => x.CreatedAt >= input.FromDate.Value);
            if (input.ToDateExclusive.HasValue)
                query = query.Where(x => x.CreatedAt < input.ToDateExclusive.Value);
            if (!string.IsNullOrWhiteSpace(input.SearchText))
            {
                var text = input.SearchText.Trim();
                query = query.Where(x => x.TicketNumber.Contains(text) || x.Subject.Contains(text));
            }
            return query;
        }

        internal static int GetSkip(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber));
            if (pageSize < 1 || pageSize > 100)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be between 1 and 100.");
            return checked((pageNumber - 1) * pageSize);
        }
    }
}
