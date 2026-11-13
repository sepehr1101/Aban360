using Aban360.ClaimPool.Application.Features.AssessmentApk.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.AssessmentApk.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.NotificationPool.Application.Features.Sms;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using Aban360.Api.Cronjobs;
using Aban360.ReportPool.Domain.Base;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.ClaimPool
{
    public class AssessmentController : BaseController
    {
        private readonly IAssessmentApkValidateVersionHandler _c0_validateVersionHandler;
        private readonly IAssessmentApkLatestDownloadHandler _c0_latestDownloadHandler;
        private string _c0_apkContentType = DirectoryLiterals.ApplicationContentType;
        private readonly IHttpContextAccessor _c1_contextAccessor;
        private readonly ISetAssessmentResultHandler _c1_setAssessmentResultHandler;
        private readonly IAssessmentResultCompletionHandler _c1_assessmentResultCompletionHandler;
        private readonly ISetAssessmentTimeHandler _c1_setAssessmentTimeHandler;
        private readonly ISetLightAssessmentResultHandler _c1_setLightAssessmentResultHandler;
        private readonly ISetPreAssessmentResultHandler _c1_setPreAssessmentResultHandler;
        private readonly IAssessmentByTrackIdGetHandler _c1_assessmentByIdGetHandler;
        private readonly IReAssessmentRequestHandler _c1_reAssessmentRequestHandler;
        private readonly ISmsOldHandler _c1_smsOldHandler;
        private readonly IBackgroundJobClient _c1_backgroudJobClient;
        private readonly IAssessmentDuplicateDetailHandler _c2_assessmentDuplicateDetailHandler;
        private readonly IAssessmentDuplicateParametersGetHandler _c2_assessmentDuplicateParametersGetHandler;
        private readonly IReportGenerator _c2_reportGenerator;
        private readonly IAssessmentTaskGetAllHandler _c3_assessmentTaskHandler;
        private readonly IAssessmentLocatoinsGetHandler _c3_assessmentLocatoinsGetHandler;
        private readonly IAssessmentDuplicateDetailHandler _c3_assessmentDuplicateDetailHandler;
        public AssessmentController(IAssessmentApkValidateVersionHandler c0_validateVersionHandler, IAssessmentApkLatestDownloadHandler c0_latestDownloadHandler, IHttpContextAccessor c1_contextAccessor, ISetAssessmentResultHandler c1_setAssessmentResultHandler, IAssessmentResultCompletionHandler c1_assessmentResultCompletionHandler, ISetAssessmentTimeHandler c1_setAssessmentTimeHandler, ISetLightAssessmentResultHandler c1_setLightAssessmentResultHandler, ISetPreAssessmentResultHandler c1_setPreAssessmentResultHandler, IAssessmentByTrackIdGetHandler c1_assessmentByIdGetHandler, IReAssessmentRequestHandler c1_reAssessmentRequestHandler, ISmsOldHandler c1_smsOldHandler, IBackgroundJobClient c1_backgroudJobClient, IAssessmentDuplicateDetailHandler c2_assessmentDuplicateDetailHandler, IAssessmentDuplicateParametersGetHandler c2_assessmentDuplicateParametersGetHandler, IReportGenerator c2_reportGenerator, IAssessmentTaskGetAllHandler c3_assessmentTaskHandler, IAssessmentLocatoinsGetHandler c3_assessmentLocatoinsGetHandler, IAssessmentDuplicateDetailHandler c3_assessmentDuplicateDetailHandler)
        {
            _c0_validateVersionHandler = c0_validateVersionHandler;
            _c0_validateVersionHandler.NotNull(nameof(c0_validateVersionHandler));
            _c0_latestDownloadHandler = c0_latestDownloadHandler;
            _c0_latestDownloadHandler.NotNull(nameof(c0_latestDownloadHandler));
            _c1_contextAccessor = c1_contextAccessor;
            _c1_contextAccessor.NotNull(nameof(c1_contextAccessor));
            _c1_setAssessmentResultHandler = c1_setAssessmentResultHandler;
            _c1_setAssessmentResultHandler.NotNull(nameof(c1_setAssessmentResultHandler));
            _c1_assessmentResultCompletionHandler = c1_assessmentResultCompletionHandler;
            _c1_assessmentResultCompletionHandler.NotNull(nameof(c1_assessmentResultCompletionHandler));
            _c1_setAssessmentTimeHandler = c1_setAssessmentTimeHandler;
            _c1_setAssessmentTimeHandler.NotNull(nameof(c1_setAssessmentTimeHandler));
            _c1_setLightAssessmentResultHandler = c1_setLightAssessmentResultHandler;
            _c1_setLightAssessmentResultHandler.NotNull(nameof(c1_setLightAssessmentResultHandler));
            _c1_setPreAssessmentResultHandler = c1_setPreAssessmentResultHandler;
            _c1_setPreAssessmentResultHandler.NotNull(nameof(c1_setPreAssessmentResultHandler));
            _c1_assessmentByIdGetHandler = c1_assessmentByIdGetHandler;
            _c1_assessmentByIdGetHandler.NotNull(nameof(c1_assessmentByIdGetHandler));
            _c1_reAssessmentRequestHandler = c1_reAssessmentRequestHandler;
            _c1_reAssessmentRequestHandler.NotNull(nameof(c1_reAssessmentRequestHandler));
            _c1_smsOldHandler = c1_smsOldHandler;
            _c1_smsOldHandler.NotNull(nameof(c1_smsOldHandler));
            _c1_backgroudJobClient = c1_backgroudJobClient;
            _c1_backgroudJobClient.NotNull(nameof(c1_backgroudJobClient));
            _c2_assessmentDuplicateDetailHandler = c2_assessmentDuplicateDetailHandler;
            _c2_assessmentDuplicateDetailHandler.NotNull(nameof(c2_assessmentDuplicateDetailHandler));
            _c2_assessmentDuplicateParametersGetHandler = c2_assessmentDuplicateParametersGetHandler;
            _c2_assessmentDuplicateParametersGetHandler.NotNull(nameof(c2_assessmentDuplicateParametersGetHandler));
            _c2_reportGenerator = c2_reportGenerator;
            _c2_reportGenerator.NotNull(nameof(_c2_reportGenerator));
            _c3_assessmentTaskHandler = c3_assessmentTaskHandler;
            _c3_assessmentTaskHandler.NotNull(nameof(c3_assessmentTaskHandler));
            _c3_assessmentLocatoinsGetHandler = c3_assessmentLocatoinsGetHandler;
            _c3_assessmentLocatoinsGetHandler.NotNull(nameof(c3_assessmentLocatoinsGetHandler));
            _c3_assessmentDuplicateDetailHandler = c3_assessmentDuplicateDetailHandler;
            _c3_assessmentDuplicateDetailHandler.NotNull(nameof(c3_assessmentDuplicateDetailHandler));
        }

        [HttpPost, HttpGet]
        [Route("~/v1/assessment/apk-validate/{userVersion}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AssessmentApkValidateOutputDto>), StatusCodes.Status200OK)]
        public IActionResult ValidateVersion(string userVersion, CancellationToken cancellationToken)
        {
            AssessmentApkValidateOutputDto result = _c0_validateVersionHandler.Handle(userVersion, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("~/v1/assessment/apk-latest")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageGroup1InsertDto>), StatusCodes.Status200OK)]
        public FileResult GetLatestApk(CancellationToken cancellationToken)
        {
            var(fileStream, fileName) = _c0_latestDownloadHandler.Handle(cancellationToken);
            return File(fileStream, _c0_apkContentType, fileName, enableRangeProcessing: true);
        }

        [HttpPost]
        [Route("~/v1/assessment/result")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AssessmentResultInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetReult(CancellationToken cancellationToken)
        {
            int examinerCode = UserService.GetUserCode(CurrentUser.Username);
            AssessmentResultInputDto inputDto = await _c1_setAssessmentResultHandler.Handle(examinerCode, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("~/v1/assessment/result-completion")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AssessmentResultInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetCompletionReult(CancellationToken cancellationToken)
        {
            AssessmentResultInputDto inputDto = await _c1_assessmentResultCompletionHandler.Handle(CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("~/v1/assessment/result-assessment-sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetResultAssessmentSti([FromBody] GuidInput input, CancellationToken cancellationToken)
        {
            int reportCode = 2021;
            JsonReportId reportId = await GetSetResultJsonReport(input.Input, reportCode, cancellationToken);
            return Ok(reportId);
        }

        [HttpPost]
        [Route("~/v1/assessment/result-address-sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetResultAddressSti([FromBody] GuidInput input, CancellationToken cancellationToken)
        {
            int reportCode = 2022;
            JsonReportId reportId = await GetSetResultJsonReport(input.Input, reportCode, cancellationToken);
            return Ok(reportId);
        }

        [HttpPost]
        [Route("~/v1/assessment/result-trench-sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetResultTrenchSti([FromBody] GuidInput input, CancellationToken cancellationToken)
        {
            int reportCode = 2023;
            JsonReportId reportId = await GetSetResultJsonReport(input.Input, reportCode, cancellationToken);
            return Ok(reportId);
        }

        [HttpPost]
        [Route("~/v1/assessment/set-time")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SetAssessmentTimeOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetTime([FromBody] AssessmentSetTimeInputDto inputDto, CancellationToken cancellationToken)
        {
            int examinerCode = UserService.GetUserCode(CurrentUser.Username);
            SetAssessmentTimeDataOutputDto result = await _c1_setAssessmentTimeHandler.Handle(inputDto, examinerCode, cancellationToken);
            SetAssessmentTimeOutputDto outputDto = GetAssessmentTimeOutputDto(inputDto.HasCustomerSms, inputDto.HasAssessmentSms, result);
            return Ok(outputDto);
        }

        [HttpPost]
        [Route("~/v1/assessment/set-light-result")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MoshtrakUpdateInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetAssessmentLite([FromBody] LightAssessmentResultInputDto inputDto, CancellationToken cancellationToken)
        {
            int userName = UserService.GetUserCode(CurrentUser.Username);
            await _c1_setLightAssessmentResultHandler.Handle(inputDto, userName, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("~/v1/assessment/set-pre-result")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MoshtrakUpdateInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetPreAssessment([FromBody] PreAssessmentResultInputDto inputDto, CancellationToken cancellationToken)
        {
            int userName = UserService.GetUserCode(CurrentUser.Username);
            await _c1_setPreAssessmentResultHandler.Handle(inputDto, userName, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("~/v1/assessment/reAssessment")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SetAssessmentTimeOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ReAssessment([FromBody] SetReAssessmentTimeInputDto inputDto, CancellationToken cancellationToken)
        {
            int userName = UserService.GetUserCode(CurrentUser.Username);
            SetAssessmentTimeDataOutputDto result = await _c1_reAssessmentRequestHandler.Handle(inputDto, userName, cancellationToken);
            SetAssessmentTimeOutputDto outputDto = GetAssessmentTimeOutputDto(hasCustomerSms: true, hasAssessmentSms: true, result);
            return Ok(outputDto);
        }

        private SetAssessmentTimeOutputDto GetAssessmentTimeOutputDto(bool hasCustomerSms, bool hasAssessmentSms, SetAssessmentTimeDataOutputDto result)
        {
            string customerText = string.Format(SmsTemplates.RequestTimeSet, result.AssessmentName, result.AssessmentMobileNumber, result.AssessmentDateJalai, result.TrackNumber);
            string assessmentText = result.ServiceGroupId == 1 ? string.Format(SmsTemplates.NewRequestTimeSetAssessment, result.AssessmentName, result.AssessmentDateJalai, result.Address, result.FullName, result.NeighbourBillId, result.MobileNumber, result.ServiceSelectedList, result.TrackNumber, result.NeighbourBillId) : string.Format(SmsTemplates.AfterSaleRequestTimeSetAssessment, result.AssessmentName, result.AssessmentDateJalai, result.Address, result.FullName, result.BillId, result.MobileNumber, result.ServiceSelectedList, result.TrackNumber);
            if (hasAssessmentSms)
            {
                _c1_backgroudJobClient.Enqueue(() => _c1_smsOldHandler.SendNew(result.AssessmentMobileNumber, assessmentText, result.TrackId));
            }

            if (hasCustomerSms)
            {
                _c1_backgroudJobClient.Enqueue(() => _c1_smsOldHandler.SendNew(result.MobileNumber, customerText, result.TrackId));
            }

            return new SetAssessmentTimeOutputDto(result.TrackNumber, hasAssessmentSms, hasCustomerSms, hasAssessmentSms ? assessmentText : null, hasCustomerSms ? customerText : null);
        }

        private async Task<JsonReportId> GetSetResultJsonReport(Guid id, int reportCode, CancellationToken cancellationToken)
        {
            AssessmentDataOutputDto result = await _c1_assessmentByIdGetHandler.Handle(id, cancellationToken);
            if (result is null || result.AllInJson is null || string.IsNullOrWhiteSpace(result.AllInJson))
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidShowPreviousRequest);
            }

            JObject jsonObject = new JObject
            {
                ["reportData"] = JToken.Parse(result.AllInJson)
            };
            // Transform the embedded JSON string into an actual array
            JObject reportData = (JObject)jsonObject["reportData"];
            if (reportData != null)
            {
                var property = reportData.Property("commercialUnitsDetailsJson");
                if (property != null && property.Value.Type == JTokenType.String)
                {
                    string arrayJson = property.Value.ToString();
                    // Guard against empty / whitespace JSON string
                    if (!string.IsNullOrWhiteSpace(arrayJson))
                    {
                        // Parse the string into a JArray and replace the value
                        property.Value = JToken.Parse(arrayJson);
                    }
                    else
                    {
                        // Optionally set to an empty array or leave as null
                        property.Value = new JArray();
                    }
                }
            }

            JsonReportId reportId = await JsonOperation.ExportToJsonFlat(jsonObject, cancellationToken, reportCode);
            return reportId;
        }

        [HttpGet]
        [Route("~/v1/assessment/duplicate-dictionary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public IActionResult GetDictionary(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> result = _c2_assessmentDuplicateParametersGetHandler.Handle(cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("~/v1/assessment/duplicate-raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<AssessmentDuplicatHeaderOutputDto, AssessmentDuplicateDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw([FromBody] AssessmentDuplicateInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<AssessmentDuplicatHeaderOutputDto, AssessmentDuplicateDetailDataOutputDto> result = await _c2_assessmentDuplicateDetailHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/assessment/duplicate-excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, AssessmentDuplicateInputDto inputDto, CancellationToken cancellationToken)
        {
            await _c2_reportGenerator.FireAndInform(inputDto, cancellationToken, _c2_assessmentDuplicateDetailHandler.Handle, CurrentUser, ReportLiterals.ConsumptionManagerDetail, connectionId);
            return Ok(inputDto);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/assessment/task")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AssessmentTasksOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            int examinerCode = UserService.GetUserCode(CurrentUser.Username);
            AssessmentTasksOutputDto result = await _c3_assessmentTaskHandler.Handle(examinerCode, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("~/v1/assessment/locations/{trackId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AssessmentLocationsGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLocations(Guid trackId, CancellationToken cancellationToken)
        {
            AssessmentLocationsGetDto result = await _c3_assessmentLocatoinsGetHandler.Handle(trackId, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("~/v1/assessment/duplicate")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<AssessmentDuplicatHeaderOutputDto, AssessmentDuplicateDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDuplicateDetail([FromBody] AssessmentDuplicateInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<AssessmentDuplicatHeaderOutputDto, AssessmentDuplicateDetailDataOutputDto> result = await _c3_assessmentDuplicateDetailHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }
    }
}
