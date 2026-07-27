using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Persistence.Features.Tagging;
using Aban360.ReportPool.Persistence.Features.Tagging.Commands;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Implementations
{
    internal sealed class CreateBillIdTagHandler : AbstractBaseConnection, ICreateBillIdTagHandler
    {
        private readonly IBillIdTagService _service;
        public CreateBillIdTagHandler(
            IBillIdTagService service,
            IConfiguration configuration)
            : base(configuration)
        {
            _service = service;
            _service.NotNull(nameof(service));
        }

        public async Task<long> Handle(CreateBillIdTagDto dto)
        {
            BillIdTagValidation(dto);
            long id = 0;
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    BillIdTagCommandService billIdTagCommandService = new(connection, transaction);

                    id = await billIdTagCommandService.Create(dto);

                    transaction.Commit();
                }
            }

            return id;
        }

        private async void BillIdTagValidation(CreateBillIdTagDto dto)
        {
            bool hasBillIdTag = await _service.HasBillIdTags(dto.BillId, dto.TagId);
            if (hasBillIdTag)
            {
                throw new DuplicateEntityException(ExceptionLiterals.DuplicateBillIdTags);
            }

            bool hasBillId = await _service.HasBillId(dto.BillId);
            if (!hasBillId)
            {
                throw new InvalidBillIdException(ExceptionLiterals.BillIdNotFound);
            }
        }
    }
}
