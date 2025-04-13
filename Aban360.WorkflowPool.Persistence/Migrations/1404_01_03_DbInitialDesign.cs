using Aban360.WorkflowPool.Persistence.Extensions;
using Aban360.WorkflowPool.Persistence.Migrations.Enums;
using Aban360.WorkflowPool.Persistence.Constants;
using FluentMigrator;
using System.Reflection;

namespace Aban360.WorkflowPool.Persistence.Migrations
{
    [Migration(14040103)]
    public class DbInitialDesign : Migration
    {
        string _schema = TableSchema.Name, Id = nameof(Id), Hash = nameof(Hash);
        int _31 = 31, _255 = 255, _1023 = 1023, _15 = 15;
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

        private void CreateAssignAlgorithm()
        {
            var table = TableName.AssignAlgorithm;
            Create.Table(nameof(TableName.AssignAlgorithm)).InSchema(_schema)
                .WithColumn(Id).AsInt16().PrimaryKey()
                .WithColumn("Title").AsString(_255).NotNullable();
        }
        private void _CreateStateType()
        {

        }

        private void CreateWorkflowStatus()
        {
            var table = TableName.WorkflowStatus;
            Create.Table(nameof(TableName.WorkflowStatus)).InSchema(_schema)
                .WithColumn(Id).AsInt16().PrimaryKey()
                .WithColumn("Title").AsString(_255).NotNullable();//created, published, ...
        }
        private void CreateWorkflow()
        {
            var table = TableName.Workflow;
            Create.Table($"{nameof(TableName.Workflow)}").InSchema(_schema)
                .WithColumn(Id).AsInt32().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn("Title").AsString(_255).NotNullable()
                .WithColumn("Name").AsAnsiString(_31)
                .WithColumn("JsonDefinition").AsString(int.MaxValue).Nullable()
                .WithColumn("Version").AsInt16()
                .WithColumn("ValidFrom").AsDateTime().NotNullable()
                .WithColumn("ValidTo").AsDateTime().Nullable()
                .WithColumn("Description").AsAnsiString(_255).Nullable()
                .WithColumn("WorkflowStatusId").AsInt16().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.WorkflowStatus, table), _schema, nameof(TableName.WorkflowStatus), Id);

        }
        private void CreateState()
        {
            var table = TableName.State;
            Create.Table($"{nameof(TableName.State)}").InSchema(_schema)
                .WithColumn(Id).AsInt32().PrimaryKey(NamingHelper.Pk(table)).Identity()
                .WithColumn("Code").AsInt32().NotNullable()//
                .WithColumn("Title").AsString(_255).NotNullable()// 
                .WithColumn("WorkflowId").AsInt32().NotNullable()
                    .ForeignKey(NamingHelper.Fk(TableName.Workflow, table), _schema, nameof(TableName.Workflow), Id)
                .WithColumn("ValidFrom").AsDateTime().NotNullable()
                .WithColumn("ValidTo").AsDateTime().Nullable()
                .WithColumn("InsertLogInfo").AsString(int.MaxValue).NotNullable()
                .WithColumn("RemoveLogInfo").AsString(int.MaxValue).Nullable()
                .WithColumn(Hash).AsString(int.MaxValue).NotNullable();
        }
        private void CreateOperation()
        {
            var table = TableName.Operation;
            Create.Table($"{nameof(TableName.Operation)}").InSchema(_schema)
                .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table))
                .WithColumn("Title").AsString(_255)
                .WithColumn("ValidFrom").AsDateTime().NotNullable()
                .WithColumn("ValidTo").AsDateTime().Nullable()
                .WithColumn("InsertLogInfo").AsString(int.MaxValue).NotNullable()
                .WithColumn("RemoveLogInfo").AsString(int.MaxValue).Nullable();
        }
        //private void CreateCartable()
        //{
        //    var table= TableName.Cartable;
        //    Create.Table($"{nameof(TableName.Cartable)}").InSchema(_schema)
        //        .WithColumn(Id).AsInt16().PrimaryKey(NamingHelper.Pk(table)).Identity()
        //        .WithColumn("WorkflowId").AsInt32().NotNullable()
        //            .ForeignKey(NamingHelper.Fk(TableName.Workflow, table), _schema, nameof(TableName.Workflow), Id)
        //        .WithColumn("StateCode").AsInt32()
        //            .ForeignKey(NamingHelper.Fk(TableName.State, table), _schema, nameof(TableName.State), "Code")
        //        .WithColumn("GrantedRoles").AsAnsiString(int.MaxValue).Nullable()
        //        .WithColumn("ConfirmRequired").AsBoolean().NotNullable()
        //        .WithColumn("ValidFrom").AsDateTime().NotNullable()
        //        .WithColumn("ValidTo").AsDateTime().Nullable()
        //        .WithColumn("InsertLogInfo").AsString(int.MaxValue).NotNullable()
        //        .WithColumn("RemoveLogInfo").AsString(int.MaxValue).Nullable();
        //}
        private void _CreateWorkflowVariable()
        {
            Create.Table("Variables")
               .WithColumn("VariableId").AsInt32().PrimaryKey().Identity()
               .WithColumn("InstanceId").AsString(36).NotNullable().ForeignKey("WorkflowInstances", "InstanceGuidId")
               .WithColumn("Name").AsString(255).NotNullable()
               .WithColumn("Value").AsString(255).Nullable()
               .WithColumn("Type").AsString(50).Nullable()
               .WithColumn("Scope").AsString(50).Nullable();
        }
        private void _CreateWorkflowInstance()
        {
            Create.Table("WorkflowInstances")
          .WithColumn("InstanceGuidId").AsString(36).PrimaryKey()
          .WithColumn("WorkflowId").AsInt32().NotNullable().ForeignKey("Workflows", "WorkflowId")
          .WithColumn("InstanceId").AsString(255).NotNullable()
          .WithColumn("CorrelationId").AsString(255).Nullable()
          .WithColumn("StartedDate").AsDateTime().Nullable()
          .WithColumn("CompletedDate").AsDateTime().Nullable()
          .WithColumn("Status").AsString(50).NotNullable()
          .WithColumn("Data").AsString(int.MaxValue).Nullable();
        }


        private void _CreateActivityType()
        {

        }
        private void _CreateActivity()
        {
            Create.Table("Activities")
           .WithColumn("ActivityId").AsInt32().PrimaryKey().Identity()
           .WithColumn("WorkflowId").AsInt32().NotNullable().ForeignKey("Workflows", "WorkflowId")
           .WithColumn("InstanceId").AsString(36).Nullable().ForeignKey("WorkflowInstances", "InstanceGuidId")
           .WithColumn("Type").AsString(50).NotNullable()
           .WithColumn("Name").AsString(255).NotNullable()
           .WithColumn("Definition").AsString(int.MaxValue).Nullable()
           .WithColumn("Status").AsString(50).NotNullable()
           .WithColumn("StartedDate").AsDateTime().Nullable()
           .WithColumn("CompletedDate").AsDateTime().Nullable();
        }

        private void _CreateActivityInstance()
        {
            Create.Table("ActivityInstances")
           .WithColumn("ActivityInstanceId").AsInt32().PrimaryKey().Identity()
           .WithColumn("ActivityId").AsInt32().NotNullable().ForeignKey("Activities", "ActivityId")
           .WithColumn("InstanceId").AsString(36).NotNullable().ForeignKey("WorkflowInstances", "InstanceGuidId")
           .WithColumn("StartedDate").AsDateTime().Nullable()
           .WithColumn("CompletedDate").AsDateTime().Nullable()
           .WithColumn("Status").AsString(50).NotNullable()
           .WithColumn("Data").AsString(int.MaxValue).Nullable();
        }
        private void _CreateActivityEvent()
        {
            Create.Table("Events")
           .WithColumn("EventId").AsInt32().PrimaryKey().Identity()
           .WithColumn("InstanceId").AsString(36).NotNullable().ForeignKey("WorkflowInstances", "InstanceGuidId")
           .WithColumn("ActivityInstanceId").AsInt32().Nullable().ForeignKey("ActivityInstances", "ActivityInstanceId")
           .WithColumn("Type").AsString(50).NotNullable()
           .WithColumn("Data").AsString(int.MaxValue).Nullable()
           .WithColumn("Timestamp").AsDateTime().WithDefault(SystemMethods.CurrentUTCDateTime);

        }
        private void _CreateTransition()
        {
            Create.Table("Transitions")
           .WithColumn("TransitionId").AsInt32().PrimaryKey().Identity()
           .WithColumn("SourceActivityId").AsInt32().NotNullable().ForeignKey("Activities", "ActivityId")
           .WithColumn("TargetActivityId").AsInt32().NotNullable().ForeignKey("Activities", "ActivityId")
           .WithColumn("Condition").AsString(255).Nullable()
           .WithColumn("Trigger").AsString(255).Nullable();
        }

    }
}