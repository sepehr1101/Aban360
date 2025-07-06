using Aban360.BlobPool.Application.Features.Base;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Validations
{
    public class ServiceLinkRawItemsValidator : BaseValidator<ServiceLinkRawItemsInputDto>
    {
        public ServiceLinkRawItemsValidator()
        {
        }
    }
}
