using Aban360.MeterPool.Persistence.Migrations.Enums;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Aban360.MeterPool.Persistence.Extentions
{
    internal static class NamingHelper
    {
        /// <summary>
        /// Naming convention of Foreign key constraint
        /// </summary>
        /// <param name="primaryTable">The table containing primary-key</param>
        /// <param name="referenceTable">the table that references the primary table</param>
        /// <param name="primaryColumn">the primary table`s primary-key name</param>
        /// <returns></returns>
        internal static string Fk([NotNull] TableName primaryTable, [NotNull] TableName referenceTable, [Optional] string? primaryColumn)
            => primaryColumn is null ?
                    $"FK_{GetName(primaryTable)}_REFERS_{GetName(referenceTable)}_{GetName(primaryTable)}Id" :
                    $"FK_{GetName(primaryTable)}_REFERS_{GetName(referenceTable)}_{primaryColumn}";

        internal static string Uq([NotNull] TableName tableName, string columnName) =>
            $"UQ_{GetName(tableName)}_{columnName}";

        internal static string Pk([NotNull] TableName tableName) =>
            $"PK_{GetName(tableName)}";

        internal static string Chk([NotNull] TableName tableName, string columnName) =>
            $"CHK_{GetName(tableName)}_{columnName}";

        private static string GetName([NotNull] TableName tableName) =>
            Enum.GetName(typeof(TableName), tableName);
    }
}
