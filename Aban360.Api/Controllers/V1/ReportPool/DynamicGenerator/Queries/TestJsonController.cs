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
            ForceFonts();
            void SetLicense()
            {               
                string basePath = AppContext.BaseDirectory;
                string relativePath = @"\AppData\DynamicReport\StiLicense.txt";
                string path = string.Concat(basePath, relativePath);
                StiLicense.Key = System.IO.File.ReadAllText(path);
            }
            void ForceFonts()
            {
                //StiFontCollection.AddFontFile(@"C:\Fonts\Vazir.ttf");
            }
        }

        [Route("")]
        [HttpGet]
        public IActionResult DisplayViewer(Guid jsonId, int reportCode)
        {
            return View(@"~/Views/V1/ReportTestJson/DisplayViewer.cshtml");
        }

        [HttpGet, HttpPost]
        [Route("display")]
        public async Task<IActionResult> DisplayReport(Guid jsonId, int reportCode)
        {
            StiReport report = new ();
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string reportPath = Path.Combine("AppData", "Mrts", $"{reportCode}.mrt");
            report.Load(reportPath);

            // Load JSON data 
            string jsonPath = Path.Combine("AppData", "Jsons", $"{jsonId}.json");
            DataSet dataSet = StiJsonToDataSetConverter.GetDataSetFromFile(jsonPath);
            dataSet.NotNull(nameof(dataSet));

            // Clear existing data sources
            report.Dictionary.Databases.Clear();
            report.Dictionary.DataSources.Clear();
         
            report.RegData(dataSet);
            report.Dictionary.Synchronize();

            return StiNetCoreViewer.GetReportResult(this, report);
        }

        [HttpGet, HttpPost]
        [Route("event")]
        public IActionResult ViewerEvent()
        {
            return StiNetCoreViewer.ViewerEventResult(this);
        }

        [HttpGet, HttpPost]
        [Route("pdf")]
        public IActionResult Pdf(Guid jsonId, int reportCode)
        {
            StiReport report = new();
            string reportPath = Path.Combine("AppData", "Mrts", $"{reportCode}.mrt");
            report.Load(reportPath);

            // Load JSON data 
            string jsonPath = Path.Combine("AppData", "Jsons", $"{jsonId}.json");
            DataSet dataSet = StiJsonToDataSetConverter.GetDataSetFromFile(jsonPath);
            dataSet.NotNull(nameof(dataSet));

            // Clear existing data sources
            report.Dictionary.Databases.Clear();
            report.Dictionary.DataSources.Clear();

            report.RegData(dataSet);
            report.Dictionary.Synchronize();

            report.Compile();
            report.Render();

            var settings = new Stimulsoft.Report.Export.StiPdfExportSettings
            {
                EmbeddedFonts = true
            };
            var service = new Stimulsoft.Report.Export.StiPdfExportService();
            using var stream = new MemoryStream();
            service.ExportPdf(report, stream, settings);
            return File(stream.ToArray(), "application/pdf", "report.pdf");
        }       
    }
}
