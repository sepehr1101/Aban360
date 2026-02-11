using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Persistence.Constants.Literals;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations
{
    public sealed class ExaminationCommandService
    {
        private readonly SqlConnection _sqlRonnection;
        private readonly IDbTransaction _transaction;
        public ExaminationCommandService(
            SqlConnection sqlRonnection,
            IDbTransaction transaction)
        {
            _sqlRonnection = sqlRonnection;
            _sqlRonnection.NotNull(nameof(sqlRonnection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task Insert(AssessmentInsertDto inputDto)
        {
            string command = GetInsertQuery();
            int recordCount = await _sqlRonnection.ExecuteAsync(command, inputDto);
            if (recordCount <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidInsertAssessment);
            }
        }
        private string GetInsertQuery()
        {
            return @"INSERT INTO AbAndFazelab.dbo.Examination (
                        Id, TrackNumber, BillId, ExaminerCode, ExaminerName, ExaminerMobile,
                        DayJalali, DayMiladi, ZoneId, ResultId, SetResultDateTime, ResultDescription,
                        TrackId, TrackIdResult, X1, Y1, X2, Y2, Accuracy,
                        FaseleKhakiA, FaseleKhakiF, FaseleAsphaultA, FaseleAsphaultF,
                        FaseleSangA, FaseleSangF, FaseleOtherA, FaseleOtherF,
                        OmgheZirzamin, ChahAbBaran, HasMap, ArzeshMelk, Eshterak, Arse, KarbariId
                    )
                    VALUES (
                        NEWID(), @TrackNumber, @BillId, @AssessmentCode, @AssessmentName, @AssessmentMobile,
                        @AssessmentDateJalali, @AssessmentGregorianDateTime, @ZoneId, @ResultId, @SetResultDateTime, @Description,
                        @TrackId, @TrackIdResult, @X1, @Y1, @X2, @Y2, @Accuracy,
                        @TrenchLenW, @TrenchLenS, @AsphaltLenW, @AsphaltLenS, 
                        @RockyLenW, @RockyLenS, @OtherLenW, @OtherLenS,
                        @BasementDepth, NULL, @HasMap, @HouseValue, @ReadingNumber, @Premises, @UsageId
                    );";
        }
    }
}
