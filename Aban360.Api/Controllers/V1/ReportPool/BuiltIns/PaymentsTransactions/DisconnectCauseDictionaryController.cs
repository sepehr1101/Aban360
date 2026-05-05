using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.PaymentsTransactions
{
    [Route("v1/disconnect-cause")]
    public class DisconnectCauseDictionaryController : BaseController
    {
        [HttpGet]
        [Route("get-dictionary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<NumericDictionary>>), StatusCodes.Status200OK)]
        public IActionResult GetDictionary(CancellationToken cancellationToken)
        {
            ICollection<NumericDictionary> datas = new List<NumericDictionary>()
            {
              new NumericDictionary(1,"بدهی آببها"),
              new NumericDictionary(2,"بدهی حق انشعاب"),
              new NumericDictionary(3,"بسته بیش از سه دوره"),
              new NumericDictionary(4,"انشعاب غیر مجاز آب"),
              new NumericDictionary(5,"انشعاب غیر مجاز فاضلاب"),
              new NumericDictionary(6,"به درخواست مشترک یا مصرف کننده"),
              new NumericDictionary(7,"نصب مستقیم پمپ"),
            };
            return Ok(datas);
        }
    }
}
