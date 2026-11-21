using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Application.Features.MeterReading.Services;
using Aban360.Common.Extensions;
using Hangfire;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Implementations;

// Jobهای قبلاً ذخیره‌شده در Hangfire نام این کلاس و متدهای آن را نگه داشته‌اند.
// فقط اجرای آن‌ها را به سرویس تجمیع‌شده هدایت می‌کنیم تا Jobهای معوق از کار نیفتند.
internal sealed class MeterReadingDetailSendSmsHandler : IMeterReadingDetailSendSmsHandler
{
    private readonly IMeterReadingSmsService _service;

    public MeterReadingDetailSendSmsHandler(IMeterReadingSmsService service)
    {
        service.NotNull(nameof(service));
        _service = service;
    }

    public Task Handle(int latestFlowId, Guid userId, CancellationToken cancellationToken) =>
        _service.MeterReadingDetailSendSmsAsync(latestFlowId, userId, cancellationToken);

    [AutomaticRetry(Attempts = 2)]
    public Task CreateJob(int firstFlowId, Guid userId) =>
        _service.MeterReadingDetailSendSmsCreateJob(firstFlowId, userId);

    [AutomaticRetry(Attempts = 2)]
    public Task SendSms(int newSmsFlowId, Guid userId) =>
        _service.MeterReadingDetailSendSmsSendSms(newSmsFlowId, userId);
}
