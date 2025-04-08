using Aban360.BlobPool.Persistence.Constants;
using Aban360.BlobPool.Persistence.Extensions;
using Aban360.BlobPool.Persistence.Migrations.Enums;
using FluentMigrator;
using System.Reflection;

namespace Aban360.BlobPool.Persistence.Migrations
{
    [Migration(14040117)]
    public class DbInitialDesign : Migration
    {
        string _schema = TableSchema.Name, Title=nameof(Title), Id = nameof(Id), Hash = nameof(Hash);
        int _31 = 31, _255 = 255, _1023 = 1023, _10 = 10;
        public override void Up()
        {
            Create.Schema(_schema);

            var methods =
               GetType()
              .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
              .Where(m => m.Name.StartsWith("Create"))
              .ToList();
            methods.ForEach(m => m.Invoke(this, null));
        }
        public override void Down()
        {
            var tableNames =
              GetType()
             .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
             .Where(m => m.Name.StartsWith("Create"))
             .Select(m => m.Name.Replace("Create", string.Empty))
             .ToList();
            tableNames.ForEach(t => Delete.Table(t));
        }

        private void CreateDocumentCategory()
        {
            var table = TableName.DocumentCategory;
            Create.Table(nameof(TableName.DocumentCategory)).InSchema(_schema)
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn(Title).AsString(_255).NotNullable()
                .WithColumn("Icon").AsAnsiString(int.MaxValue).Nullable()
                .WithColumn("Css").AsAnsiString(_255).Nullable();
        }
        private void CreateDocumentType()
        {
            var table = TableName.DocumentType;
            Create.Table(nameof(TableName.DocumentType)).InSchema(_schema)
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table)).NotNullable()
                .WithColumn($"{nameof(TableName.DocumentCategory)}Id").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.DocumentCategory, table), _schema, nameof(TableName.DocumentCategory), Id)
                .WithColumn(Title).AsString(_255)
                .WithColumn("Name").AsAnsiString(_255)
                .WithColumn("Icon").AsAnsiString(int.MaxValue)
                .WithColumn("Css").AsAnsiString(_255);
        }
        private void CreateImageWatermark()
        {

        }
        private void CreateMimetypeCategory()
        {
            var table = TableName.MimetypeCategory;
            Create.Table(nameof(TableName.MimetypeCategory)).InSchema(_schema)
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn(Title).AsString(_255)
                .WithColumn("Name").AsAnsiString(_255);
        }
        private void CreateExecutableMimetype()
        {
            var table= TableName.ExecutableMimetype;
            Create.Table(nameof(TableName.ExecutableMimetype)).InSchema(_schema)
                .WithColumn(Id).AsInt16()
                .WithColumn(Title).AsString(_255).NotNullable()
                .WithColumn("StreamingOption").AsBoolean().NotNullable()
                .WithColumn("FrontendExecutor").AsBoolean().NotNullable();

        }
        private void CreateDocument()
        {           
            string command=$"CREATE TABLE [{_schema}].[Document] "+
                           $" (" +
                           $"     [Id] UNIQUEIDENTIFIER NOT NULL CONSTRAINT [DF_Document_Id] DEFAULT NEWID()," +
                           $"     [FileRowId] UNIQUEIDENTIFIER ROWGUIDCOL NOT NULL " +
                           $"         CONSTRAINT [DF_Document_FileRowId] DEFAULT NEWID()," +
                           $"     [Name] NVARCHAR(255) NOT NULL, " +
                           $"     [Extension] NVARCHAR(255) NOT NULL," +
                           $"     [SizeInByte] BIGINT NOT NULL, " +
                           $"     [ContentType] NVARCHAR(255) NOT NULL," +
                           $"     [FileContent] VARBINARY(MAX) FILESTREAM NULL," +
                           $"     [CreatedDateTime] DATETIME NOT NULL " +
                           $"         CONSTRAINT [DF_Document_CreatedDateTime] DEFAULT GETDATE()," +
                           $"     [Description] NVARCHAR(1023), " +
                           $"     CONSTRAINT [PK_Document] PRIMARY KEY ([Id])," +
                           $"     CONSTRAINT [UQ_Document_FileRowId] UNIQUE ([FileRowId])" +
                           $" )";
            Execute.Sql(command);
        }
    }
}