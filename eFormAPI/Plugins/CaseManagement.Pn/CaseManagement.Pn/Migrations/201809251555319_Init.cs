namespace CaseManagement.Pn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CaseManagementSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SelectedTemplateId = c.Int(),
                        SelectedTemplateName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CaseManagementSettings");
        }
    }
}
