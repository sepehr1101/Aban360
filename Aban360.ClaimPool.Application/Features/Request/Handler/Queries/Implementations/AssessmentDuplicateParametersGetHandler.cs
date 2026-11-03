using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations
{
    internal sealed class AssessmentDuplicateParametersGetHandler : IAssessmentDuplicateParametersGetHandler
    {
        public IEnumerable<NumericDictionary> Handle(CancellationToken cancellationToken)
        {
            return new List<NumericDictionary>()
            {
                new NumericDictionary(1,"شماره موبایل"),
                //new NumericDictionary(2,"کد پستی"),
                //new NumericDictionary(3,"کد ملی"),
                new NumericDictionary(4,"شناسه قبض"),
                new NumericDictionary(5,"شناسه قبض همسایه"),
            };
        }
    }
}