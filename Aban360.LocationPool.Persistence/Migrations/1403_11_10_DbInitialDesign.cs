using FluentMigrator;
using System.Reflection;
using Aban360.LocationPool.Persistence.Extensions;
using Aban360.LocationPool.Persistence.Migrations.Enums;

namespace Aban360.LocationPool.Persistence.Migrations
{
    [Migration(14031110)]
    public class DbInitialDesign : Migration
    {
        string Id = nameof(Id), Title = nameof(Title), Hash = nameof(Hash);
        int _31 = 31, _255 = 255, _1023 = 1023;
        public override void Up()
        {
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

        private void CreateCountry()
        {
            var table = TableName.Country;
            Create.Table(nameof(TableName.Country))
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn(Title).AsString(_255).NotNullable();
        }
        private void CreateCordinalDirection()
        {
            var table = TableName.CordinalDirection;
            Create.Table(nameof(TableName.CordinalDirection))
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn(Title).AsString(_255).NotNullable();
        }
        private void CreateProvince()
        {
            var table = TableName.Province;
            Create.Table(nameof(TableName.Province))
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn(Title).AsString(_255)
                .WithColumn($"{nameof(TableName.CordinalDirection)}{Id}").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.CordinalDirection, table), nameof(TableName.CordinalDirection), Id)
                .WithColumn($"{nameof(TableName.Country)}{Id}").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.Country, table), nameof(TableName.Country), Id);
        }
        private void CreateHeadquarters()
        {
            var table = TableName.Headquarters;
            Create.Table(nameof(TableName.Headquarters))
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn(Title).AsString(_255).NotNullable()
                .WithColumn($"{nameof(TableName.Province)}{Id}").AsInt16().NotNullable()
                   .ForeignKey(NamingHelper.Fk(TableName.Province, table), nameof(TableName.Province), Id);
        }
        private void CreateRegion()
        {
            var table = TableName.Region;
            Create.Table(nameof(TableName.Region))
                .WithColumn(Id).AsInt32().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn(Title).AsString(_255).NotNullable()
                .WithColumn($"{nameof(TableName.Headquarters)}{Id}").AsInt16().NotNullable()
                   .ForeignKey(NamingHelper.Fk(TableName.Headquarters, table), nameof(TableName.Headquarters), Id);
        }
        private void CreateZone()
        {
            var table = TableName.Zone;
            Create.Table(nameof(TableName.Zone))
                .WithColumn(Id).AsInt32().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn(Title).AsString(_255).NotNullable()
                .WithColumn($"{nameof(TableName.Region)}{Id}").AsInt32().NotNullable()
                   .ForeignKey(NamingHelper.Fk(TableName.Region, table), nameof(TableName.Region), Id)
                .WithColumn("UnstandardCode").AsAnsiString(5).Nullable();
        }
        private void CreateMunicipality()//شهرداری، شهر،روستا
        {
            var table = TableName.Municipality;
            Create.Table(nameof(TableName.Municipality))
                .WithColumn(Id).AsInt32().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn(Title).AsString(_255).NotNullable()
                .WithColumn($"{nameof(TableName.Zone)}{Id}").AsInt32().NotNullable()
                   .ForeignKey(NamingHelper.Fk(TableName.Zone, table), nameof(TableName.Zone), Id)
                .WithColumn("IsVillage").AsBoolean().NotNullable();
        }
        private void CreateReadingBound()
        {
            var table = TableName.ReadingBound;
            Create.Table(nameof(TableName.ReadingBound))
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn(Title).AsString(_255).NotNullable()
                .WithColumn("FromReadingNumber").AsAnsiString(20).NotNullable()
                .WithColumn("ToReadingNumber").AsAnsiString(20).NotNullable()
                .WithColumn($"{nameof(TableName.Zone)}{Id}").AsInt32().NotNullable()
                   .ForeignKey(NamingHelper.Fk(TableName.Zone, table), nameof(TableName.Zone), Id);
        }
        private void CreateReadingBlock()
        {
            var table = TableName.ReadingBlock;
            Create.Table(nameof(TableName.ReadingBlock))
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn(Title).AsString(_255).NotNullable()
                .WithColumn("FromReadingNumber").AsAnsiString(20).NotNullable()
                .WithColumn("ToReadingNumber").AsAnsiString(20).NotNullable()
                .WithColumn($"{nameof(TableName.ReadingBound)}{Id}").AsInt16().NotNullable()
                   .ForeignKey(NamingHelper.Fk(TableName.ReadingBound, table), nameof(TableName.ReadingBound), Id);
        }
    }
}