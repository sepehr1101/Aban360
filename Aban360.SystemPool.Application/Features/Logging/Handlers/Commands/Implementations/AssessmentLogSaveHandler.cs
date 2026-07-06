using Aban360.Common.Extensions;
using Aban360.SystemPool.Application.Features.Logging.Handlers.Commands.Contracts;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Input;

namespace Aban360.SystemPool.Application.Features.Logging.Handlers.Commands.Implementations
{
    internal sealed class AssessmentLogSaveHandler : IAssessmentLogSaveHandler
    {
        private string _path = @"AppData\AssessmentLogs";
        public async Task Handle(AssessmentLogInsertDto input, CancellationToken cancellationToken)
        {
            string path = await IoExtensions.SaveToDisk(input.File, _path);
        }
    }
}
