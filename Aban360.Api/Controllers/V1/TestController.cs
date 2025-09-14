using Microsoft.AspNetCore.Mvc;
using DynamicExpresso;

namespace Aban360.Api.Controllers.V1
{
    [Route("test")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Route("calc")]
        public IActionResult TesCalc()
        {
            var interpreter = new Interpreter();
            int[] tags = { 100, 200, 300 };
            var c = 42;

            int i = 0;
            foreach (var tag in tags)
            {
                string tagName = $"a{i}";
                interpreter.SetVariable(tagName, tag);
                i++;
            }
            var result = interpreter
                .SetVariable("tags", tags)
                .SetVariable("c", c)
                .SetVariable("y",0)
                .Eval("tags!=null && tags.Contains(1) ? a1*43 : a2*1.7");

            return Ok(result);
        }
    }
}
