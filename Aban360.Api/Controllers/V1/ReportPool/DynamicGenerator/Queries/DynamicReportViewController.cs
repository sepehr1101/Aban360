using Microsoft.AspNetCore.Mvc;
using Stimulsoft.Base;
using Stimulsoft.Report.Mvc;
using Stimulsoft.Report;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.DynamicGenerator.Handlers.Queries.Contracts;

namespace Aban360.Api.Controllers.V1.ReportPool.DynamicGenerator
{
    [Route("v1/dynamic-report-view")]
    public class DynamicReportViewController : BaseMvcController
    {        
        private readonly IDynamicReportGetTemplateJsonHandler _dynamicReportGetTemplateJsonHandler;
        public DynamicReportViewController(
            IDynamicReportGetTemplateJsonHandler dynamicReportGetTemplateJsonHandler)
        {
            _dynamicReportGetTemplateJsonHandler = dynamicReportGetTemplateJsonHandler;
            _dynamicReportGetTemplateJsonHandler.NotNull(nameof(dynamicReportGetTemplateJsonHandler));

            SetLicense();
            void SetLicense()
            {               
                string basePath = AppContext.BaseDirectory;
                string relativePath = @"\AppData\DynamicReport\StiLicense.txt";
                string path = string.Concat(basePath, relativePath);
                StiLicense.Key = System.IO.File.ReadAllText(path);
            }
        }

        [Route("")]
        [HttpGet]
        public IActionResult DisplayViewer(int? id = null)//moshahede gozaresh
        {
            return View(@"~/Views/V1/DynamicGenerator/DisplayViewer.cshtml");
        }

        [HttpGet, HttpPost]
        [Route("display")]
        public async Task<IActionResult> DisplayReport(int id)//namayesh
        {
            string reportTemplateJson = await _dynamicReportGetTemplateJsonHandler.Handle(id);
            using (var report = new StiReport())
            {
                report.LoadFromJson(reportTemplateJson);
                return StiNetCoreViewer.GetReportResult(this, report);
            }
        }

        [HttpGet, HttpPost]
        [Route("event")]
        public IActionResult ViewerEvent()//roydad moshahede
        {
            return StiNetCoreViewer.ViewerEventResult(this);
        }
    }
}
