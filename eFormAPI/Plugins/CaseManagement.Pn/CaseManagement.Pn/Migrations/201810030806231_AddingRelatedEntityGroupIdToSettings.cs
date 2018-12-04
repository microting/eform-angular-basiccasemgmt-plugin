namespace CaseManagement.Pn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingRelatedEntityGroupIdToSettings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CaseManagementSettings", "RelatedEntityGroupId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CaseManagementSettings", "RelatedEntityGroupId");
        }
    }
}
