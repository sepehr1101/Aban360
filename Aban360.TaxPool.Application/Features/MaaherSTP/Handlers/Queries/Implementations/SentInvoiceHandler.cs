using Aban360.Common.ApplicationUser;
using Aban360.Common.Extensions;
using Aban360.TaxPool.Application.Features.MaaherSTP.Handlers.Queries.Contracts;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.RecieveDto;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto;
using Aban360.TaxPool.Infrastructure.Features.MaaherTSP.Contracts;
using Aban360.TaxPool.Persistence.Features.MaaherTSP.Contracts;

namespace Aban360.TaxPool.Application.Features.MaaherSTP.Handlers.Queries.Implementations
{
    internal sealed class SentInvoiceHandler : ISentInvoiceHandler
    {
        private readonly IMaaherService _maaherService;
        private readonly IMaaherErrorsQueryService _maaherQueryService;
        private readonly IMaliatMaaherDetailService _maaherDetailService;
        private readonly IMaliatMaaherWrapperService _maaherWrapperService;
        public SentInvoiceHandler(
            IMaaherService maaherService,
            IMaaherErrorsQueryService maaherQueryService,
            IMaliatMaaherDetailService maaherDetailService,
            IMaliatMaaherWrapperService maaherWrapperService)
        {
            _maaherService = maaherService;
            _maaherService.NotNull(nameof(maaherService));

            _maaherQueryService = maaherQueryService;
            _maaherQueryService.NotNull(nameof(maaherQueryService));

            _maaherDetailService = maaherDetailService;
            _maaherDetailService.NotNull(nameof(maaherDetailService));

            _maaherWrapperService = maaherWrapperService;
            _maaherWrapperService.NotNull(nameof(maaherWrapperService));
        }

        public async Task<IEnumerable<MaaherResponseNew>> Handle(ICollection<MaaherRequestWrapper_New> inputDto, CancellationToken cancellationToken)
        {
            ICollection<MaaherResponseNew> invoiceResult = await _maaherService.SendInvoice(inputDto);

            foreach (var result in invoiceResult)
            {
                MaaherErrorsDto errors = await _maaherQueryService.GetErrors(result.ResultCode ?? 0);

                result.StatusCode = errors.HttpStatus;
                result.Description = errors.Description;
            }

            return invoiceResult.ToList();
        }
        public async Task<IEnumerable<MaaherResponseNew>> Handle(int wrapperId, IAppUser appUser, CancellationToken cancellationToken)
        {
            await WrapperConfirmedUpdate(wrapperId);
            ICollection<MaaherRequestWrapper_New> newInvoices = await GetMaaherInvoiceListDto(wrapperId);

            ICollection<MaaherResponseNew> invoicesResult = await SendInvoices(newInvoices);

            await WrapperSendUpdate(wrapperId, appUser);

            return invoicesResult;
        }
        private async Task WrapperConfirmedUpdate(int wrapperId)
        {
            MaliatMaaherWrapperConfirmedUpdateDto wrapperConfirmedUpdate = new(wrapperId, DateTime.Now);
            await _maaherWrapperService.UpdateConfirmed(wrapperConfirmedUpdate);
        }
        private async Task WrapperSendUpdate(int wrapperId, IAppUser appUser)
        {
            MaliatMaaherWrapperSendUpdateDto wrapperSendUpdate = new(wrapperId, DateTime.Now, appUser.UserId);
            await _maaherWrapperService.UpdateSend(wrapperSendUpdate);
        }

        private async Task<ICollection<MaaherResponseNew>> SendInvoices(ICollection<MaaherRequestWrapper_New> invoices)
        {
            ICollection<MaaherResponseNew> invoiceResult = await _maaherService.SendInvoice(invoices);

            foreach (var result in invoiceResult)
            {
                MaaherErrorsDto errors = await _maaherQueryService.GetErrors(result.ResultCode ?? 0);

                result.StatusCode = errors.HttpStatus;
                result.Description = errors.Description;
            }

            return invoiceResult.ToList();
        }
        private async Task<ICollection<MaaherRequestWrapper_New>> GetMaaherInvoiceListDto(int wrapperId)
        {
            IEnumerable<MaliatMaaherDetailGetDto> invoiceDetail = await _maaherDetailService.Get(wrapperId);
            ICollection<MaaherRequestWrapper_New> newInvoices = invoiceDetail.Select(s => GetMaaherInvoiceDto(s)).ToList();

            return newInvoices;
        }
        private MaaherRequestWrapper_New GetMaaherInvoiceDto(MaliatMaaherDetailGetDto input)
        {
            MaaherHeader header = GetHeader(input);
            ICollection<MaaherBody> body = GetBody(input);

            MaaherRequestWrapper_New newInvoice = new(header, body);
            return newInvoice;
        }
        private MaaherHeader GetHeader(MaliatMaaherDetailGetDto input)
        {
            MaaherHeader header = new MaaherHeader();
            header.Indatim = input.Indatim;
            header.Inty = input.Inty;
            header.Inno = input.Inno;
            header.Inp = input.Inp;
            header.Ins = input.Ins;
            header.Tob = input.Tob;
            header.Billid = input.BillId;
            header.Irtaxid = null;

            return header;
        }
        private ICollection<MaaherBody> GetBody(MaliatMaaherDetailGetDto input)
        {
            ICollection<MaaherBody> body = new List<MaaherBody>();


            if (input.Item1 > 0)
            {
                MaaherBody abBaha = new(input.Sstid, $"آب بها {input.BillId} در روز {input.Date_Bed}", input.ItemUnit1, 1, input.Item1, 0);
                body.Add(abBaha);
            }
            if (input.Item2 > 0)
            {
                MaaherBody fazBaha = new(input.Sstid, $"کارمزد دفع فاضلاب {input.BillId} در روز {input.Date_Bed}", input.ItemUnit2, 1, input.Item2, 0);
                body.Add(fazBaha);
            }
            if (input.Item3 > 0)
            {
                MaaherBody abonAb = new(input.Sstid, $"آبونمان آب {input.BillId} در روز {input.Date_Bed}", input.ItemUnit3, 1, input.Item3, 0);
                body.Add(abonAb);
            }
            if (input.Item4 > 0)
            {
                MaaherBody abonFaz = new(input.Sstid, $"آبونمان فاضلاب {input.BillId} در روز {input.Date_Bed}", input.ItemUnit4, 1, input.Item4, 0);
                body.Add(abonFaz);
            }
            if (input.Item5 > 0)
            {
                MaaherBody haqeEnsheab = new(input.Sstid, $"خدمات فروش و پس از فروش {input.BillId} در روز {input.Date_Bed}", input.ItemUnit5, 1, input.Item5, 0);
                body.Add(haqeEnsheab);
            }

            return body;
        }
    }
}