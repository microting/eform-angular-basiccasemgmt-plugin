namespace CaseManagement.Pn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CalendarUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SiteId = c.Int(nullable: false),
                        IsVisibleInCalendar = c.Boolean(nullable: false),
                        NameInCalendar = c.String(),
                        Color = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.SiteId, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.CalendarUsers", new[] { "SiteId" });
            DropTable("dbo.CalendarUsers");
        }
    }
}
