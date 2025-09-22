using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.ServiceLink
{
    [Route("v1/service-link")]
    public class ServiceLinkCalculationController : BaseController
    {
        [HttpPost]
        [Route("connection-fee")]
        [AllowAnonymous]
        public IActionResult CalculateConnectionFee()
        {
            Fee fee = new()
            {
                FeeItems = new[] { new FeeItem(27_000_000, 1, "حق انشعاب") },
                AmountSum = 27_000_000
            };
            return Ok(fee);
        }
    }

    public record Fee
    {
        public ICollection<FeeItem> FeeItems { get; set; } = default!;
        public long AmountSum { get; set; }
    }
    public record FeeItem
    {
        public long Amount { get; set; }
        public int ItemId { get; set; }
        public string ItemTitle { get; set; } = default!;
        public FeeItem(long amount, int itemId, string itemTitle)
        {
            Amount=amount;
            ItemId=itemId;
            ItemTitle=itemTitle;
        }
    }
}
