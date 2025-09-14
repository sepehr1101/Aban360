using Aban360.ReportPool.Domain.Features.Tagging.CustomerWarehouse.Application.DTOs;

namespace Aban360.ReportPool.Application.Features.Tagging.Contracts
{
    public interface ICreateBillIdTagHandler
    {
        Task<long> Handle(CreateBillIdTagDto dto);
    }
}