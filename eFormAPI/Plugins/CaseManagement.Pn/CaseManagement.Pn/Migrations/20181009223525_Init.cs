using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaseManagement.Pn.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalendarUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SiteId = table.Column<int>(nullable: false),
                    IsVisibleInCalendar = table.Column<bool>(nullable: false),
                    NameInCalendar = table.Column<string>(nullable: true),
                    Color = table.Column<string>(nullable: true),
                    RelatedEntityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CaseManagementSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SelectedTemplateId = table.Column<int>(nullable: true),
                    SelectedTemplateName = table.Column<string>(nullable: true),
                    RelatedEntityGroupId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseManagementSettings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalendarUsers_SiteId",
                table: "CalendarUsers",
                column: "SiteId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalendarUsers");

            migrationBuilder.DropTable(
                name: "CaseManagementSettings");
        }
    }
}
