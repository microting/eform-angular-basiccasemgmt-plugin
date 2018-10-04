namespace CaseManagement.Pn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingRelatedEntityIdToCalendarUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CalendarUsers", "RelatedEntityId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CalendarUsers", "RelatedEntityId");
        }
    }
}
