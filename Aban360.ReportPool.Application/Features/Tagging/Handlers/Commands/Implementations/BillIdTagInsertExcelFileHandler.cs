using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Persistence.Features.Tagging.Commands;
using Aban360.ReportPool.Persistence.Features.Tagging.Queries.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;
using Excel = MiniExcelLibs;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Implementations
{
    internal sealed class BillIdTagInsertExcelFileHandler : AbstractBaseConnection, IBillIdTagInsertExcelFileHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly ITagService _tagService;
        private string _filePath = ReportLiterals.ExcelFolderPath;
        public BillIdTagInsertExcelFileHandler(
            IHttpContextAccessor contextAccessor,
            ICommonMemberQueryService commonMemberQueryService,
            ITagService tagService,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _tagService = tagService;
            _tagService.NotNull(nameof(tagService));
        }

        public async Task Handle(BillIdTagInsertByExcelFileInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            string filePath = await IoExtensions.SaveToDisk(inputDto.ExcelFile, _filePath);
            ICollection<BillIdTagByStringCodeDto> billIdTagByStrinCodeList = ReadExcel(filePath, appUser.UserId);
            string opLogText = string.Format(OpLogLiterals.BillIdTagListInsert, inputDto.ExcelFile.FileName, billIdTagByStrinCodeList?.Count() ?? 0);

            await ExecSql(billIdTagByStrinCodeList, appUser, opLogText);
        }
        private async Task ExecSql(ICollection<BillIdTagByStringCodeDto> billIdTagInsertDto, IAppUser appUser, string opLogText)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    BillIdTagCommandService billIdTagCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);


                    IEnumerable<ZoneIdAndCustomerNumberAndBillId> billIdWithCustomerNumbers= await _commonMemberQueryService.Get(billIdTagInsertDto.Select(b => b.BillId),connection,transaction);
                    foreach (var item in billIdWithCustomerNumbers)
                    {
                        if (item.ZoneId <= 0 || item.CustomerNumber <= 0)
                            throw new InvalidBillIdException(ExceptionLiterals.NotFoundBillId(item.BillId));
                    }
                    
                    await billIdTagCommandService.Create(billIdTagInsertDto);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        private ICollection<BillIdTagByStringCodeDto> ReadExcel(string filePath, Guid userId)
        {
            ICollection<BillIdTagByStringCodeDto> billIdTagList = new List<BillIdTagByStringCodeDto>();
            var rows = Excel.MiniExcel.Query(filePath, useHeaderRow: false);//, sheetName: ExceptionLiterals.Page(1));
            string errorMessage = ExceptionLiterals.InvalidReadingFile;
            int count = 0;

            try
            {
                foreach (var item in rows.Skip(1))
                {
                    var row = (IDictionary<string, object>)item;
                    count++;
                    try
                    {
                        //0:BillId  1:StringCode  2:ExpireDateJalali
                        string billId = row.ElementAt(0).Value.ToString();
                        string stringCode = row.ElementAt(1).Value.ToString();
                        string? expireDateJalali = row.ElementAt(2).Value?.ToString() ?? string.Empty;

                        BillIdTagByStringCodeDto singleBillIdTag = new(billId, stringCode, expireDateJalali);
                        billIdTagList.Add(singleBillIdTag);
                    }
                    catch
                    {
                        errorMessage = ExceptionLiterals.InvalidRecord(count);
                        throw new ReadingException(errorMessage);
                    }
                }
            }
            catch
            {
                throw new ReadingException(errorMessage);
            }
            return billIdTagList;
        }
    }
}
