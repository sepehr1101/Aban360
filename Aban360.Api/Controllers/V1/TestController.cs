using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1
{
    [Route("test")]
    public class TestController : BaseController
    {
        [HttpPost]
        [Route("check-zone")]
        public IActionResult TesCalc([FromBody] ZoneTest zoneTest)
        {
            return Ok(zoneTest);
        }
    }
    public class ZoneTest
    {
        public ICollection<int>? ZoneIds { get; set; }
    }
}
