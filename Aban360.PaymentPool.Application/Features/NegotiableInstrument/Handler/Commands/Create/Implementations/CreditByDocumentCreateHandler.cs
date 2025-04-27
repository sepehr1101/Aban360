using Aban360.BlobPool.GatewayAddHoc.Features.Taxonomy.Contracts;
using Aban360.CalculationPool.GatewayAdhoc.Features.Bill.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Create.Contracts;
using Aban360.PaymentPool.Domain.Constansts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Commands.Contracts;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts;
using AutoMapper;
using DNTPersianUtils.Core;
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

            _bankFileStructureQueryService= bankFileStructureQueryService;
            _bankFileStructureQueryService.NotNull(nameof(_bankFileStructureQueryService));
        }

        public async Task Handle(IAppUser currentUser, CreditByDocumentCreateDto createDto, CancellationToken cancellationToken)
        {
            var document = await _DocumentAddhoc.Handle(createDto.DocumentId, cancellationToken);
            var documentText = Encoding.UTF8.GetString(document);
            var bankFileStructure = (await _bankFileStructureQueryService.Get())
                .Where(b=>b.BankId==createDto.BankId)
                .ToList();

            var documentDate = documentText.Split("\r\n");

            Uploader uploader = new Uploader()
            {
                UserId = currentUser.UserId,
                Username = currentUser.FullName,
                BankId = createDto.BankId,
                InsertDateTime = DateTime.Now,//todo: persian
                InsertRecordCount = documentDate.Length,//count off Document OR count off insert?
                LetterNumber = createDto.LetterNumber,
                DocumentId = createDto.DocumentId,
            };
            ICollection<Credit> credits = new List<Credit>();
            foreach (var item in documentDate)
            {
                string paymentId = item.Substring(34, 12);
                var invoiceInstallment = await _InvoiceInstallmentGetByPaymentId.Handle(paymentId, cancellationToken);

                if (invoiceInstallment != null)
                {
                    Credit currentCredit = new Credit()
                    {
                        //BillId = item.Substring(20, 9),
                        //PaymentId = paymentId,
                        InvoiceId = invoiceInstallment.InvoiceId,
                        InvoiceInstallmentId = invoiceInstallment.Id,
                        Amount = invoiceInstallment.Amount,
                        Uploader = uploader,
                        CreditorTypeId = CreditorTypeEnum.ElectronicPayment,
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

                            var property = creditType.GetProperty(structure.Title);
                            if (property != null && property.CanWrite && property.PropertyType == typeof(string))
                            {
                                property.SetValue(currentCredit, extractedText);
                            }
                        }
                    }
                    credits.Add(currentCredit);
                }
            }
            uploader.Amount=credits.Sum(x => x.Amount);
            uploader.Credits=credits;
            await _uploaderCommandService.Add(uploader);
        }
    }
}
