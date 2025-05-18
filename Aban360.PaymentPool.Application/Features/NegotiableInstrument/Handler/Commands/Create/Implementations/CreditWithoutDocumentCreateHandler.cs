using Aban360.CalculationPool.GatewayAdhoc.Features.Bill.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Create.Contracts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Commands.Contracts;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Create.Implementations
{
    internal sealed class CreditWithoutDocumentCreateHandler : ICreditWithoutDocumentCreateHandler
    {
        private readonly ICreditCommandService _creditCommandService;
        private readonly IInvoiceInstallmentGetByPaymentIdAddhoc _InvoiceInstallmentGetByPaymentId;

        public CreditWithoutDocumentCreateHandler(ICreditCommandService creditCommandService,
            IInvoiceInstallmentGetByPaymentIdAddhoc InvoiceInstallmentGetByPaymentId)
        {
            _creditCommandService = creditCommandService;
            _creditCommandService.NotNull(nameof(creditCommandService));

            _InvoiceInstallmentGetByPaymentId = InvoiceInstallmentGetByPaymentId;
            _InvoiceInstallmentGetByPaymentId.NotNull(nameof(InvoiceInstallmentGetByPaymentId));
        }

        public async Task Handle(IAppUser currentUser, CreditWithoutDocumentCreateDto createDto, Guid documentId, CancellationToken cancellationToken)
        {
            var invoiceInstallment = await _InvoiceInstallmentGetByPaymentId.Handle(createDto.PaymentId, cancellationToken);

            Uploader uploader = new Uploader()
            {
                DocumentId = documentId == Guid.Empty ? null : documentId,
                UserId = currentUser.UserId,
                Username = currentUser.FullName,
                BankId = createDto.BankId,
                InsertDateTime = DateTime.Now,
                InsertRecordCount = 1,
                Amount = invoiceInstallment.Amount,
            };
            Credit currentCredit = new Credit()
            {
                PaymentMethodId = createDto.paymentMethodId,
                BillId = invoiceInstallment.BillId,
                PaymentId = invoiceInstallment.PaymentId,
                InvoiceId = invoiceInstallment.InvoiceId,
                InvoiceInstallmentId = invoiceInstallment.Id,
                Amount = invoiceInstallment.Amount,
                Uploader = uploader,
                CreditorTypeId = createDto.CreditorTypeId,
                InsertLogInfo = "loginfo",
                ValidFrom = DateTime.Now,
                Hash = "hash",
            };
            await _creditCommandService.Add(currentCredit);

        }
    }
}
