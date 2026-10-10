using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.MeterPool.Application.Features.AutoReading.Exceptions;
using Aban360.MeterPool.Application.Features.AutoReading.Handlers.Queries.Contracts;
using Aban360.MeterPool.Domain.Features.AutoReading.Dtos.Queries;
using FluentValidation;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;
using DNTPersianUtils.Core;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;

namespace Aban360.MeterPool.Application.Features.AutoReading.Handlers.Queries.Implementations
{
    internal sealed class FlowMeterReportGetHandler : IFlowMeterReportGetHandler
    {
        private const string SupportedBillId = "4441707016311";
        private const string FlowMeterSerial = "PYC-929801010342";
        private const int FlowMeterType = 3;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AutoReadingOptions _options;
        private readonly IValidator<FlowMeterReportInputDto> _validator;
        private readonly IConsumerSummaryGetHandler _consumerSummaryGetHandler; 

        public FlowMeterReportGetHandler(
            IHttpClientFactory httpClientFactory,
            IOptions<AutoReadingOptions> options,
            IValidator<FlowMeterReportInputDto> validator,
            IConsumerSummaryGetHandler consumerSummaryGetHandler)
        {
            _httpClientFactory = httpClientFactory;
            _httpClientFactory.NotNull(nameof(httpClientFactory));
            options.NotNull(nameof(options));
            _options = options.Value;
            _validator = validator;
            _validator.NotNull(nameof(validator));

            _consumerSummaryGetHandler = consumerSummaryGetHandler;
            _consumerSummaryGetHandler.NotNull(nameof(_consumerSummaryGetHandler));
        }

        public async Task<FlowMeterReportGetDto> Handle(FlowMeterReportInputDto inputDto, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!string.Equals(inputDto.BillId, SupportedBillId, StringComparison.Ordinal))
                throw new NonUltrasonicMeterException();

            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
                throw new CustomValidationException(string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage)));

            string requestUrl = _options.FlowMeterReportEndpoint
                + "?FromDate=" + Uri.EscapeDataString(inputDto.FromDate)
                + "&ToDate=" + Uri.EscapeDataString(inputDto.ToDate)
                + "&FlowMeterSerial=" + Uri.EscapeDataString(FlowMeterSerial)
                + "&FlowMeterType=" + FlowMeterType;

            using HttpClient httpClient = _httpClientFactory.CreateClient(HttpClientNames.AutoReading);
            using HttpResponseMessage response = await httpClient.GetAsync(requestUrl, cancellationToken);
            response.EnsureSuccessStatusCode();
            FlowMeterReportGetDto result = await response.Content.ReadFromJsonAsync<FlowMeterReportGetDto>(cancellationToken)
                ?? throw new JsonException("پاسخ سرویس قرائت خودکار معتبر نیست.");
            result.ReadAllReportModelList ??= new List<JsonElement>();
            result.UltraSonicReadAllReportModelList ??= new List<UltraSonicReadingGetDto>();

            ConsumerSummaryDto customerInfo= await _consumerSummaryGetHandler.Handle(SupportedBillId, cancellationToken);
            result.ReportHeader = new FlowMeterReportHeader();
            result.ReportHeader.ReportDateJalali = DateTime.Now.ToShortPersianDateString();
            result.ReportHeader.BillId = customerInfo.BillId;
            result.ReportHeader.Address = customerInfo.Address;
            result.ReportHeader.Firstname = customerInfo.FirstName;
            result.ReportHeader.Surname = customerInfo.Surname;
            result.ReportHeader.UsageTitle = customerInfo.UsageSell;
            result.ReportHeader.Title = "آنالیز قرائت اولتراسونیک";

            return result;
        }
    }
}
