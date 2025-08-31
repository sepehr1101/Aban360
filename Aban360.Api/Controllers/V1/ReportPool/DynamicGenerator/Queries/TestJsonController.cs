using Microsoft.AspNetCore.Mvc;
using Stimulsoft.Base;
using Stimulsoft.Report.Mvc;
using Stimulsoft.Report;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.DynamicGenerator.Handlers.Queries.Contracts;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Aban360.Api.Controllers.V1.ReportPool.DynamicGenerator
{
    [Route("v1/report-test-json")]
    [AllowAnonymous]
    public class TestJsonController : BaseMvcController
    {        
        private readonly IDynamicReportGetTemplateJsonHandler _dynamicReportGetTemplateJsonHandler;
        public TestJsonController(
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
            return View(@"~/Views/V1/ReportTestJson/DisplayViewer.cshtml");
        }

        [HttpGet, HttpPost]
        [Route("display")]
        public async Task<IActionResult> DisplayReport()//namayesh
        {
            StiReport report = new ();
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string reportPath = Path.Combine("AppData", "Mrts", "non-permanent-branch-report8.mrt");
            report.Load(reportPath);

            // Load JSON data (from file or URL)
            string jsonPath = Path.Combine("AppData", "Jsons", "non-permanent-branch-report3.json");
            DataSet dataSet = StiJsonToDataSetConverter.GetDataSetFromFile(jsonPath);

            // Clear existing data sources
            report.Dictionary.Databases.Clear();
            report.Dictionary.DataSources.Clear();

            // Register data
            //report.RegData("root", "root", dataSet);
         
            report.RegData(dataSet);

            report.Dictionary.Synchronize();

            // Render report
            //report.Render();

            return StiNetCoreViewer.GetReportResult(this, report);
        }

        [HttpGet, HttpPost]
        [Route("event")]
        public IActionResult ViewerEvent()//roydad moshahede
        {
            return StiNetCoreViewer.ViewerEventResult(this);
        }
    }
}
