using Aban360.BlobPool.GatewayAddHoc.Features.Taxonomy.Contracts;
using Aban360.CalculationPool.GatewayAdhoc.Features.Bill.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Exceptions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Create.Contracts;
using Aban360.PaymentPool.Domain.Constansts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Commands.Contracts;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts;
using AutoMapper;
using System.Text;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Create.Implementations
{
    internal sealed class CreditByDocumentCreateHandler : ICreditByDocumentCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IUploaderCommandService _uploaderCommandService;
        private readonly IDocumentGetContentTypeAddhoc _DocumentAddhoc;
        private readonly IInvoiceInstallmentGetByPaymentIdAddhoc _InvoiceInstallmentGetByPaymentId;
        private readonly IBankFileStructureQueryService _bankFileStructureQueryService;
        public CreditByDocumentCreateHandler(
            IMapper mapper,
            IUploaderCommandService uploaderCommandService,
            IDocumentGetContentTypeAddhoc DocumentAddhoc,
            IInvoiceInstallmentGetByPaymentIdAddhoc InvoiceInstallmentGetByBillId,
            IBankFileStructureQueryService bankFileStructureQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _uploaderCommandService = uploaderCommandService;
            _uploaderCommandService.NotNull(nameof(_uploaderCommandService));

            _DocumentAddhoc = DocumentAddhoc;
            _DocumentAddhoc.NotNull(nameof(_DocumentAddhoc));

            _InvoiceInstallmentGetByPaymentId = InvoiceInstallmentGetByBillId;
            _InvoiceInstallmentGetByPaymentId.NotNull(nameof(_InvoiceInstallmentGetByPaymentId));

            _bankFileStructureQueryService = bankFileStructureQueryService;
            _bankFileStructureQueryService.NotNull(nameof(_bankFileStructureQueryService));
        }

        public async Task Handle(IAppUser currentUser, string? letterNumber, short bankId, Guid documentId, CancellationToken cancellationToken)
        {
            var document = await _DocumentAddhoc.Handle(documentId, cancellationToken);
            var documentText = Encoding.UTF8.GetString(document);
            var bankFileStructure = (await _bankFileStructureQueryService.Get())
                .ToList();

            BankFileStructure paymentIdBankFileInfo = bankFileStructure
                .Where(p => p.BankStructureItemId == BankStructureItemEnum.PaymentId)
                .First();

            string[] documentTexts = documentText.Split("\r\n");
            string[] userData = documentTexts.Skip(1).ToArray();

            Uploader uploader = new Uploader()
            {
                UserId = currentUser.UserId,
                Username = currentUser.FullName,
                BankId = bankId,
                InsertDateTime = DateTime.Now,
                ReferenceNumber = letterNumber,
                DocumentId = documentId,
            };
            ICollection<Credit> credits = new List<Credit>();
            foreach (var item in userData)
            {
                string paymentId = item.Substring(paymentIdBankFileInfo.FromIndex, paymentIdBankFileInfo.StringLenght);
                var invoiceInstallment = await _InvoiceInstallmentGetByPaymentId.Handle(paymentId, cancellationToken);

                if (invoiceInstallment != null)
                {
                    Credit currentCredit = new Credit()
                    {
                        InvoiceId = invoiceInstallment.InvoiceId,
                        InvoiceInstallmentId = invoiceInstallment.Id,
                        Amount = invoiceInstallment.Amount,
                        Uploader = uploader,
                        CreditorTypeId = CreditorTypeEnum.ElectronicPayment,
                        PaymentMethodId = PaymentMethodEnum.PaymentBank,
                        InsertLogInfo = "loginfo",
                        ValidFrom = DateTime.Now,
                        Hash = "hash"
                    };
                    var creditType = typeof(Credit);

                    foreach (var structure in bankFileStructure)
                    {
                        int startIndex = structure.FromIndex;
                        int length = structure.StringLenght;

                        if (item.Length >= startIndex + length)
                        {
                            string extractedText = item.Substring(startIndex, length);

                            var property = creditType.GetProperty(Enum.GetName(typeof(BankStructureItemEnum), structure.BankStructureItemId));
                            if (property != null && property.CanWrite && property.PropertyType == typeof(string))
                            {
                                property.SetValue(currentCredit, extractedText);
                            }
                        }
                    }
                    credits.Add(currentCredit);
                }
            }

            var firstSentence = documentTexts[0];
            long creditsAmount = credits.Sum(x => x.Amount);
            uploader.Amount = creditsAmount;
            uploader.InsertRecordCount = credits.Count();


            BankFileValidation(bankFileStructure, firstSentence, BankStructureItemEnum.TotalPrice, creditsAmount, ExceptionLiterals.InvalidTotalPrice);
            BankFileValidation(bankFileStructure,firstSentence, BankStructureItemEnum.RecordNO, credits.Count(), ExceptionLiterals.InvalidRecordCount);
            BankFileValidation(bankFileStructure, firstSentence, BankStructureItemEnum.BankCode, bankId, ExceptionLiterals.InvalidBankId);

            if (bankFileStructure.Count() < 2)
            {
                LocalException(ExceptionLiterals.InvalidRecordCount);
            }


            uploader.Credits = credits;
            await _uploaderCommandService.Add(uploader);
        }
        private void BankFileValidation(List<BankFileStructure> bankFileStructure,string text, BankStructureItemEnum bankStructureId,long credits,string errorMessage)
        {
            BankFileStructure singleBankFileStucture = bankFileStructure
                .Where(b => b.BankStructureItemId == bankStructureId)
                .First();
            string bankFileData = text.Substring(singleBankFileStucture.FromIndex, singleBankFileStucture.StringLenght);
            if (Convert.ToInt64(bankFileData) != credits)
            {
                LocalException(errorMessage);
            }
        }
        private void LocalException(string message)
        {
            throw new CreditExceptions(ExceptionLiterals.InvalidBankDocument(message));
        }
    }
}
