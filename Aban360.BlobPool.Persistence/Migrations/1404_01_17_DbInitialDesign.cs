﻿using Aban360.BlobPool.Persistence.Constants;
using FluentMigrator;
using System.Reflection;

namespace Aban360.BlobPool.Persistence.Migrations
{
    [Migration(14040117)]
    public class DbInitialDesign : Migration
    {
        string _schema = TableSchema.Name;
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

        }
        private void CreateDocumentType()
        {

        }
        private void CreateImageWatermark()
        {

        }
        private void CreateMimetypeCategory()
        {

        }
        private void CreateExecutableMimetype()
        {
            //field supportStream
            //field frontendExecutor

        }
        private void CreateDocument()
        {

        }
    }
}
