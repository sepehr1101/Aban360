using Aban360.Common.ApplicationUser;
using Aban360.Common.Extensions;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.RecieveDto;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto;
using Aban360.TaxPool.Persistence.Features.MaaherTSP.Contracts;

namespace Aban360.TaxPool.Application.Features.MaaherSTP.Handlers.Commands.Contracts
{
    public interface INewListCreateHandler
    {
        Task Handle(NewListCreateDto input, IAppUser appUser, CancellationToken cancellationToken);
    }
    internal sealed class NewListCreateHandler : INewListCreateHandler
    {
        private readonly IMaliatMaaherWrapperService _maliatMaaherWrapperService;
        private readonly IMaliatMaaherDetailService _maliatMaaherDetailService;
        public NewListCreateHandler(
            IMaliatMaaherWrapperService maliatMaaherWrapperService,
            IMaliatMaaherDetailService maliatMaaherDetailService)
        {
            _maliatMaaherWrapperService = maliatMaaherWrapperService;
            _maliatMaaherWrapperService.NotNull(nameof(maliatMaaherWrapperService));

            _maliatMaaherDetailService = maliatMaaherDetailService;
            _maliatMaaherDetailService.NotNull(nameof(maliatMaaherDetailService));
        }
        public async Task Handle(NewListCreateDto input, IAppUser appUser, CancellationToken cancellationToken)
        {
            int newWrapperId = await InsertWrapper(appUser);
            MaliatMaaherDetailInsertBatchDto dateInterval = GetMaaherDetailDTo(input, newWrapperId);
            IEnumerable<MaliatMaaherDetailGetDto> maaherDetail = await _maliatMaaherDetailService.Get(dateInterval);
            await _maliatMaaherDetailService.Inserts(maaherDetail);
            await UpdateWrapperCountAndAmount(newWrapperId);
        }
        private async Task<int> InsertWrapper(IAppUser appUser)
        {
            MaliatMaaherWrapperInsertDto newWrapper = new MaliatMaaherWrapperInsertDto(DateTime.Now, appUser.UserId, 0, 0);
            int newWrapperId = await _maliatMaaherWrapperService.Insert(newWrapper);
            return newWrapperId;
        }
        private MaliatMaaherDetailInsertBatchDto GetMaaherDetailDTo(NewListCreateDto input, int newWrapperId)
        {
            string fromDateJalali = $"{input.Year}/{input.Month}/01";
            string toDateJalali = $"{input.Year}/{input.Month}/{GetToDayFromMonth(input.Month)}";

            return new MaliatMaaherDetailInsertBatchDto(newWrapperId, fromDateJalali, toDateJalali);
        }
        private int GetToDayFromMonth(string month)
        {
            int _month = Convert.ToInt32(month);
            if (_month >= 1 && _month <= 6) return 31;
            if (_month >= 7 && _month <= 11) return 30;
            if (_month == 12) return 29;
            return 0;
        }
        private async Task UpdateWrapperCountAndAmount(int newWrapperId)
        {
            MaliatMaaherDetailAmountAndCountDto result = await _maliatMaaherDetailService.Get(newWrapperId);
            UpdateMaliatMaaherWrapperAmountAndCountDto updateWrapper = new UpdateMaliatMaaherWrapperAmountAndCountDto(newWrapperId, result.InvoiceCount, result.SumAmount);
            await _maliatMaaherWrapperService.UpdateAmountAndCount(updateWrapper);
        }
    }
}
