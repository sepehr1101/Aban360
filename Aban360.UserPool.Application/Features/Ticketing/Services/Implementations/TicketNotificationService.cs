using System.Globalization;
using Aban360.Common.Literals;
using Aban360.NotificationPool.Application.Features.Sms;
using Aban360.UserPool.Application.Features.Ticketing.Services.Contracts;
using Aban360.UserPool.Domain.Features.Ticketing.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Aban360.UserPool.Application.Features.Ticketing.Services.Implementations
{
    internal sealed class TicketNotificationService : ITicketNotificationService
    {
        private readonly IUnitOfWork _uow;
        private readonly IServiceProvider _services;
        private readonly ILogger<TicketNotificationService> _logger;

        public TicketNotificationService(IUnitOfWork uow, IServiceProvider services, ILogger<TicketNotificationService> logger)
        {
            _uow = uow;
            _services = services;
            _logger = logger;
        }

        public async Task Created(string ticketNumber)
        {
            try
            {
                var ticket = await _uow.Set<Ticket>().AsNoTracking().Include(x => x.CreatedByUser)
                    .SingleAsync(x => x.TicketNumber == ticketNumber);
                var requestNumber = ticket.Id.ToString("D8", CultureInfo.InvariantCulture);
                await Send(ticket, $"{ticket.CreatedByUser.FullName} عزیز، درخواست تیکت شما ثبت گردید و تیم پشتیبانی در اولین فرصت آن را بررسی خواهند کرد.\nشماره درخواست: {requestNumber}\nزمان ثبت: {FormatTime(ticket.CreatedAt)}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not send ticket creation SMS for {TicketNumber}", ticketNumber);
            }
        }

        public async Task Answered(long ticketId, Guid senderUserId)
        {
            try
            {
                var ticket = await _uow.Set<Ticket>().AsNoTracking().Include(x => x.CreatedByUser)
                    .SingleAsync(x => x.Id == ticketId);
                // An owner's follow-up is not a support response, even when the owner is an admin.
                if (ticket.CreatedByUserId == senderUserId)
                    return;
                var requestNumber = ticket.Id.ToString("D8", CultureInfo.InvariantCulture);
                await Send(ticket, $"{ticket.CreatedByUser.FullName} عزیز، تیکت شما با شماره درخواست {requestNumber} پاسخ داده شد.\nزمان پاسخ: {FormatTime(ticket.UpdatedAt)}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not send ticket response SMS for ticket {TicketId}", ticketId);
            }
        }

        private async Task Send(Ticket ticket, string text)
        {
            if (string.IsNullOrWhiteSpace(ticket.CreatedByUser.Mobile))
            {
                _logger.LogWarning("Ticket {TicketId} owner has no mobile number; SMS skipped", ticket.Id);
                return;
            }
            // Transport construction validates SMS configuration. Keep that validation
            // inside the notification error boundary, after the ticket has committed.
            var sms = _services.GetRequiredService<ISmsOldHandler>();
            await sms.Send(ticket.CreatedByUser.Mobile, text, Guid.NewGuid(), SmsMessageType.Notification);
        }

        private static string FormatTime(DateTime utc)
        {
            var local = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(utc, DateTimeKind.Utc),
                TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time"));
            var calendar = new PersianCalendar();
            var day = CultureInfo.GetCultureInfo("fa-IR").DateTimeFormat.GetDayName(local.DayOfWeek);
            return FormattableString.Invariant($"{day} {calendar.GetYear(local):0000}/{calendar.GetMonth(local):00}/{calendar.GetDayOfMonth(local):00} ساعت {local:HH:mm}");
        }
    }
}
