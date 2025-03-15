using Microsoft.AspNetCore.Mvc;
using Stimulsoft.Base;
using Stimulsoft.Report.Mvc;
using Stimulsoft.Report;

namespace Aban360.Api.Controllers.V1.ReportPool.DynamicGenerator
{
    [Route("dynamic-report")]
    public class StimulsoftReportManagerController : Controller
    {
        //private readonly IUnitOfWork _uow;
        //private readonly IDynamicReportService _dynamicReportService;
        //private readonly IInactiveEntityLogService _inactiveEntityLogService;
        public StimulsoftReportManagerController(
            /*IUnitOfWork uow,
            IDynamicReportService dynamicReportService,
            IInactiveEntityLogService inactiveEntityLogService*/)
        {
            /*_uow = uow;
            _uow.CheckArgumentIsNull(nameof(_uow));

            _dynamicReportService = dynamicReportService;
            _dynamicReportService.CheckArgumentIsNull(nameof(_dynamicReportService));

            _inactiveEntityLogService = inactiveEntityLogService;
            _inactiveEntityLogService.CheckArgumentIsNull(nameof(_inactiveEntityLogService));*/
            StiLicense.Key = GetLicense();
            string GetLicense()
            {
                string basePath = AppContext.BaseDirectory;
                string relativePath = @"\AppData\DynamicReport\StiLicense.txt";
                string path = string.Concat(basePath, relativePath);
                string content = System.IO.File.ReadAllText(path);
                return content;
            }
        }

        [Route("display-designer")]
        [HttpGet]
        public IActionResult DisplayDesigner(int? id = null)
        {   
            return View(@"~/Views/V1/DynamicGenerator/DisplayDesigner.cshtml");
        }

        [HttpGet, HttpPost]
        [Route("display-viewer")]
        public IActionResult DisplayViewer(int? id = null)
        {
            return View();
        }

        [HttpGet,HttpPost]
        [Route("display-designer/design-report")]
        public async Task<IActionResult> DesignReport(int? id = null)
        {
            return StiNetCoreDesigner.GetReportResult(this);
            //if (!id.HasValue)
            //{
            var fileName = "DynamicReportBase.mrt";
                var fileLocation = Path.Combine("App_Data/Reports/", fileName);
                if (Directory.Exists(fileLocation))
                {
                    throw new Exception("No File Found on " + fileLocation);
                }
                using (var report = new StiReport())
                {
                    report.Load(fileLocation);
                    return StiNetCoreDesigner.GetReportResult(this, report);
                }
            //}
            //else
            //{
            //    var dynamicReport = await _dynamicReportService.Get(id.Value);
            //    using (var report = new StiReport())
            //    {
            //        report.LoadFromJson(dynamicReport.JsonReport);
            //        return StiNetCoreDesigner.GetReportResult(this, report);
            //    }
            //}
        }

        [HttpGet, HttpPost]
        [Route("display")]
        public async Task<IActionResult> DisplayReport(int? id = null)
        {
            //if (!id.HasValue)
            //{
                var fileName = "DynamicReportBase.mrt";
                var fileLocation = Path.Combine("App_Data/Reports/", fileName);
                if (Directory.Exists(fileLocation))
                {
                    throw new Exception("No File Found on " + fileLocation);
                }
                using (var report = new StiReport())
                {
                    report.Load(fileLocation);
                    return StiNetCoreViewer.GetReportResult(this, report);
                }
            //}
            //else
            //{
            //    var dynamicReport = await _dynamicReportService.Get(id.Value);
            //    using (var report = new StiReport())
            //    {
            //        report.LoadFromJson(dynamicReport.JsonReport);
            //        return StiNetCoreViewer.GetReportResult(this, report);
            //    }
            //}
        }

        [HttpGet, HttpPost]
        [Route("display-designer/save-report")]
        public async Task<IActionResult> SaveReport(int? id = null)
        {
            //var report = StiNetCoreDesigner.GetReportObject(this);
            //if (id.HasValue)
            //{
            //    var dynamicReportOld = await _dynamicReportService.Get(id.Value);
            //    UpdateDynamicReport(report, dynamicReportOld);
            //}
            //else
            //{
            //    var dynamicReport = CreateDynamicReport(report);
            //    await _dynamicReportService.Add(dynamicReport);
            //}
            //await _uow.SaveChangesAsync();
            return StiNetCoreDesigner.SaveReportResult(this);
        }

        [HttpGet, HttpPost]
        [Route("display-designer/save-as-report")]
        public async Task<IActionResult> SaveReportAs(int? id = null)
        {
            //if (id.HasValue)
            //{
            //    var dynamicReportOld = await _dynamicReportService.Get(id.Value);
            //    dynamicReportOld.IsActive = false;
            //}
            //var report = StiNetCoreDesigner.GetReportObject(this);
            //var dynamicReport = CreateDynamicReport(report);
            //await _dynamicReportService.Add(dynamicReport);

            //await _inactiveEntityLogService.Add(CurrentUser, "گزارش پویا", $"ذخیره");

            //await _uow.SaveChangesAsync();
            return StiNetCoreDesigner.SaveReportResult(this);
        }

        [HttpGet, HttpPost]
        [Route("display-designer/design-event")]
        public IActionResult DesignerEvent()
        {
            return StiNetCoreDesigner.DesignerEventResult(this);
        }

        [HttpGet, HttpPost]
        [Route("view-event")]
        public IActionResult ViewerEvent()
        {
            return StiNetCoreViewer.ViewerEventResult(this);
        }
    }
}
