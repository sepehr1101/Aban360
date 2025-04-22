using Microsoft.AspNetCore.Mvc;
using Stimulsoft.Base;
using Stimulsoft.Report.Mvc;
using Stimulsoft.Report;
using Aban360.ReportPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.DynamicGenerator.Handlers.Commands.Create.Contracts;
using Aban360.ReportPool.Application.Features.DynamicGenerator.Handlers.Commands.Update.Contracts;
using Aban360.ReportPool.Application.Features.DynamicGenerator.Handlers.Queries.Contracts;

namespace Aban360.Api.Controllers.V1.ReportPool.DynamicGenerator.Queries
{
    [Route("dynamic-report-design")]
    public class DynamicReportViewController : BaseMvcController
    {
        private readonly IUnitOfWork _uow;
        private readonly IDynamicReportCreateHandler _dynamicReportCreateHandler;
        private readonly IDynamicReportUpdateHandler _dynamicReportUpdateHandler;
        private readonly IDynamicReportGetTemplateJsonHandler _dynamicReportGetTemplateJsonHandler;
        public DynamicReportViewController(
            IUnitOfWork uow,
            IDynamicReportCreateHandler dynamicReportCreateHandler,
            IDynamicReportUpdateHandler dynamicReportUpdateHandler,
            IDynamicReportGetTemplateJsonHandler dynamicReportGetTemplateJsonHandler)
        {

            _uow = uow;
            _uow.NotNull(nameof(uow));

            _dynamicReportCreateHandler = dynamicReportCreateHandler;
            _dynamicReportCreateHandler.NotNull(nameof(dynamicReportCreateHandler));

            _dynamicReportUpdateHandler = dynamicReportUpdateHandler;
            _dynamicReportUpdateHandler.NotNull(nameof(dynamicReportUpdateHandler));

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
        public IActionResult DisplayDesigner(int? id = null)
        {
            return View(@"~/Views/V1/DynamicGenerator/DisplayDesigner.cshtml");
        }

        [HttpGet, HttpPost]
        [Route("design")]
        public async Task<IActionResult> DesignReport(int? id = null)
        {
            if (!id.HasValue)
            {
                return StiNetCoreDesigner.GetReportResult(this);
            }
            var reportTemplateJson = await _dynamicReportGetTemplateJsonHandler.Handle(id.Value);
            using (var report = new StiReport())
            {
                report.LoadFromJson(reportTemplateJson);
                return StiNetCoreDesigner.GetReportResult(this, report);
            }
        }

        [HttpGet, HttpPost]
        [Route("save")]
        public async Task<IActionResult> SaveReport(int? id = null)
        {
            StiReport report = StiNetCoreDesigner.GetReportObject(this);
            string reportTemplateJson = report.SaveToJsonString();
            if (!id.HasValue)
            {
                await _dynamicReportCreateHandler.Handle(CurrentUser, report.ReportName, reportTemplateJson);
            }
            else
            {
                await _dynamicReportUpdateHandler.Handle(CurrentUser, id.Value, report.ReportName, reportTemplateJson);
            }
            await _uow.SaveChangesAsync();
            return StiNetCoreDesigner.SaveReportResult(this);
        }

        [HttpGet, HttpPost]
        [Route("save-as")]
        public async Task<IActionResult> SaveReportAs(int? id = null)
        {
            StiReport report = StiNetCoreDesigner.GetReportObject(this);
            string reportTemplateJson = report.SaveToJsonString();
            await _dynamicReportCreateHandler.Handle(CurrentUser, report.ReportName, reportTemplateJson);
            await _uow.SaveChangesAsync();
            return StiNetCoreDesigner.SaveReportResult(this);
        }

        [HttpGet, HttpPost]
        [Route("design-event")]
        public IActionResult DesignerEvent()
        {
            return StiNetCoreDesigner.DesignerEventResult(this);
        }
    }
}
